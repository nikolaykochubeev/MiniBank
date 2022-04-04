using System;
using System.Collections.Generic;
using Minibank.Core.Domain.BankAccounts.Repositories;
using Minibank.Core.Domain.Currency.Services;
using Minibank.Core.Domain.Transactions;
using Minibank.Core.Domain.Transactions.Repositories;
using Minibank.Core.Domain.Users.Repositories;
using Minibank.Core.Exceptions;

namespace Minibank.Core.Domain.BankAccounts.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrencyService _currencyService;
        private readonly IUnitOfWork _unitOfWork;

        public BankAccountService(IUnitOfWork unitOfWork, IBankAccountRepository bankAccountRepository,
            IUserRepository userRepository,
            ITransactionRepository transactionRepository, ICurrencyService currencyService)
        {
            _bankAccountRepository = bankAccountRepository;
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _currencyService = currencyService;
            _unitOfWork = unitOfWork;
        }

        public BankAccountModel GetById(Guid id)
        {
            var bankAccount = _bankAccountRepository.GetById(id);

            if (bankAccount is null)
            {
                throw new ObjectNotFoundException($"BankAccount with id = {id} does not exist");
            }

            return bankAccount;
        }

        public IEnumerable<BankAccountModel> GetAll()
        {
            return _bankAccountRepository.GetAll();
        }

        public IEnumerable<TransactionModel> GetAllTransactions()
        {
            return _transactionRepository.GetAll();
        }

        public Guid Create(BankAccountModel bankAccountModel)
        {
            var user = _userRepository.GetById(bankAccountModel.UserId);
            if (user is null)
            {
                throw new ObjectNotFoundException($"User with id = {bankAccountModel.UserId} does not exist");
            }

            if (bankAccountModel.AmountOfMoney <= decimal.Zero)
            {
                throw new ValidationException("Unable to create an account, the amount of money cannot be negative.");
            }

            var bankAccountId = Guid.NewGuid();
            _bankAccountRepository.Create(new BankAccountModel
            {
                Id = bankAccountId,
                UserId = bankAccountModel.UserId,
                AmountOfMoney = bankAccountModel.AmountOfMoney,
                Currency = bankAccountModel.Currency,
                OpeningDate = DateTime.Now,
                ClosingDate = DateTime.Now.AddYears(4),
                IsActive = true
            });
            _unitOfWork.SaveChanges();

            return bankAccountId;
        }

        public decimal CalculateCommission(TransactionModel transactionModel)
        {
            var fromAccount = _bankAccountRepository.GetById(transactionModel.FromAccountId);
            var toAccount = _bankAccountRepository.GetById(transactionModel.ToAccountId);

            if (fromAccount.Currency != transactionModel.Currency)
            {
                throw new ValidationException(
                    "The final currencies of the transaction and the accounts to which the money is received do not match");
            }

            var transactionMoney = transactionModel.AmountOfMoney;

            return fromAccount.UserId != toAccount.UserId ? transactionMoney * (decimal)0.02 : 0;
        }

        public Guid Transfer(TransactionModel transactionModel)
        {
            var fromAccount = _bankAccountRepository.GetById(transactionModel.FromAccountId);
            var toAccount = _bankAccountRepository.GetById(transactionModel.ToAccountId);

            if (fromAccount is null)
            {
                throw new ObjectNotFoundException(
                    $"fromBankAccount with id = {transactionModel.FromAccountId} does not exist");
            }

            if (toAccount is null)
            {
                throw new ObjectNotFoundException(
                    $"toBankAccount with id = {transactionModel.ToAccountId} does not exist");
            }

            if (!fromAccount.IsActive)
            {
                throw new ValidationException("fromAccount is not active");
            }

            if (!toAccount.IsActive)
            {
                throw new ValidationException("toAccount is not active");
            }

            if (fromAccount.Currency != transactionModel.Currency)
            {
                throw new ValidationException(
                    "The final currencies of the transaction and the accounts to which the money is received do not match");
            }

            var transactionMoney = transactionModel.AmountOfMoney;
            var commission = CalculateCommission(transactionModel);

            if (fromAccount.AmountOfMoney - transactionMoney - commission < decimal.Zero)
            {
                throw new ValidationException(
                    $"There is not enough money on the fromBankAccount with id = {fromAccount.Id} for this transfer");
            }

            fromAccount.AmountOfMoney -= (transactionMoney + commission);

            _bankAccountRepository.UpdateAmount(fromAccount.Id, fromAccount.AmountOfMoney);

            if (toAccount.Currency != fromAccount.Currency)
            {
                transactionMoney = _currencyService.Convert(transactionMoney, fromAccount.Currency.ToString(),
                        toAccount.Currency.ToString())
                    .Result;
            }

            toAccount.AmountOfMoney += transactionMoney;
            _bankAccountRepository.UpdateAmount(toAccount.Id, toAccount.AmountOfMoney);

            var transaction = _transactionRepository.Create(transactionModel);
            _unitOfWork.SaveChanges();

            return transaction;
        }

        public void Close(Guid id)
        {
            var bankAccount = _bankAccountRepository.GetById(id);

            if (bankAccount is null)
            {
                throw new ObjectNotFoundException($"BankAccount with id = {id} does not exist");
            }

            if (bankAccount.AmountOfMoney != decimal.Zero)
            {
                throw new ValidationException("It is impossible to close an account if there is money left on it");
            }

            _bankAccountRepository.Close(bankAccount);
            _unitOfWork.SaveChanges();
        }
    }
}
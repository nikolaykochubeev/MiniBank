using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Minibank.Core.Domain.BankAccounts.Repositories;
using Minibank.Core.Domain.Currency.Services;
using Minibank.Core.Domain.Transactions;
using Minibank.Core.Domain.Transactions.Repositories;
using Minibank.Core.Domain.Users;
using Minibank.Core.Domain.Users.Repositories;
using Minibank.Core.Exceptions;
using ValidationException = Minibank.Core.Exceptions.ValidationException;

namespace Minibank.Core.Domain.BankAccounts.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IValidator<BankAccountModel> _bankAccountValidator;
        private readonly ICurrencyService _currencyService;
        private readonly IUnitOfWork _unitOfWork;

        public BankAccountService(IUnitOfWork unitOfWork, IBankAccountRepository bankAccountRepository,
            IUserRepository userRepository,
            ITransactionRepository transactionRepository, ICurrencyService currencyService,
            IValidator<BankAccountModel> bankAccountValidator)
        {
            _bankAccountRepository = bankAccountRepository;
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _bankAccountValidator = bankAccountValidator;
            _currencyService = currencyService;
            _unitOfWork = unitOfWork;
        }

        public async Task<BankAccountModel> GetById(Guid id)
        {
            var bankAccount = await _bankAccountRepository.GetById(id);

            if (bankAccount is null)
            {
                throw new ObjectNotFoundException($"BankAccount with id = {id} does not exist");
            }

            return bankAccount;
        }

        public async Task<IEnumerable<BankAccountModel>> GetAll()
        {
            return await _bankAccountRepository.GetAll();
        }

        public async Task<IEnumerable<TransactionModel>> GetAllTransactions()
        {
            return await _transactionRepository.GetAll();
        }

        public async Task<Guid> Create(BankAccountModel bankAccountModel)
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
            await _bankAccountRepository.Create(new BankAccountModel
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

        public async Task<decimal> CalculateCommission(TransactionModel transactionModel)
        {
            var fromAccount = await _bankAccountRepository.GetById(transactionModel.FromAccountId);
            var toAccount = await _bankAccountRepository.GetById(transactionModel.ToAccountId);

            if (fromAccount.Currency != transactionModel.Currency)
            {
                throw new ValidationException(
                    "The final currencies of the transaction and the accounts to which the money is received do not match");
            }

            var transactionMoney = transactionModel.AmountOfMoney;

            return fromAccount.UserId != toAccount.UserId ? transactionMoney * (decimal)0.02 : 0;
        }

        public async Task<Guid> Transfer(TransactionModel transactionModel)
        {
            var fromAccount = await _bankAccountRepository.GetById(transactionModel.FromAccountId);
            var toAccount = await _bankAccountRepository.GetById(transactionModel.ToAccountId);

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

            if (fromAccount.Currency != transactionModel.Currency)
            {
                throw new ValidationException(
                    "The final currencies of the transaction and the accounts to which the money is received do not match");
            }

            await _bankAccountValidator.ValidateAndThrowAsync(fromAccount);
            await _bankAccountValidator.ValidateAndThrowAsync(toAccount);

            var transactionMoney = transactionModel.AmountOfMoney;
            var commission = await CalculateCommission(transactionModel);

            if (fromAccount.AmountOfMoney - transactionMoney - commission < decimal.Zero)
            {
                throw new ValidationException(
                    $"There is not enough money on the fromBankAccount with id = {fromAccount.Id} for this transfer");
            }

            fromAccount.AmountOfMoney -= (transactionMoney + commission);

            await _bankAccountRepository.UpdateAmount(fromAccount.Id, fromAccount.AmountOfMoney);

            if (toAccount.Currency != fromAccount.Currency)
            {
                transactionMoney = _currencyService.Convert(transactionMoney, fromAccount.Currency.ToString(),
                        toAccount.Currency.ToString())
                    .Result;
            }

            toAccount.AmountOfMoney += transactionMoney;
            await _bankAccountRepository.UpdateAmount(toAccount.Id, toAccount.AmountOfMoney);

            var transaction = await _transactionRepository.Create(transactionModel);
            _unitOfWork.SaveChanges();

            return transaction;
        }

        public async Task Close(Guid id)
        {
            var bankAccount = await _bankAccountRepository.GetById(id);

            if (bankAccount is null)
            {
                throw new ObjectNotFoundException($"BankAccount with id = {id} does not exist");
            }

            if (bankAccount.AmountOfMoney != decimal.Zero)
            {
                throw new ValidationException("It is impossible to close an account if there is money left on it");
            }

            await _bankAccountRepository.Close(bankAccount);
            _unitOfWork.SaveChanges();
        }
    }
}
using System;
using System.Collections.Generic;
using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.Currency.Services;
using Minibank.Core.Domains.Transactions;
using Minibank.Core.Domains.Transactions.Repositories;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Exceptions;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrencyService _currencyService;

        public BankAccountService(IBankAccountRepository bankAccountRepository, IUserRepository userRepository,
            ITransactionRepository transactionRepository, ICurrencyService currencyService)
        {
            _bankAccountRepository = bankAccountRepository;
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _currencyService = currencyService;
        }

        public BankAccountModel GetById(Guid id)
        {
            var bankAccount = _bankAccountRepository.GetById(id);

            if (bankAccount is null)
            {
                throw new ObjectNotFoundException("bankAccount with this guid does not exist");
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
                throw new ObjectNotFoundException("User with this guid does not exist");
            }

            if (bankAccountModel.AmountOfMoney <= decimal.Zero)
            {
                throw new ValidationException("The amount of money cannot be negative.");
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
            _userRepository.Update(new UserModel
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email,
            });
            return bankAccountId;
        }

        public decimal CalculateCommission(TransactionModel transactionModel)
        {
            var fromAccount = _bankAccountRepository.GetById(transactionModel.FromAccountId);
            var toAccount = _bankAccountRepository.GetById(transactionModel.ToAccountId);

            if (toAccount.Currency != transactionModel.Currency)
            {
                throw new ValidationException(
                    "The final currencies of the transaction and the accounts to which the money is received do not match");
            }

            var transactionMoney = transactionModel.AmountOfMoney;
            if (toAccount.Currency != fromAccount.Currency)
            {
                transactionMoney = _currencyService.Convert(transactionMoney, fromAccount.Currency.ToString(), toAccount.Currency.ToString())
                    .Result;
            }

            return fromAccount.UserId != toAccount.UserId ? transactionMoney * (decimal)0.02 : 0;
        }

        public Guid Transfer(TransactionModel transactionModel)
        {
            var fromAccount = _bankAccountRepository.GetById(transactionModel.FromAccountId);
            var toAccount = _bankAccountRepository.GetById(transactionModel.ToAccountId);

            if (fromAccount is null)
            {
                throw new ObjectNotFoundException("fromBankAccount with this guid does not exist");
            }
            
            if (toAccount is null)
            {
                throw new ObjectNotFoundException("toBankAccount with this guid does not exist");
            }
            
            if (!fromAccount.IsActive)
            {
                throw new ValidationException("fromAccount is not active");
            }

            if (!toAccount.IsActive)
            {
                throw new ValidationException("toAccount is not active");
            }

            if (toAccount.Currency != transactionModel.Currency)
            {
                throw new ValidationException(
                    "The final currencies of the transaction and the accounts to which the money is received do not match");
            }

            var transactionMoney = transactionModel.AmountOfMoney;
            var commission = CalculateCommission(transactionModel);
            if (toAccount.Currency != fromAccount.Currency)
            {
                transactionMoney = _currencyService.Convert(transactionMoney, fromAccount.Currency.ToString(), toAccount.Currency.ToString())
                    .Result;
            }


            if (fromAccount.AmountOfMoney - transactionMoney - commission < decimal.Zero)
            {
                throw new ValidationException("There is not enough money on the account for this transfer");
            }

            _bankAccountRepository.Update(new BankAccountModel
            {
                Id = fromAccount.Id,
                AmountOfMoney = fromAccount.AmountOfMoney - transactionMoney - commission,
                Currency = fromAccount.Currency,
                OpeningDate = fromAccount.OpeningDate,
                ClosingDate = fromAccount.ClosingDate,
                IsActive = fromAccount.IsActive,
            });
            _bankAccountRepository.Update(new BankAccountModel
            {
                Id = toAccount.Id,
                AmountOfMoney = toAccount.AmountOfMoney + transactionMoney,
                Currency = toAccount.Currency,
                OpeningDate = toAccount.OpeningDate,
                ClosingDate = toAccount.ClosingDate,
                IsActive = toAccount.IsActive,
            });

            return _transactionRepository.Create(transactionModel);
        }

        public void Close(Guid id)
        {
            var bankAccount = _bankAccountRepository.GetById(id);

            if (bankAccount is null)
            {
                throw new ObjectNotFoundException("BankAccount with this guid does not exist");
            }

            if (bankAccount.AmountOfMoney != decimal.Zero)
            {
                throw new ValidationException("It is impossible to close an account if there is money left on it");
            }

            _bankAccountRepository.Close(bankAccount);
        }
    }
}
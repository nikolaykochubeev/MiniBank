using System;
using System.Collections.Generic;
using Minibank.Core.Domains.BankAccount.Repositories;
using Minibank.Core.Domains.Currency.Services;
using Minibank.Core.Domains.Transactions;
using Minibank.Core.Domains.Transactions.Repositories;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Domains.Users.Services;
using Minibank.Core.Exceptions;

namespace Minibank.Core.Domains.BankAccount.Services
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

        public BankAccountModel Get(Guid id)
        {
            return _bankAccountRepository.Get(id);
        }

        public IEnumerable<BankAccountModel> GetAll()
        {
            return _bankAccountRepository.GetAll();
        }

        public Guid Create(BankAccountModel bankAccountModel)
        {
            var user = _userRepository.Get(bankAccountModel.UserId);
            if (user is null)
            {
                throw new ValidationException("User with this guid does not exist");
            }

            if (bankAccountModel.AmountOfMoney <= decimal.Zero)
            {
                throw new ValidationException("The amount of money cannot be negative.");
            }

            if (bankAccountModel.Currency != "RUB" && bankAccountModel.Currency != "USD" &&
                bankAccountModel.Currency != "EUR")
            {
                throw new ValidationException("It is impossible to create a bank account with this currency");
            }

            var bankAccountId = Guid.NewGuid();
            _bankAccountRepository.Create(new BankAccountModel
            {
                Id = bankAccountId,
                UserId = bankAccountModel.UserId,
                AmountOfMoney = bankAccountModel.AmountOfMoney,
                Currency = bankAccountModel.Currency,
                OpeningDate = bankAccountModel.OpeningDate,
                ClosingDate = bankAccountModel.ClosingDate,
            });
            _userRepository.Update(new UserModel
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email,
                AmountOfBankAccounts = user.AmountOfBankAccounts + 1
            });
            return bankAccountId;
        }

        public decimal CalculateCommission(TransactionModel transactionModel)
        {
            var fromAccount = _bankAccountRepository.Get(transactionModel.FromAccountId);
            var toAccount = _bankAccountRepository.Get(transactionModel.ToAccountId);
            if (transactionModel.Currency != "RUB" && transactionModel.Currency != "USD" &&
                transactionModel.Currency != "EUR")
            {
                throw new ValidationException("It is impossible to create a bank account with this currency");
            }

            if (toAccount.Currency != transactionModel.Currency)
            {
                throw new ValidationException(
                    "The final currencies of the transaction and the accounts to which the money is received do not match");
            }

            var transactionMoney = transactionModel.AmountOfMoney;
            if (toAccount.Currency != fromAccount.Currency)
            {
                transactionMoney = _currencyService.Convert(transactionMoney, fromAccount.Currency, toAccount.Currency)
                    .Result;
            }

            return fromAccount.UserId != toAccount.UserId ? transactionMoney * (decimal)0.02 : 0;
        }

        public Guid Transfer(TransactionModel transactionModel)
        {
            var fromAccount = _bankAccountRepository.Get(transactionModel.FromAccountId);
            var toAccount = _bankAccountRepository.Get(transactionModel.ToAccountId);
            if (!fromAccount.IsActive)
            {
                throw new ValidationException("fromAccount is not active");
            }

            if (!toAccount.IsActive)
            {
                throw new ValidationException("toAccount is not active");
            }

            if (transactionModel.Currency != "RUB" && transactionModel.Currency != "USD" &&
                transactionModel.Currency != "EUR")
            {
                throw new ValidationException("It is impossible to create a bank account with this currency");
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
                transactionMoney = _currencyService.Convert(transactionMoney, fromAccount.Currency, toAccount.Currency)
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
            var bankAccount = _bankAccountRepository.Get(id);

            if (bankAccount.AmountOfMoney != decimal.Zero)
            {
                throw new ValidationException("It is impossible to close an account if there is money left on it");
            }

            _bankAccountRepository.Update(new BankAccountModel
            {
                Id = id,
                AmountOfMoney = bankAccount.AmountOfMoney,
                Currency = bankAccount.Currency,
                OpeningDate = bankAccount.OpeningDate,
                ClosingDate = bankAccount.ClosingDate,
                IsActive = false,
            });
        }
    }
}
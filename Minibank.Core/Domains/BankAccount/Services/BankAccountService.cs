using System;
using System.Collections.Generic;
using Minibank.Core.Domains.BankAccount.Repositories;
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

        public BankAccountService(IBankAccountRepository bankAccountRepository, IUserRepository userRepository)
        {
            _bankAccountRepository = bankAccountRepository;
            _userRepository = userRepository;
        }

        public BankAccountModel Get(Guid id)
        {
            return _bankAccountRepository.Get(id);
        }

        public IEnumerable<BankAccountModel> GetAll()
        {
            return _bankAccountRepository.GetAll();
        }

        public void Create(BankAccountModel bankAccountModel)
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

            _bankAccountRepository.Create(bankAccountModel);
            _userRepository.Update(new UserModel
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email,
                AmountOfBankAccounts = user.AmountOfBankAccounts + 1
            });
        }

        public void Update(BankAccountModel bankAccountModel)
        {
            _bankAccountRepository.Update(bankAccountModel);
        }

        public void Close(Guid id)
        {
            _bankAccountRepository.Get(id).IsActive = false;
        }

        public void Delete(Guid id)
        {
            _bankAccountRepository.Delete(id);
        }
    }
}
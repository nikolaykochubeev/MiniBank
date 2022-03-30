using System;
using System.Collections.Generic;
using Minibank.Core.Domain.BankAccounts.Repositories;
using Minibank.Core.Domain.Users.Repositories;
using Minibank.Core.Exceptions;

namespace Minibank.Core.Domain.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBankAccountRepository _bankAccountRepository;

        public UserService(IUserRepository userRepository, IBankAccountRepository bankAccountRepository)
        {
            _userRepository = userRepository;
            _bankAccountRepository = bankAccountRepository;
        }

        public UserModel GetById(Guid id)
        {
            var user = _userRepository.GetById(id);

            if (user is null)
            {
                throw new ObjectNotFoundException($"User with id = {id} does not exist");
            }

            return user;
        }

        public IEnumerable<UserModel> GetAll()
        {
            return _userRepository.GetAll();
        }

        public Guid Create(UserModel userModel)
        {
            if (string.IsNullOrWhiteSpace(userModel.Email))
            {
                throw new ValidationException("Email can not be the empty string.");
            }

            if (string.IsNullOrWhiteSpace(userModel.Login))
            {
                throw new ValidationException("Login can not be the empty string.");
            }

            return _userRepository.Create(userModel);
        }

        public void Update(UserModel userModel)
        {
            _userRepository.Update(userModel);
        }

        public void Delete(Guid id)
        {
            var user = _userRepository.GetById(id);

            if (user is null)
            {
                throw new ObjectNotFoundException($"User with id = {id} doesnt exists");
            }

            if (_bankAccountRepository.Any(id))
            {
                throw new ValidationException($"User with id = {id} have an active bank accounts");
            }

            _userRepository.Delete(id);
        }
    }
}
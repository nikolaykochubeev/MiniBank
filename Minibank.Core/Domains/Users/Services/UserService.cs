using System;
using System.Collections.Generic;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Exceptions;

namespace Minibank.Core.Domains.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserModel Get(Guid id)
        {
            return _userRepository.Get(id);
        }

        public IEnumerable<UserModel> GetAll()
        {
            return _userRepository.GetAll();
        }

        public void Create(UserModel userModel)
        {
            if (string.IsNullOrEmpty(userModel.Email))
                throw new ValidationException("Email can not be the empty string.");
            if (string.IsNullOrEmpty(userModel.Login))
            {
                throw new ValidationException("Login can not be the empty string.");
            }
            _userRepository.Create(userModel);
        }

        public void Update(UserModel userModel)
        {
            _userRepository.Update(userModel);
        }

        public void Delete(Guid id)
        {
            var user = _userRepository.Get(id);
            if (user.AmountOfBankAccounts != 0)
            {
                throw new ValidationException("User have a bankaccounts");
            }
            _userRepository.Delete(id);
        }
    }
}
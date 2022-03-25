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

        public UserModel GetById(Guid id)
        {
            var user = _userRepository.GetById(id);

            if (user is null)
            {
                throw new ObjectNotFoundException("user with this guid does not exist");
            }

            return user;
        }

        public IEnumerable<UserModel> GetAll()
        {
            return _userRepository.GetAll();
        }

        public Guid Create(UserModel userModel)
        {
            if (string.IsNullOrEmpty(userModel.Email))
            {
                throw new ValidationException("Email can not be the empty string.");
            }

            if (string.IsNullOrEmpty(userModel.Login))
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
                throw new ObjectNotFoundException("User with this guid doesnt exists");
            }

            if (user.AmountOfBankAccounts != 0)
            {
                throw new ValidationException("User have an active bank accounts");
            }

            _userRepository.Delete(id);
        }
    }
}
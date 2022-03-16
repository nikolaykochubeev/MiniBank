using System;
using System.Collections.Generic;
using Minibank.Core.Domains.Services;
using Minibank.Core.Domains.Users.Repositories;

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

        public void Create(UserModel userModelModel)
        {
            _userRepository.Create(userModelModel);
        }

        public void Update(UserModel userModel)
        {
            _userRepository.Update(userModel);
        }

        public void Delete(Guid id)
        {
            _userRepository.Delete(id);
        }
    }
}
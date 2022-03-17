using System;
using System.Collections.Generic;
using System.Linq;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Data.DbModels;

namespace Minibank.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static List<UserDbModel> _userStorage = new();

        public UserModel Get(Guid id)
        {
            var entity = _userStorage.FirstOrDefault(it => it.Id == id);

            if (entity == null)
            {
                return null;
            }

            return new UserModel
            {
                Id = entity.Id,
                Login = entity.Login,
                Email = entity.Email,
            };
        }

        public IEnumerable<UserModel> GetAll()
        {
            return _userStorage.Select(it => new UserModel()
            {
                Id = it.Id,
                Login = it.Login,
                Email = it.Email,
            });
        }

        public void Create(UserModel userModel)
        {
            var entity = new UserDbModel
            {
                Id = Guid.NewGuid(),
                Email = userModel.Email,
                Login = userModel.Login
            };

            _userStorage.Add(entity);
        }

        public void Update(UserModel userModel)
        {
            var entity = _userStorage.FirstOrDefault(it => it.Id == userModel.Id);

            if (entity == null)
                return;
            entity.Email = userModel.Email;
            entity.Login = userModel.Login;
        }

        public void Delete(Guid id)
        {
            var entity = _userStorage.FirstOrDefault(it => it.Id == id);

            if (entity != null)
            {
                _userStorage.Remove(entity);
            }
        }
    }
}
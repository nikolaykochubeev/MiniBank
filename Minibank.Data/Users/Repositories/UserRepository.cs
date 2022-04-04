using System;
using System.Collections.Generic;
using System.Linq;
using Minibank.Core.Domain.Users;
using Minibank.Core.Domain.Users.Repositories;
using Minibank.Core.Exceptions;

namespace Minibank.Data.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MiniBankContext _context;

        public UserRepository(MiniBankContext context)
        {
            _context = context;
        }
        public UserModel GetById(Guid id)
        {
            var entity = _context.Users.FirstOrDefault(it => it.Id == id);

            if (entity is null)
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
            return _context.Users.Select(it => new UserModel()
            {
                Id = it.Id,
                Login = it.Login,
                Email = it.Email,
            });
        }

        public Guid Create(UserModel userModel)
        {
            var entity = new UserDbModel
            {
                Id = Guid.NewGuid(),
                Email = userModel.Email,
                Login = userModel.Login
            };

            _context.Users.Add(entity);
            return entity.Id;
        }

        public void Update(UserModel userModel)
        {
            var entity = _context.Users.FirstOrDefault(it => it.Id == userModel.Id);

            if (entity is null)
            {
                throw new ValidationException($"User with id = {userModel.Id} doesn't exists");
            }

            entity.Email = userModel.Email;
            entity.Login = userModel.Login;
        }

        public void Delete(Guid id)
        {
            var entity = _context.Users.FirstOrDefault(it => it.Id == id);

            if (entity is null)
            {
                throw new ValidationException($"User with id = {id} doesn't exists");
            }

            _context.Users.Remove(entity);
        }
    }
}
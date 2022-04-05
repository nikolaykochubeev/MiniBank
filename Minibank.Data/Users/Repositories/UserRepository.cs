using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<UserModel> GetById(Guid id)
        {
            var entity = await _context.Users.AsNoTracking().FirstOrDefaultAsync(it => it.Id == id);

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

        public async Task<IEnumerable<UserModel>> GetAll()
        {
            var users = await _context.Users.AsNoTracking().ToListAsync();

            return users.Select(it => new UserModel()
            {
                Id = it.Id,
                Login = it.Login,
                Email = it.Email,
            });
        }

        public async Task<Guid> Create(UserModel userModel)
        {
            var entity = new UserDbModel
            {
                Id = Guid.NewGuid(),
                Email = userModel.Email,
                Login = userModel.Login
            };

            await _context.Users.AddAsync(entity);
            return entity.Id;
        }

        public async Task Update(UserModel userModel)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(it => it.Id == userModel.Id);

            if (entity is null)
            {
                throw new ValidationException($"User with id = {userModel.Id} doesn't exists");
            }

            entity.Email = userModel.Email;
            entity.Login = userModel.Login;
        }

        public async Task Delete(Guid id)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(it => it.Id == id);

            if (entity is null)
            {
                throw new ValidationException($"User with id = {id} doesn't exists");
            }

            _context.Users.Remove(entity);
        }
    }
}
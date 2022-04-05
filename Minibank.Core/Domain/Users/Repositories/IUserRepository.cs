using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domain.Users.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel> GetById(Guid id);
        Task<IEnumerable<UserModel>> GetAll();
        Task<Guid> Create(UserModel userModel);
        Task Update(UserModel userModel);
        Task Delete(Guid id);
    }
}
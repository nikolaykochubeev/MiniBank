using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domain.Users.Services
{
    public interface IUserService
    {
        Task<UserModel> GetById(Guid id);
        Task<IEnumerable<UserModel>> GetAll();
        Task<Guid> Create(UserModel userModel);
        Task Update(UserModel user);
        Task Delete(Guid id);
    }
}
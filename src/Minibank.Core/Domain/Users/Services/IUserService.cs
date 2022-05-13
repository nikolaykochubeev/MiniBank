using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domain.Users.Services
{
    public interface IUserService
    {
        Task<UserModel> GetByIdAsync(Guid id);
        Task<IEnumerable<UserModel>> GetAllAsync();
        Task<Guid> CreateAsync(UserModel userModel);
        Task UpdateAsync(UserModel user);
        Task DeleteAsync(Guid id);
    }
}
using System;
using System.Collections.Generic;

namespace Minibank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        UserModel Get(Guid id);
        IEnumerable<UserModel> GetAll();
        void Create(UserModel userModel);
        void Update(UserModel user);
        void Delete(Guid id);
    }
}
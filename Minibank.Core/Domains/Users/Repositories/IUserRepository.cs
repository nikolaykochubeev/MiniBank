using System;
using System.Collections.Generic;

namespace Minibank.Core.Domains.Users.Repositories
{
    public interface IUserRepository
    {
        UserModel Get(Guid id);
        IEnumerable<UserModel> GetAll();
        Guid Create(UserModel userModel);
        void Update(UserModel userModel);
        void Delete(Guid id);
    }
}
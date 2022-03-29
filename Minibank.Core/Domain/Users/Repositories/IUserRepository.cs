using System;
using System.Collections.Generic;

namespace Minibank.Core.Domain.Users.Repositories
{
    public interface IUserRepository
    {
        UserModel GetById(Guid id);
        IEnumerable<UserModel> GetAll();
        Guid Create(UserModel userModel);
        void Update(UserModel userModel);
        void Delete(Guid id);
    }
}
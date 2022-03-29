using System;
using System.Collections.Generic;

namespace Minibank.Core.Domain.Users.Services
{
    public interface IUserService
    {
        UserModel GetById(Guid id);
        IEnumerable<UserModel> GetAll();
        Guid Create(UserModel userModel);
        void Update(UserModel user);
        void Delete(Guid id);
    }
}
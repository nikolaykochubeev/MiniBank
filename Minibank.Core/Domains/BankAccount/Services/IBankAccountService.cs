using System;
using System.Collections.Generic;

namespace Minibank.Core.Domains.BankAccount.Services
{
    public interface IBankAccountService
    {
        BankAccountModel Get(Guid id);
        IEnumerable<BankAccountModel> GetAll();
        void Create(BankAccountModel bankAccountModel);
        void Update(BankAccountModel bankAccountModel);
        void Close(Guid id);
        void Delete(Guid id);
    }
}
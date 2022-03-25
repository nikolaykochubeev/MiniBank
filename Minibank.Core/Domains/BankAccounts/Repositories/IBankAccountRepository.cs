using System;
using System.Collections.Generic;

namespace Minibank.Core.Domains.BankAccounts.Repositories
{
    public interface IBankAccountRepository
    {
        BankAccountModel GetById(Guid id);
        IEnumerable<BankAccountModel> GetAll();
        void Create(BankAccountModel bankAccountModel);
        void Update(BankAccountModel bankAccountModel);
    }
}
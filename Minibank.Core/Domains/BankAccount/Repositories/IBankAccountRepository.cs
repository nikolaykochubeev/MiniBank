using System;
using System.Collections.Generic;

namespace Minibank.Core.Domains.BankAccount.Repositories
{
    public interface IBankAccountRepository
    {
        BankAccountModel Get(Guid id);
        IEnumerable<BankAccountModel> GetAll();
        void Create(BankAccountModel bankAccountModel);
        void Update(BankAccountModel bankAccountModel);
    }
}
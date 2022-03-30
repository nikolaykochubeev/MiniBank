using System;
using System.Collections.Generic;

namespace Minibank.Core.Domain.BankAccounts.Repositories
{
    public interface IBankAccountRepository
    {
        BankAccountModel GetById(Guid id);
        IEnumerable<BankAccountModel> GetAll();
        void Create(BankAccountModel bankAccountModel);
        void Update(BankAccountModel bankAccountModel);
        void Close(BankAccountModel bankAccountModel);
        void UpdateAmount(Guid id, decimal amount);
        bool Any(Guid id);
    }
}
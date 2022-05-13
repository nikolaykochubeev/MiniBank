using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domain.BankAccounts.Repositories
{
    public interface IBankAccountRepository
    {
        Task<BankAccountModel> GetById(Guid id);
        Task<IEnumerable<BankAccountModel>> GetAll();
        Task<Guid> Create(BankAccountModel bankAccountModel);
        Task Update(BankAccountModel bankAccountModel);
        Task Close(Guid id);
        Task UpdateAmount(Guid id, decimal amount);
        Task<bool> Any(Guid id);
    }
}
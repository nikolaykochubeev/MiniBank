using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domain.Transactions.Repositories
{
    public interface ITransactionRepository
    {
        Task<TransactionModel> GetById(Guid id);
        Task<IEnumerable<TransactionModel>> GetAll();
        Task<Guid> Create(TransactionModel transactionModel);
        Task Update(TransactionModel transactionModel);
        Task Delete(Guid id);
    }
}
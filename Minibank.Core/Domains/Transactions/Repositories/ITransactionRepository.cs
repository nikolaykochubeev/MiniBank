using System;
using System.Collections.Generic;

namespace Minibank.Core.Domains.Transactions.Repositories
{
    public interface ITransactionRepository
    {
        TransactionModel Get(Guid id);
        IEnumerable<TransactionModel> GetAll();
        void Create(TransactionModel transactionModel);
        void Update(TransactionModel transactionModel);
        void Delete(Guid id);
    }
}
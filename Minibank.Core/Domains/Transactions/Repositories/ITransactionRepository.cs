using System;
using System.Collections.Generic;

namespace Minibank.Core.Domains.Transactions.Repositories
{
    public interface ITransactionRepository
    {
        TransactionModel GetById(Guid id);
        IEnumerable<TransactionModel> GetAll();
        Guid Create(TransactionModel transactionModel);
        void Update(TransactionModel transactionModel);
        void Delete(Guid id);
    }
}
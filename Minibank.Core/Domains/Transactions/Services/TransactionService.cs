using System;
using System.Collections.Generic;
using Minibank.Core.Domains.Transactions.Repositories;

namespace Minibank.Core.Domains.Transactions.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public TransactionModel Get(Guid id)
        {
            return _transactionRepository.Get(id);
        }

        public IEnumerable<TransactionModel> GetAll()
        {
            return _transactionRepository.GetAll();
        }

        public void Create(TransactionModel transactionModel)
        {
            _transactionRepository.Create(transactionModel);
        }

        public void Update(TransactionModel transactionModel)
        {
            _transactionRepository.Update(transactionModel);
        }

        public void Delete(Guid id)
        {
            _transactionRepository.Delete(id);
        }
    }
}
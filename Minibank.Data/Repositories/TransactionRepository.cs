using System;
using System.Collections.Generic;
using System.Linq;
using Minibank.Core.Domains.Transactions;
using Minibank.Core.Domains.Transactions.Repositories;
using Minibank.Data.DbModels;

namespace Minibank.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private static List<TransactionDbModel> _transactionModelStorage = new();
        public TransactionModel Get(Guid id)
        {
            var entity = _transactionModelStorage.FirstOrDefault(it => it.Id == id);

            if (entity == null)
            {
                return null;
            }

            return new TransactionModel
            {
                Id = id,
                AmountOfMoney = entity.AmountOfMoney,
                Currency = entity.Currency,
                FromAccountId = entity.FromAccountId,
                ToAccountId = entity.ToAccountId
            };
        }

        public IEnumerable<TransactionModel> GetAll()
        {
            return _transactionModelStorage.Select(entity => new TransactionModel()
            {
                Id = entity.Id,
                AmountOfMoney = entity.AmountOfMoney,
                Currency = entity.Currency,
                FromAccountId = entity.FromAccountId,
                ToAccountId = entity.ToAccountId
            });     
        }

        public void Create(TransactionModel transactionModel)
        {
            var entity = new TransactionDbModel()
            {
                Id = transactionModel.Id,
                AmountOfMoney = transactionModel.AmountOfMoney,
                Currency = transactionModel.Currency,
                FromAccountId = transactionModel.FromAccountId,
                ToAccountId = transactionModel.ToAccountId
            };
            _transactionModelStorage.Add(entity);
        }

        public void Update(TransactionModel transactionModel)
        {
            var entity = _transactionModelStorage.FirstOrDefault(it => it.Id == transactionModel.Id);

            if (entity == null)
                return;
            entity.AmountOfMoney = transactionModel.AmountOfMoney;
            entity.Currency = transactionModel.Currency;
            entity.FromAccountId = transactionModel.FromAccountId;
            entity.ToAccountId = transactionModel.ToAccountId;
        }

        public void Delete(Guid id)
        {
            var entity = _transactionModelStorage.FirstOrDefault(it => it.Id == id);

            if (entity != null)
            {
                _transactionModelStorage.Remove(entity);
            }
        }
    }
}
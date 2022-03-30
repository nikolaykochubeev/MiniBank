﻿using System;
using System.Collections.Generic;
using System.Linq;
using Minibank.Core.Domain.Transactions;
using Minibank.Core.Domain.Transactions.Repositories;
using Minibank.Core.Exceptions;

namespace Minibank.Data.Transactions.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private static List<TransactionDbModel> _transactionModelStorage = new();

        public TransactionModel GetById(Guid id)
        {
            var entity = _transactionModelStorage.FirstOrDefault(it => it.Id == id);

            if (entity is null)
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

        public Guid Create(TransactionModel transactionModel)
        {
            var entity = new TransactionDbModel()
            {
                Id = Guid.NewGuid(),
                AmountOfMoney = transactionModel.AmountOfMoney,
                Currency = transactionModel.Currency,
                FromAccountId = transactionModel.FromAccountId,
                ToAccountId = transactionModel.ToAccountId
            };

            _transactionModelStorage.Add(entity);

            return entity.Id;
        }

        public void Update(TransactionModel transactionModel)
        {
            var entity = _transactionModelStorage.FirstOrDefault(it => it.Id == transactionModel.Id);

            if (entity is null)
            {
                throw new ValidationException($"Transfer with id = {transactionModel.Id} doesn't exists");
            }

            entity.AmountOfMoney = transactionModel.AmountOfMoney;
            entity.Currency = transactionModel.Currency;
            entity.FromAccountId = transactionModel.FromAccountId;
            entity.ToAccountId = transactionModel.ToAccountId;
        }

        public void Delete(Guid id)
        {
            var entity = _transactionModelStorage.FirstOrDefault(it => it.Id == id);

            if (entity is not null)
            {
                _transactionModelStorage.Remove(entity);
            }
        }
    }
}
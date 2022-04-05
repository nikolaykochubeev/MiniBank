using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minibank.Core.Domain.Transactions;
using Minibank.Core.Domain.Transactions.Repositories;
using Minibank.Core.Exceptions;

namespace Minibank.Data.Transactions.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly MiniBankContext _context;

        public TransactionRepository(MiniBankContext miniBankContext)
        {
            _context = miniBankContext;
        }

        public async Task<TransactionModel> GetById(Guid id)
        {
            var entity = await _context.Transactions.FirstOrDefaultAsync(it => it.Id == id);

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

        public async Task<IEnumerable<TransactionModel>> GetAll()
        {
            var transactions = await _context.Transactions.ToListAsync();

            return transactions.Select(entity => new TransactionModel()
            {
                Id = entity.Id,
                AmountOfMoney = entity.AmountOfMoney,
                Currency = entity.Currency,
                FromAccountId = entity.FromAccountId,
                ToAccountId = entity.ToAccountId
            });
        }

        public async Task<Guid> Create(TransactionModel transactionModel)
        {
            var entity = new TransactionDbModel()
            {
                Id = Guid.NewGuid(),
                AmountOfMoney = transactionModel.AmountOfMoney,
                Currency = transactionModel.Currency,
                FromAccountId = transactionModel.FromAccountId,
                ToAccountId = transactionModel.ToAccountId
            };
            await _context.Transactions.AddAsync(entity);

            return entity.Id;
        }

        public async Task Update(TransactionModel transactionModel)
        {
            var entity = await _context.Transactions.FirstOrDefaultAsync(it => it.Id == transactionModel.Id);

            if (entity is null)
            {
                throw new ValidationException($"Transfer with id = {transactionModel.Id} doesn't exists");
            }

            entity.AmountOfMoney = transactionModel.AmountOfMoney;
            entity.Currency = transactionModel.Currency;
            entity.FromAccountId = transactionModel.FromAccountId;
            entity.ToAccountId = transactionModel.ToAccountId;
        }

        public async Task Delete(Guid id)
        {
            var entity = await _context.Transactions.FirstOrDefaultAsync(it => it.Id == id);

            if (entity is not null)
            {
                _context.Transactions.Remove(entity);
            }
        }
    }
}
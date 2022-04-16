using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minibank.Core.Domain.BankAccounts;
using Minibank.Core.Domain.BankAccounts.Repositories;
using Minibank.Core.Exceptions;

namespace Minibank.Data.BankAccounts.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly MiniBankContext _context;

        public BankAccountRepository(MiniBankContext context)
        {
            _context = context;
        }

        public async Task<BankAccountModel> GetById(Guid id)
        {
            var entity = await _context.BankAccounts.FirstOrDefaultAsync(it => it.Id == id);

            if (entity is null)
            {
                return null;
            }

            return new BankAccountModel
            {
                Id = entity.Id,
                UserId = entity.UserId,
                AmountOfMoney = entity.AmountOfMoney,
                Currency = entity.Currency,
                IsActive = entity.IsActive,
                OpeningDate = entity.OpeningDate,
                ClosingDate = entity.ClosingDate,
            };
        }

        public async Task<IEnumerable<BankAccountModel>> GetAll()
        {
            var bankAccounts = await _context.BankAccounts.ToListAsync();

            return bankAccounts.Select(entity => new BankAccountModel()
            {
                Id = entity.Id,
                UserId = entity.UserId,
                AmountOfMoney = entity.AmountOfMoney,
                Currency = entity.Currency,
                IsActive = entity.IsActive,
                OpeningDate = entity.OpeningDate,
                ClosingDate = entity.ClosingDate,
            });
        }

        public async Task<Guid> Create(BankAccountModel bankAccountModel)
        {
            var entity = new BankAccountDbModel()
            {
                Id = Guid.NewGuid(),
                UserId = bankAccountModel.UserId,
                AmountOfMoney = bankAccountModel.AmountOfMoney,
                Currency = bankAccountModel.Currency,
                IsActive = true,
                OpeningDate = DateTime.Now,
                ClosingDate = DateTime.Now.AddYears(4),
            };

            await _context.BankAccounts.AddAsync(entity);
            return entity.Id;
        }

        public async Task Update(BankAccountModel bankAccountModel)
        {
            var entity = await _context.BankAccounts.FirstOrDefaultAsync(it => it.Id == bankAccountModel.Id);

            if (entity is null)
            {
                throw new ValidationException($"BankAccount with id = {bankAccountModel.Id} doesn't exists");
            }

            entity.UserId = bankAccountModel.UserId;
            entity.Currency = bankAccountModel.Currency;
            entity.AmountOfMoney = bankAccountModel.AmountOfMoney;
            entity.ClosingDate = bankAccountModel.ClosingDate;
            entity.OpeningDate = bankAccountModel.OpeningDate;
            entity.IsActive = bankAccountModel.IsActive;
        }

        public async Task UpdateAmount(Guid id, decimal amount)
        {
            var entity = await _context.BankAccounts.FirstOrDefaultAsync(it => it.Id == id);
            if (entity == null)
            {
                throw new ObjectNotFoundException($"BankAccount id = {id} does not exists");
            }

            entity.AmountOfMoney = amount;
        }

        public async Task<bool> Any(Guid id)
        {
            return await _context.BankAccounts.AnyAsync(model => model.UserId == id);
        }

        public async Task Close(Guid id)
        {
            var entity = await _context.BankAccounts.FirstOrDefaultAsync(it => it.Id == id);

            if (entity is null)
            {
                throw new ValidationException($"BankAccount with id = {id} doesn't exists");
            }

            entity.ClosingDate = DateTime.Now;
            entity.IsActive = false;
        }
    }
}
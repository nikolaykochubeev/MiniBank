using System;
using System.Collections.Generic;
using System.Linq;
using Minibank.Core.Domains.BankAccount;
using Minibank.Core.Domains.BankAccount.Repositories;
using Minibank.Core.Services;
using Minibank.Data.DbModels;

namespace Minibank.Data.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private static List<BankAccountDbModel> _bankAccountStorage = new();
        public BankAccountModel Get(Guid id)
        {
            var entity = _bankAccountStorage.FirstOrDefault(it => it.Id == id);

            if (entity == null)
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

        public IEnumerable<BankAccountModel> GetAll()
        {
            return _bankAccountStorage.Select(entity => new BankAccountModel()
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

        public void Create(BankAccountModel bankAccountModel)
        {
            var entity = new BankAccountDbModel()
            {
                Id = Guid.NewGuid(),
                UserId = bankAccountModel.UserId,
                AmountOfMoney = bankAccountModel.AmountOfMoney,
                Currency = bankAccountModel.Currency,
                IsActive = bankAccountModel.IsActive,
            };
            _bankAccountStorage.Add(entity);
        }

        public void Update(BankAccountModel bankAccountModel)
        {
            var entity = _bankAccountStorage.FirstOrDefault(it => it.Id == bankAccountModel.Id);

            if (entity == null)
                return;
            entity.UserId = bankAccountModel.UserId;
            entity.Currency = bankAccountModel.Currency;
            entity.AmountOfMoney = bankAccountModel.AmountOfMoney;
            entity.ClosingDate = bankAccountModel.ClosingDate;
            entity.OpeningDate = bankAccountModel.OpeningDate;
            entity.IsActive = bankAccountModel.IsActive;
        }

        public void Delete(Guid id)
        {
            var entity = _bankAccountStorage.FirstOrDefault(it => it.Id == id);

            if (entity != null)
            {
                _bankAccountStorage.Remove(entity);
            }
        }
    }
}
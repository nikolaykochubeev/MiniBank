﻿using System;
using System.Collections.Generic;
using System.Linq;
using Minibank.Core.Domain.BankAccounts;
using Minibank.Core.Domain.BankAccounts.Repositories;
using Minibank.Core.Exceptions;

namespace Minibank.Data.BankAccounts.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private static List<BankAccountDbModel> _bankAccountStorage = new();

        public BankAccountModel GetById(Guid id)
        {
            var entity = _bankAccountStorage.FirstOrDefault(it => it.Id == id);

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
                Id = bankAccountModel.Id,
                UserId = bankAccountModel.UserId,
                AmountOfMoney = bankAccountModel.AmountOfMoney,
                Currency = bankAccountModel.Currency,
                IsActive = bankAccountModel.IsActive,
                OpeningDate = bankAccountModel.OpeningDate,
                ClosingDate = bankAccountModel.ClosingDate
            };

            _bankAccountStorage.Add(entity);
        }

        public void Update(BankAccountModel bankAccountModel)
        {
            var entity = _bankAccountStorage.FirstOrDefault(it => it.Id == bankAccountModel.Id);

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

        public void UpdateAmount(Guid id, decimal amount)
        {
            var entity = _bankAccountStorage.FirstOrDefault(it => it.Id == id);
            if (entity == null)
            {
                throw new ObjectNotFoundException($"BankAccount id = {id} does not exists");
            }

            entity.AmountOfMoney = amount;
        }

        public bool Any(Guid id)
        {
            return _bankAccountStorage.Any(model => model.UserId == id);
        }

        public void Close(BankAccountModel bankAccountModel)
        {
            var entity = _bankAccountStorage.FirstOrDefault(it => it.Id == bankAccountModel.Id);

            if (entity is null)
            {
                throw new ValidationException($"BankAccount with id = {bankAccountModel.Id} doesn't exists");
            }

            entity.ClosingDate = DateTime.Now;
            entity.IsActive = false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.Transactions;
using Minibank.Core.Domains.Transactions.Services;
using Minibank.Web.Dto;

namespace Minibank.Web.Controllers
{
    [ApiController]
    [Route("api/v1/minibank/[controller]/[action]")]
    public class TransactionController
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("{id}")]
        public TransactionModel Get(Guid id)
        {
            var model = _transactionService.Get(id);

            return new TransactionModel
            {
                Id = model.Id,
                AmountOfMoney = model.AmountOfMoney,
                Currency = model.Currency,
                FromAccountId = model.FromAccountId,
                ToAccountId = model.ToAccountId
            };
        }
        
        [HttpGet]
        public IEnumerable<TransactionModel> GetAll()
        {
            return _transactionService.GetAll()
                .Select(model => new TransactionModel
                {
                    Id = model.Id,
                    AmountOfMoney = model.AmountOfMoney,
                    Currency = model.Currency,
                    FromAccountId = model.FromAccountId,
                    ToAccountId = model.ToAccountId
                });
        }
        
        [HttpPost]
        public void Create(TransactionDto model)
        {
            _transactionService.Create(new TransactionModel
            {
                AmountOfMoney = model.AmountOfMoney,
                Currency = model.Currency,
                FromAccountId = model.FromAccountId,
                ToAccountId = model.ToAccountId
            });
        }
        
        [HttpPut("{id}")]
        public void Update(Guid id, TransactionDto model)
        {
            _transactionService.Update(new TransactionModel
            {
                Id = id,
                AmountOfMoney = model.AmountOfMoney,
                Currency = model.Currency,
                FromAccountId = model.FromAccountId,
                ToAccountId = model.ToAccountId
            });
        }
        
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _transactionService.Delete(id);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.BankAccount;
using Minibank.Core.Domains.BankAccount.Services;
using Minibank.Core.Domains.Transactions;
using Minibank.Web.Dto;

namespace Minibank.Web.Controllers
{
    [ApiController]
    [Route("api/v1/minibank/[controller]/[action]")]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        [HttpGet]
        public Task<BankAccountModel> Get(Guid id)
        {
            return Task.FromResult(_bankAccountService.Get(id));
        }
        
        [HttpGet]
        public IEnumerable<BankAccountModel> GetAll()
        {
            return _bankAccountService.GetAll();
        }
        
        [HttpPost]
        public Guid Create(BankAccountDto bankAccountDto)
        {
            return _bankAccountService.Create(new BankAccountModel()
            {
                UserId = bankAccountDto.UserId,
                Currency = bankAccountDto.Currency,
                AmountOfMoney = bankAccountDto.AmountOfMoney,
                IsActive = true
            });
        }

        [HttpGet]
        public decimal GetTransferCommission(TransactionDto transactionModel)
        {
            return _bankAccountService.CalculateCommission(new TransactionModel
            {
                Currency = transactionModel.Currency,
                AmountOfMoney = transactionModel.AmountOfMoney,
                FromAccountId = transactionModel.FromAccountId,
                ToAccountId = transactionModel.ToAccountId
            });
        }

        [HttpPost]
        public Guid CreateTransfer(TransactionDto transactionModel)
        {
            return _bankAccountService.Transfer(new TransactionModel
            {
                Currency = transactionModel.Currency,
                AmountOfMoney = transactionModel.AmountOfMoney,
                FromAccountId = transactionModel.FromAccountId,
                ToAccountId = transactionModel.ToAccountId
            });
        }
        
        [HttpPut("{id}")]
        public void Close(Guid id)
        {
            _bankAccountService.Close(id);
        }
    }
}
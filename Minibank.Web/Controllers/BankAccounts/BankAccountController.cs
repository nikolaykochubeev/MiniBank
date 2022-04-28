using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domain.BankAccounts;
using Minibank.Core.Domain.BankAccounts.Services;
using Minibank.Core.Domain.Transactions;
using Minibank.Web.Controllers.BankAccounts.Dto;
using Minibank.Web.Controllers.Transactions.Dto;

namespace Minibank.Web.Controllers.BankAccounts
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

        [HttpGet("{id}")]
        public async Task<BankAccountModel> GetById(Guid id)
        {
            return await _bankAccountService.GetByIdAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<BankAccountModel>> GetAll()
        {
            return await _bankAccountService.GetAllAsync();
        }

        [HttpGet]
        public async Task<IEnumerable<TransactionModel>> GetAllTransactions()
        {
            return await _bankAccountService.GetAllTransactionsAsync();
        }

        [HttpGet]
        public async Task<decimal> GetTransferCommission(TransactionDto transactionModel)
        {
            return await _bankAccountService.CalculateCommissionAsync(new TransactionModel
            {
                Currency = transactionModel.Currency,
                AmountOfMoney = transactionModel.AmountOfMoney,
                FromAccountId = transactionModel.FromAccountId,
                ToAccountId = transactionModel.ToAccountId
            });
        }

        [HttpPost]
        public async Task<Guid> Create(BankAccountDto bankAccountDto)
        {
            return await _bankAccountService.CreateAsync(new BankAccountModel()
            {
                UserId = bankAccountDto.UserId,
                Currency = bankAccountDto.Currency,
                AmountOfMoney = bankAccountDto.AmountOfMoney,
                IsActive = true
            });
        }

        [HttpPost]
        public async Task<Guid> CreateTransfer(TransactionDto transactionModel)
        {
            return await _bankAccountService.TransferAsync(new TransactionModel
            {
                Currency = transactionModel.Currency,
                AmountOfMoney = transactionModel.AmountOfMoney,
                FromAccountId = transactionModel.FromAccountId,
                ToAccountId = transactionModel.ToAccountId
            });
        }

        [HttpPost("{id}")]
        public async Task CloseById(Guid id)
        {
            await _bankAccountService.CloseAsync(id);
        }
    }
}
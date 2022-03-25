﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.BankAccounts;
using Minibank.Core.Domains.BankAccounts.Services;
using Minibank.Core.Domains.Transactions;
using Minibank.Web.Controllers.BankAccounts.Dto;

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

        [HttpGet("{id}")]
        public Task<BankAccountModel> GetById(Guid id)
        {
            return Task.FromResult(_bankAccountService.GetById(id));
        }

        [HttpGet]
        public IEnumerable<BankAccountModel> GetAll()
        {
            return _bankAccountService.GetAll();
        }

        [HttpGet]
        public IEnumerable<TransactionModel> GetAllTransactions()
        {
            return _bankAccountService.GetAllTransactions();
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

        [HttpPost("{id}")]
        public void CloseById(Guid id)
        {
            _bankAccountService.Close(id);
        }
    }
}
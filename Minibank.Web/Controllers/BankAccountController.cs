using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.BankAccount;
using Minibank.Core.Domains.BankAccount.Services;
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
        public async Task<BankAccountModel> Get(Guid id)
        {
            return _bankAccountService.Get(id);
        }

        [HttpGet]
        public IEnumerable<BankAccountModel> GetAll()
        {
            return _bankAccountService.GetAll();
        }
        [HttpPost]
        public void Create(BankAccountDto bankAccountDto)
        {
            _bankAccountService.Create(new BankAccountModel()
            {
                UserId = bankAccountDto.UserId,
                AmountOfMoney = bankAccountDto.AmountOfMoney,
                Currency = bankAccountDto.Currency,
                OpeningDate = bankAccountDto.OpeningDate,
                ClosingDate = bankAccountDto.ClosingDate,
                IsActive = bankAccountDto.IsActive
            });
        }
        
        [HttpPut("id")]
        public void Update(Guid id, BankAccountDto model)
        {
            _bankAccountService.Update(new BankAccountModel
            {
                Id = id,
                UserId = model.UserId,
                Currency = model.Currency,
                AmountOfMoney = model.AmountOfMoney,
                ClosingDate = model.ClosingDate,
                OpeningDate = model.OpeningDate,
                IsActive = model.IsActive,
            });
        }
        
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _bankAccountService.Delete(id);
        }
    }
}
using System;

namespace Minibank.Web.Controllers.BankAccounts.Dto
{
    public class BankAccountDto
    {
        public Guid UserId { get; set; }
        public decimal AmountOfMoney { get; set; }
        public string Currency { get; set; }
    }
}
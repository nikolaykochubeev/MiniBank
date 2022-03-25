using System;

namespace Minibank.Web.Controllers.BankAccounts.Dto
{
    public class TransactionDto
    {
        public decimal AmountOfMoney { get; set; }
        public string Currency { get; set; }
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
    }
}
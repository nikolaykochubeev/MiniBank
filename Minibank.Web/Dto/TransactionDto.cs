using System;

namespace Minibank.Web.Dto
{
    public class TransactionDto
    {
        public decimal AmountOfMoney { get; set; }
        public string Currency { get; set; }
        public Guid FromAccountId;
        public Guid ToAccountId;
    }
}
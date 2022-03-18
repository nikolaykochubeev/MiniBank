using System;

namespace Minibank.Web.Dto
{
    public class BankAccountDto
    {
        public Guid UserId { get; set; }
        public decimal AmountOfMoney { get; set; }
        public string Currency { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime ClosingDate { get; set; }
    }
}
using System;

namespace Minibank.Core.Domains.BankAccount
{
    public class BankAccount
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal AmountOfMoney { get; set; }
        public string Currency { get; set; }
        public string IsActive { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime ClosingDate { get; set; }
    }
}
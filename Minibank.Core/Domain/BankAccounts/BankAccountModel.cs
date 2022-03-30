using System;
using Minibank.Core.Domain.Currency;

namespace Minibank.Core.Domain.BankAccounts
{
    public class BankAccountModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal AmountOfMoney { get; set; }
        public CurrencyModel Currency { get; set; }
        public bool IsActive { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime ClosingDate { get; set; }
    }
}
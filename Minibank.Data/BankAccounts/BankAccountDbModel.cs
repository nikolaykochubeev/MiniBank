﻿using System;
using Minibank.Core.Domains.Currency;

namespace Minibank.Data.BankAccounts
{
    public class BankAccountDbModel
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
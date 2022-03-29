using System;
using Minibank.Core.Domain.Currency;

namespace Minibank.Core.Domain.Transactions
{
    public class TransactionModel
    {
        public Guid Id { get; set; }
        public decimal AmountOfMoney { get; set; }
        public CurrencyModel Currency { get; set; }
        public Guid FromAccountId;
        public Guid ToAccountId;
    }
}
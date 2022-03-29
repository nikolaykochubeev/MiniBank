using System;
using Minibank.Core.Domain.Currency;

namespace Minibank.Data.Transactions
{
    public class TransactionDbModel
    {
        public Guid Id { get; set; }
        public decimal AmountOfMoney { get; set; }
        public CurrencyModel Currency { get; set; }
        public Guid FromAccountId;
        public Guid ToAccountId;
    }
}
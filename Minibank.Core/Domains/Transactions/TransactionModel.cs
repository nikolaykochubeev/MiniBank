using System;

namespace Minibank.Core.Domains.Transactions
{
    public class TransactionModel
    {
        public Guid Id { get; set; }
        public decimal AmountOfMoney { get; set; }
        public string Currency { get; set; }
        public Guid FromAccountId;
        public Guid ToAccountId;
    }
}
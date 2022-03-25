using System;

namespace Minibank.Data.Transactions
{
    public class TransactionDbModel
    {
        public Guid Id { get; set; }
        public decimal AmountOfMoney { get; set; }
        public string Currency { get; set; }
        public Guid FromAccountId;
        public Guid ToAccountId;
    }
}
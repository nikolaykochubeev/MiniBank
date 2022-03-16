using System;
using Minibank.Core.Domains.MoneyTransferHistory.Repositories;
using Minibank.Core.Domains.Services;

namespace Minibank.Core.Domains.MoneyTransferHistory
{
    public class MoneyTransferHistory
    {
        public Guid Id { get; set; }
        public decimal AmountOfMoney { get; set; }
        public string Currency { get; set; }
        public Guid FromAccountId;
        public Guid ToAccountId;
    }
}
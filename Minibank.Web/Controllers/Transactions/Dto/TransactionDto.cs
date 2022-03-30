using System;
using Minibank.Core.Domain.Currency;

namespace Minibank.Web.Controllers.Transactions.Dto
{
    public class TransactionDto
    {
        public decimal AmountOfMoney { get; set; }
        public CurrencyModel Currency { get; set; }
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
    }
}
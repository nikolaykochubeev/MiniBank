using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Minibank.Core.Domain.Currency;

namespace Minibank.Data.Transactions
{
    public class TransactionDbModel
    {
        public Guid Id { get; init; }
        public Guid FromAccountId  { get; set; }
        public Guid ToAccountId { get; set; }
        public decimal AmountOfMoney { get; set; }
        public CurrencyModel Currency { get; set; }

        internal class Map : IEntityTypeConfiguration<TransactionDbModel>
        {
            public void Configure(EntityTypeBuilder<TransactionDbModel> builder)
            {
                builder.ToTable("transaction");
            }
        }
    }
}
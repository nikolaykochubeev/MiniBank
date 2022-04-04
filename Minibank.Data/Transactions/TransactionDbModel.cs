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

                builder.Property(it => it.Id)
                    .HasColumnName("id");

                builder.Property(it => it.FromAccountId)
                    .HasColumnName("fromAccountId");

                builder.Property(it => it.ToAccountId)
                    .HasColumnName("toAccountId");

                builder.Property(it => it.AmountOfMoney)
                    .HasColumnName("amountOfMoney");

                builder.Property(it => it.Currency)
                    .HasColumnName("currency");

                //builder.HasKey(it => it.Id).HasName("pk_id");
            }
        }
    }
}
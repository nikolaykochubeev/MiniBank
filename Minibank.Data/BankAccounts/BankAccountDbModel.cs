using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Minibank.Core.Domain.Currency;

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

    internal class Map : IEntityTypeConfiguration<BankAccountDbModel>
    {
        public void Configure(EntityTypeBuilder<BankAccountDbModel> builder)
        {
            builder.ToTable("bankAccount");

            builder.Property(it => it.Id)
                .HasColumnName("id");

            builder.Property(it => it.UserId)
                .HasColumnName("userId");

            builder.Property(it => it.AmountOfMoney)
                .HasColumnName("amountOfMoney");

            builder.Property(it => it.Currency)
                .HasColumnName("currency");

            builder.Property(it => it.IsActive)
                .HasColumnName("isActive");

            builder.Property(it => it.OpeningDate)
                .HasColumnName("openingDate");

            builder.Property(it => it.ClosingDate)
                .HasColumnName("closingDate");

            //builder.HasKey(it => it.Id).HasName("pk_id");
        }
    }
}
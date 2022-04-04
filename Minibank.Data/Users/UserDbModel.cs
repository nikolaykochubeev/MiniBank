using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Minibank.Data.BankAccounts;

namespace Minibank.Data.Users
{
    public class UserDbModel
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }

        internal class Map : IEntityTypeConfiguration<UserDbModel>
        {
            public void Configure(EntityTypeBuilder<UserDbModel> builder)
            {
                builder.ToTable("user");
                builder.Property(it => it.Id)
                    .HasColumnName("id");

                builder.Property(it => it.Login)
                    .HasColumnName("login");

                builder.Property(it => it.Email)
                    .HasColumnName("email");

                // builder.Property("_id").HasColumnName("id");
                // builder.OwnsMany<BankAccountDbModel>(options => );

                //builder.HasKey(it => it.Id).HasName("pk_id");
            }
        }
    }
}
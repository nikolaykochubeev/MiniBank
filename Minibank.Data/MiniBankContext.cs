using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Minibank.Data.BankAccounts;
using Minibank.Data.Transactions;
using Minibank.Data.Users;

namespace Minibank.Data
{
    public class MiniBankContext : DbContext
    {
        public DbSet<UserDbModel> Users { get; set; }
        public DbSet<BankAccountDbModel> BankAccounts { get; set; }
        public DbSet<TransactionDbModel> Transactions { get; set; }
        public string ConnectionString { get; }
        public MiniBankContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            ConnectionString = configuration["PostgresConnectionString"];
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MiniBankContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.LogTo(Console.WriteLine);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
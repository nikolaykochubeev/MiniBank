using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minibank.Core.Domain.BankAccounts.Repositories;
using Minibank.Core.Domain.Currency.Services;
using Minibank.Core.Domain.Transactions.Repositories;
using Minibank.Core.Domain.Users.Repositories;
using Minibank.Data.BankAccounts.Repositories;
using Minibank.Data.HttpClients;
using Minibank.Data.Transactions.Repositories;
using Minibank.Data.Users.Repositories;

namespace Minibank.Data
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<ICurrencyCourseProvider, CurrencyCourseHttpProvider>(options =>
            {
                options.BaseAddress = new Uri(configuration["CbrDaily"]);
            });
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            return services;
        }
    }
}
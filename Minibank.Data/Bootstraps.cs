using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minibank.Core.Domains.BankAccount.Repositories;
using Minibank.Core.Domains.Transactions.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Services;
using Minibank.Data.HttpClients;
using Minibank.Data.Repositories;

namespace Minibank.Data
{
    public static class Bootstraps
    {
        public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<ICurrencyCourseProvider, CurrencyCourseHttpProvider>(options  =>
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
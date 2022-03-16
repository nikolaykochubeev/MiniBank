using Microsoft.Extensions.DependencyInjection;
using Minibank.Core.Domains.MoneyTransferHistory;
using Minibank.Core.Domains.Services;
using Minibank.Core.Domains.Users.Services;
using Minibank.Core.Services;

namespace Minibank.Core
{
    public static class Bootstraps
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddScoped<IMoneyTransferHistoryService, MoneyTransferHistoryService>();
            return services;
        }
    }
}
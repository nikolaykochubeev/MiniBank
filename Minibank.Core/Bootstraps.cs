using Microsoft.Extensions.DependencyInjection;
using Minibank.Core.Domains.BankAccount.Services;
using Minibank.Core.Domains.Currency.Services;
using Minibank.Core.Domains.Users.Services;

namespace Minibank.Core
{
    public static class Bootstraps
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBankAccountService, BankAccountService>();
            return services;
        }
    }
}
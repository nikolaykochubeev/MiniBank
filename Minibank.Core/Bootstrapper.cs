using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Minibank.Core.Domain.BankAccounts.Services;
using Minibank.Core.Domain.BankAccounts.Validators;
using Minibank.Core.Domain.Currency.Services;
using Minibank.Core.Domain.Users.Services;
using Minibank.Core.Domain.Users.Validators;

namespace Minibank.Core
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddFluentValidation().AddValidatorsFromAssembly(typeof(UserValidator).Assembly);
            services.AddFluentValidation().AddValidatorsFromAssembly(typeof(BankAccountValidator).Assembly);
            return services;
        }
    }
}
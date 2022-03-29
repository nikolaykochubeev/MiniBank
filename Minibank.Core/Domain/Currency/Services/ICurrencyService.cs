using System.Threading.Tasks;

namespace Minibank.Core.Domain.Currency.Services
{
    public interface ICurrencyService
    {
        Task<decimal> Convert(decimal amount, string fromCurrency, string toCurrency);
        Task<decimal> GetRubleCourse(string currencyCode);
    }
}
using System.Threading.Tasks;

namespace Minibank.Core.Domain.Currency.Services
{
    public interface ICurrencyService
    {
        Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency);
        Task<decimal> GetRubleCourseAsync(string currencyCode);
    }
}
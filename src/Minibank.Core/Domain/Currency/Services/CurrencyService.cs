using System.Threading.Tasks;

namespace Minibank.Core.Domain.Currency.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyCourseProvider _currencyCourseProvider;

        public CurrencyService(ICurrencyCourseProvider currencyCourseProvider)
        {
            _currencyCourseProvider = currencyCourseProvider;
        }

        public async Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency)
        {
            var fromCurrencyCourse = await _currencyCourseProvider.GetRubleCourse(fromCurrency);
            var toCurrencyCourse = await _currencyCourseProvider.GetRubleCourse(toCurrency);
            return (fromCurrencyCourse * amount) / toCurrencyCourse;
        }

        public async Task<decimal> GetRubleCourseAsync(string currencyCode)
        {
            return await _currencyCourseProvider.GetRubleCourse(currencyCode);
        }
    }
}
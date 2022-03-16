using System.Threading.Tasks;
using Minibank.Core.Exceptions;

namespace Minibank.Core.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyCourseProvider _currencyCourseProvider;

        public CurrencyService(ICurrencyCourseProvider currencyCourseProvider)
        {
            _currencyCourseProvider = currencyCourseProvider;
        }

        public async Task<decimal> Convert(decimal amount, string fromCurrency, string toCurrency)
        {
            var fromCurrencyCourse = await _currencyCourseProvider.GetRubleCourse(fromCurrency);
            var toCurrencyCourse = await _currencyCourseProvider.GetRubleCourse(toCurrency);
            return (fromCurrencyCourse * amount) / toCurrencyCourse;
        }

        public async Task<decimal> GetRubleCourse(string currencyCode)
        {
            return await _currencyCourseProvider.GetRubleCourse(currencyCode);
        }
    }
}
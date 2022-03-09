using Minibank.Core.Exceptions;

namespace Minibank.Core.Services
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly ICurrentCourseDb _currentCourseDb;

        public CurrencyConverter(ICurrentCourseDb currentCourseDb)
        {
            _currentCourseDb = currentCourseDb;
        }

        public decimal Convert(decimal amount, string currency)
        {
            var rate = _currentCourseDb.GetRate(currency);
            
            if (rate == -1)
            {
                throw new MinibankException($"Currency {currency} does not exist");
            }
            
            return rate * amount;
        }
    }
}
using Minibank.Core.Exceptions;
using Minibank.Core.Models;

namespace Minibank.Core.Services
{
    public class Convertor : IConvertor
    {
        private readonly ICurrentCourseDb _currentCourseDb;

        public Convertor(ICurrentCourseDb currentCourseDb)
        {
            _currentCourseDb = currentCourseDb;
        }

        public decimal Convert(decimal amount, string currency)
        {
            var rate = _currentCourseDb.GetRate(currency);
            
            if (rate == default)
            {
                throw new UserFriendlyException($"Currency {currency} does not exists");
            }
            
            return rate * amount;
        }
    }
}
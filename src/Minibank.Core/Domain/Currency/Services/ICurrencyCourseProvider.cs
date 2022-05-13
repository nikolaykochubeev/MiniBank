using System.Threading.Tasks;

namespace Minibank.Core.Domain.Currency.Services
{
    public interface ICurrencyCourseProvider
    {
        Task<decimal> GetRubleCourse(string currencyCode);
    }
}
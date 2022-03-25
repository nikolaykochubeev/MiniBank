using System.Threading.Tasks;

namespace Minibank.Core.Domains.Currency.Services
{
    public interface ICurrencyCourseProvider
    {
        Task<decimal> GetRubleCourse(string currencyCode);
    }
}
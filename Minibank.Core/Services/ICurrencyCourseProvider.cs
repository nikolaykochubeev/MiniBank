using System.Threading.Tasks;

namespace Minibank.Core.Services
{
    public interface ICurrencyCourseProvider
    {
        Task<decimal> GetRubleCourse(string currencyCode);
    }
} 
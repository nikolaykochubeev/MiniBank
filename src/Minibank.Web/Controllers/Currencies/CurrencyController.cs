using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domain.Currency.Services;

namespace Minibank.Web.Controllers.Currencies
{
    [ApiController]
    [Authorize]
    [Route("api/v1/minibank/[controller]/[action]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        public async Task<decimal> Convert(int amount, string fromCurrency, string toCurrency)
        {
            return await _currencyService.ConvertAsync(amount, fromCurrency, toCurrency);
        }
        
        [HttpGet]
        public async Task<decimal> GetRubleCourse(string currencyCode)
        {
            return await _currencyService.GetRubleCourseAsync(currencyCode);
        }
    }
}
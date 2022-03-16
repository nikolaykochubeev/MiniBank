using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Services;
using Minibank.Data.HttpClients.Models;

namespace Minibank.Web.Controllers
{
    [ApiController]
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
            return await _currencyService.Convert(amount, fromCurrency, toCurrency);
        }
        
        [HttpGet]
        public async Task<decimal> GetRubleCourse(string currencyCode)
        {
            return await _currencyService.GetRubleCourse(currencyCode);
        }
    }
}
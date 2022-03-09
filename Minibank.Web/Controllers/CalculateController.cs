using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Services;

namespace Minibank.Web.Controllers
{
    [ApiController]
    [Route("api/v1/minibank/[controller]/[action]")]
    public class CalculateController : ControllerBase
    {
        private readonly ICurrencyConverter _currencyConverter;

        public CalculateController(ICurrencyConverter currencyConverter)
        {
            _currencyConverter = currencyConverter;
        }

        [HttpGet]
        public decimal ConvertRublesTo(decimal amount, string currency)
        {
            return _currencyConverter.Convert(amount, currency);
        }
    }
}
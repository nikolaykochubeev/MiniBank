using Microsoft.AspNetCore.Mvc;
using Minibank.Core;
using Minibank.Core.Services;

namespace Minibank.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculateController : ControllerBase
    {
        private readonly IConvertor _convertor;

        public CalculateController(IConvertor convertor)
        {
            _convertor = convertor;
        }

        [HttpGet]
        public decimal ConvertRublesTo(decimal amount, string currency)
        {
            return _convertor.Convert(amount, currency);
        }
    }
}
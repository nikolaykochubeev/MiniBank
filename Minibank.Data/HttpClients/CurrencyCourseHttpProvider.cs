using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Minibank.Core.Domain.Currency.Services;
using Minibank.Data.HttpClients.Models;

namespace Minibank.Data.HttpClients
{
    public class CurrencyCourseHttpProvider : ICurrencyCourseProvider
    {
        private readonly HttpClient _httpClient;

        public CurrencyCourseHttpProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetRubleCourse(string currencyCode)
        {
            if (currencyCode == "RUB")
            {
                return 1;
            }

            var response = await GetRubleCourses();
            var valute = response.Valute[currencyCode];

            return ((decimal)valute.Value / valute.Nominal);
        }

        private async Task<CurrencyResponseModel> GetRubleCourses()
        {
            return await _httpClient.GetFromJsonAsync<CurrencyResponseModel>("daily_json.js") ??
                   throw new Exception("Response From HttpsClient is null");
        }
    }
}
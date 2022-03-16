using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Minibank.Core.Services;
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
            var response = await GetRubleCourses();
            var valute = response.Valute[currencyCode];

            return (decimal)(valute.Value / valute.Nominal);
        }

        private async Task<CurrencyResponseModel> GetRubleCourses()
        {
            return await _httpClient.GetFromJsonAsync<CurrencyResponseModel>("daily_json.js") ??
                   throw new Exception("Response From HttpsClient is null");
        }
    }
}
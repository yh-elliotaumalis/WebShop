using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Webshop.Application.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient = new();
        public async Task<decimal> GetRateAsync(string from, string to)
        { 
        var url = $"https://api.frankfurter.app/latest?from={from}&to={to}";
            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<CurrencyResponse>(json);
            return data.Rates[to];


        }
    }
}

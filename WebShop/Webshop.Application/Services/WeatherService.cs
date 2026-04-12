using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Webshop.Application.Models;

namespace Webshop.Application.Services
{
    internal class WeatherService
    {
        private readonly HttpClient _httpClient;
        public WeatherService()
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("x-Api-Key", "6cwGGKJg2rUoPQzBmjE7sLhPTsQkquSkjCvXl20z");

        }
        public async Task<double> GetTemperatureAsync()
        {
            var response = await _httpClient.GetAsync("https://api.api-ninjas.com/v1/weather?lat=59.33&lon=18.06");

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<WeatherResponse>(json);
            return data.Temperature;


        }
     

    }
}

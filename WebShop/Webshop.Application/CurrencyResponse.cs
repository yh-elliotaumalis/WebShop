using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Webshop.Application
{
    internal class CurrencyResponse
    {
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("base")]      
        public string BaseCurrency { get; set; }
        [JsonPropertyName("rates")]
        public Dictionary<string, decimal> Rates { get; set; } 
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Webshop.Application
{
    internal class CurrencyResponse
    {
        public decimal Amount { get; set; }
        public string Base{ get; set; }
        public Dictionary<string, decimal> Rates { get; set; } 
    }
}

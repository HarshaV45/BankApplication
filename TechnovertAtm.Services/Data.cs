using System;
using System.Collections.Generic;
using System.Text;
using TechonovertAtm.Models;

namespace TechnovertAtm.Services
{
    public class Data
    {
        public List<Bank> banks;
        public List<Currency> currencies;
        public Data()
        {
            this.banks = new List<Bank>();
            this.currencies = new List<Currency>();
            this.currencies.Add(new Currency() { CurrencyCode = "INR", CurrencyName = "Rupee", CurrencyExchangeRate = 1 });
        }
    }
}

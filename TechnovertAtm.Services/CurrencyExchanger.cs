using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Cache;
using System.Net;
using Newtonsoft.Json;
using TechonovertAtm.Models;

namespace TechnovertAtm.Services
{
    public class CurrencyExchanger
    {
        private Data data;
        public CurrencyExchanger(Data data)
        {
            this.data = data;
        }
        public decimal Converter(decimal amount,decimal exchangeRate)
        {
            decimal actualAmount = amount * exchangeRate;
            return actualAmount;
        }

     public void CurrencyExchange()
        {
            string url = "http://www.floatrates.com/daily/inr.json";
            string json = new WebClient().DownloadString(url);
            var currency = JsonConvert.DeserializeObject<dynamic>(json);
            this.data.currencies.Add(new Currency()
            {
                CurrencyCode = currency.usd.code,
                CurrencyName = currency.usd.name,
                CurrencyExchangeRate = currency.usd.inverseRate
            });

            this.data.currencies.Add(new Currency()
            {
                CurrencyCode = currency.eur.code,
                CurrencyName = currency.eur.name,
                CurrencyExchangeRate = currency.eur.inverseRate
            });

        }
    }
}

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
        //  private Data data;
        public BankDbContext DbContext = new BankDbContext();
        public CurrencyExchanger(Data data)
        {
            CurrencyExchange();
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
            var newCurrency=(new Currency()
            {
                CurrencyCode = currency.usd.code,
                CurrencyName = currency.usd.name,
                CurrencyExchangeRate = currency.usd.inverseRate
            });
            DbContext.Curriencies.Add(newCurrency);
            var newcurrency =(new Currency()
            {
                CurrencyCode = currency.eur.code,
                CurrencyName = currency.eur.name,
                CurrencyExchangeRate = currency.eur.inverseRate
            });

            DbContext.Curriencies.Add(newcurrency);
        }
    }
}

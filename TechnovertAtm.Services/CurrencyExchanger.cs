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
        
        public BankDbContext DbContext = new BankDbContext();
        public CurrencyExchanger()
        {
           // CurrencyExchange();
        }
        public decimal Converter(decimal amount,decimal exchangeRate)
        {
            decimal actualAmount = amount * exchangeRate;
            return actualAmount;
        }

        public void CurrencyExchange()
        {
            try
            {


                string url = "http://www.floatrates.com/daily/inr.json";
                string json = new WebClient().DownloadString(url);
                var currencies = JsonConvert.DeserializeObject<dynamic>(json);
                int limit = 10;
                int currencyCounter = 0;
                foreach (var currency in currencies)
                {
                    if (currencyCounter == limit)
                    {
                        break;
                    }

                    var newCurrency = (new Currency()
                    {
                        CurrencyCode = currency.Value.code,
                        CurrencyName = currency.Value.name,
                        CurrencyExchangeRate = currency.Value.inverseRate
                    });
                    DbContext.Curriencies.Add(newCurrency);

                    currencyCounter += 1;
                }
                DbContext.SaveChanges();
                
             
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Cache;
using System.Net;
using Newtonsoft.Json;
using TechonovertAtm.Models;
using TechnovertAtm.Services.Interfaces;

namespace TechnovertAtm.Services
{
    public class CurrencyExchanger:ICurrencyService
    {

        private BankDbContext _DbContext;
        public CurrencyExchanger(BankDbContext DbContext)
        {
            _DbContext = DbContext;
            CurrencyExchange();
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
                    _DbContext.Curriencies.Add(newCurrency);

                    currencyCounter += 1;
                }
                _DbContext.SaveChanges();
                
             
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            }
    }
}

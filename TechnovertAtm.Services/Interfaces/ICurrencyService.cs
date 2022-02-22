using System;
using System.Collections.Generic;
using System.Text;

namespace TechnovertAtm.Services.Interfaces
{
    public interface ICurrencyService
    {
        public decimal Converter(decimal amount, decimal exchangeRate);
        public void CurrencyExchange();
    }
}

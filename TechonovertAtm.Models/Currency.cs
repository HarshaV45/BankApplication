using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechonovertAtm.Models
{
    public class Currency
    {
        [Key]
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public decimal CurrencyExchangeRate { get; set; }
    }
}

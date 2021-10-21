using System;
using System.Collections.Generic;
using System.Text;

namespace TechonovertAtm.Models
{
    public class Tranaction
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public DateTime On { get; set; }
    }
}

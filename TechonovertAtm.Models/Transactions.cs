using System;
using System.Collections.Generic;
using System.Text;

namespace TechonovertAtm.Models
{
    public class Transactions
    {
        public string Id { get; set; }
        public int SourceAccountId { get; set; }
        public int DestinationAccountId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public DateTime On { get; set; }
    }
}

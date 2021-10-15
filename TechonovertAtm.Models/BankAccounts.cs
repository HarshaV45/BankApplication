using System;
using System.Collections.Generic;
using System.Text;


namespace TechonovertAtm.Models
{
    public class BankAccounts
    {
        public int AccountNumber { get; set; }
        public int Pin { get; set; }
        public int Amount { get; set; }
        public List<string> Transactions { get; set; }
       
    }
}

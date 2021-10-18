using System;
using System.Collections.Generic;
using System.Text;


namespace TechonovertAtm.Models
{
    public class BankAccounts
    {
        public string Id { get; set; }
        public int AccountNumber { get; set; }
        public string Password { get; set; }
        public decimal Amount { get; set; }
        public List<Transactions> Transactions { get; set; }
        public string Name { get; set; }
        public bool IsMale { get; set; }
        public AccountStatus Status { get; set; }
        

       
    }
}

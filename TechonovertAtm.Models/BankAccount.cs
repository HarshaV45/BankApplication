using System;
using System.Collections.Generic;
using System.Text;
using TechonovertAtm.Models.Enums;

namespace TechonovertAtm.Models
{
    public class BankAccount
    {
        public string AccountId { get; set; }
        public string BankId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public decimal Amount { get; set; }
        public List<Transaction> Transactions { get; set; }
        public GenderType Gender { get; set; }
        

       
    }
}

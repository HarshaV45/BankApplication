using System;
using System.Collections.Generic;
using System.Text;
using TechonovertAtm.Models.Enums;

namespace TechonovertAtm.Models
{
    public class Bank
    {
        public string BankId { get; set; }
        public string Name { get; set; }

        public List<BankAccount> BankAccounts { get; set; }
        


    }
}

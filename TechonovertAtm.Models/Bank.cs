using System;
using System.Collections.Generic;
using System.Text;

namespace TechonovertAtm.Models
{
    public class Bank
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<BankAccount> BankAccounts { get; set; }


    }
}

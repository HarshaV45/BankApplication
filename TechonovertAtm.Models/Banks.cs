using System;
using System.Collections.Generic;
using System.Text;

namespace TechonovertAtm.Models
{
    public class Banks
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<BankAccounts> BankAccounts { get; set; }


    }
}

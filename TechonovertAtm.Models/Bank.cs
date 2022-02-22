using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TechonovertAtm.Models.Enums;

namespace TechonovertAtm.Models
{
    public class Bank
    {
       [Key]
        public string BankId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public  Status BankStatus { get; set; }
       // public AccountStatus AccountStatus { get; set; }
        //public List<BankAccount> BankAccounts { get; set; }
        


    }
}

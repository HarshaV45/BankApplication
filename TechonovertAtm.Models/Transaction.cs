using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TechonovertAtm.Models.Enums;

namespace TechonovertAtm.Models
{
    public class Transaction
    {
        [Key]
        public string Id { get; set; }
        
        public decimal Amount { get; set; }
        public string OnDate { get; set; }
        public string TaxType { get; set; }
        public decimal Tax { get; set; }
        public string BankId { get; set; }
        public string AccountId { get; set; }
        public string DestinationBankId { get; set; }
        public string DestinationAccountId { get; set; }

        public TransactionType TransactionType { get; set; }
    }
}

using System;
using TechonovertAtm.Models.Enums;

namespace Technovert.WebApi.DTOs.TransactionDTOs
{
    public class DefaultTransactionDTO
    {
        public string Id { get; set; }
        public string BankId { get; set; }
        public string AccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime On{ get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
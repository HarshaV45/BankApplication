

using System;
using TechonovertAtm.Models.Enums;

namespace Technovert.WebApi.DTOs.TransactionDTOs
{
    public class GetTransactionDTO
    {
        public string Id { get; set; }
        public string BankId { get; set; }
        public string AccountId { get; set; }
        public string DestinationBankId { get; set; }
        public string DestinationAccountId { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxAmount { get; set; }
        public string TaxType { get; set; }
        public DateTime OnDate { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
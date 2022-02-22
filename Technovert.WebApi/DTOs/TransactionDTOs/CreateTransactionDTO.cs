
using System.ComponentModel.DataAnnotations;
using TechonovertAtm.Models.Enums;

namespace Technovert.WebApi.DTOs.TransactionDTOs
{
    public class CreateTransactionDTO
    {
        [Required]
        public string DestinationBankId { get; set; }
        [Required]
        public string DestinationAccountId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public TaxType TaxType { get; set; }
    }
}

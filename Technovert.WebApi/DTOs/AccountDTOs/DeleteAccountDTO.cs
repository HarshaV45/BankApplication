using TechonovertAtm.Models.Enums;

namespace Technovert.WebApi.DTOs.AccountDTOs
{
    // Class to represent the properties available for Account holders
    public class DeleteAccountDTO
    {
        public string BankId { get; set; }
        public string AccountId { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public int Age { get; set; }
        public  GenderType Gender { get; set; }
        public Status AccountStatus { get; set; }
    }
}
using TechonovertAtm.Models.Enums;

namespace Technovert.WebApi.DTOs.AccountDTOs
{
    // Class to represent the properties available for Account holders
    public class PutAccountDTO
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public GenderType Gender { get; set; }
    }
}
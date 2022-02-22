using System.ComponentModel.DataAnnotations;
using TechonovertAtm.Models.Enums;

namespace Technovert.WebApi.DTOs.AccountDTOs
{
    // Class to represent the properties available for Account holders
    public class PostAccountDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConformPassword { get; set; }
        public int Age { get; set; }
        public GenderType Gender { get; set; }
    }
}
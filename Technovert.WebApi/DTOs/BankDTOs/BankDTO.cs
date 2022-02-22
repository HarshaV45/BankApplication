using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TechonovertAtm.Models.Enums;

namespace Technovert.WebApi.DTOs.BankDTO
{
    // Class to represent the properties available for Account holders
    public class BankDTO
    {
        [Required]
        public string Name { get; set; }
        public string Desciption { get; set; }
    }
}
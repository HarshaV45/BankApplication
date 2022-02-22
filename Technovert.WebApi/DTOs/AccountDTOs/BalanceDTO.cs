using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TechonovertAtm.Models.Enums;

namespace Technovert.WebApi.DTOs.AccountDTOs
{
    // Class to represent the properties available for Account holders
    public class BalanceDTO
    {
        [Display(Name = "Enter the Amount")]
        public decimal Amount{ get; set; }
    }
}

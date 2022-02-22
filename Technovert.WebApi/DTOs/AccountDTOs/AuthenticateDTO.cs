using System;
using System.ComponentModel.DataAnnotations;

namespace Technovert.WebApi.DTOs.AccountDTOs
{
    public class AuthenticateDTO
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Password { get; set; }
    }
}



using AutoMapper;
using Intercom.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Technovert.WebApi.DTOs.BankDTO;
using TechnovertAtm.Services.Interfaces;
using TechonovertAtm.Models;

namespace Technovert.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class BanksController : ControllerBase
    {
        private IMapper mapper;
        private IBankService bankService;
        public BanksController(IMapper mapper, IBankService bankService)
        {
            this.mapper = mapper;
            this.bankService = bankService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(bankService.GetAllBAnks());
        }

        [Authorize(Roles = "Staff")]
        [HttpPost]
        public IActionResult Post(BankDTO bank)
        {
            if (bank == null)
                return BadRequest();
            var newBank = mapper.Map<Bank>(bank);
            newBank.Description = bank.Desciption;
            newBank.BankStatus = TechonovertAtm.Models.Enums.Status.Active;
            return Ok(bankService.BankCreation(newBank));
        }

       [Authorize(Roles = "Staff")]
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var bank = bankService.CloseBank(id);
                var bankToDelete = mapper.Map<BankDTO>(bank);
                bankToDelete.Desciption = bank.Description;
                return Ok(bankToDelete);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
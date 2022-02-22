using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Technovert.WebApi.DTOs.AccountDTOs;
using TechnovertAtm.Services.Interfaces;
using TechonovertAtm.Models;

using TechonovertAtm.Models.Enums;
namespace Technovert.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        
        private IMapper mapper;
        private IAccountService accountService;
        public AccountsController(IAccountService AccountService, IMapper Mapper)
        {
            this.mapper = Mapper;
            this.accountService = AccountService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate/{bankId}")]
        public IActionResult Authenticate(string bankId, AuthenticateDTO authDTO)
        {
            try
            {
                var token = accountService.Authenticate(bankId, authDTO.Id, authDTO.Password);
                if (token == null)
                    return Unauthorized();
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("{bankId}")]
        public IActionResult Get(string bankId)
        {
            try
            {
                var accounts = accountService.GetAllAccounts(bankId);
                var accountDTOs = mapper.Map<IEnumerable<GetAccountDTO>>(accounts);
                return Ok(accountDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("{bankId}/{id}")]
        public IActionResult Get(string bankId, string id)
        {
            try
            {
                var account = accountService.GetAccount(bankId, id);
                var accountDTO = mapper.Map<GetAccountDTO>(account);
                return Ok(accountDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("{bankId}")]
        public IActionResult Post(string bankId, PostAccountDTO newAccountDTO)
        {
            try
            {
                if (newAccountDTO == null)
                    return BadRequest();
                var acc = mapper.Map<BankAccount>(newAccountDTO);
                acc.AccountStatus = TechonovertAtm.Models.Enums.AccountStatus.Active;
                acc.Role = Roles.User;
                acc.AccountId= Guid.NewGuid().ToString();
                acc.BankId = bankId;
                var newAcc = accountService.CreateAccount(acc);
                var token = accountService.CreateToken(newAcc);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Roles = "Staff")]
        [HttpPut("{bankId}/{id}")]
        public IActionResult Put(string bankId, string id, PutAccountDTO accountDTO)
        {
            try
            {
                if (accountDTO == null)
                    return BadRequest();
                var account = accountService.GetAccount(bankId, id);
                if (account.Role == Roles.Admin)
                    return BadRequest("Can't Update Administrator");
                account.Name = accountDTO.Name;
                account.Age = accountDTO.Age;
                account.Gender = accountDTO.Gender;
                var updatedAccount = accountService.UpdateAccount(account);
                return Ok(mapper.Map<GetAccountDTO>(updatedAccount));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Staff")]
        [HttpDelete("{bankId}/{id}")]
        public IActionResult Delete(string bankId, string id)
        {
            try
            {
                BankAccount acc = accountService.CloseAccount(bankId, id);
                return Ok(mapper.Map<DeleteAccountDTO>(acc));
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet("balance/{bankId}/{id}")]
        public IActionResult GetBalance(string bankId, string id)
        {
            try
            {
                var acc = accountService.GetAccount(bankId, id);
                if (acc == null)
                    return BadRequest();
                return Ok(mapper.Map<BalanceDTO>(acc));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Staff")]
        [HttpPut("balance/{bankId}/{id}")]
        public IActionResult PutBalance(string bankId, string id, BalanceDTO balanceDTO)
        {
            try
            {
                if (balanceDTO == null)
                    return BadRequest();
                var acc = accountService.GetAccount(bankId, id);
                acc.Amount += balanceDTO.Amount;
                accountService.UpdateAccount(acc);
                return Ok("Balance has been Updated!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
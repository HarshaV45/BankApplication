using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnovertAtm.Services.Interfaces;
using TechonovertAtm.Models;
using TechonovertAtm.Models.Exceptions;
using TechonovertAtm.Models.Enums;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace TechnovertAtm.Services
{
    

    public class AccountServices:IAccountService
    {
        
        DateTime PresentDate = DateTime.Today;
        private BankDbContext _DbContext;
        private IConfiguration configuration;


        private readonly AppSettings _appSettings;
        public AccountServices(BankDbContext dbContext,IOptions<AppSettings> appSettings,IConfiguration configuration)
        {
            _DbContext = dbContext;
            _appSettings = appSettings.Value;
            this.configuration = configuration;

        }

        public string Authenticate(string bankId, string id, string password)
        {
            try
            {
                var info = _DbContext.Staff.SingleOrDefault(m => m.Id == id );
                if (info == null)
                    return null;
                
                if (info.Password != password)
                    throw new Exception("Invalid Password!");
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, info.Id),
                        new Claim(ClaimTypes.Role,"Staff")
                    }),
                    Expires = DateTime.Now.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string CreateToken(BankAccount account)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,account.Name),
                new Claim(ClaimTypes.Role,"User")
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken
            (
                claims: claims,
                signingCredentials: cred,
                expires: DateTime.Now.AddDays(1));
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public BankAccount CreateAccount(BankAccount account)
        {
            try
            {
                _DbContext.BankAccounts.Add(account);
                _DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return account;
        }

        public BankAccount UpdateAccount(BankAccount account)
        {
            _DbContext.BankAccounts.Attach(account);
            _DbContext.SaveChanges();
            var UpdatedAccount = _DbContext.BankAccounts.FirstOrDefault(m => m.AccountId == account.AccountId);
            return UpdatedAccount;

        }

        public BankAccount CloseAccount(string bankId,string accountId)
        {
            var account = _DbContext.BankAccounts.SingleOrDefault(m => m.BankId == bankId && m.AccountId == accountId);
            if (account.AccountStatus == AccountStatus.Closed)
                throw new Exception("Account is  closed");
            if (account.Role == Roles.Admin)
                throw new Exception("Can't Delete a Administrator account ");
            account.AccountStatus = AccountStatus.Closed;
            _DbContext.SaveChanges();
            return account;
        }


        public BankAccount GetAccount(string bankId, string accountId)
        {
            var account = _DbContext.BankAccounts.FirstOrDefault(m => m.BankId == bankId && m.AccountId == accountId);
            if (account == null)
                throw new Exception("Account Not Found!");
            if (account.AccountStatus == AccountStatus.Closed)
                throw new Exception("Account was Closed!");
            return account;
        }

        public List<BankAccount> GetAllAccounts(string bankId)
        {
            return _DbContext.BankAccounts.Where(m => m.BankId == bankId).ToList();
        }










      
        
            

      
    }

}

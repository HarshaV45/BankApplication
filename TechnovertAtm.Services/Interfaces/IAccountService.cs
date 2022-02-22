using System;
using System.Collections.Generic;
using System.Text;
using TechonovertAtm.Models;

namespace TechnovertAtm.Services.Interfaces
{
   public interface IAccountService
    {
        public string Authenticate(string bankId, string id, string password);
        public BankAccount CreateAccount(BankAccount account);
        public BankAccount UpdateAccount(BankAccount account);

        public string CreateToken(BankAccount account);

        public BankAccount CloseAccount(string bankId, string accountId);
        public BankAccount GetAccount(string bankId, string accountId);
        public List<BankAccount> GetAllAccounts(string bankId);
    }
}

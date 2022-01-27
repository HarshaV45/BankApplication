using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechonovertAtm.Models;
using TechonovertAtm.Models.Exceptions;

namespace TechnovertAtm.Services
{
    

    public class CustomerService
    {
        private BankServices bankService;
        DateTime PresentDate = DateTime.Today;
        private BankDbContext DbContext;
        public CustomerService(BankDbContext dbContext,BankServices bankService)
        {
            this.DbContext = dbContext;
            this.bankService = bankService;
        }



        //public BankAccount AccountChecker(string bankId, string accountId)
        //{   
        //    if(String.IsNullOrWhiteSpace(bankId) && String.IsNullOrWhiteSpace(accountId))
        //    {
        //        throw new InvalidInputException();
        //    }
        //    Bank bank = this.bankService.BankChecker(bankId);
        //    foreach (var d in bank.BankAccounts)
        //    {
        //        if (d.BankId ==bankId && d.AccountId == accountId)
        //        {
        //            return d;
        //        }
        //    }
        //    throw new InvalidUserException();

        //}



       



        public void AccountLogin(string bankId, string accountId, string password)
        {
            if(String.IsNullOrWhiteSpace(password))
            {
                throw new InvalidInputException();
            }
            
            var user = DbContext.BankAccounts.SingleOrDefault( m => m.BankId==bankId && m.AccountId == accountId);
            if (user == null || user.Password != password)
            {
                throw new InvalidInputException();
            }
        }
            

      
    }

}

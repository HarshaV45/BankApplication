using System;
using System.Collections.Generic;
using System.Text;
using TechonovertAtm.Models;
using TechonovertAtm.Models.Exceptions;

namespace TechnovertAtm.Services
{
    

    public class CustomerService
    {
        private BankServices bankService;
        DateTime PresentDate = DateTime.Today;
        public CustomerService(BankServices bankService)
        {
            this.bankService = bankService;
        }



        public BankAccount AccountChecker(string bankId, string accountId)
        {   
            if(String.IsNullOrWhiteSpace(bankId) && String.IsNullOrWhiteSpace(accountId))
            {
                throw new InvalidInputException();
            }
            Bank bank = this.bankService.BankChecker(bankId);
            foreach (var d in bank.BankAccounts)
            {
                if (d.BankId ==bankId && d.AccountId == accountId)
                {
                    return d;
                }
            }
            throw new InvalidUserException();

        }



       



        public void AccountLogin(string bankId, string accountId, string password)
        {
            if(String.IsNullOrWhiteSpace(password))
            {
                throw new InvalidInputException();
            }
            
            BankAccount user = AccountChecker(bankId, accountId);
            if (user == null || user.Password != password)
            {
                throw new InvalidInputException();
            }
        }
            

        public void ResetPassword(string bankId, string accountId, string password)
        {
            BankAccount account = AccountChecker(bankId, accountId);
            account.Password = password;

        }
    }

}

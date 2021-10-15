using System;
using System.Collections.Generic;
using System.Text;
using TechonovertAtm.Models;

namespace TechnovertAtm.Services
{
    public class BankService
    {
        private List<BankAccounts> accounts; //can be accsesible only this class
        
        public BankService() //constructor 
        {
            accounts = new List<BankAccounts>();
            BankAccounts account = new BankAccounts
            {
                AccountNumber = 1234567,
                Pin = 997745,
                Amount = 1000
            };
            this.accounts.Add(account);

           
            
        }


        public BankAccounts IsAccountPresent(int cardNo)
        {
            foreach(var d in accounts)
            {
                if(d.AccountNumber == cardNo)
                {
                    return d;
                }
            }
            return null;
        }

        public void AddAccount(int accountNumber, int pin)
        {
            BankAccounts account = new BankAccounts
            {
                AccountNumber = accountNumber,
                Pin = pin,
                Amount = 1000,
                Transactions = new List<string>()
            };
            this.accounts.Add(account); //added the new account number into bank
        }


        public bool validateCardDetails(int cardNo, int pin)
        {
            foreach (var d in accounts)
            {
                if (d.AccountNumber == cardNo && d.Pin == pin)
                {
                    return true;
                }
                
            }
            return false;
       

        }


        public bool deposit(int accountNumber, int deposit_amount)
        {
             

            if (deposit_amount > 0)
            {
                BankAccounts userAccounts = IsAccountPresent(accountNumber);
                if(userAccounts==null)
                {
                    return false;
                }
                userAccounts.Amount += deposit_amount;
                userAccounts.Transactions.Add(deposit_amount + "has been deposited into Account Number"+accountNumber);
                return true;
                        
                    
                
            }

            return false;
            
        }

        public bool withdraw(int accountNumber, int withdraw_amount)
        {
            bool flag = false;

            BankAccounts userAccounts = IsAccountPresent(accountNumber);
            if (userAccounts != null)
            {
                if (userAccounts.Amount < withdraw_amount)
                {
                    flag = false;
                }
                else
                {
                    flag = true;
                    userAccounts.Amount -= withdraw_amount;
                    userAccounts.Transactions.Add(withdraw_amount + " has been withdrawn from Account Number " + accountNumber);

                }
                
            }
            return flag;
        }
   
        public bool transfer(int accnum, int accnum1, int amount)
        {
            bool flag = false;

            foreach (var d in accounts)
            {
                if (d.AccountNumber == accnum)
                {
                    if (d.Amount < amount)
                    {
                        flag = false;
                    }
                    else
                    {
                        flag = true;
                        d.Amount -= amount;
                    }
                    break;
                }
            }
            foreach (var d in accounts)
            {
                if (d.AccountNumber == accnum1)
                {
                    d.Amount += amount;
                    
                }
            }

            return flag;
        }
        public List<string> TransactionLog(int accountNumber)
        {
            BankAccounts userAccounts = IsAccountPresent(accountNumber);
            return userAccounts.Transactions;


        }
        
    }
}

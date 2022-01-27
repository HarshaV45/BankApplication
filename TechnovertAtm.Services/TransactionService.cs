using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechonovertAtm.Models;
using TechonovertAtm.Models.Enums;
using TechonovertAtm.Models.Exceptions;

namespace TechnovertAtm.Services
{
    public class TransactionService
    {
        private CustomerService customerService;
        //private Data data;
        private CurrencyExchanger currencyExchanger;
        DateTime PresentDate = DateTime.Today;
        private BankDbContext DbContext;
       // public int limit = 50000;

        Random random = new Random();
        public TransactionService(BankDbContext dbContext,CustomerService customer,CurrencyExchanger currencyExchanger)

        {
            this.DbContext = dbContext; 
            this.currencyExchanger = currencyExchanger;
            this.customerService = customer;
        }

        
    
        public string TransactionIdGenerator(string bankId, string accountId)
        {
        return "TXN" + bankId + accountId + PresentDate.ToString("dd") + PresentDate.ToString("MM") + PresentDate.ToString("yyyy") + PresentDate.ToString("hh") + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");
        }


        public void Deposit(string bankId, string accountId, decimal amount,string currencyCode)
        {
            var UserInfo = DbContext.BankAccounts.SingleOrDefault(m => m.AccountId == accountId && m.BankId == bankId);
            var currentCurrency = DbContext.Curriencies.SingleOrDefault(x => x.CurrencyCode == currencyCode);
            if (UserInfo == null)
            {
                throw new InvalidUserException();
            }
            if(currentCurrency==null)
            {
               throw new InvalidCurrencyException();
            }
            Decimal newBanlance = currencyExchanger.Converter(amount, currentCurrency.CurrencyExchangeRate);
            
            UserInfo.Amount += newBanlance;
            DbContext.BankAccounts.Update(UserInfo);

            var newTransaction = new Transaction()
            {
                Id = TransactionIdGenerator(bankId, accountId),
                Amount = newBanlance,
                Type = "Credit",
                On = PresentDate.ToString("g"),
                BankId=bankId,
                AccountId=accountId
              


            };
            DbContext.Transactions.Add(newTransaction);
            DbContext.SaveChanges();
            
        }



        public void Withdraw(string bankId, string accountId, decimal amount)
        {
            var UserInfo = DbContext.BankAccounts.SingleOrDefault(m => m.AccountId == accountId && m.BankId == bankId);

            if (UserInfo == null)
            {
                throw new InvalidUserException();
            }
            if (UserInfo.Amount < amount)
            {
                throw new AmountNotSufficientException();
            }
            
            UserInfo.Amount -= amount;
            DbContext.BankAccounts.Update(UserInfo);
            var newTransaction =new Transaction()
            { 
                AccountId=accountId,
                BankId=bankId,
                Id = TransactionIdGenerator(bankId, accountId),
                Amount = amount,
                Type = "Debit",
                On =PresentDate.ToString("g")


            };
            DbContext.Transactions.Add(newTransaction);
            DbContext.SaveChanges();
            
        }

        public decimal TaxCalculator(string sourceBankId,string destinationBankId,decimal amount,string taxType)
        {
            decimal tax = 0;
            if(taxType=="IMPS")
            {
                if(sourceBankId==destinationBankId)
                {
                    tax = amount * (int)IMPSCharges.SameBank;
                }
                else
                {
                    tax = amount * (int)IMPSCharges.SameBank;
                }
            }
            else
            {
                if (sourceBankId == destinationBankId)
                {
                    tax = amount * (int)RTGSCharges.SameBank;

                }
                else
                {
                    tax = amount * (int)RTGSCharges.DifferentBank;
                }
            }
            return tax / 100;
        }

        public void Transfer(string userBankId, string userAccountId, decimal amount, string destinationBankId, string destinationAccountId,string taxType)

        {
            if (userBankId == destinationBankId && userAccountId == destinationAccountId)
            {
                throw new Exception("Self Transfer is not Allowed!");
            }

           
            amount = Math.Round(amount, 2);
            decimal tax = TaxCalculator(userBankId, destinationBankId, amount, taxType);
            tax = Math.Round(tax, 2);



            var UserInfo = DbContext.BankAccounts.SingleOrDefault(m => m.AccountId == userAccountId && m.BankId == userBankId);


            if (UserInfo ==null )
            {
                throw new InvalidCustomerNameException();

            }
            if(UserInfo.Amount < amount+tax)
            {
                throw new AmountNotSufficientException();
            }
            var beneInfo = DbContext.BankAccounts.SingleOrDefault(m => m.AccountId == destinationAccountId && m.BankId == destinationBankId);

            UserInfo.Amount -= amount+tax;
            beneInfo.Amount += amount;

            var userTransaction =new Transaction()
            {
                Id = TransactionIdGenerator(userBankId, userAccountId),
                Amount = amount,
                Type =  "Debit",
                On=PresentDate.ToString("g"),
                Tax=tax,
                TaxType=taxType,
                BankId=userBankId,
                AccountId=userBankId,
                DestinationBankId=destinationBankId,
                DestinationAccountId=destinationAccountId

            };
            DbContext.Transactions.Add(userTransaction);
           
            var beneTXN =new Transaction()
            {
                Id = TransactionIdGenerator(destinationBankId, destinationAccountId),
                Amount = amount,
                Type = "Credit" ,
                On = PresentDate.ToString("g"),
                Tax = tax,
                TaxType=taxType,
                BankId = userBankId,
                AccountId = userBankId,
                DestinationBankId = destinationBankId,
                DestinationAccountId = destinationAccountId

            };
            DbContext.Transactions.Add(beneTXN);
            DbContext.SaveChanges();
            
        }

        public List<Transaction> TransactionLog(string bankId, string accountId)
        {
            try
            {
                var info = DbContext.Transactions.Where(m => m.BankId == bankId && m.AccountId == accountId).ToList();

                List<Transaction> transactions = new List<Transaction>();

                foreach (var row in info)
                {
                    transactions.Add(row);
                }
                return transactions;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public decimal ViewBalance(string bankId,string accountId)
        {
            var info = DbContext.BankAccounts.SingleOrDefault(m => m.AccountId == accountId && m.BankId == bankId);
            if(info==null)
            {
                throw new InvalidCustomerNameException();
            }
            return info.Amount;
        }
    }
}

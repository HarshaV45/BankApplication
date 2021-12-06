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
        private BankDbContext DbContext = new BankDbContext();
       // public int limit = 50000;

        Random random = new Random();
        public TransactionService(Data data,CustomerService customer,CurrencyExchanger currencyExchanger)

        {
            
            this.currencyExchanger = currencyExchanger;
            this.customerService = customer;
        }
        public string TransactionIdGenerator(string bankId, string accountId)
        {
            return "TXN" + bankId + accountId + PresentDate.ToString("dd") + PresentDate.ToString("MM") + PresentDate.ToString("yyyy")+random.Next(1000,9999);
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
            amount = currencyExchanger.Converter(amount, currentCurrency.CurrencyExchangeRate);
            amount = Math.Round(amount, 2);
            UserInfo.Amount += amount;
            DbContext.BankAccounts.Update(UserInfo);

            var newTransaction = new Transaction()
            {
                Id = TransactionIdGenerator(bankId, accountId),
                Amount = amount,
                Type = TransactionType.Credit,
                On = PresentDate.ToString("g"),
                SourceBankId=bankId,
                SourceAccountId=accountId
              


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
            amount = Math.Round(amount, 2);
            UserInfo.Amount -= amount;
            var newTransaction =new Transaction()
            { 
                SourceAccountId=accountId,
                SourceBankId=bankId,
                Id = TransactionIdGenerator(bankId, accountId),
                Amount = amount,
                Type = TransactionType.Debit,
                On =PresentDate.ToString("g")


            };
            DbContext.Transactions.Add(newTransaction);
            DbContext.SaveChanges();
            
        }

        public decimal TaxCalculator(string sourceBankId,string destinationBankId,decimal amount,TaxType taxType)
        {
            decimal tax = 0;
            if(taxType==TaxType.IMPS)
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

        public void Transfer(string sourceBankId, string sourceAccountId, decimal amount, string destinationBankId, string destinationAccountId,TaxType taxType)

        {
        
           // BankAccount sourceAccount = this.customerService.AccountChecker(sourceBankId, sourceAccountId);
           // BankAccount destinationAccount = this.customerService.AccountChecker(destinationBankId, destinationAccountId);
            amount = Math.Round(amount, 2);
            decimal tax = TaxCalculator(sourceBankId, destinationBankId, amount, taxType);
            tax = Math.Round(tax, 2);



            var UserInfo = DbContext.BankAccounts.SingleOrDefault(m => m.AccountId == sourceAccountId && m.BankId == sourceBankId);


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
                Id = TransactionIdGenerator(sourceBankId, sourceAccountId),
                Amount = amount,
                Type = TransactionType.Debit,
                On=PresentDate.ToString("g"),
                Tax=tax,
                TaxType=taxType,
                SourceBankId=sourceBankId,
                SourceAccountId=sourceBankId,
                DestinationBankId=destinationBankId,
                DestinationAccountId=destinationAccountId

            };
           
            var beneTXN =new Transaction()
            {
                Id = TransactionIdGenerator(destinationBankId, destinationAccountId),
                Amount = amount,
                Type = TransactionType.Credit,
                On = PresentDate.ToString("g"),
                Tax = tax,
                TaxType=taxType,
                SourceBankId = sourceBankId,
                SourceAccountId = sourceBankId,
                DestinationBankId = destinationBankId,
                DestinationAccountId = destinationAccountId

            };
            
        }

        public List<List<string>> TransactionLog(string bankId, string accountId)
        {
            var info = DbContext.Transactions.Where(m => m.Id == "TXN1").Select(c => new { c.Id, c.SourceBankId, c.SourceAccountId, c.Amount, c.Tax,  c.TaxType, c.DestinationBankId, c.DestinationAccountId }).ToList();
            List<List<string>> transactions = new List<List<string>>();
            foreach(var row in info)
            {
                List<string> temp = new List<string>();
                string[] values = { row.Id, row.SourceBankId, row.SourceAccountId, row.Amount.ToString(), row.Tax.ToString(), row.TaxType.ToString(), row.DestinationBankId, row.DestinationAccountId };
                temp.AddRange(values);
                transactions.Add(temp);
            }
            return transactions;
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

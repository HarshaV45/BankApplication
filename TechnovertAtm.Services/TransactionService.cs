using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnovertAtm.Services.Interfaces;
using TechonovertAtm.Models;
using TechonovertAtm.Models.Enums;
using TechonovertAtm.Models.Exceptions;

namespace TechnovertAtm.Services
{
    public class TransactionService:ITransactionService
    {
        private AccountServices customerService;
        //private Data data;
        private CurrencyExchanger currencyExchanger;
        DateTime PresentDate = DateTime.Today;
        private BankDbContext _DbContext;
        private IAccountService accountService;
       // public int limit = 50000;

        Random random = new Random();
        public TransactionService(BankDbContext dbContext,IAccountService accountService)

        {
            _DbContext = dbContext;
            this.accountService = accountService;
        }

        public Transaction GetTransaction(string id)
        {
            return _DbContext.Transactions.SingleOrDefault(m => m.Id == id);
        }


        public Transaction AddTransaction(Transaction transaction)
        {
            try
            {
                _DbContext.Transactions.Add(transaction);
                _DbContext.SaveChanges();
                return transaction;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Transaction UpdateTransaction(Transaction transaction)
        {
            throw new Exception("Not Implemented");
        }

        public Transaction DeleteTransaction(Transaction transaction)
        {
            throw new Exception("Not Implemented");
        }

        public string TransactionIdGenerator(string bankId, string accountId)
        {
        return "TXN" + bankId + accountId + PresentDate.ToString("dd") + PresentDate.ToString("MM") + PresentDate.ToString("yyyy") + PresentDate.ToString("hh") + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");
        }


        public Transaction Deposit(string bankId, string accountId, decimal amount)
        {
            try
            {
                var info = accountService.GetAccount(bankId, accountId);
                info.Amount += amount;

                var newTransaction = new Transaction();
                newTransaction.Id = Guid.NewGuid().ToString();
                newTransaction.BankId = bankId;
                newTransaction.AccountId = accountId;
                newTransaction.Amount = amount;
                newTransaction.OnDate = PresentDate.ToString();
                newTransaction.TransactionType = TransactionType.Credit;

                AddTransaction(newTransaction);
                return newTransaction;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Transaction Withdraw(string bankId, string accountId, decimal amount)
        {
            try
            {
                var info = accountService.GetAccount(bankId, accountId);
                if (info.Amount <= amount)
                    throw new Exception("Insufficient funds");

                info.Amount -= amount;

                var newTransaction = new Transaction();
                newTransaction.Id = Guid.NewGuid().ToString();
                newTransaction.BankId = bankId;
                newTransaction.AccountId = accountId;
                newTransaction.Amount = amount;
                newTransaction.OnDate = PresentDate.ToString();
                newTransaction.TransactionType = TransactionType.Debit;

                AddTransaction(newTransaction);
                return newTransaction;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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



            var UserInfo = _DbContext.BankAccounts.SingleOrDefault(m => m.AccountId == userAccountId && m.BankId == userBankId);


            if (UserInfo ==null )
            {
                throw new InvalidCustomerNameException();

            }
            if(UserInfo.Amount < amount+tax)
            {
                throw new AmountNotSufficientException();
            }
            var beneInfo = _DbContext.BankAccounts.SingleOrDefault(m => m.AccountId == destinationAccountId && m.BankId == destinationBankId);

            UserInfo.Amount -= amount+tax;
            beneInfo.Amount += amount;

            var userTransaction =new Transaction()
            {
                Id = TransactionIdGenerator(userBankId, userAccountId),
                Amount = amount,
                TransactionType =  TransactionType.Debit,
                OnDate=PresentDate.ToString("g"),
                Tax=tax,
                TaxType=taxType,
                BankId=userBankId,
                AccountId=userBankId,
                DestinationBankId=destinationBankId,
                DestinationAccountId=destinationAccountId

            };
            _DbContext.Transactions.Add(userTransaction);
           
            var beneTXN =new Transaction()
            {
                Id = TransactionIdGenerator(destinationBankId, destinationAccountId),
                Amount = amount,
                TransactionType = TransactionType.Credit ,
                OnDate = PresentDate.ToString("g"),
                Tax = tax,
                TaxType=taxType,
                BankId = userBankId,
                AccountId = userBankId,
                DestinationBankId = destinationBankId,
                DestinationAccountId = destinationAccountId

            };
            _DbContext.Transactions.Add(beneTXN);
            _DbContext.SaveChanges();
            
        }

        public List<Transaction> GetAllTransactions(string bankId, string accountId)
        {
            try
            {
                var info = _DbContext.Transactions.Where(m => (m.BankId == bankId && m.AccountId == accountId) || (m.DestinationBankId == bankId && m.DestinationAccountId == accountId)).ToList();

                return info;
            }
            catch
            {
                throw new Exception("No Available Transactions");
            }
        }

        public List<Transaction> TransactionLog(string bankId, string accountId)
        {
            try
            {
                var info = _DbContext.Transactions.Where(m => m.BankId == bankId && m.AccountId == accountId).ToList();

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
            var info = _DbContext.BankAccounts.SingleOrDefault(m => m.AccountId == accountId && m.BankId == bankId);
            if(info==null)
            {
                throw new InvalidCustomerNameException();
            }
            return info.Amount;
        }
    }
}

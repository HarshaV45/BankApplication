using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechonovertAtm.Models;
using TechonovertAtm.Models.Enums;
using TechonovertAtm.Models.Exceptions;

namespace TechnovertAtm.Services
{
    public class StaffService
    {
       // private List<StaffAccount> staff;
        DateTime PresentDate = DateTime.Today;
        
        private BankServices bankService;
        private CustomerService customerService;
        private TransactionService transactionService;
        private BankDbContext DbContext;
        Random random = new Random();



        public StaffService(BankDbContext dbContext)
        {
            this.DbContext = dbContext;

        }

        public string PasswordGeneration(string name)
        {
            return name.First().ToString().ToUpper() + "@" + random.Next(1000, 9999) + name.Substring(1);

        }


        public string AccountIdPattern(string CustomerName)
        {
            if (CustomerName.Length < 3)
            {
                throw new InvalidCustomerNameException();
            }
            return CustomerName.Substring(0, 3) + PresentDate.ToString("dd") + PresentDate.ToString("MM") + PresentDate.ToString("yyyy") + PresentDate.ToString("mm")+PresentDate.ToString("ss");
        }

       

        public void CreateStaffAccount(string name)
        {
            try
            {
                var newStaff = new StaffAccount()
                {
                    Name = name,
                    Password = "STA" + name + "@123",
                    Id = name,

                };
                DbContext.Staff.Add(newStaff);
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public string[] CreateAccount(string name, string bankId,int age)
        {
            var newAccount = new BankAccount()
            {
                Name = name,
                BankId = bankId,
                Age = age,
                AccountId = AccountIdPattern(name),
                Password = PasswordGeneration(name),
                Amount = 0,
                
                
                
            };
            
            DbContext.BankAccounts.Add(newAccount);
            DbContext.SaveChanges();


            return new string[] { newAccount.AccountId, newAccount.Password };

        }


        public void Login(BankServices bankService, CustomerService customerService, TransactionService transactionService, string id,string password)
        {
            if (String.IsNullOrWhiteSpace(id) || String.IsNullOrWhiteSpace(password))
            {
               throw new InvalidInputException();
            }
            var staffAccount = DbContext.Staff.SingleOrDefault(m =>m.Id==id);
            
            if(staffAccount==null)
            {
                throw new InvalidUserException();
            }
            if(staffAccount.Password!=password)
            {
                throw new InvalidAccountPasswordException();
            }
            //this.data = data;
            this.bankService = bankService;
            this.customerService = customerService;
            this.transactionService = transactionService;

        }


        public BankAccount viewAccountDetails(string bankId, string accountId)
        {
            var account = DbContext.BankAccounts.SingleOrDefault(m => m.BankId == bankId && m.AccountId == accountId);
            if (account == null)
            {
                throw new InvalidUserException();
            }
            return account;
        }



        public void UpdateAccount(string accountId, string bankId, string newName,string newPassword,int? newAge=0)
        {
            var info = DbContext.BankAccounts.SingleOrDefault(m => m.AccountId == accountId && m.BankId == bankId);
            if (info == null)
            {
                throw new InvalidUserException(); 
            }
            if (newName != null)
            {
                info.Name = newName;
            }
            if (newPassword != null)
            {
                info.Password = newPassword;
            }
          
            //else if (String.IsNullOrEmpty(newGender + "") == false)
            //{
            //    info.Gender = (GenderType)newGender;
            //}
            DbContext.BankAccounts.Update(info);
            DbContext.SaveChanges();
        }


        public void DeleteAccount(string accountId,string bankId)
        {

            var info = DbContext.BankAccounts.SingleOrDefault(m => m.AccountId == accountId && m.BankId == bankId);
            if(accountId ==null)
            {
                throw new InvalidUserException();  
            }
            DbContext.BankAccounts.Remove(info);
            DbContext.SaveChanges();
        }



       //public List<List<Transaction>>viewTransactions(string bankId,string accountId)
       // {
       //     List<List<string>> result = this.transactionService.TransactionLog(bankId, accountId);

       //     if (result==null)
       //     {
       //         throw new InvalidUserException();
       //     }
       //     return result;

       // }

        public void AddNewCurrency( string name, string code, decimal exchangeRate)
        {
            var newCurrency = new Currency()
            {
                CurrencyName = name,
                CurrencyCode = code,
                CurrencyExchangeRate = exchangeRate
            };
            DbContext.Curriencies.Add(newCurrency);
            DbContext.SaveChanges();

        }


        public void RevertTransaction(string bankId,string accountId,string transactionId)
        {
            try
            {
                var userInfo = DbContext.BankAccounts.SingleOrDefault(m => m.BankId == bankId && m.AccountId == accountId);
                if (userInfo == null)
                    throw new Exception("Invalid Transaction Id ");

                var userTxn = DbContext.Transactions.SingleOrDefault(m => m.Id == transactionId && m.AccountId == accountId && m.BankId == bankId);
                if (userTxn == null)
                    throw new Exception("Invalid Transacrion Id ");

                var beneInfo = DbContext.BankAccounts.SingleOrDefault(m => m.BankId == userTxn.DestinationBankId && m.AccountId == userTxn.DestinationAccountId);
                if (beneInfo == null)
                    throw new Exception("Beneficiary Account was not Available!");

                string beneTxnId = "TXN" + userTxn.DestinationBankId + userTxn.DestinationAccountId + transactionId.Substring(29);

                var beneTxn = DbContext.Transactions.SingleOrDefault(m => m.Id == beneTxnId && m.BankId == userTxn.DestinationBankId && m.AccountId == userTxn.DestinationAccountId);

                if (userTxn.Type == "Debit")
                {
                    userInfo.Amount += userTxn.Amount;
                    beneInfo.Amount-= userTxn.Amount;
                }
                else
                {
                    userInfo.Amount -= userTxn.Amount;
                    beneInfo.Amount += userTxn.Amount;
                }

                DbContext.Transactions.Remove(userTxn);
                DbContext.Transactions.Remove(beneTxn);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        }


    }




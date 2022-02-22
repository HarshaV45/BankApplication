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
        Random rand = new Random();
        
        private BankServices bankService;
        private AccountServices customerService;
        private TransactionService transactionService;
        private BankDbContext _DbContext;
        



        public StaffService(BankDbContext dbContext)
        {
            _DbContext = dbContext;
            CreateStaffAccount("Admin");

        }

        public string PasswordGeneration(string name)
        {
            string temp = "";
            foreach(var letter in name)
            {
                if (letter != ' ')
                    temp += letter;
            }
            return temp.First().ToString().ToUpper() + temp.Substring(1, 4).ToLower().Trim() + "@" + rand.Next(1000, 9999);
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
            string newId = name;
            string newPassword = name + "@123";
            try
            {
                var newStaff = new StaffAccount()
                {
                    Name = name,
                    Password = newPassword,
                    Id = newId,

                };
                _DbContext.Staff.Add(newStaff);
                _DbContext.SaveChanges();
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
            
            _DbContext.BankAccounts.Add(newAccount);
            _DbContext.SaveChanges();

            return new string[] { newAccount.AccountId, newAccount.Password };

        }


        public void Login(BankServices bankService, AccountServices customerService, TransactionService transactionService, string id,string password)
        {
            if (String.IsNullOrWhiteSpace(id) || String.IsNullOrWhiteSpace(password))
            {
               throw new InvalidInputException();
            }
            var staffAccount = _DbContext.Staff.SingleOrDefault(m =>m.Id==id);
            
            if(staffAccount==null)
            {
                throw new Exception("Invalid Staff Id");
            }
            if(staffAccount.Password!=password)
            {
                throw new Exception("Invalid Password");
            }
            //this.data = data;
            this.bankService = bankService;
            this.customerService = customerService;
            this.transactionService = transactionService;

        }


        public BankAccount viewAccountDetails(string bankId, string accountId)
        {
            var account = _DbContext.BankAccounts.SingleOrDefault(m => m.BankId == bankId && m.AccountId == accountId);
            if (account == null)
            {
                throw new InvalidUserException();
            }
            return account;
        }



        public void UpdateAccount(string accountId, string bankId, string newName,string newPassword,int? newAge=0,string newGender=null)
        {
            try
            {
                var info = _DbContext.BankAccounts.SingleOrDefault(m => m.AccountId == accountId && m.BankId == bankId);
                if (info == null)
                {
                    throw new Exception("Invalid Details");
                }
                if (info.AccountStatus == AccountStatus.Closed)
                {
                    throw new Exception("Account was Closed! Can't Update the Account Details");
                }
                if (newAge != 0)
                {
                    info.Age = (int)newAge;
                }
                else if (String.IsNullOrEmpty(newName) == false)
                {
                    info.Name = newName;
                }
                else if (String.IsNullOrEmpty(newPassword) == false)
                {
                    info.Password = newPassword;
                }
                else if (String.IsNullOrEmpty(newGender) == false)
                {
                    info.Gender = GenderType.Male;
                }
                _DbContext.BankAccounts.Update(info);
                _DbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void DeleteAccount(string accountId,string bankId)
        {
            try
            {

                var info = _DbContext.BankAccounts.SingleOrDefault(m => m.AccountId == accountId && m.BankId == bankId);
                if (accountId == null)
                {
                    throw new InvalidUserException();
                }
                if (info.AccountStatus == AccountStatus.Closed)
                    throw new Exception("Account was already closed!");

                info.AccountStatus = AccountStatus.Closed;

                _DbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
            try
            {
                var newCurrency = new Currency()
                {
                    CurrencyName = name,
                    CurrencyCode = code,
                    CurrencyExchangeRate = exchangeRate
                };
                _DbContext.Curriencies.Add(newCurrency);
                _DbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void RevertTransaction(string bankId,string accountId,string transactionId)
        {
            try
            {
                var userInfo = _DbContext.BankAccounts.SingleOrDefault(m => m.BankId == bankId && m.AccountId == accountId);
                if (userInfo == null)
                    throw new Exception("Invalid Transaction Id ");

                var userTxn = _DbContext.Transactions.SingleOrDefault(m => m.Id == transactionId && m.AccountId == accountId && m.BankId == bankId);
                if (userTxn == null)
                    throw new Exception("Invalid Transacrion Id ");

                var beneInfo = _DbContext.BankAccounts.SingleOrDefault(m => m.BankId == userTxn.DestinationBankId && m.AccountId == userTxn.DestinationAccountId);
                if (beneInfo == null)
                    throw new Exception("Beneficiary Account was not Available!");

                string beneTxnId = "TXN" + userTxn.DestinationBankId + userTxn.DestinationAccountId + transactionId.Substring(29);

                var beneTxn = _DbContext.Transactions.SingleOrDefault(m => m.Id == beneTxnId && m.BankId == userTxn.DestinationBankId && m.AccountId == userTxn.DestinationAccountId);

                /*  if (userTxn.Type == "Debit")
                  {
                      userInfo.Amount += userTxn.Amount;
                      beneInfo.Amount-= userTxn.Amount;
                  }
                  else
                  {
                      userInfo.Amount -= userTxn.Amount;
                      beneInfo.Amount += userTxn.Amount;
                  }

                  _DbContext.Transactions.Remove(userTxn);
                  _DbContext.Transactions.Remove(beneTxn); */
                _DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        }


    }




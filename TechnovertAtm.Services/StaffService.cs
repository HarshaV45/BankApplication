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
        private List<StaffAccount> staff;
        DateTime PresentDate = DateTime.Today;
        private Data data;
        private BankServices bankService;
        private CustomerService customerService;
        private TransactionService transactionService;
        Random random = new Random();



        public StaffService()
        {
            this.staff = new List<StaffAccount>();

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

        public StaffAccount StaffFinder(string id)
        {
            return this.staff.SingleOrDefault(x => x.Id == id);
        }


        public void CreateStaffAccount(string name)
        {
            StaffAccount newStaff = new StaffAccount()
            {
                Name = name,
                Password = "STA" + name + "@123",
                Id = name,
               
            };
            this.staff.Add(newStaff);


        }


        public string[] CreateAccount(string name, string bankId)
        {
            BankAccount newAccount = new BankAccount()
            {
                Name = name,
                BankId = bankId,
                AccountId = AccountIdPattern(name),
                Password = PasswordGeneration(name),
                Amount = 0,
                Transactions = new List<Transaction>()
            };
            Bank bank = this.bankService.BankChecker(bankId);
            bank.BankAccounts.Add(newAccount);


            return new string[] { newAccount.AccountId, newAccount.Password };

        }


        public void Login(Data data,BankServices bankService, CustomerService customerService, TransactionService transactionService, string id,string password)
        {
            if (String.IsNullOrWhiteSpace(id) || String.IsNullOrWhiteSpace(password))
            {
               throw new InvalidInputException();
            }
            StaffAccount staffAccount = StaffFinder(id);
            if(staffAccount==null)
            {
                throw new InvalidUserException();
            }
            if(staffAccount.Password!=password)
            {
                throw new InvalidAccountPasswordException();
            }
            this.data = data;
            this.bankService = bankService;
            this.customerService = customerService;
            this.transactionService = transactionService;

        }


        public BankAccount viewAccountDetails(string bankId, string accountId)
        {
            BankAccount account = this.customerService.AccountChecker(bankId, accountId);
            if(account==null)
            {
                throw new InvalidUserException();
            }
            return account;
        }

       

        public void UpdateAccount(string accountId, string bankId, string newName,string newPassword)
        {
            BankAccount bankAccount = this.customerService.AccountChecker(bankId, accountId);
            if (bankAccount == null)
            {
                throw new InvalidUserException(); 
            }
            if (newName != null)
            {
                bankAccount.Name = newName;
            }
            if (newPassword != null)
            {
                bankAccount.Password = newPassword;
            }

        }


        public void DeleteAccount(string accountId,string bankId)
        {
            Bank bank =this.bankService.BankChecker(bankId);
            BankAccount bankAccount = this.customerService.AccountChecker(bankId, accountId);
            if(accountId ==null)
            {
                throw new InvalidUserException();  
            }
            bank.BankAccounts.Remove(bankAccount);
        }



        public List<Transaction>viewTransactions(string bankId,string accountId)
        {
            BankAccount bankAccount = this.customerService.AccountChecker(bankId, accountId);
            if(bankAccount==null)
            {
                throw new InvalidUserException();
            }
            return bankAccount.Transactions;

        }

        public void AddNewCurrency( string name, string code, decimal exchangeRate)
        {
            Currency newCurrency = new Currency()
            {
                CurrencyName = name,
                CurrencyCode = code,
                CurrencyExchangeRate = exchangeRate
            };
            this.data.currencies.Add(newCurrency);

        }


        public void RevertTransaction(string bankId,string accountId,string transactionId)
        {
            BankAccount bankAccount = this.customerService.AccountChecker(bankId, accountId);
            if(bankAccount==null)
            {
                throw new InvalidCustomerNameException();
            }
            List<Transaction> transactions = bankAccount.Transactions;
            if(transactions.Count==0)
            {
                throw new InsufficientTransactions();
            }
            Transaction revertTransaction = transactions.SingleOrDefault(x => x.Id == transactionId);
            if(revertTransaction==null)
            {
                throw new InvalidInputException();
            }
            transactions.Remove(revertTransaction);
            BankAccount destinationAccount = this.customerService.AccountChecker(revertTransaction.DestinationBankId, revertTransaction.DestinationAccountId);
            List<Transaction> destinationtransaction = destinationAccount.Transactions;
            Transaction destinationUndoTransaction = destinationtransaction.SingleOrDefault(x => x.Id == transactionId);
            destinationtransaction.Remove(destinationUndoTransaction);

            destinationAccount.Amount -= revertTransaction.Amount;
            bankAccount.Amount += revertTransaction.Amount;

        }



        


    }
}



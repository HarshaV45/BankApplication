using System;
using System.Collections.Generic;
using System.Text;
using TechonovertAtm.Models;
using TechonovertAtm.Models.Exceptions;

namespace TechnovertAtm.Services
{
    public class BankService
    {
        private List<Bank> banks;
        DateTime PresentDate = DateTime.Today;
        public BankService()
        {
            this.banks = new List<Bank>();

        }
        public bool InputChecker(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return true;
        }
       
    
        public string BankIdPattern(string bankName)
        {
            if (bankName.Length < 3)
            {
                throw new InvalidBankNameException();

            }
               return bankName.Substring(0, 3) + PresentDate.ToString("dd") + PresentDate.ToString("MM") + PresentDate.ToString("yyyy");
               
        }
        public string AccountIdPattern(string CustomerName)
        {
            if (CustomerName.Length < 3)
            {
                throw new InvalidCustomerNameException();
            }
            return CustomerName.Substring(0, 3) + PresentDate.ToString("dd") + PresentDate.ToString("MM") + PresentDate.ToString("yyyy");
        }

        public string TransactionIdGenerator(string bankId, string accountId)
        {
            return "TXN" + bankId + accountId + PresentDate.ToString("dd") + PresentDate.ToString("MM") + PresentDate.ToString("yyyy");
        }
    



        public Bank BankChecker(string bankName)
        {
            foreach(var d in banks)
            {
                string bankId = BankIdPattern(bankName);
                if(d.Id  == bankId)
                {
                    return d;
                }
            }
            return null;
        }

        public BankAccount AccountChecker(string bankName, string customerName)
        {
            Bank bank = BankChecker(BankIdPattern(bankName));
            foreach(var d in bank.BankAccounts)
            {
                if(d.Id==AccountIdPattern(customerName))
                {
                    return d;
                }
            }
            return null;
        }


        public void BankCreation(string bankName)
        {
            Bank newBank = new Bank()
            {
                Name = bankName,
                Id = BankIdPattern(bankName),
                BankAccounts = new List<BankAccount>()

            };
            this.banks.Add(newBank);
            
        }


       


        public void AccountCreation(string bankName, string CustomerName, string password)
        {
            string bankId = BankIdPattern(bankName);
            BankAccount account = new BankAccount()
            {
                Id = AccountIdPattern(CustomerName),
                Password = password,
                Amount = 0,
                Transactions = new List<Tranaction>()
            };
            Bank bank = BankChecker(bankId);
            bank.BankAccounts.Add(account);

        }


        public bool BankLogin(string bankName)
        {
            foreach (var d in banks)
            {
                if (d.Id == BankIdPattern(bankName))
                {
                    return true;
                }
            }
            return false;
        }



        public bool AccountLogin(string bankName, string customerName, string password)
        {
            if(InputChecker(BankIdPattern( bankName)) && InputChecker(AccountIdPattern(customerName)))
            {
                BankAccount user = AccountChecker(BankIdPattern( bankName),AccountIdPattern(customerName));
                if(user ==null || user.Password!=password)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
         

        public bool Deposit(string bankName,string customerName, decimal amount)
        {
            string bankId = BankIdPattern(bankName);
            string accountId = AccountIdPattern(customerName);
            if(InputChecker(bankId) && InputChecker(accountId))
            {
                BankAccount user = AccountChecker(bankId, accountId);
                if(user == null)
                {
                    throw new InvalidUserException();
                }
                user.Amount += amount;
                user.Transactions.Add(new Tranaction()
                {
                    Id = TransactionIdGenerator(bankId, accountId),
                    Amount = amount,
                    Type = TransactionType.Credit


                });
                return true;

            }
            return false;
        }



        public bool Withdraw(string bankName, string customerName, decimal amount)
        {
            string bankId = BankIdPattern(bankName);
            string accountId = AccountIdPattern(customerName);
            if (InputChecker(bankId) && InputChecker(accountId))
            {
                BankAccount user = AccountChecker(bankId, accountId);
                if (user == null )
                {
                    throw new InvalidUserException();
                }
                if(user.Amount < amount)
                {
                    throw new AmountNotSufficientException();
                }
                user.Amount -= amount;
                user.Transactions.Add(new Tranaction()
                {
                    Id = TransactionIdGenerator(bankId, accountId),
                    Amount = amount,
                    Type = TransactionType.Debit


                });
                return true;

            }
            return false;
        }



        public bool Transfer(string sourceBankName, string sourceCustomerName,decimal amount,string destinationBankName,string destinationAccountNumber)

        {
            string sourceBankId = BankIdPattern(sourceBankName);
            string sourceAccountId = AccountIdPattern(sourceCustomerName);
            string destinationBankId = BankIdPattern(destinationBankName);
            string destinationAccountId = AccountIdPattern(destinationAccountNumber);
            if(InputChecker(sourceAccountId) && InputChecker(destinationAccountId)&& InputChecker(sourceBankId))
            {
                BankAccount sourceAccount = AccountChecker(sourceBankId, sourceAccountId);
                BankAccount destinationAccount = AccountChecker(destinationBankId, destinationAccountId);
                if(sourceAccount==null || destinationAccount == null || sourceAccount.Amount<amount)
                {
                    return false;
                    
                }
                sourceAccount.Amount -= amount;
                sourceAccount.Transactions.Add(new Tranaction()
                {
                    Id = TransactionIdGenerator(sourceBankId, sourceAccountId),
                    Amount = amount,
                    Type = TransactionType.Debit

                });
                destinationAccount.Amount += amount;
                destinationAccount.Transactions.Add(new Tranaction()
                {
                    Id = TransactionIdGenerator(destinationBankId, destinationAccountId),
                    Amount = amount,
                    Type = TransactionType.Credit
                });
            }
            return true;
        }



        public List<Tranaction> TransactionLog(string bankName,string customerName)
        {
            string bankId = BankIdPattern(bankName);
            string accountId = AccountIdPattern(customerName);
            List<Tranaction> transactions = new List<Tranaction>();
            foreach(var d in banks)
            {
                if(d.Id==bankId)
                {
                    foreach(var k in d.BankAccounts)
                    {
                        if(k.Id == accountId)
                        {
                            transactions = k.Transactions;
                        }
                    }
                }
            }
            return transactions;
        }








    }
}













































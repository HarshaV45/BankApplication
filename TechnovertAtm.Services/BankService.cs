using System;
using System.Collections.Generic;
using System.Text;
using TechonovertAtm.Models;

namespace TechnovertAtm.Services
{
    public class BankService
    {
        private List<Banks> banks; 
        DateTime PresentDate = DateTime.Today;
        public BankService() 
        {
            this.banks = new List<Banks>();

        }
        public bool InputChecker(string input)
        {
            if(string.IsNullOrEmpty(input))
            {
                return false;
            }
            return true;
        }
        public string  BankIdPattern(string bankName)
        {
            return bankName.Substring(0, 3) + PresentDate.ToString("dd") + PresentDate.ToString("MM") + PresentDate.ToString("yyyy");

        }
        public string AccountIdPattern(string CustomerName)
        {
            return CustomerName.Substring(0, 3) + PresentDate.ToString("dd") + PresentDate.ToString("MM") + PresentDate.ToString("yyyy");
        }

        public string TransactionIdGenerator(string bankId,string accountId)
        {
            return "TXN" + bankId + accountId + PresentDate.ToString("dd") + PresentDate.ToString("MM") + PresentDate.ToString("yyyy");
        }



        public Banks BankChecker(string bankName)
        {
            foreach(var d in banks)
            {
                string bankId = BankIdPattern(bankName);
                if(d.Id == bankId)
                {
                    return d;
                }
            }
            return null;
        }

        public BankAccounts AccountChecker(string bankName, string accountNumber)
        {
            Banks bank = BankChecker(BankIdPattern(bankName));
            foreach(var d in bank.BankAccounts)
            {
                if(d.Id==AccountIdPattern(accountNumber))
                {
                    return d;
                }
            }
            return null;
        }


        public string BankCreation(string bankName)
        {
            Banks newBank = new Banks()
            {
                Name = bankName,
                Id = BankIdPattern(bankName),
                BankAccounts = new List<BankAccounts>()

            };
            this.banks.Add(newBank);
            return newBank.Id;
        }


       


        public void AccountCreation(string bankName, string CustomerName, string password)
        {
            string bankId = BankIdPattern(bankName);
            BankAccounts account = new BankAccounts()
            {
                Id = AccountIdPattern(CustomerName),
                Password = password,
                Amount = 0,
                Transactions = new List<Transactions>()
            };
            Banks bank = BankChecker(bankId);
            bank.BankAccounts.Add(account);

        }


        public bool IsBankPresent(string bankName)
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



        public bool IsAccountPresent(string bankName, string accountNumber, string password)
        {
            if(InputChecker(BankIdPattern( bankName)) && InputChecker(AccountIdPattern( accountNumber)))
            {
                BankAccounts user = AccountChecker(BankIdPattern( bankName),AccountIdPattern( accountNumber));
                if(user ==null || user.Password!=password)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
         

        public bool Deposit(string bankName,string accountNumber,decimal amount)
        {
            string bankId = BankIdPattern(bankName);
            string accountId = AccountIdPattern(accountNumber);
            if(InputChecker(bankId) && InputChecker(accountId))
            {
                BankAccounts user = AccountChecker(bankId, accountId);
                if(user == null)
                {
                    return false;
                }
                user.Amount += amount;
                user.Transactions.Add(new Transactions()
                {
                    Id = TransactionIdGenerator(bankId, accountId),
                    Amount = amount,
                    Type = TransactionType.Credit


                });
                return true;

            }
            return false;
        }



        public bool Withdraw(string bankName, string accountNumber, decimal amount)
        {
            string bankId = BankIdPattern(bankName);
            string accountId = AccountIdPattern(accountNumber);
            if (InputChecker(bankId) && InputChecker(accountId))
            {
                BankAccounts user = AccountChecker(bankId, accountId);
                if (user == null || user.Amount<amount)
                {
                    return false;
                }
                user.Amount -= amount;
                user.Transactions.Add(new Transactions()
                {
                    Id = TransactionIdGenerator(bankId, accountId),
                    Amount = amount,
                    Type = TransactionType.Debit


                });
                return true;

            }
            return false;
        }



        public bool Transfer(string sourceBankName, string sourceAccountNumber,decimal amount,string destinationBankName,string destinationAccountNumber)

        {
            string sourceBankId = BankIdPattern(sourceBankName);
            string sourceAccountId = AccountIdPattern(sourceAccountNumber);
            string destinationBankId = BankIdPattern(destinationBankName);
            string destinationAccountId = AccountIdPattern(destinationAccountNumber);
            if(InputChecker(sourceAccountId) && InputChecker(destinationAccountId)&& InputChecker(sourceBankId))
            {
                BankAccounts sourceAccount = AccountChecker(sourceBankId, sourceAccountId);
                BankAccounts destinationAccount = AccountChecker(destinationBankId, destinationAccountId);
                if(sourceAccount==null || destinationAccount == null || sourceAccount.Amount<amount)
                {
                    return false;
                    
                }
                sourceAccount.Amount -= amount;
                sourceAccount.Transactions.Add(new Transactions()
                {
                    Id = TransactionIdGenerator(sourceBankId, sourceAccountId),
                    Amount = amount,
                    Type = TransactionType.Debit

                });
                destinationAccount.Amount += amount;
                destinationAccount.Transactions.Add(new Transactions()
                {
                    Id = TransactionIdGenerator(destinationBankId, destinationAccountId),
                    Amount = amount,
                    Type = TransactionType.Credit
                });
            }
            return true;
        }



        public List<Transactions> TransactionLog(string bankName,string accountNumber)
        {
            string bankId = BankIdPattern(bankName);
            string accountId = AccountIdPattern(accountNumber);
            List<Transactions> transactions = new List<Transactions>();
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













































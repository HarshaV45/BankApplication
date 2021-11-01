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
        private Data data;
        private CurrencyExchanger currencyExchanger;
        DateTime PresentDate = DateTime.Today;
        public int limit = 50000;

        Random random = new Random();
        public TransactionService(Data data,CustomerService customer,CurrencyExchanger currencyExchanger)

        {
            this.data = data;
            this.currencyExchanger = currencyExchanger;
            this.customerService = customer;
        }
        public string TransactionIdGenerator(string bankId, string accountId)
        {
            return "TXN" + bankId + accountId + PresentDate.ToString("dd") + PresentDate.ToString("MM") + PresentDate.ToString("yyyy")+random.Next(1000,9999);
        }


        public void Deposit(string bankId, string accountId, decimal amount,string currencyCode)
        {
            BankAccount user = this.customerService.AccountChecker(bankId, accountId);
            Currency currentCurrency = this.data.currencies.SingleOrDefault(x => x.CurrencyCode == currencyCode);
            if (user == null)
            {
                throw new InvalidUserException();
            }
            if(currentCurrency==null)
            {
               throw new InvalidCurrencyException();
            }
            amount = currencyExchanger.Converter(amount, currentCurrency.CurrencyExchangeRate);
            amount = Math.Round(amount, 2);
            user.Amount += amount;
            user.Transactions.Add(new Transaction()
            {
                Id = TransactionIdGenerator(bankId, accountId),
                Amount = amount,
                Type = TransactionType.Credit,
                On =PresentDate.ToString("g")
                
              


            });
            
        }



        public void Withdraw(string bankId, string accountId, decimal amount)
        {
            BankAccount user = this.customerService.AccountChecker(bankId, accountId);
            if (user == null)
            {
                throw new InvalidUserException();
            }
            if (user.Amount < amount)
            {
                throw new AmountNotSufficientException();
            }
            amount = Math.Round(amount, 2);
            user.Amount -= amount;
            user.Transactions.Add(new Transaction()
            {
                Id = TransactionIdGenerator(bankId, accountId),
                Amount = amount,
                Type = TransactionType.Debit,
                On =PresentDate.ToString("g")


            });
            
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
        
            BankAccount sourceAccount = this.customerService.AccountChecker(sourceBankId, sourceAccountId);
            BankAccount destinationAccount = this.customerService.AccountChecker(destinationBankId, destinationAccountId);
            amount = Math.Round(amount, 2);
            decimal tax = TaxCalculator(sourceBankId, destinationBankId, amount, taxType);
            tax = Math.Round(tax, 2);

            



            if (sourceAccount == null || destinationAccount == null )
            {
                throw new InvalidCustomerNameException();

            }
            if(sourceAccount.Amount < amount+tax)
            {
                throw new AmountNotSufficientException();
            }
            sourceAccount.Amount -= amount+tax;
            sourceAccount.Transactions.Add(new Transaction()
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

            });
            destinationAccount.Amount += amount;
            destinationAccount.Transactions.Add(new Transaction()
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

            });
            
        }

        public List<Transaction> TransactionLog(string bankId, string accountId)
        {
            BankAccount account = this.customerService.AccountChecker(bankId, accountId);
            return account.Transactions;

        }

        public decimal ViewBalance(string bankId,string accountId)
        {
            BankAccount bankAccount = this.customerService.AccountChecker(bankId, accountId);
            if(bankAccount==null)
            {
                throw new InvalidCustomerNameException();
            }
            return bankAccount.Amount;
        }
    }
}

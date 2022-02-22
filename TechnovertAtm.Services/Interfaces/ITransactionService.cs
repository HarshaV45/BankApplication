using System;
using System.Collections.Generic;
using System.Text;
using TechonovertAtm.Models;

namespace TechnovertAtm.Services.Interfaces
{
   public  interface ITransactionService
    {
        public Transaction GetTransaction(string id);
        public Transaction AddTransaction(Transaction transaction);
        public Transaction UpdateTransaction(Transaction transaction);
        public Transaction DeleteTransaction(Transaction transaction);
        public List<Transaction> GetAllTransactions(string bankId,string accountId);
        public Transaction Deposit(string bankId, string accountId, decimal amount);
        public Transaction Withdraw(string bankId, string accountId, decimal amount);
    }
}

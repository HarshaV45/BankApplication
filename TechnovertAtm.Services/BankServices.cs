using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechonovertAtm.Models;
using TechonovertAtm.Models.Exceptions;


namespace TechnovertAtm.Services
{
    public class BankServices
    {
       
        DateTime PresentDate = DateTime.Today;
        private BankDbContext DbContext ;
        public BankServices(BankDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public string BankIdPattern(string bankName)
        {
            if (bankName.Length < 3)
            {
                throw new InvalidBankNameException();

            }
            return bankName.Substring(0, 3) + PresentDate.ToString("dd") + PresentDate.ToString("MM") + PresentDate.ToString("yyyy") + PresentDate.ToString("T");

        }


        public void BankChecker(string bankId) 
        {
            var info = DbContext.Banks.SingleOrDefault(m => m.BankId == bankId);
            if (info == null)
            {
                throw new BankNotPresentException();
            }
        }

        public void BankCreation(string bankName,string description)
        {
            var newBank = new Bank()
            {
                Name = bankName,
                BankId = bankName+123,
                Description =description
               
            };
            try
            {
                DbContext.Banks.Add(newBank);
                DbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }

}

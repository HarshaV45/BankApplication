using System;
using System.Collections.Generic;
using System.Text;
using TechonovertAtm.Models;
using TechonovertAtm.Models.Exceptions;


namespace TechnovertAtm.Services
{
    public class BankServices
    {
        private Data bankdata;
        DateTime PresentDate = DateTime.Today;
        private BankDbContext DbContext = new BankDbContext();
        public BankServices(Data bankdata)
        {
            this.bankdata = bankdata;
        }

        public string BankIdPattern(string bankName)
        {
            if (bankName.Length < 3)
            {
                throw new InvalidBankNameException();

            }
            return bankName.Substring(0, 3) + PresentDate.ToString("dd") + PresentDate.ToString("MM") + PresentDate.ToString("yyyy") + PresentDate.ToString("T");

        }


        public Bank BankChecker(string bankId)
        {
            foreach (var d in DbContext.Banks)
            {
                if (d.BankId == bankId)
                {
                    return d;
                }
            }
            throw new BankNotPresentException();
        }

        public void BankCreation(string bankName)
        {
            var newBank = new Bank()
            {
                Name = bankName,
                BankId = bankName+123,
               
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

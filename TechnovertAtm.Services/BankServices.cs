using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnovertAtm.Services.Interfaces;
using TechonovertAtm.Models;
using TechonovertAtm.Models.Exceptions;
using TechonovertAtm.Models.Enums;


namespace TechnovertAtm.Services
{
    public class BankServices : IBankService
    {

        DateTime PresentDate = DateTime.Today;
        private BankDbContext _DbContext;
        public BankServices(BankDbContext dbContext)
        {
            _DbContext = dbContext;
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
            var info = _DbContext.Banks.SingleOrDefault(m => m.BankId == bankId);
            if (info == null)
            {
                throw new BankNotPresentException();
            }
        }

        public Bank BankCreation(Bank bank)
        {
            try
            {
                bank.BankId = BankIdPattern(bank.Name);
                var duplicateBank = _DbContext.Banks.FirstOrDefault(m => m.Name == bank.Name && m.Description == bank.Description);
                if (duplicateBank != null)
                    throw new Exception("Bank already exists!");
                _DbContext.Banks.Add(bank);
                _DbContext.SaveChanges();
                return bank;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Bank GetBank(string bankId)
        {
            var bank = _DbContext.Banks.FirstOrDefault(m => m.BankId == bankId);
            if (bank == null)
                throw new Exception("Bank Not Found!");
            if(bank.BankStatus ==Status.Closed)
                throw new Exception("Bank was Closed!");  
               
            return bank;
        }

        public List<Bank> GetAllBAnks()
        {
            return _DbContext.Banks.Where(m => m.BankStatus == Status.Active).ToList();
        }

        public Bank UpdateBank(Bank bank)
        {
            _DbContext.Banks.Attach(bank);
            _DbContext.SaveChanges();
            var UpdatedBank = _DbContext.Banks.FirstOrDefault(m => m.BankId == bank.BankId);
            return UpdatedBank;
        }


        public Bank CloseBank(string bankId)
        {
            try
            {
                var bankToDelete = _DbContext.Banks.SingleOrDefault(m => m.BankId == bankId);
                if (bankToDelete == null)
                    throw new Exception("Invalid Bank Id");
                if (bankToDelete.BankStatus == Status.Closed)
                    throw new Exception("Bank Already Closed!");
                bankToDelete.BankStatus = Status.Closed;
                _DbContext.SaveChanges();
                return bankToDelete;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}

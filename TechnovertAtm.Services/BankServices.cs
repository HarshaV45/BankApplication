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
            foreach (var d in bankdata.banks)
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
            Bank newBank = new Bank()
            {
                Name = bankName,
                BankId = bankName+123,
                BankAccounts = new List<BankAccount>()

            };
            this.bankdata.banks.Add(newBank);

        }
    }

}

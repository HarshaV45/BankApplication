using System;
using System.Collections.Generic;
using System.Text;
using TechonovertAtm.Models;

namespace TechnovertAtm.Services.Interfaces
{
    public interface IBankService
    {
        public Bank BankCreation(Bank bank);
        public Bank UpdateBank(Bank bank);
        public Bank CloseBank(string BankId);
        public Bank GetBank(string bankId);

        public List<Bank> GetAllBAnks();
    }
}

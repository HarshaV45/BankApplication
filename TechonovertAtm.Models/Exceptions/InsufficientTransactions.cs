using System;
using System.Collections.Generic;
using System.Text;

namespace TechonovertAtm.Models.Exceptions
{
    public class InsufficientTransactions:Exception
    {
        public override string Message
        {
            get
            {
                return "There is no transactions present in the account, please perform some transactions to use this feature";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TechonovertAtm.Models.Exceptions
{
    public class InvalidBankNameException : Exception
    {
        public override string Message
        {
            get
            {
                return "Bank Name should contain more than 3 letters";
            }
        }
    }
}
    

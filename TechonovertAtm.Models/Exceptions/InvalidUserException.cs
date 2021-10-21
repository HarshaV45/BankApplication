using System;
using System.Collections.Generic;
using System.Text;

namespace TechonovertAtm.Models.Exceptions
{
    public class InvalidUserException : Exception
    {
        public override string Message
        {
            get
            {
                return "Account not present, Please enter valid account details ";
            }
        }
    }
}
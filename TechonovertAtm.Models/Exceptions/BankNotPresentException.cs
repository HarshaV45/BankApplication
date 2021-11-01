using System;
using System.Collections.Generic;
using System.Text;

namespace TechonovertAtm.Models.Exceptions
{
    public class BankNotPresentException:Exception
    {
        public override string Message
        {
            get
            {  
                return "Bank detials not present ,Please enter valid details";
            }
        }
    }
}

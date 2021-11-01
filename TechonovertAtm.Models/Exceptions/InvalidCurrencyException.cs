using System;
using System.Collections.Generic;
using System.Text;

namespace TechonovertAtm.Models.Exceptions
{
    public class InvalidCurrencyException:Exception
    {
        public override string Message
        {
            get
            {
                
                return "Currency not Available for Transfer,Please enter valid currency";
            }
        }
    }
}

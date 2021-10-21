using System;
using System.Collections.Generic;
using System.Text;

namespace TechonovertAtm.Models.Exceptions
{
    public class AmountNotSufficientException:Exception
    {
        public override string Message
        {
            get
            {
                return "Amount not sufficient to withdraw ";
            }
        }

    }
}

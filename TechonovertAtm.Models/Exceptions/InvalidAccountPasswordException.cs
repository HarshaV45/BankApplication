using System;
using System.Collections.Generic;
using System.Text;

namespace TechonovertAtm.Models.Exceptions
{
    public class InvalidAccountPasswordException:Exception
    {
        public override string Message
        {
            get
            {
                return "invalid Password,Please enter valid Password ";
            }
        }
    }
}

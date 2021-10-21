using System;
using System.Collections.Generic;
using System.Text;

namespace TechonovertAtm.Models.Exceptions
{
    public class InvalidCustomerNameException:Exception
    {
        public override string Message
        {
            get
            {
                return "Customer Name should be more than 3 words";
            }
        }
    }
}

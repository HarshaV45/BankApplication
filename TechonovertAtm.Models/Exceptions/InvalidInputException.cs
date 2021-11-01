using System;
using System.Collections.Generic;
using System.Text;

namespace TechonovertAtm.Models.Exceptions
{
    public class InvalidInputException:Exception
    {
        public override string Message
        {
            get
            {
                
                return "Invalid Input Format, Please enter valid Input";
            }
        }


    }
}

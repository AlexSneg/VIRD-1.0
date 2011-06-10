using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Exceptions
{
    public class InvalidParameterException : ApplicationException
    {
        public InvalidParameterException() : base()
        {}

        public InvalidParameterException(string message) :
            base (message)
        {}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Exceptions
{
    public class InterruptOperationException : ApplicationException
    {
        public InterruptOperationException()
            : base()
        { }

        public InterruptOperationException(string message) :
            base(message)
        { }

    }
}

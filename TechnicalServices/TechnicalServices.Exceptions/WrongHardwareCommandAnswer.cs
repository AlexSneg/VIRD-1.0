using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Exceptions
{
    public class WrongHardwareCommandAnswer : ApplicationException
    {
        public WrongHardwareCommandAnswer()
            : base()
        {
        }
        public WrongHardwareCommandAnswer(string message) :
            base (message)
        {}
    }
}

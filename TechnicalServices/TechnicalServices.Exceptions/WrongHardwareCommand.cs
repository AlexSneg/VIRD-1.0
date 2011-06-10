using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Exceptions
{
    public class WrongHardwareCommand : ApplicationException
    {
        public WrongHardwareCommand()
            : base()
        {
        }
        public WrongHardwareCommand(string message) :
            base (message)
        {}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Exceptions
{
    public class WrongAgentUIDException : ApplicationException
    {
        public WrongAgentUIDException()
            : base("Неверный идентификатор агента")
        {}
    }
}

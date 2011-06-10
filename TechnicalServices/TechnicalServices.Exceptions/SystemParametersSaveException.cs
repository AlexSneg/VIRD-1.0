using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TechnicalServices.Exceptions
{
    [Serializable]
    public class SystemParametersSaveException : ApplicationException
    {
        public SystemParametersSaveException(string message)
            : base(message)
        {

        }

        public SystemParametersSaveException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

    }
}

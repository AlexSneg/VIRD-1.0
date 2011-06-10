using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TechnicalServices.Exceptions
{
    public class InvalideFileException : ApplicationException
    {
        public InvalideFileException(string message)
            : base(message)
        { }

        public InvalideFileException()
            : base()
        { }

        public InvalideFileException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        public InvalideFileException(string message, Exception innerException)
            : base(message, innerException)
        { }

    }
}

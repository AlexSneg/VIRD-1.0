using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TechnicalServices.Exceptions
{
    /// <summary>
    /// ексепшн - если добавлен ресурс не поддерживаемый сорсом
    /// </summary>
    public class InvalidResourceException : ApplicationException
    {
        public InvalidResourceException(string message) : base(message)
        {}

        public InvalidResourceException() : base()
        {}

        public InvalidResourceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {}

        public InvalidResourceException(string message, Exception innerException)
            : base(message, innerException)
        {}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TechnicalServices.Exceptions
{
    [Serializable]
    public class LabelUsedInPresentationException : ApplicationException
    {
        //private List<string> messageList { get; set; }

        public LabelUsedInPresentationException(string message) : base(message)
        {

        }

        public LabelUsedInPresentationException(SerializationInfo info, StreamingContext context) :base(info, context)
        {

        }

        
    }
}

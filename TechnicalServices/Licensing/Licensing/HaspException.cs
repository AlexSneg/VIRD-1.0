using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Aladdin.HASP;

namespace TechnicalServices.Licensing
{
    public class HaspException : Exception
    {
        public HaspException()
        {
        }

        public HaspException(string message) : base(message)
        {
        }

        public HaspException(string message, Exception inner) : base(message, inner)
        {
        }

        internal HaspException(HaspStatus haspStatus, string message) : this(message)
        {
            _haspStatus = Convert.ToInt32(haspStatus);
        }

        protected HaspException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _haspStatus = info.GetInt32("_haspStatus");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_haspStatus", _haspStatus);

            base.GetObjectData(info, context);
        }

        public override string ToString()
        {
            return string.Format("Message: {0} Hasp Status: {1}", Message, HaspStatus);
        }

        public int? HaspStatus
        {
            get { return _haspStatus; }
        }

        private readonly int? _haspStatus;
    }
}

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace TechnicalServices.Licensing
{
    public class LicenseInvalidException : Exception
    {
        public LicenseInvalidException()
        {
        }

        public LicenseInvalidException(string message) : base(message)
        {
        }

        public LicenseInvalidException(string message, Exception inner) : base(message, inner)
        {
        }

        internal LicenseInvalidException(LicenseInvalidReason reason, string message) : this(message)
        {
            _reason = reason;
        }

        protected LicenseInvalidException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            int reason = info.GetInt32("_reason");
            if (Enum.IsDefined(typeof (LicenseInvalidReason), reason))
            {
                _reason = (LicenseInvalidReason)reason;
            }
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_reason", _reason);

            base.GetObjectData(info, context);
        }

        public override string ToString()
        {
            return string.Format("Message: {0} License Invalid Reason: {1}", Message, Reason);
        }

        public LicenseInvalidReason Reason
        {
            get { return _reason; }
        }

        private readonly LicenseInvalidReason _reason;
    }
}

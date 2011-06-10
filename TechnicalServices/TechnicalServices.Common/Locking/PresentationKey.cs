using System;
using System.Diagnostics;


namespace TechnicalServices.Common.Locking
{
    [Serializable]
    public class PresentationKey : ObjectKey
    {
        private readonly string _presentationName;

        public PresentationKey(string presentationName)
        {
            _presentationName = presentationName;
        }

        public string PresentationName
        {
            [DebuggerStepThrough]
            get { return _presentationName; }
        }

        public override bool Equals(ObjectKey other)
        {
            PresentationKey value = other as PresentationKey;
            if (value != null && value.GetType() == this.GetType())
            {
                return PresentationName.Equals(value.PresentationName);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.PresentationName.GetHashCode();
        }

        [DebuggerStepThrough]
        public override string ToString()
        {
            return PresentationName;
        }
    }
}
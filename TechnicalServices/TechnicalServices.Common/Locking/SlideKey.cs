using System;
using System.Diagnostics;
using System.Globalization;

namespace TechnicalServices.Common.Locking
{
    [Serializable]
    public class SlideKey : ObjectKey
    {
        private readonly ObjectKey _presentationKey;
        private readonly int _id;

        public SlideKey(ObjectKey presentationKey, int slideId)
        {
            _presentationKey = presentationKey;
            _id = slideId;
        }

        public SlideKey(string presentationName, int slideId)
        {
            _presentationKey = ObjectKeyCreator.CreatePresentationKey(presentationName);
            _id = slideId;
        }

        public ObjectKey PresentationKey
        {
            [DebuggerStepThrough]
            get { return _presentationKey; }
        }

        public int Id
        {
            [DebuggerStepThrough]
            get { return _id; }
        }

        public override bool Equals(ObjectKey other)
        {
            SlideKey value = other as SlideKey;
            if (value != null && other.GetType() == this.GetType())
            {
                return PresentationKey.Equals(value.PresentationKey) &&
                    Id == value.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return string.Format(CultureInfo.InvariantCulture, 
                "{0}{1}", PresentationKey.ToString(), Id).GetHashCode();
        }
    }
}

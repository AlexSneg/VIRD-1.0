using System;
using System.Diagnostics;
using System.Globalization;
using TechnicalServices.Entity;

namespace TechnicalServices.Entity
{
    [Serializable]
    public class SlideKey : ObjectKey, IEquatable<SlideKey>
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
            _presentationKey = new PresentationKey(presentationName);
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

        public override ObjectType GetObjectType()
        {
            return ObjectType.Slide;
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

        public bool Equals(SlideKey other)
        {
            return PresentationKey.Equals(other.PresentationKey) &&
                   Id == other.Id;
        }

        #region object override

        public override int GetHashCode()
        {
            return string.Format(CultureInfo.InvariantCulture, 
                                 "{0}{1}", PresentationKey.ToString(), Id).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            SlideKey other = obj as SlideKey;
            if (other == null) return base.Equals(obj);
            return this.Equals(other);
        }

        #endregion
    }
}
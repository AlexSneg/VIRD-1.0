using System;
using System.Diagnostics;
using TechnicalServices.Entity;


namespace TechnicalServices.Entity
{
    [Serializable]
    public class PresentationKey : ObjectKey, IEquatable<PresentationKey>
    {
        private readonly string _presentationUniqueName;

        public PresentationKey(string presentationUniqueName)
        {
            _presentationUniqueName = presentationUniqueName;
        }

        public string PresentationUniqueName
        {
            [DebuggerStepThrough]
            get { return _presentationUniqueName; }
        }

        public override ObjectType GetObjectType()
        {
            return ObjectType.Presentation;
        }

        public override bool Equals(ObjectKey other)
        {
            PresentationKey value = other as PresentationKey;
            if (value != null && value.GetType() == this.GetType())
            {
                return PresentationUniqueName.Equals(value.PresentationUniqueName);
            }
            return false;
        }

        public bool Equals(PresentationKey other)
        {
            return PresentationUniqueName.Equals(other.PresentationUniqueName,
                StringComparison.InvariantCultureIgnoreCase);
        }

        [DebuggerStepThrough]
        public override string ToString()
        {
            return PresentationUniqueName;
        }

        #region object override
        public override int GetHashCode()
        {
            return this.PresentationUniqueName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            PresentationKey other = obj as PresentationKey;
            if (other == null) return base.Equals(obj);
            return this.Equals(other);
        }
        #endregion
    }
}
using System;

namespace TechnicalServices.Common.Locking
{
    [Serializable]
    public abstract class ObjectKey : IEquatable<ObjectKey>
    {
        #region IEquatable<ObjectKey> Members

        public abstract bool Equals(ObjectKey other);

        #endregion
    }
}
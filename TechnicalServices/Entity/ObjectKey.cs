using System;

namespace TechnicalServices.Entity
{
    /// <summary>
    /// определяет что за объект
    /// </summary>
    [Serializable]
    public enum ObjectType
    {
        /// <summary>
        /// слайд
        /// </summary>
        Slide,
        /// <summary>
        /// презентация
        /// </summary>
        Presentation
    }

    [Serializable]
    public abstract class ObjectKey : IEquatable<ObjectKey>
    {
        public abstract ObjectType GetObjectType();

        #region IEquatable<ObjectKey> Members

        public abstract bool Equals(ObjectKey other);

        #endregion
    }
}
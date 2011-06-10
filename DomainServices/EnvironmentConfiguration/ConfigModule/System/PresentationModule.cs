using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TechnicalServices.Interfaces.ConfigModule.System;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.SystemModule
{
    public abstract class PresentationModule : IPresentationModule
    {
        #region IPresentationModule Members

        public virtual Type[] GetDevice()
        {
            return new Type[0];
        }

        public virtual Type[] GetDisplay()
        {
            return new Type[0];
        }

        public virtual Type[] GetSource()
        {
            return new Type[0];
        }

        public virtual Type[] GetExtensionType()
        {
            return new Type[0];
        }

        public virtual Type[] GetWindow()
        {
            return new Type[0];
        }


        #endregion
    }
}
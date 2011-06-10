using System;
using System.Diagnostics;

using TechnicalServices.Interfaces.ConfigModule.Designer;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.Designer
{
    public abstract class DesignerModule : IDesignerModule
    {
        #region IDesignerModule Members

        public void Init()
        {
            CheckLicense();
        }

        public virtual void Preview(string file)
        {
            throw new NotSupportedException("Операция не поддерживается");
        }

        #endregion


        public abstract void CheckLicense();
    }
}
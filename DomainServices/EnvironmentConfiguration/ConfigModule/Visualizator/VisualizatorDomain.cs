using System;
using System.IO;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator
{
    public abstract class VisualizatorDomain : MarshalByRefObject, IExecute
    {
        #region IExecute Members

        public bool Init()
        {
            return OnInit();
        }

        public virtual IntPtr ShowForm(DisplayType display, Window window)
        {
            return IntPtr.Zero;
        }

        public virtual void DestroyForm(IntPtr handle)
        {
        }

        public void Done()
        {
            OnDone();
        }

        public virtual void BringToFront(IntPtr handle)
        {
        }

        public virtual void SendToBack(IntPtr handle)
        {
        }

        public virtual string DoCommand(IntPtr handle, string command)
        {
            return null;
        }

        public virtual bool IsAlive(IntPtr handle)
        {
            return false;
        }

        #endregion

        protected abstract bool OnInit();
        protected abstract void OnDone();

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
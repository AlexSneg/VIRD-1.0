using System;
using System.Diagnostics;

using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.ConfigModule.Visualizator;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator
{
    public class VisualizatorModule : IVisualizatorModule
    {
        #region IVisualizatorModule Members

        public bool IsSupportView
        {
            get { return false; }
        }

        public bool Init(IEventLogging log)
        {
            Debug.Assert(log != null, "IEventLogging не может быть null");
            return true;
        }

        public void Show(DisplayType display, Window[] window)
        {
        }

        public void Done()
        {
        }

        public string DoCommand(string sourceId, string command)
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {}

        public void BringToFront(Window window)
        {}

        public void HideAll()
        {}

        #endregion
    }
}
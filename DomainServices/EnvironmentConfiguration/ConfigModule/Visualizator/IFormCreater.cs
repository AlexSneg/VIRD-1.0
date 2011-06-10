using System.Windows.Forms;

using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator
{
    internal interface IFormCreater
    {
        Form CreateForm(DisplayType display, Window window, out bool needProcessing);
    }
}
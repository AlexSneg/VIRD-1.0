using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace TechnicalServices.Interfaces.ConfigModule.Visualizator
{
    public interface IVisualizatorModule
    {
        bool IsSupportView { get; }

        bool Init(IEventLogging log);
        void Show(DisplayType display, Window[] windows);
        void Done();
        string DoCommand(string sourceId, string command);
        void Pause();
        void BringToFront(Window window);
        void HideAll();
    }
}
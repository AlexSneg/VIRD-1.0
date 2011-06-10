using System;

using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator
{
    public interface IExecute
    {
        bool Init();
        IntPtr ShowForm(DisplayType display, Window window);
        void DestroyForm(IntPtr handle);
        void Done();
        void BringToFront(IntPtr handle);
        void SendToBack(IntPtr handle);
        string DoCommand(IntPtr handle, string command);
        bool IsAlive(IntPtr handle);
    }
}
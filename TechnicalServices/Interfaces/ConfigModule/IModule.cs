using TechnicalServices.Interfaces.ConfigModule.Configurator;
using TechnicalServices.Interfaces.ConfigModule.Designer;
using TechnicalServices.Interfaces.ConfigModule.Server;
using TechnicalServices.Interfaces.ConfigModule.System;
using TechnicalServices.Interfaces.ConfigModule.Visualizator;
using TechnicalServices.Interfaces.ConfigModule.Player;

namespace TechnicalServices.Interfaces.ConfigModule
{
    public interface IModule
    {
        string Name { get; set; } 
        ISystemModule SystemModule { get; }
        IDesignerModule DesignerModule { get; }
        IConfiguratorModule ConfiguratorModule { get; }
        IVisualizatorModule VisualizatorModule { get; }
        IServerModule ServerModule { get; }
        IPlayerModule PlayerModule { get; }
    }
}
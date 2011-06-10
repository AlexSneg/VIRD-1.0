using System;
using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Interfaces.ConfigModule.Configurator;
using TechnicalServices.Interfaces.ConfigModule.Designer;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Interfaces.ConfigModule.Server;
using TechnicalServices.Interfaces.ConfigModule.System;
using TechnicalServices.Interfaces.ConfigModule.Visualizator;

namespace DomainServices.EnvironmentConfiguration.ConfigModule
{
    public abstract class ModuleGeneric<TSystem, TDesigner, TConfigurator, TVisualizator, TServer, TPlayer> : IModule
        where TSystem : ISystemModule, new()
        where TDesigner : IDesignerModule, new()
        where TConfigurator : IConfiguratorModule, new()
        where TVisualizator : IVisualizatorModule, new()
        where TServer : IServerModule, new()
        where TPlayer : IPlayerModule, new()
    {
        private static readonly object syncRoot = new Object();
        private IConfiguratorModule _configurator;
        private IDesignerModule _designer;
        private ISystemModule _system;
        private IVisualizatorModule _visualizator;
        private IServerModule _server;
        private IPlayerModule _player;

        #region IModule Members

        public string Name { get; set; }

        public ISystemModule SystemModule
        {
            get
            {
                if (_system == null)
                {
                    lock (syncRoot)
                    {
                        if (_system == null)
                            _system = new TSystem();
                    }
                }
                return _system;
            }
        }

        public IDesignerModule DesignerModule
        {
            get
            {
                if (_designer == null)
                {
                    lock (syncRoot)
                    {
                        if (_designer == null)
                            _designer = new TDesigner();
                    }
                }
                return _designer;
            }
        }

        public IConfiguratorModule ConfiguratorModule
        {
            get
            {
                if (_configurator == null)
                {
                    lock (syncRoot)
                    {
                        if (_configurator == null)
                            _configurator = new TConfigurator();
                    }
                }
                return _configurator;
            }
        }

        public IVisualizatorModule VisualizatorModule
        {
            get
            {
                if (_visualizator == null)
                {
                    lock (syncRoot)
                    {
                        if (_visualizator == null)
                            _visualizator = new TVisualizator();
                    }
                }
                return _visualizator;
            }
        }

        public IServerModule ServerModule
        {
            get
            {
                if (_server == null)
                {
                    lock (syncRoot)
                    {
                        if (_server == null)
                            _server = new TServer();
                    }
                }
                return _server;
            }
        }

        public IPlayerModule PlayerModule
        {
            get
            {
                if (_player == null)
                {
                    lock (syncRoot)
                    {
                        if (_player == null)
                            _player = new TPlayer();
                    }
                }
                return _player;
            }
        }

        #endregion
    }
}

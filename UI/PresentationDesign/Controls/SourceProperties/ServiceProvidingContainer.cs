using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using TechnicalServices.Persistence.CommonPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using UI.PresentationDesign.DesignUI.Classes.Controller;

namespace UI.PresentationDesign.DesignUI.Controls.SourceProperties
{
    public class ServiceProvidingContainer: Container
    {
        IServiceContainer _serviceContainer;

        public IServiceContainer ServiceContainer
        {
            get { return _serviceContainer; }
        }

        public ServiceProvidingContainer()
        {
            _serviceContainer = new ServiceContainer();
            _serviceContainer.AddService(typeof(IServiceContainer), _serviceContainer);
        }

        protected override object GetService(Type service)
        {
            return _serviceContainer.GetService(service);
        }
    }

    public class ResourceProvider : Component, IResourceProvider
    {
        SourcesController _controller;
        public ResourceProvider(SourcesController AController)
        {
            _controller = AController;
        }

        public override ISite Site
        {
            get
            {
                return base.Site;
            }
            set
            {
                base.Site = value;

                IServiceContainer serviceContainer = (IServiceContainer)GetService(typeof(IServiceContainer));
                if (serviceContainer != null)
                {
                    serviceContainer.AddService(typeof(IResourceProvider), this);
                }
            }
        }

        #region IResourceProvider Members

        public ResourceDescriptor[] GetResourcesByType(string Type, bool checkMapping)
        {
            return _controller.GetResourcesByType(Type, checkMapping);
        }

        public Device GetDeviceByName(DeviceType deviceType)
        {
            return _controller.GetDeviceByName(deviceType);
        }

        #endregion
    }
}

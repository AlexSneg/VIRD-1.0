using System;
using System.Diagnostics;
using System.IO;

using TechnicalServices.Common.Utils;
using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Interfaces.ConfigModule.Server;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.Server
{
    public enum Feature : int
    {
        AudioMixer = 100,
        BusinessGraphics = 101,
        Computer = 102,
        DVDPlayer = 103,
        GangSwitch = 104,
        Image = 105,
        Jupiter = 106,
        Light = 107,
        Monitor = 108,
        StandardSource = 109,
        VDCServer = 110,
        VDCTerminal = 111,
        Video = 112,
        VideoCamera = 113,
        VNC = 114,
        PowerPointPresentation = 115,
        ArcGISMap = 199,
        WordDocument = 116,
        IEDocument = 117,
    }

    public abstract class ServerModule : IServerModule
    {
        protected IConfiguration _config;
        protected IControllerChannel _controller;

        #region IServerModule Members

        public virtual void Init(IConfiguration config, IModule module, IControllerChannel controller)
        {
            _controller = controller;
            _config = config;
        }

        public virtual void Done()
        {
        }

        public virtual MemoryStream CaptureScreen(DisplayType displayType)
        {
            return null;
        }

        public virtual void CloseWindows()
        {
            
        }

        public virtual void ShowDisplay(Display display, BackgroundImageDescriptor backgroundImageDescriptor)
        {
        }

        public virtual bool IsOnLine(EquipmentType equipmentType)
        {
            // если устройство хардварное - спрашиваем у контроллера
            if (equipmentType.IsHardware)
                return _controller.IsOnLine(equipmentType.UID);
            return true;
        }

        //public virtual ResourceForUpload[] GetResourcesForUpload(Display display,
        //    ResourceDescriptor[] resourceDescriptors,
        //    out bool isEnoughFreeSpace)
        //{
        //    isEnoughFreeSpace = true;
        //    return new ResourceForUpload[] { };
        //}

        public virtual ResourceDescriptor[] GetResourcesForUpload(DisplayType displayType,
                                                                  ResourceDescriptor[] resourceDescriptors,
                                                                  out bool isEnoughFreeSpace)
        {
            isEnoughFreeSpace = true;
            return new ResourceDescriptor[] { };
        }

        public virtual ResourceDescriptor[] UploadResources(DisplayType displayType,
                                                            ResourceDescriptor[] resourceDescriptors,
                                                            ISourceDAL sourceDAL)
        {
            return resourceDescriptors;
        }


        public virtual void DeleteResourcesUploaded(DisplayType displayType,
            ResourceDescriptor[] resourceDescriptors)
        {
        }


        public virtual void TerminateUpload()
        {
        }

        public virtual void TerminateUpload(string client)
        {
        }

        public virtual void Pause()
        {
        }

        public CommandDescriptor[] GetCommand(Slide slide1, Slide slide2)
        {
            return GetCommand(slide1, slide2, new EquipmentType[] { });
        }

        public virtual CommandDescriptor[] GetCommand(Slide slide1, Slide slide2, EquipmentType[] freezedEquipment)
        {
            return new CommandDescriptor[] { };
        }

        public event EventHandler OnResourceTransmit;
        public event Action<double, string, string> OnUploadSpeed;
        public event EventHandler<EqiupmentStateChangeEventArgs> OnStateChange;

        public virtual void DeleteAllResources(DisplayType displayType)
        {
        }

        public virtual string DoSourceCommand(DisplayType displayType, string sourceId, string command)
        {
            return null;
        }

        #endregion

        #region protected

        protected void ResourceTransmit(object sender, int currentResource, int totalResources)
        {
            if (OnResourceTransmit != null)
            {
                OnResourceTransmit(sender, new PartSendEventArgs(currentResource, totalResources, null));
            }
        }

        protected void ResourceTransmit(object sender, EventArgs args)
        {
            if (OnResourceTransmit != null)
            {
                OnResourceTransmit(sender, args);
            }
        }

        protected void UploadSpeed(double speed, string display, string file)
        {
            if (OnUploadSpeed != null)
            {
                OnUploadSpeed(speed, display, file);
            }
        }

        protected void StateChange(object sender, EqiupmentStateChangeEventArgs args)
        {
            if (OnStateChange != null)
            {
                OnStateChange(sender, args);
            }
        }

        #endregion

        public abstract void CheckLicense();
    }
}
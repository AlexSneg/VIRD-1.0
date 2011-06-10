using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.ServiceModel;
using System.Threading;
using System.Linq;

using Domain.PresentationShow.ShowCommon;
using TechnicalServices.Communication.Communication.Client;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Util.FileTransfer;

namespace TechnicalServices.ActiveDisplay.Util
{
    internal class ServiceStateEventArg : EventArgs
    {
        private readonly bool _isOnLine;
        private readonly DisplayType _displayType;
        public ServiceStateEventArg(bool isOnLine, DisplayType displayType)
        {
            _isOnLine = isOnLine;
            _displayType = displayType;
        }

        public bool IsOnLine { get { return _isOnLine; } }
        public DisplayType DisplayType { get { return _displayType; } }
    }

    internal class ActiveDisplayClient
    {
        //private const int TryCount = 2;
        private readonly Uri _address;
        private ComputerClientService _service;
        private readonly int _pingInterval;
        private int _serviceState = 0;
        private readonly DisplayType _displayType;
        private readonly IEventLogging _logging;

        public ActiveDisplayClient(Uri address, int pingInterval, DisplayType displayType, IEventLogging logging)
        {
            _address = address;
            _pingInterval = pingInterval;
            _displayType = displayType;
            _logging = logging;
            _service = new ComputerClientService(_address, _pingInterval);
            _service.OnChanged += new EventHandler<ClientState>(_service_OnChanged);
            new Action(ServiceOpen).BeginInvoke(null, null);
            //ServiceOpen();
        }

        public event EventHandler<ServiceStateEventArg> OnStateChange;

        void ServiceOpen()
        {
            try
            {
                _service.Open();
                _service.Ping();
            }
            catch (CommunicationException)
            { }
        }

        void _service_OnChanged(object sender, ClientState e)
        {
            if (e.State == CommunicationState.Opened) StateChange(true);
            else StateChange(false);
        }

        public bool IsServiceOnLine
        {
            get { return _serviceState == 1; }
            private set { Interlocked.Exchange(ref _serviceState, value ? 1 : 0); }
        }

        private void StateChange(bool isOnLine)
        {
            if (IsServiceOnLine == isOnLine) return;
            IsServiceOnLine = isOnLine;
            if (OnStateChange != null)
            {
                OnStateChange(this, new ServiceStateEventArg(isOnLine, _displayType));
            }
        }

        internal MemoryStream CaptureScreen()
        {
            if (!IsServiceOnLine) return null;
            //int count = TryCount;
            //while (count-- > 0)
            {
                try
                {
                    //CreateService();
                    return _service.Service.GetScreenShort(ImageFormat.Png.Guid);
                }
                catch (CommunicationException)
                {
                    //DestroyService();
                }
            }
            return null;
        }

        internal void CloseWindows()
        {
            if (!IsServiceOnLine) return;
            //int count = TryCount;
            //while (count-- > 0)
            {
                try
                {
                    //CreateService();
                    _service.Service.CloseWindows();
                    return;
                }
                catch (CommunicationException)
                {
                    //DestroyService();
                }
            }
            return;
        }

        //private void CreateService()
        //{
        //    if (_service == null)
        //    {
        //        _service = new ComputerClientService(_address, _pingInterval);
        //        _service.Open();
        //    }
        //}

        internal void Done()
        {
            if (_uploadTerminate != null) _uploadTerminate.Close();
            _uploadTerminate = null;
            DestroyService();
        }

        private void DestroyService()
        {
            if (_service == null) return;
            _service.Dispose();
            _service = null;
        }

        internal void ShowWindow(Window[] windows, BackgroundImageDescriptor backgroundImageDescriptor)
        {
            if (!IsServiceOnLine) return;
            //int count = TryCount;
            //while (count-- > 0)
            {
                try
                {
                    //CreateService();
                    _service.Service.ShowWindow(windows, backgroundImageDescriptor);
                    return;
                }
                catch (CommunicationException)
                {
                    //DestroyService();
                }
            }
        }

        internal bool IsConnected()
        {
            return IsServiceOnLine;
        }

        internal ResourceDescriptor[] GetResourcesForUpload(ResourceDescriptor[] resourceDescriptors,
            out bool isEnoughFreeSpace)
        {
            isEnoughFreeSpace = true;
            //int count = TryCount;
            //while (count-- > 0)
            if (IsServiceOnLine)
            {
                try
                {
                    //CreateService();
                    return _service.Service.GetResourcesForUpload(resourceDescriptors, out isEnoughFreeSpace);
                }
                catch (CommunicationException)
                {
                    //DestroyService();
                }
            }
            //List<ResourceForUpload> list = new List<ResourceForUpload>(resourceDescriptors.Length);
            //foreach (ResourceDescriptor descriptor in resourceDescriptors)
            //{
            //    ResourceFileInfo resourceFileInfo = descriptor.ResourceInfo as ResourceFileInfo;
            //    if (resourceFileInfo == null) continue;
            //    list.Add(new ResourceForUpload(descriptor, resourceFileInfo.ResourceFileDictionary.Values));
            //}
            return resourceDescriptors;
        }

        internal event EventHandler OnResourceTransmit;
        internal event Action<double, string, string> OnUploadSpeed;

        private IClientResourceCRUD<ResourceDescriptor> clientResourceCRUD = null;
        private ManualResetEvent _uploadTerminate = new ManualResetEvent(false);
        internal ResourceDescriptor[] UploadResources(ResourceDescriptor[] resourceDescriptors, ISourceDAL sourceDAL)
        {
            _uploadTerminate.Reset();
            List<ResourceDescriptor> notUploadedResources = new List<ResourceDescriptor>(resourceDescriptors);
            if (!IsServiceOnLine) return notUploadedResources.ToArray();
            try
            {
                //CreateService();
                clientResourceCRUD =
                    ClientSourceTransferFactory.CreateClientFileTransfer(false, _service.Service, sourceDAL);
                //clientResourceCRUD.OnPartTransmit += new EventHandler<PartSendEventArgs>(_resourceCRUD_OnPartTransmit);
                clientResourceCRUD.OnComplete += new EventHandler<OperationStatusEventArgs<ResourceDescriptor>>(_resourceCRUD_OnComplete);
                clientResourceCRUD.OnUploadSpeed += new Action<double, string>(_resourceCRUD_OnUploadSpeed);
                foreach (ResourceDescriptor resourceDescriptor in resourceDescriptors)
                {
                    if (_uploadTerminate.WaitOne(0)) break;
                    string otherResourceId;
                    FileSaveStatus status = clientResourceCRUD.SaveSource(resourceDescriptor, out otherResourceId);
                    
                    if (status == FileSaveStatus.Ok 
                        || status == FileSaveStatus.LoadInProgress) // Если загрузка была отменена, то файл считается в процессе докачки
                    {
                        notUploadedResources.Remove(resourceDescriptor);
                    }
                }
            }
            catch (CommunicationException)
            {
                //DestroyService();
            }
            return notUploadedResources.ToArray();
        }

        internal void DeleteResourcesUploaded(ResourceDescriptor[] resourceDescriptors)
        {
            if (!IsServiceOnLine) return;
            try
            {
                _service.Service.DeleteResourcesUploaded(resourceDescriptors);
            }
            catch (CommunicationException)
            {
            }
        }

        internal string DoSourceCommand(string sourceId, string command)
        {
            if (!IsServiceOnLine) return null;
            try
            {
                //CreateService();
                return _service.Service.DoSourceCommand(sourceId, command);
            }
            catch (Exception)
            {
                //DestroyService();
            }
            return null;
        }

        internal void DeleteAllResources()
        {
            if (!IsServiceOnLine) return;
            try
            {
                //CreateService();
                _service.Service.DeleteAllResources();
            }
            catch (CommunicationException ex)
            {
                //DestroyService();
            }
        }

        public void Pause()
        {
            if (!IsServiceOnLine) return;
            _service.Service.Pause();
        }


        void _resourceCRUD_OnComplete(object sender, OperationStatusEventArgs<ResourceDescriptor> e)
        {
            if (OnResourceTransmit != null)
            {
                OnResourceTransmit(this._displayType.Name.ToString(), e);
            }
        }
        void _resourceCRUD_OnUploadSpeed(double speed, string file)
        {
            if (OnUploadSpeed != null)
            {
                OnUploadSpeed(speed, this._displayType.Name, file);
            }
        }
        internal void TerminateUpload()
        {
            _uploadTerminate.Set();
            if (clientResourceCRUD != null)
            {
                clientResourceCRUD.Terminate();
                clientResourceCRUD.OnComplete -= _resourceCRUD_OnComplete;        //_resourceCRUD_OnPartTransmit;
            }
        }
    }

    internal class ComputerClientService : PingClient<IShowAgent>
    {
        private const string _endPointName = "IShowAgent";

        internal ComputerClientService(Uri address, int pingInterval)
            : base(pingInterval, _endPointName, address)
        {
        }

        public new void Ping()
        {
            base.Ping();
        }

        public IShowAgent Service
        {
            get { return Channel; }
        }
    }
}
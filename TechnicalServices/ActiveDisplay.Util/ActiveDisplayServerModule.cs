using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DomainServices.EnvironmentConfiguration.ConfigModule.Server;
using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Interfaces.ConfigModule.Server;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Util;

namespace TechnicalServices.ActiveDisplay.Util
{
    public abstract class ComputerServerModule<TModule, TDisplay> : ServerModule
        where TModule : IModule
        where TDisplay : DisplayTypeUriCapture
    {
        private readonly Dictionary<string, ActiveDisplayClient> _clientList =
            new Dictionary<string, ActiveDisplayClient>();

        private readonly List<TDisplay> _displayList = new List<TDisplay>();

        private TModule _module;

        public override void Init(IConfiguration config, IModule module, IControllerChannel controller)
        {
            base.Init(config, module, controller);
            Debug.Assert(config != null, "config не может быть null");
            Debug.Assert(module is TModule, "module должен быть ComputerModule");

            _module = (TModule) module;

            Type[] typeList;
            typeList = _module.SystemModule.Configuration.GetDisplay();
            foreach (DisplayType display in _config.ModuleConfiguration.DisplayList)
            {
                if (typeList.Contains(display.GetType()))
                {
                    Debug.Assert(typeList.Length == 1, "Жопа");
                    TDisplay computerDisplay = (TDisplay) display;
                    _displayList.Add(computerDisplay);
                    ActiveDisplayClient client = new ActiveDisplayClient(computerDisplay.Uri, _config.PingInterval, computerDisplay, _config.EventLog);
                    _clientList.Add(computerDisplay.Name, client);
                    client.OnResourceTransmit += new EventHandler(Value_OnResourceTransmit);
                    client.OnUploadSpeed += new Action<double, string, string>(client_OnUploadSpeed);
                    client.OnStateChange += new EventHandler<ServiceStateEventArg>(client_OnStateChange);
                }
            }
            //_clientList.Single().Value.OnResourceTransmit += new EventHandler<PartSendEventArgs>(Value_OnResourceTransmit);
        }

        public override void Done()
        {
            foreach (KeyValuePair<string, ActiveDisplayClient> item in _clientList)
            {
                item.Value.OnResourceTransmit -= Value_OnResourceTransmit;
                item.Value.OnUploadSpeed -= client_OnUploadSpeed;
                item.Value.OnStateChange -= client_OnStateChange;
                item.Value.Done();
            }
            //_clientList.Single().Value.OnResourceTransmit -= Value_OnResourceTransmit;
            _clientList.Clear();
            _displayList.Clear();
        }

        public override MemoryStream CaptureScreen(DisplayType display)
        {
            Debug.Assert(display is TDisplay, "Тип дисплея не соответствует ComputerDisplayConfig");
            TDisplay computerDisplay = (TDisplay) display;

            if (!_clientList.ContainsKey(computerDisplay.Name)) return null;
            ActiveDisplayClient client = _clientList[computerDisplay.Name];

            return client.CaptureScreen();
        }

        public override void CloseWindows()
        {
            foreach (KeyValuePair<string, ActiveDisplayClient> displayClient in _clientList)
            {
                displayClient.Value.CloseWindows();
            }
        }

        public override void ShowDisplay(Display display, BackgroundImageDescriptor backgroundImageDescriptor)
        {
            ActiveDisplayClient client;
            if (_clientList.TryGetValue(display.Type.Name, out client))
                client.ShowWindow(display.WindowList.ToArray(), backgroundImageDescriptor);
        }

        public override bool IsOnLine(EquipmentType equipmentType)
        {
            DisplayType displayType = equipmentType as DisplayType;
            if (displayType == null) return base.IsOnLine(equipmentType);
            ActiveDisplayClient client;
            if (_clientList.TryGetValue(displayType.Name, out client))
                return client.IsConnected();
            return false;
        }

        public override ResourceDescriptor[] GetResourcesForUpload(DisplayType displayType,
            ResourceDescriptor[] resourceDescriptors,
            out bool isEnoughFreeSpace)
        {
            ActiveDisplayClient client;
            if (_clientList.TryGetValue(displayType.Name, out client))
                return client.GetResourcesForUpload(resourceDescriptors, out isEnoughFreeSpace);
            isEnoughFreeSpace = true;
            return new ResourceDescriptor[] { };
        }

        public override ResourceDescriptor[] UploadResources(DisplayType displayType, ResourceDescriptor[] resourceDescriptors,
                                                             ISourceDAL sourceDAL)
        {
            ActiveDisplayClient client;
            if (_clientList.TryGetValue(displayType.Name, out client))
            {
                return client.UploadResources(resourceDescriptors, sourceDAL);
            }
            return resourceDescriptors;
        }

        public override void DeleteResourcesUploaded(DisplayType displayType, ResourceDescriptor[] resourceDescriptors)
        {
            ActiveDisplayClient client;
            if (_clientList.TryGetValue(displayType.Name, out client))
            {
                client.DeleteResourcesUploaded(resourceDescriptors);
            }
        }

        public override void DeleteAllResources(DisplayType displayType)
        {
            ActiveDisplayClient client;
            if (_clientList.TryGetValue(displayType.Name, out client))
            {
                client.DeleteAllResources();
            }
        }

        public override void TerminateUpload()
        {
            foreach (ActiveDisplayClient client in _clientList.Values)
            {
                client.TerminateUpload();
            }
        }

        public override void TerminateUpload(string client)
        {
            _clientList[client].TerminateUpload();
            //foreach (ActiveDisplayClient client in _clientList.Values)
            //{
            //    client.TerminateUpload();
            //}
        }

        public override string DoSourceCommand(DisplayType displayType, string sourceId, string command)
        {
            ActiveDisplayClient client;
            if (_clientList.TryGetValue(displayType.Name, out client))
            {
                return client.DoSourceCommand(sourceId, command);
            }
            return null;
        }

        public override void Pause()
        {
            List<IAsyncResult> asyncResults = new List<IAsyncResult>(_clientList.Count);
            foreach (KeyValuePair<string, ActiveDisplayClient> pair in _clientList)
            {
                asyncResults.Add(AsyncCaller.BeginCall<string, ActiveDisplayClient>(Pause, pair.Key, pair.Value));
            }
            foreach (IAsyncResult asyncResult in asyncResults)
            {
                AsyncCaller.EndCall<string, ActiveDisplayClient>(asyncResult);
            }
        }

        private void Pause(string displayName, ActiveDisplayClient client)
        {
            try
            {
                client.Pause();
            }
            catch (Exception ex)
            {
                _config.EventLog.WriteError(string.Format("ComputerServerModule.Pause: Ошибка для дисплея {0}\n{1}",
                    displayName, ex));
            }
        }

        void Value_OnResourceTransmit(object sender, EventArgs e)
        {
            ResourceTransmit(sender, e);
        }

        void client_OnUploadSpeed(double arg1, string arg2, string arg3)
        {
            UploadSpeed(arg1, arg2, arg3);
        }

        void client_OnStateChange(object sender, ServiceStateEventArg e)
        {
            StateChange(sender, new EqiupmentStateChangeEventArgs(e.DisplayType, e.IsOnLine));
        }

    }
}
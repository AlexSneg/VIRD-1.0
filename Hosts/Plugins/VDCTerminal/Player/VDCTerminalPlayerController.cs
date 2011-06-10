using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Hosts.Plugins.VDCTerminal.SystemModule.Design;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using System.Threading;
using TechnicalServices.Exceptions;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;

namespace Hosts.Plugins.VDCTerminal.Player
{
    internal class PlayerController : PlayerPlaginsRGBController<VDCTerminalDeviceDesign, IVDCTerminalView>
    {
        private readonly List<VDCTerminalAbonentInfo> _abonents = new List<VDCTerminalAbonentInfo>();

        internal PlayerController(IPresentationClient presClient, Source source, IVDCTerminalView view, 
            IPlayerCommand playerCommand, IEventLogging logging)
            : base(presClient, (HardwareSource)source, (VDCTerminalDeviceDesign)source.Device, 
            playerCommand, logging, view)
        {
            if (source.ResourceDescriptor != null)
                _initAbonentList((VDCTerminalResourceInfo)source.ResourceDescriptor.ResourceInfo);
            View.InitializeData(Device, _abonents, Device.Abonent, Device.DirectNumber);
            UpdateView();
        }

        /// <summary>метод для тестирования </summary>
        private void _initAbonentList(VDCTerminalResourceInfo _abonentInfo)
        {
            _abonents.Clear();
            _abonents.AddRange(_abonentInfo.AbonentList);
        }

        protected override void UpdateView()
        {
            bool isSuccess; int errorCount = 0;
            string VCSGetCallStateTemp = ExecuteCommand("GetCallState", out isSuccess);
            errorCount += Convert.ToInt32(!isSuccess);
            ConnectionStateEnum VCSGetCallState = VCSGetCallStateTemp == "Connected" ?
                ConnectionStateEnum.Connected : ConnectionStateEnum.Disconnected;
            string VCSGetStatus = ExecuteCommand("GetStatus", out isSuccess).Trim('"');
            errorCount += Convert.ToInt32(!isSuccess);
            string VCSGetError = ExecuteCommand("GetError", out isSuccess).Trim('"');
            errorCount += Convert.ToInt32(!isSuccess);
            string VCSIncomingCall = ExecuteCommand("IncomingCall", out isSuccess).Trim('"');
            errorCount += Convert.ToInt32(!isSuccess);
            if (string.IsNullOrEmpty(VCSIncomingCall))
            {
                VCSIncomingCall = "нет входящего звонка";
            }
            else
            {
                VDCTerminalAbonentInfo abn = FindAbonentByNumber(VCSIncomingCall);
                VCSIncomingCall = abn == null ? VCSIncomingCall : "Звонит " + abn.Name + " (" + abn.Number1 + ")";
            }
            bool VCSGetContent = ExecuteCommandBool("GetContent", out isSuccess);
            errorCount += Convert.ToInt32(!isSuccess);
            bool VCSGetPrivacy = ExecuteCommandBool("GetPrivacy", out isSuccess);
            errorCount += Convert.ToInt32(!isSuccess);
            bool VCSGetDND = ExecuteCommandBool("GetDND", out isSuccess);
            errorCount += Convert.ToInt32(!isSuccess);
            bool VCSGetPIP = ExecuteCommandBool("GetPIP", out isSuccess);
            errorCount += Convert.ToInt32(!isSuccess);
            bool VCSGetAutoAnswer = ExecuteCommandBool("GetAutoAnswer", out isSuccess);
            errorCount += Convert.ToInt32(!isSuccess);

            isSuccess = !Convert.ToBoolean(errorCount);
            View.UpdateView(isSuccess, VCSGetCallState, VCSGetStatus, VCSGetError, VCSIncomingCall,
                VCSGetPrivacy, VCSGetDND, VCSGetContent, VCSGetPIP, VCSGetAutoAnswer);
        }

        public VDCTerminalAbonentInfo FindAbonentByNumber(string number)
        {
            VDCTerminalAbonentInfo abonent = _abonents.Find(
                delegate(VDCTerminalAbonentInfo info) { 
                    return info.Number1.Equals(number); });
            return abonent;
        }
    }
}

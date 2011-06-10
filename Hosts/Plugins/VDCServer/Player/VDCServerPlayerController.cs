using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hosts.Plugins.VDCServer.SystemModule.Design;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using Hosts.Plugins.VDCServer.SystemModule.Config;
using TechnicalServices.Interfaces;
using System.Threading;

namespace Hosts.Plugins.VDCServer.Player
{
    internal class VDCServerPlayerController : PlayerPlaginsHardController<VDCServerDeviceDesign, IVDCServerPlayerView>
    {
        private int _timerTickCount = 0;

        internal VDCServerPlayerController(Device device, IPlayerCommand playerProvider, IVDCServerPlayerView view, IEventLogging logging)
            : base((VDCServerDeviceDesign)device, playerProvider, logging, view)
        {
            View.InitializeData(Device, (VDCServerDeviceConfig)Device.Type);
            _timerTickCount = ((VDCServerDeviceConfig)Device.Type).RequestPeriod;
            UpdateView();
        }

        protected override void ViewPushCommandButtonEvent(string command, params IConvertible[] parameters)
        {
            bool isSuccess = false, status = false;
            switch (command)
            {
                case "CreateConference":
                    status = CheckConference(out isSuccess);
                    if (isSuccess && !status)
                        ExecuteCommand(command, out isSuccess,
                            Device.ConferenceName,
                            Convert.ToInt32(Device.VoiceSwitched),
                            Convert.ToInt32(Device.Private),
                            Device.Private ? Device.Password : string.Empty,
                            parameters[0]);
                    break;
                case "UpdateConference":
                    status = CheckConference(out isSuccess);
                    if (isSuccess && status)
                        ExecuteCommand(command, out isSuccess,
                            Device.ConferenceName,
                            Convert.ToInt32(Device.VoiceSwitched),
                            Convert.ToInt32(Device.Private),
                            Device.Private ? Device.Password : string.Empty,
                            parameters[0]);
                    break;
                case "AddParticipant":
                    List<VDCServerAbonentInfo> tmpList = Device.AbonentList.Where(
                        abn => abn.Name.Equals((string)parameters[0])).ToList();
                    if ((tmpList != null) && (tmpList.Count > 0))
                    {
                        status = CheckConference(out isSuccess);
                        if (isSuccess && status)
                            ExecuteCommandWithMembersResults(command, out isSuccess,
                                Device.ConferenceName,
                                tmpList[0].Name,
                                tmpList[0].Number1,
                                tmpList[0].Number2,
                                tmpList[0].ConnectionType.ToString(),
                                tmpList[0].ConnectionQuality.ToString());
                    }
                    break;
                default:
                    ExecuteCommand(command, out isSuccess, parameters);
                    break;
            }
            //обновим состояние контрола
            if (isSuccess)
            {
                _timerTickCount = ((VDCServerDeviceConfig)Device.Type).RequestPeriod;
                UpdateView();
            }
            else
                View.UpdateView(isSuccess, status, null);
        }

        protected override void UpdateView()
        {
            bool isSuccess;
            _timerTickCount++;
            bool status = CheckConference(out isSuccess);
            List<VDCServerAbonentInfo> members = null;
            //если конференция создана, и если периодичность соответсвует параметру RequestPeriod (я считаю что таймер тикает 1 раз в секунду)
            //то получим список участников
            if (isSuccess && (_timerTickCount >= ((VDCServerDeviceConfig)Device.Type).RequestPeriod))
            {
                _timerTickCount = 0;
                members = ExecuteCommandWithMembersResults("GetParticipantList", out isSuccess, Device.ConferenceName);
            }
            View.UpdateView(isSuccess, status, members);
        }

        private List<VDCServerAbonentInfo> ExecuteCommandWithMembersResults(string command, out bool isSuccess, params IConvertible[] parameters)
        {
            List<VDCServerAbonentInfo> members = null;
            string[] resString = ExecuteCommandArrayString(command, out isSuccess, parameters);
            //если список участников успешно получен
            if (isSuccess && (resString.Length >= 2))
            {
                int countMembers = Convert.ToInt32(resString[1]);
                members = new List<VDCServerAbonentInfo>();
                for (int i = 2; (i < resString.Length) && (i - 1 <= countMembers); i++)
                {
                    //нам крестрон вернет имена участников, заполним по нему список
                    List<VDCServerAbonentInfo> tmpList = Device.AbonentList.Where(abn => abn.Name.Equals(resString[i])).ToList();
                    if ((tmpList != null) && (tmpList.Count > 0))
                    {
                        VDCServerAbonentInfo member = new VDCServerAbonentInfo(tmpList[0]);
                        members.Add(member);
                    }
                }
            }
            return members;
        }

        private bool CheckConference(out bool isSuccess)
        {
            string[] resString = ExecuteCommandArrayString("CheckConference", out isSuccess, Device.ConferenceName);
            bool result = false;
            if (isSuccess && (resString != null) && (resString.Length >= 2))
                result = Convert.ToBoolean(Convert.ToInt32(resString[1]));
            return result;
        }
    }
}

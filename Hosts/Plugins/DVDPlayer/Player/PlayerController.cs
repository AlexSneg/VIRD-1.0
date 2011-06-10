using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Hosts.Plugins.DVDPlayer.SystemModule.Design;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Interfaces;
using System.Threading;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Exceptions;
using Hosts.Plugins.DVDPlayer.SystemModule.Config;
using System.Reflection;
using System.ComponentModel;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;

namespace Hosts.Plugins.DVDPlayer.Player
{
    internal class PlayerController : PlayerPlaginsRGBController<DVDPlayerDeviceDesign, IDVDPlayerView>
    {
        internal PlayerController(IPresentationClient presClient, Source source, IDVDPlayerView view, IPlayerCommand playerCommand, IEventLogging logging)
            : base(presClient, (HardwareSource)source, (DVDPlayerDeviceDesign)source.Device, 
            playerCommand, logging, view)
        {
            initCommonData();
            View.InitializeData(Device);
            UpdateView();
        }

        private int initCommonData()
        {
            bool isSuccess; int errorCount = 0;
            int GetChapter = ExecuteCommandInt32("GetChapter", out isSuccess);
            errorCount += Convert.ToInt32(!isSuccess);
            if (isSuccess && (GetChapter != Device.DVDChapterAmount))
            {
                Device.DVDChapterAmount = GetChapter;
                int GetTrack = ExecuteCommandInt32("GetTrack", out isSuccess, 1);
                errorCount += Convert.ToInt32(!isSuccess);
                if (isSuccess && (GetTrack != Device.TrackAmount))
                    Device.TrackAmount = GetTrack;
            }
            string GetTime = ExecuteCommand("GetTime", out isSuccess);
            errorCount += Convert.ToInt32(!isSuccess);
            if (isSuccess && !string.IsNullOrEmpty(GetTime))
                Device.TotalPlaybackTime = GetTime.Replace('"', ' ');
            return errorCount;
        }

        protected override void UpdateView()
        {
            if (Device.InterfaceType == InterfaceTypeEnum.RS232)
            {
                bool isSuccess; int errorCount = initCommonData();
                DVDState GetState = parseDVDState(ExecuteCommand("GetState", out isSuccess));
                errorCount += Convert.ToInt32(!isSuccess);
                bool GetPower = ExecuteCommand("GetPower", out isSuccess) == "On" ? true : false;
                errorCount += Convert.ToInt32(!isSuccess);
                string GetPlayTime = ExecuteCommand("GetPlayTime", out isSuccess).Replace('"', ' ');
                errorCount += Convert.ToInt32(!isSuccess);
                int GetCurrChapter = ExecuteCommandInt32("GetCurrentChapter", out isSuccess);
                errorCount += Convert.ToInt32(!isSuccess);
                int GetCurrTrack = ExecuteCommandInt32("GetCurrentTrack", out isSuccess);
                errorCount += Convert.ToInt32(!isSuccess);
                isSuccess = !Convert.ToBoolean(errorCount);
                View.UpdateView(isSuccess, Device, GetPower, GetPlayTime, GetCurrTrack, GetCurrChapter, GetState);
            }
        }
        private void _view_PushCommandButtonEvent(string command, params IConvertible[] parameters)
        {
            bool isSuccess;
            if (command.Equals("GetTrack"))
            {
                int GetTrack = ExecuteCommandInt32("GetTrack", out isSuccess, parameters);
                if (isSuccess && (GetTrack != Device.TrackAmount))
                    Device.TrackAmount = GetTrack;
            } 
            else
                ExecuteCommand(command, out isSuccess, parameters);
        }

        protected override string ParseCommandAnswerParameter(string parameter, EquipmentCommand cmd)
        {
            return parameter.Substring(cmd.Answer.Length + 1);
        }
        /// <summary>
        /// метод парсит строку и превращает ее в Enum: DVDState
        /// </summary>
        private DVDState parseDVDState(string value)
        {
            DVDState result = DVDState.NO_CD;
            FieldInfo[] fInfo = typeof(DVDState).GetFields();
            foreach (FieldInfo item in fInfo)
            {
                object[] attrs = item.GetCustomAttributes(true);
                if ((attrs != null) && (attrs.Length > 0))
                {
                    DescriptionAttribute attr = (DescriptionAttribute)attrs[0];
                    if (attr.Description.Equals(value))
                    {
                        result = (DVDState)Enum.Parse(typeof(DVDState), item.Name);
                        break;
                    }
                }
            }
            return result;
        }
    }
}

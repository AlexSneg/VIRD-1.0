using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hosts.Plugins.AudioMixer.SystemModule.Design;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using Hosts.Plugins.AudioMixer.SystemModule.Config;
using Hosts.Plugins.AudioMixer.UI;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Interfaces;

namespace Hosts.Plugins.AudioMixer.Player
{
    internal class AudioMixerPlayerController : PlayerPlaginsHardController<AudioMixerDeviceDesign, IAudioMixerPlayerOperativeView>
    {
        private IAudioMixerFullView _fullView;
        private int MatrixId
        {
            get { return Convert.ToInt32(((AudioMixerDeviceConfig)Device.Type).InstanceID); }
        }

        internal AudioMixerPlayerController(Device device, IPlayerCommand playerProvider, IAudioMixerPlayerOperativeView view, IEventLogging logging)
            : base((AudioMixerDeviceDesign)device, playerProvider, logging, view)
        {
            InitView();
            View.DetailExecuteEvent += _view_DetailExecuteEvent;
            UpdateView();
        }

        private void _view_DetailExecuteEvent()
        {
            AudioMixerFullForm form = new AudioMixerFullForm();
            _fullView = form.AudioMixerFullView;
            form.Width = _fullView.GetWidthControl(Device.InputList.Count, Device.FaderGroupList.Sum(fGroup => fGroup.FaderList.Count));
            _fullView.InitializeFaderGroups(Device.FaderGroupList);
            _fullView.InitializeMatrix(Device.HasMatrix, MatrixId, 
                ((AudioMixerDeviceConfig)Device.Type).InputList, 
                ((AudioMixerDeviceConfig)Device.Type).OutputList, 
                Device.GetMatrixUnit);
            UpdateView();
            _fullView.PushCommandButtonEvent += _fullView_PushCommandButtonEvent;
            form.ShowModal();
            _fullView.PushCommandButtonEvent -= _fullView_PushCommandButtonEvent;
            _fullView = null;
        }

        private void _fullView_PushCommandButtonEvent(string command, IConvertible[] parameters)
        {
            ViewPushCommandButtonEvent(command, parameters);
            if (command.Equals("MixerSetFader"))
            {
                int instanceId = Convert.ToInt32(parameters[0]);
                bool mute = Convert.ToBoolean(parameters[1]);
                int level = Convert.ToInt32(parameters[2]);
                View.UpdateFaderValue(true, instanceId, mute, level);
            }
        }

        private void InitView()
        {
            foreach(AudioMixerFaderGroupDesign group in Device.FaderGroupList)
            {
                foreach(AudioMixerFaderDesign fader in group.FaderList)
                {
                    if (fader.HasOnlineControl)
                        View.AddMixerFader(fader);
                }
            }
        }

        protected override void UpdateView()
        {
            UpdateMatrix();
            UpdateFaders();
        }

        private void UpdateMatrix()
        {
            bool isSuccess;
            if ((Device.HasMatrix) && _fullView != null)
            {
                int[] tmpResult = ExecuteCommandArrayInt32("MixerGetTies", out isSuccess, MatrixId);
                if (tmpResult.Length > 3)
                {
                    int index = 3;
                    //количество выходов в 2-м элементе, это строки
                    bool[][] result = new bool[tmpResult[2]][];
                    for (int i = 0; i < tmpResult[2]; i++)
                    {
                        //количество входов в 1-ом элементе, это столбцы
                        result[i] = new bool[tmpResult[1]];
                        for (int j = 0; j < tmpResult[1]; j++)
                        {
                            result[i][j] = Convert.ToBoolean(tmpResult[index]);
                            index++;
                        }
                    }
                    _fullView.UpdateMixerTies(result);
                }
            }
        }

        private void UpdateFaders()
        {
            bool isSuccess;
            foreach (AudioMixerFaderGroupDesign group in Device.FaderGroupList)
            {
                foreach (AudioMixerFaderDesign item in group.FaderList)
                {
                    bool mute;
                    int level;
                    int instanceId = Convert.ToInt32(item.InstanceID);
                    if ((item.HasOnlineControl) || (_fullView != null))
                    {
                        isSuccess = getFaderState(instanceId, out mute, out level);
                        View.UpdateFaderValue(isSuccess, instanceId, mute, level);
                        if (isSuccess && (_fullView != null))
                            _fullView.UpdateFaderValue(instanceId, mute, level);
                    }
                }
            }
        }

        private bool getFaderState(int instanceId, out bool mute, out int level)
        {
            bool isSuccess;
            mute = false; level = -1;
            int[] tmpResult = ExecuteCommandArrayInt32("MixerGetFader", out isSuccess, instanceId);
            if (tmpResult.Length >= 3)
            {
                mute = Convert.ToBoolean(tmpResult[1]);
                level = tmpResult[2];
            }
            return isSuccess;
        }

        public override void Dispose()
        {
            base.Dispose();
            View.DetailExecuteEvent -= _view_DetailExecuteEvent;
        }
    }
}

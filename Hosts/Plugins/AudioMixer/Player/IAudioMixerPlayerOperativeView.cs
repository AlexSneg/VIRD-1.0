using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hosts.Plugins.AudioMixer.SystemModule.Config;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;

namespace Hosts.Plugins.AudioMixer.Player
{
    interface IAudioMixerPlayerOperativeView : IPlayerPlaginHardBaseView
    {
        /// <summary>добавить на форму новый федер </summary>
        void AddMixerFader(AudioMixerFaderDesign unit);
        /// <summary>значение фейдера </summary>
        void UpdateFaderValue(bool isAvailable, int instanceId, bool mute, int level);
        /// <summary>пользователь вызвал контекстное меню с формой для точного ввода параметров </summary>
        event Action DetailExecuteEvent;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.Player
{
    /// <summary>интерфейс для всех контролов позволяющих менять настройки RGB (оборудование как источники) </summary>
    public interface IPlayerPlaginRGBBaseView : IPlayerPlagingBaseView
    {
        /// <summary>обновить контрол новыми настройками </summary>
        void UpdateRGBSettings(ResourceInfoForHardwareSource settings);
        /// <summary>пользователь изменил найстройки RGB и хочет сохранить</summary>
        event System.Action<ResourceInfoForHardwareSource> OnRGBSettingsChanged;
    }
}

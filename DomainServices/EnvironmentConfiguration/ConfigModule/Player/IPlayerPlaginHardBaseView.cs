using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.Player
{
    /// <summary>интерфейс для всех вьюков управляющих оборудованием (не источниками как оборудование) </summary>
    public interface IPlayerPlaginHardBaseView : IPlayerPlagingBaseView
    {
        /// <summary>изменился статус замороженности параметров (чекбокс "применить для всего сценария") </summary>
        event System.Action<FreezeStatus> OnFreezeStatusChanged;
        /// <summary>изменить чекбокс "применить для всего сценария" </summary>
        void UpdateFreezeStatus(FreezeStatus status);
    }
}

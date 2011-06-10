using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.Player
{
    /// <summary>базовый интерфейс для всех контролов управляющих оборудованием </summary>
    public interface IPlayerPlagingBaseView
    {
        /// <summary>установка статуса доступности/недоступности оборудования </summary>
        void SetAvailableStatus(bool isAvailable);
        /// <summary>свойство отображающее доступность оборудования </summary>
        bool IsHardwareAvailable { get; }
        ///// <summary>Таймер вытащен специально на контрол, чтобы он срабатывал в потоке UI,
        ///// если перейдем на независимый поток, то таймер нужно организовать в контроллере, 
        ///// а это собитие ликвидировать 
        ///// p.s. таймер реализован не везде (почти везде)</summary>
        //event System.Action ControlPlayerTimerTickEvent;
        
        /// <summary>включить/выключить таймер 
        /// обязательно отключите таймер в Dispose дочерних контролов
        /// инициализация таймера должно происходить обязательно после инициализации контролеера</summary>
        /// <param name="millisec">переодичность срабатывания таймера</param>
        void SetControlPlayerTimerEnable(bool enable, int? millisec);
        /// <summary>на контроле пользователь нажал кнопку с командой</summary>
        event System.Action<string, IConvertible[]> PushCommandButtonEvent;
    }
}

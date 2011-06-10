using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.Player
{
    public interface IPlayerPlaginsController : IDisposable
    {
        /// <summary>возвращает доступность оборудования (сервер переодически опрашивает крестрон через Check) </summary>
        bool? IsOnLine { get; } 
        /// <summary>показывает находится ли плеер в режиме показа </summary>
        bool IsShow { get; }
        /// <summary> перекинули сюда, так как таймер сделан ассинхронно</summary>
        void SetControlPlayerTimerEnable(bool enable, int? millisec);
    }
}

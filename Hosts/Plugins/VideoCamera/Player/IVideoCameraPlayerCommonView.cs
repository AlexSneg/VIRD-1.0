using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hosts.Plugins.VideoCamera.SystemModule.Design;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;

namespace Hosts.Plugins.VideoCamera.Player
{
    interface IVideoCameraPlayerCommonView : IPlayerPlaginRGBBaseView
    {
        void InitializeData(VideoCameraDeviceDesign device);
        /// <summary>на контроле пользователь отпустил кнопку с командой</summary>
        event Action UpCommandButtonEvent;
        /// <summary>пользователь вызвал контекстное меню с формой для точного ввода параметров </summary>
        event Action DetailExecuteEvent;
    }
}

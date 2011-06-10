using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Interfaces;

namespace Hosts.Plugins.StandardSource.Player
{
    public partial class StandartSourcePlayerControl : SourceHardPluginBaseControl, IStandartSourcePlayerView
    {
        public StandartSourcePlayerControl()
        {
            InitializeComponent();
        }
        public StandartSourcePlayerControl(Source source, IPlayerCommand playerCommand, IEventLogging logging, IPresentationClient client)
            : this()
        {
            InitializeController(new StandartSourcePlayerController(client, source, this, playerCommand, logging));
            SetControlPlayerTimerEnable(true, 1000);
        }

        public void UpdateView(bool? state)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool?>(UpdateView), state);
                return;
            }
            alStatus.Text = state.HasValue ? (state.Value ? "Есть" : "Нет") : "Неизвестно";
        }
    }
}

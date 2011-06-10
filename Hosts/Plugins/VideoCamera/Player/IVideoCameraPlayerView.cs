using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;
using DomainServices.EnvironmentConfiguration.ConfigModule.Player;

namespace Hosts.Plugins.VideoCamera.Player
{
    public interface IVideoCameraPlayerView : IPlayerPlaginRGBBaseView
    {
        void InitializeData(int presetAmount, int preset, bool isDomical,
            decimal LowZoomBoundary, decimal HighZoomBoundary);
        void UpdateView(ValueThree<int, int, int> state);
        ValueThree<int, int, int> GetState { get; }
    }
}

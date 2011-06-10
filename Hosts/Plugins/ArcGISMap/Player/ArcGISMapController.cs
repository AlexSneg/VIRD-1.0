using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using Hosts.Plugins.ArcGISMap.SystemModule.Design;

namespace Hosts.Plugins.ArcGISMap.Player
{
    public class ArcGISMapController
    {
        private readonly Source _source = null;
        private readonly IPlayerCommand _playerProvider;

        public ArcGISMapController(Source source, IPlayerCommand playerProvider)
        {
            _source = source;
            _playerProvider = playerProvider;
        }

        internal void Move(MoveDirection direction)
        {
            _playerProvider.DoSourceCommand(_source, "Move"+direction.ToString());
        }

        internal void Scale(double scale)
        {
            _playerProvider.DoSourceCommand(_source, "Scale:" + scale.ToString());
        }
    }
    public enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}

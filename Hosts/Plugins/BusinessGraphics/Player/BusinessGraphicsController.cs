using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using Hosts.Plugins.BusinessGraphics.SystemModule.Design;

namespace Hosts.Plugins.BusinessGraphics.Player
{
    public class BusinessGraphicsController
    {
        private readonly Source _source = null;
        private readonly IPlayerCommand _playerProvider;

        public BusinessGraphicsController(Source source, IPlayerCommand playerProvider)
        {
            _source = source;
            interactive = ((BusinessGraphicsSourceDesign)source).AllowUserInteraction;
            _playerProvider = playerProvider;
            UpdateControlState();
        }

        private bool interactive;

        public bool IsInteractive
        {
            get { return interactive; }
        }

        internal void UpdateControlState()
        {
            bool b;
            interactive = Boolean.TryParse(_playerProvider.DoSourceCommand(_source, "getInteractiveState"), out b) ? b : interactive;
        }

        internal void EnableInteractive(bool flag)
        {
            interactive = flag;
            _playerProvider.DoSourceCommand(_source, "interactive," + flag);
        }

        internal void MakeDefault()
        {
            bool b;
            _playerProvider.DoSourceCommand(_source, "default");
            interactive = Boolean.TryParse(_playerProvider.DoSourceCommand(_source, "getInteractiveState"), out b) ? b : interactive;
        }
    }
}

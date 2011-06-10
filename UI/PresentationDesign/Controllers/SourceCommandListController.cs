using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UI.PresentationDesign.DesignUI.Classes.Controller;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using System.Windows.Forms;
using Domain.PresentationDesign.Client;
using Domain.PresentationShow.ShowClient;

namespace UI.PresentationDesign.DesignUI.Controllers
{
    public class SourceCommandListController : CommandListController
    {
        private static SourceCommandListController _instance;
        protected Source _source = null;
        private IPlayerModule _playerModule = null;

        protected SourceCommandListController()
        {
            PresentationController.Instance.OnSourceChanged += Instance_OnSourceChanged;
        }

        public static void CreateSourceCommandListController()
        {
            _instance = new SourceCommandListController();
        }

        void Instance_OnSourceChanged(Source newSource)
        {
            _source = newSource;
            if (newSource != null)
            {
                _playerModule = Domain.PresentationShow.ShowClient.ShowClient.Instance.GetPlayerModule(newSource.GetType());
            }
            FillCommandList();
            FireListChanged();
        }

        public override Control CreateManagementControl(Control parent)
        {
            
            if(_source != null)
                return _playerModule.CreateControlForSource(
                    DesignerClient.Instance.ClientConfiguration.EventLog, 
                    _source, 
                    parent, 
                    ShowClient.Instance,
                    DesignerClient.Instance.PresentationWorker);
            return base.CreateManagementControl(parent);
        }

        public static SourceCommandListController Instance
        {
            get { return _instance; }
        }

        protected override void FillCommandList()
        {
            if (_source != null)
            {
                _commandList.Clear();
                foreach (Command cmd in _source.CommandList)
                    _commandList.Add(new KeyValuePair<string, object>(cmd.command, cmd));

                if (_commandList.Count != 0)
                    return;
            }
            base.FillCommandList();
        }

        public override void Dispose()
        {
            if (PresentationController.Instance != null)
                PresentationController.Instance.OnSourceChanged -= Instance_OnSourceChanged;
            base.Dispose();
            _instance = null;
        }
    }
}

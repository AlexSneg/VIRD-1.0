using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UI.PresentationDesign.DesignUI.Classes.Controller;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System.Windows.Forms;

namespace UI.PresentationDesign.DesignUI.Controllers
{
    public delegate void CommandListChanged();

    public class CommandListController : IDisposable
    {
        protected List<KeyValuePair<String, object>> _commandList = new List<KeyValuePair<string, object>>();

        public event CommandListChanged OnListChanged;

        protected CommandListController()
        {
            FillCommandList();
        }

        protected void FireListChanged()
        {
            if (OnListChanged != null)
                OnListChanged();
        }

        protected virtual void FillCommandList()
        {
            _commandList.Clear();
            for (int i = 0; i < 5; i++)
                _commandList.Add(new KeyValuePair<string, object>(UI.PresentationDesign.DesignUI.Properties.Resources.Command + " " + i, null));
        }

        public virtual Control CreateManagementControl(Control parent)
        {
            return null;
        }

        public List<KeyValuePair<String, object>> Commands
        {
            get { return _commandList; }
        }

        public void ExecuteCommand(object command)
        {
            System.Windows.Forms.MessageBox.Show("Not implemented yet");
        }

        public virtual void Dispose()
        {
            
        }
    }
}

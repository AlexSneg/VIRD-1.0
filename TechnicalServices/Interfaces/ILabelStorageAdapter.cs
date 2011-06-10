using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace TechnicalServices.Interfaces
{
    public class LabelEventArg : EventArgs
    {
        private readonly Label _label;
        public LabelEventArg(Label label)
        {
            _label = label;
        }

        public Label Label
        {
            get { return _label; }
        }
    }

    public interface ILabelStorageAdapter
    {
        Label[] GetLabelStorage();
        LabelError AddLabel(Label labelInfo);
        LabelError DeleteLabel(Label labelInfo);
        LabelError UpdateLabel(Label labelInfo);
        LabelError UnlockLabel(Label labelInfo);
        LabelError LockLabel(Label labelInfo);
        event  EventHandler<LabelEventArg> OnDelete;
        event  EventHandler<LabelEventArg> OnAdd;
        event  EventHandler<LabelEventArg> OnUpdate;
    }
}

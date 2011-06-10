using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.CommonPersistence.Configuration;

namespace TechnicalServices.Configuration.Client
{
    public class ClientLabelStorageAdapter : ILabelStorageAdapter
    {
        private HashSet<Label> _labelStorage = new HashSet<Label>();

        public ClientLabelStorageAdapter(ModuleConfiguration config)
        {
            if (config == null) return;
            foreach(Label item in config.LabelList)
                _labelStorage.Add(item);
        }

        public Label[] GetLabelStorage()
        {
            return _labelStorage.ToArray();
        }

        public LabelError AddLabel(Label labelInfo)
        {
            _labelStorage.Add(labelInfo);
            if (OnAdd != null)
            {
                OnAdd(this, new LabelEventArg(labelInfo));
            }
            return LabelError.NoError;
        }

        public LabelError DeleteLabel(Label labelInfo)
        {
            _labelStorage.Remove(labelInfo);
            if (OnDelete != null)
            {
                OnDelete(this, new LabelEventArg(labelInfo));
            }
            return LabelError.NoError;
        }

        public LabelError UpdateLabel(Label labelInfo)
        {
            if (_labelStorage.Contains(labelInfo))
                _labelStorage.Remove(labelInfo);
            _labelStorage.Add(labelInfo);
            if (OnUpdate != null)
            {
                OnUpdate(this, new LabelEventArg(labelInfo));
            }
            return LabelError.NoError;
        }

        public LabelError UnlockLabel(Label labelInfo)
        {
            throw new NotImplementedException();
        }

        public LabelError LockLabel(Label labelInfo)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<LabelEventArg> OnDelete;
        public event EventHandler<LabelEventArg> OnAdd;
        public event EventHandler<LabelEventArg> OnUpdate;
    }
}

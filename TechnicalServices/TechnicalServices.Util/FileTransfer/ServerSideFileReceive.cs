using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using TechnicalServices.Common;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace TechnicalServices.Util.FileTransfer
{
    public class ServerSideFileReceive
    {
        protected readonly Entity.UserIdentity _userIdentity;
        //protected readonly T Resource;
        protected readonly IFileInfoProvider _provider;


        //private readonly IResourceEx<T> _resourceEx;
        //private readonly ResourceDescriptor _resource;
        //private readonly ResourceFileInfo _resourceFileInfo = null;
        private ICommunicationObject _communicationObject = null;
        private int _part = -1;
        private int _numberOfParts;
        private DateTime _lastModifyDateTime;
        private string _fileName;

        private int _wasTerminated = 0;

        public ServerSideFileReceive(UserIdentity userIdentity, /*IResourceEx<T> resourceEx*/IFileInfoProvider provider)
        {
            //_resourceEx = resourceEx;
            _userIdentity = userIdentity;
            //Resource = resource;
            _provider = provider;
            //_resourceFileInfo = _resource.ResourceInfo as ResourceFileInfo;
            if (OperationContext.Current != null && OperationContext.Current.Channel != null)
            {
                _communicationObject = OperationContext.Current.Channel;
                _communicationObject.Faulted += new EventHandler(Abort);
            }

        }

        public FileTransferObject? Receive(string resourceId)
        {
            try
            {
                if (_part == -1)
                {
                    // первый вызов - определяем число частей
                    //_numberOfParts = (int)(_resourceFileInfo.ResourceFileList.Find(rfp => rfp.Id.Equals(resourceId)).Length / (Constants.PartSize + 1)) + 1;
                    _numberOfParts = _provider.GetNumberOfParts(resourceId);
                    //_fileName = _resourceEx.GetResourceFileName(Resource, resourceId);
                    _fileName = _provider.GetFileName(resourceId);
                    _lastModifyDateTime = File.GetLastWriteTime(_fileName);
                }
                _part++;
                if (!File.GetLastWriteTime(_fileName).Equals(_lastModifyDateTime))
                {
                    throw new Exception("Запрашиваемый Вами ресурс был изменен со времени последнего доступа");
                }
                if (_part >= _numberOfParts)
                {
                    throw new Exception("Все части файла уже получены");
                }
                byte[] buffer = new byte[Constants.PartSize];
                int count;
                using (FileStream file = new FileStream(_fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    file.Seek(_part*Constants.PartSize, SeekOrigin.Begin);
                    count = file.Read(buffer, 0, buffer.Length);
                }
                FileTransferObject obj = new FileTransferObject(count, _part, _numberOfParts, resourceId, buffer);
                if (_part + 1 == _numberOfParts)
                    _part = -1;
                return obj;
            }
            catch
            {
                Abort(this, EventArgs.Empty);
                throw;
            }
        }

        public Entity.UserIdentity UserIdentity
        {
            get { return _userIdentity; }
        }


        public void Terminate()
        {
            if (Interlocked.CompareExchange(ref _wasTerminated, 1, 0) == 1) return;
            Done();
        }


        public event EventHandler OnAbort;

        public void Commit()
        {
            if (Interlocked.CompareExchange(ref _wasTerminated, 1, 0) == 1) return;
            Done();
        }


        private void Abort(object sender, EventArgs e)
        {
            Terminate();
            if (OnAbort != null)
            {
                OnAbort(this, e);
            }
        }

        private void Done()
        {
            if (_communicationObject != null)
            {
                _communicationObject.Faulted -= Abort;
                _communicationObject = null;
            }
        }

    }
}

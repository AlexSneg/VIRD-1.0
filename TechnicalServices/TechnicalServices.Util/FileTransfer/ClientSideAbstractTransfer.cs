using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TechnicalServices.Common;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;

namespace TechnicalServices.Util.FileTransfer
{
    public abstract class ClientSideAbstractTransfer<TResource, TProperty> : IClientResourceCRUD<TResource>
        where TResource : class
        where TProperty : class
    {
        protected FileSaver _fileSaver = null;
        protected IFileInfoProvider<TProperty> _provider = null;

        protected readonly ManualResetEvent _terminate = new ManualResetEvent(false);


        public event EventHandler<PartSendEventArgs> OnPartTransmit;
        public event EventHandler OnTerminate;
        public event EventHandler<OperationStatusEventArgs<TResource>> OnComplete;
        public event Action<double, string> OnUploadSpeed;

        public FileSaveStatus CreateSource(TResource resource, out string otherResourceId)
        {
            Init();
            FileSaveStatus status = FileSaveStatus.Abort;
            otherResourceId = null;
            try
            {
                return status = OnCreateSource(resource, out otherResourceId);
            }
            finally
            {
                Complete(status, otherResourceId, resource);
            }
        }

        public FileSaveStatus SaveSource(TResource resource, out string otherResourceId)
        {
            Init();
            FileSaveStatus status = FileSaveStatus.Abort;
            otherResourceId = null;
            try
            {
                return status = OnSaveSource(resource, out otherResourceId);
            }
            finally
            {
                Complete(status, otherResourceId, resource);
            }
        }

        public bool GetSource(TResource resource, bool autoCommit)
        {
            Init();
            FileSaveStatus status = FileSaveStatus.Abort;
            try
            {
                bool isSuccess = false;
                try
                {
                    isSuccess = OnGetSource(resource);
                    if (isSuccess && autoCommit) Commit();
                    if (!isSuccess) RollBack();
                }
                catch
                {
                    RollBack();
                    throw;
                }
                status = isSuccess ? FileSaveStatus.Ok : status;
                return isSuccess;
            }
            finally
            {
                Complete(status, null, resource);
            }
        }

        public virtual void Commit()
        {
            if (_fileSaver != null)
                _fileSaver.Commit();
        }
        public virtual void RollBack()
        {
            if (_fileSaver != null)
                _fileSaver.Terminate();
        }

        public void Terminate()
        {
            _terminate.Set();
            if (OnTerminate != null)
            {
                OnTerminate(this, EventArgs.Empty);
            }
        }

        protected virtual bool OnGetSource(TResource resource)
        {
            if (resource == null) return false;
            UserIdentity userIdentity = Thread.CurrentPrincipal as UserIdentity;
            TResource serverResource = InitDownload(userIdentity, resource);

            try
            {
                if (serverResource == null) return false;
                _provider = GetFileInfoProvider(resource, serverResource);
                _fileSaver = new FileSaver(userIdentity, _provider, true);

                int numberOfParts = _provider.GetNumberOfParts();
                int part = 0;

                foreach (TProperty property in _provider)
                {
                    bool isComplete = false;
                    FileTransferObject? obj;
                    do
                    {
                        obj = FileTransport.Receive(userIdentity, _provider.GetResourceId(property));
                        if (_terminate.WaitOne(0) || !obj.HasValue)
                        {
                            FileTransport.Terminate(userIdentity);
                            _fileSaver.Terminate();
                            return false;
                        }
                        isComplete = _fileSaver.Save(obj.Value);
                        PartTransfer(part, numberOfParts);
                        part++;
                    } while (!isComplete);
                }
                return true;
            }
            catch
            {
                if (_fileSaver != null)
                    _fileSaver.Terminate();
                throw;
            }
            finally
            {
                DoneDownload(userIdentity);
            }
        }

        protected abstract void DoneDownload(UserIdentity userIdentity);
        protected abstract IFileTransfer FileTransport { get; }
        protected abstract IFileInfoProvider<TProperty> GetFileInfoProvider(TResource resource, TResource serverResource);
        protected abstract TResource InitDownload(UserIdentity userIdentity, TResource resource);


        protected abstract FileSaveStatus OnCreateSource(TResource resource, out string otherResourceId);
        protected abstract FileSaveStatus OnSaveSource(TResource resource, out string otherResourceId);

        protected void PartTransfer(int part, int numberOfParts)
        {
            if (OnPartTransmit != null)
            {
                OnPartTransmit(this, new PartSendEventArgs(part, numberOfParts, null));
            }
        }

        protected void Complete(FileSaveStatus status, string otherResourceId, TResource resource)
        {
            _terminate.Reset();
            if (OnComplete != null)
            {
                OnComplete(this, new OperationStatusEventArgs<TResource>(status, otherResourceId, resource));
            }
        }

        protected void UploadSpeed(double speed, string file)
        {
            if (OnUploadSpeed != null)
            {
                OnUploadSpeed(speed, file);
            }
        }

        protected void Init()
        {
            _terminate.Reset();
        }

    }
}

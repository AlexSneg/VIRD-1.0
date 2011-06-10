using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Common;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;

namespace TechnicalServices.Util.FileTransfer
{
    public abstract class ServerSideAbstractTransfer<TResource> 
        where TResource : class, IEquatable<TResource>, IId
    {
        protected readonly Dictionary<UserIdentity, SendDictionaryItem> _internalSendDic = new Dictionary<UserIdentity, SendDictionaryItem>();
        protected readonly Dictionary<UserIdentity, ReceiveDictionaryItem> _internalReceiveDic = new Dictionary<UserIdentity, ReceiveDictionaryItem>();
        protected readonly IResourceEx<TResource> _resourceEx;

        #region Nested

        protected class SendDictionaryItem
        {
            private readonly FileSaver _saver;
            private readonly IFileInfoProvider _provider;
            public SendDictionaryItem(FileSaver saver, IFileInfoProvider provider)
            {
                _saver = saver;
                _provider = provider;
            }

            public FileSaver Saver { get { return _saver; } }
            public IFileInfoProvider Provider { get { return _provider; } }
        }

        protected class ReceiveDictionaryItem
        {
            private readonly ServerSideFileReceive _receiver;
            private readonly IFileInfoProvider _provider;
            public ReceiveDictionaryItem(ServerSideFileReceive receiver, IFileInfoProvider provider)
            {
                _receiver = receiver;
                _provider = provider;
            }

            public ServerSideFileReceive Receiver { get { return _receiver; } }
            public IFileInfoProvider Provider { get { return _provider; } }
        }

        #endregion

        protected ServerSideAbstractTransfer(IResourceEx<TResource> resourceEx)
        {
            _resourceEx = resourceEx;
        }

        void saver_OnAbort(object sender, EventArgs e)
        {
            RemoveItemFromSendDictionary((FileSaver)sender);
        }

        void receive_OnAbort(object sender, EventArgs e)
        {
            RemoveItemFromReceiveDictionary((ServerSideFileReceive)sender);
        }

        private void RemoveItemFromReceiveDictionary(ServerSideFileReceive receive)
        {
            lock (this)
            {
                receive.OnAbort -= new EventHandler(receive_OnAbort);
                _internalReceiveDic.Remove(receive.UserIdentity);
            }
        }

        private void RemoveItemFromSendDictionary(FileSaver saver)
        {
            lock (this)
            {
                saver.OnAbort -= new EventHandler(saver_OnAbort);
                _internalSendDic.Remove(saver.UserIdentity);
            }
        }


        #region ISourceTransferCRUD

        public bool Contains(UserIdentity userIdentity)
        {
            lock(this)
            {
                return _internalReceiveDic.ContainsKey(userIdentity) || _internalSendDic.ContainsKey(userIdentity);
            }
        }

        public FileSaveStatus InitSourceUpload(UserIdentity identity, TResource resource, SourceStatus status, out string otherResourceId)
        {
            lock (this)
            {
                otherResourceId = null;
                if (status == SourceStatus.New && _resourceEx.IsExists(resource))
                    return FileSaveStatus.Exists;
                if (_internalSendDic.ContainsKey(identity))
                    return FileSaveStatus.LoadInProgress;
                TResource stored = _resourceEx.GetStoredSource(resource);
                List<TResource> resourcesByName = _resourceEx.SearchByName(resource);
                if (resourcesByName != null && resourcesByName.Count > 0)
                {
                    otherResourceId = resourcesByName.First().Id;
                    return FileSaveStatus.ExistsWithSameName;
                }
                //if (stored != null && resourcesByName != null && !stored.Equals(resourcesByName)) return FileSaveStatus.Exists;
                //if (status == SourceStatus.Update && stored == null)
                //    return FileSaveStatus.Abort;
                IFileInfoProvider provider = GetFileInfoProvider(resource, stored);
                otherResourceId = provider.Identity;
                if (_internalSendDic.Select(kv => kv.Value.Provider.UniqueName).Contains(provider.UniqueName))
                    return FileSaveStatus.Exists;
                //ResourceFileInfo resourceFileInfo = resource.ResourceInfo as ResourceFileInfo;
                //if (resourceFileInfo == null)
                //    throw new InvalidOperationException(
                //        string.Format(
                //            "ServerSideSourceTransfer.InitSourceUpload: ResourceInfo должен быть типа ResourceFileInfo. Resource: {0}",
                //            resource.ResourceInfo.Name));
                FileSaver saver = new FileSaver(identity, provider, true);
                saver.UseUploadResume = true; // Докачивать
                saver.DeleteFilesAfterAbort = false; // Не удалять файлы после останова
                saver.OnAbort += new EventHandler(saver_OnAbort);
                _internalSendDic.Add(identity, new SendDictionaryItem(saver, provider));
                return FileSaveStatus.Ok;
                //ResourceDescriptor storedDescriptor = _resourceEx.GetStoredSource(resource);
                //if (storedDescriptor == null)
                //{
                //    resorcesIdList.AddRange(resourceFileInfo.ResourceFileDictionary.Select(kv => kv.Key));
                //    return new SourceTransferResponse(FileSaveStatus.Ok, resorcesIdList);
                //}
                //ResourceFileInfo storedResourceFileInfo = storedDescriptor.ResourceInfo as ResourceFileInfo;
                //foreach (KeyValuePair<string, ResourceFileProperty> keyValuePair in resourceFileInfo.ResourceFileDictionary)
                //{
                //    if (!storedResourceFileInfo.ResourceFileDictionary.ContainsKey(keyValuePair.Key) ||
                //        storedResourceFileInfo.ResourceFileDictionary[keyValuePair.Key].ModifiedUtc < keyValuePair.Value.ModifiedUtc)
                //    {
                //        resorcesIdList.Add(keyValuePair.Key);
                //    }
                //}
                //return new SourceTransferResponse(FileSaveStatus.Ok, resorcesIdList);
            }
        }

        public TResource InitSourceDownload(UserIdentity identity, TResource resource)
        {
            forwardSeek = 0;
            TResource stored = null;
            lock (this)
            {
                ReceiveDictionaryItem item;
                if (_internalReceiveDic.TryGetValue(identity, out item)) return null;
                stored = _resourceEx.GetStoredSource(resource);
                if (stored == null) return null;
                IFileInfoProvider provider = GetFileInfoProvider(resource, stored);
                ServerSideFileReceive receiver = new ServerSideFileReceive(identity, provider);
                item = new ReceiveDictionaryItem(receiver, provider);// new ServerSideFileReceive<TResource>(identity, stored, _resourceEx);
                item.Receiver.OnAbort += new EventHandler(receive_OnAbort);
                //receive.OnComplete += new EventHandler(receive_OnComplete);
                _internalReceiveDic[identity] = item;
            }
            return stored;
        }

        int forwardSeek = 0;
        public int ForwardMoveNeeded()
        {
            return forwardSeek;
        }

        double currentSpeed = 0;
        public double GetCurrentSpeed()
        {
            return currentSpeed;
        }
        string currentFile = null;
        public string GetCurrentFile()
        {
            return currentFile;
        }

        public FileSaveStatus SaveSource(UserIdentity identity, TResource resource, SourceStatus status, out string newResourceId)
        {
            newResourceId = null;
            if (status == SourceStatus.New && _resourceEx.IsExists(resource)) return FileSaveStatus.Exists;
            return null == _resourceEx.SaveSource(identity, resource) ? FileSaveStatus.Exists : FileSaveStatus.Ok;
        }

        public void DoneSourceTransfer(UserIdentity identity)
        {
            SendDictionaryItem sendItem;
            if (_internalSendDic.TryGetValue(identity, out sendItem))
            {
                sendItem.Saver.Commit();
                RemoveItemFromSendDictionary(sendItem.Saver);
            }

            ReceiveDictionaryItem receiveItem;
            if (_internalReceiveDic.TryGetValue(identity, out receiveItem))
            {
                receiveItem.Receiver.Commit();
                RemoveItemFromReceiveDictionary(receiveItem.Receiver);
            }

        }

        protected abstract IFileInfoProvider GetFileInfoProvider(TResource resource, TResource storedResourceId);

        #endregion


        #region Implementation of IFileTransfer

        public FileSaveStatus Send(UserIdentity userIdentity, FileTransferObject obj)
        {
            SendDictionaryItem item;
            if (!_internalSendDic.TryGetValue(userIdentity, out item))
                throw new InvalidOperationException(
                    string.Format("ServerSideSourceTransfer.Send: в коллекции отсутсвует пользователь: {0}",
                                  userIdentity));
            bool ok = item.Saver.Save(obj);
            if (obj.Part == 0) forwardSeek = item.Saver.PartSend;
            else forwardSeek = 0;
            currentSpeed = item.Saver.Speed;
            currentFile = item.Saver.CurrentFile;
            return FileSaveStatus.Ok;
        }

        public FileTransferObject? Receive(UserIdentity userIdentity, string resourceId)
        {
            ReceiveDictionaryItem item;
            if (!_internalReceiveDic.TryGetValue(userIdentity, out item)) return null;
            return item.Receiver.Receive(resourceId);
        }

        public void Terminate(UserIdentity userIdentity)
        {
            SendDictionaryItem sendItem;
            ReceiveDictionaryItem receiverItem;
            lock (this)
            {
                _internalSendDic.TryGetValue(userIdentity, out sendItem);
                _internalReceiveDic.TryGetValue(userIdentity, out receiverItem);
            }
            if (sendItem != null)
                sendItem.Saver.Terminate();
            if (receiverItem != null)
                receiverItem.Receiver.Terminate();
        }

        #endregion

    }
}

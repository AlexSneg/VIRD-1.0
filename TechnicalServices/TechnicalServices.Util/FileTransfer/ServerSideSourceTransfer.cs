using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TechnicalServices.Common;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Util.FileTransfer;

namespace TechnicalServices.Util
{
    public class ServerSideSourceTransfer : ServerSideAbstractTransfer<ResourceDescriptor>   // : IServerResourceCRUD
    {
        //private readonly Dictionary<UserIdentity, SendDictionaryItem> _internalSendDic = new Dictionary<UserIdentity, SendDictionaryItem>();
        //private readonly Dictionary<UserIdentity, ServerSideFileReceive> _internalReceiveDic = new Dictionary<UserIdentity, ServerSideFileReceive>();
        //private readonly IResourceEx<ResourceDescriptor> _resourceEx;

        #region Nested
        
        #endregion

        public ServerSideSourceTransfer(IResourceEx<ResourceDescriptor> resourceEx)
            : base(resourceEx)
        {
        }

        //void saver_OnAbort(object sender, EventArgs e)
        //{
        //    RemoveItemFromSendDictionary((FileSaver)sender);
        //}

        //void receive_OnAbort(object sender, EventArgs e)
        //{
        //    RemoveItemFromReceiveDictionary((ServerSideFileReceive)sender);
        //}

        #region ISourceTransferCRUD

        //public FileSaveStatus InitSourceUpload(UserIdentity identity, ResourceDescriptor descriptor, SourceStatus status)
        //{
        //    lock (this)
        //    {
        //        if (status == SourceStatus.New && _resourceEx.IsExists(descriptor))
        //            return FileSaveStatus.Exists;
        //        if (_internalSendDic.ContainsKey(identity))
        //            return FileSaveStatus.LoadInProgress;
        //        if (_internalSendDic.Select(kv => kv.Value.Provider.IdentityName).Contains(descriptor.ResourceInfo.Name))
        //            return FileSaveStatus.Exists;
        //        ResourceFileInfo resourceFileInfo = descriptor.ResourceInfo as ResourceFileInfo;
        //        if (resourceFileInfo == null)
        //            throw new InvalidOperationException(
        //                string.Format(
        //                    "ServerSideSourceTransfer.InitSourceUpload: ResourceInfo должен быть типа ResourceFileInfo. Resource: {0}",
        //                    descriptor.ResourceInfo.Name));

        //        IFileInfoProvider provider = new SourceInfoProvider(_resourceEx, descriptor);
        //        FileSaver saver = new FileSaver(identity, provider, true);
        //        saver.OnAbort += new EventHandler(saver_OnAbort);
        //        _internalSendDic.Add(identity, new SendDictionaryItem(saver, provider));
        //        return FileSaveStatus.Ok;
        //        //ResourceDescriptor storedDescriptor = _resourceEx.GetStoredSource(descriptor);
        //        //if (storedDescriptor == null)
        //        //{
        //        //    resorcesIdList.AddRange(resourceFileInfo.ResourceFileDictionary.Select(kv => kv.Key));
        //        //    return new SourceTransferResponse(FileSaveStatus.Ok, resorcesIdList);
        //        //}
        //        //ResourceFileInfo storedResourceFileInfo = storedDescriptor.ResourceInfo as ResourceFileInfo;
        //        //foreach (KeyValuePair<string, ResourceFileProperty> keyValuePair in resourceFileInfo.ResourceFileDictionary)
        //        //{
        //        //    if (!storedResourceFileInfo.ResourceFileDictionary.ContainsKey(keyValuePair.Key) ||
        //        //        storedResourceFileInfo.ResourceFileDictionary[keyValuePair.Key].ModifiedUtc < keyValuePair.Value.ModifiedUtc)
        //        //    {
        //        //        resorcesIdList.Add(keyValuePair.Key);
        //        //    }
        //        //}
        //        //return new SourceTransferResponse(FileSaveStatus.Ok, resorcesIdList);
        //    }
        //}

        //public ResourceDescriptor InitSourceDownload(UserIdentity identity, ResourceDescriptor resourceDescriptor)
        //{
        //    ResourceDescriptor stored = null;
        //    lock (this)
        //    {
        //        ServerSideFileReceive receive;
        //        if (_internalReceiveDic.TryGetValue(identity, out receive)) return null;
        //        stored = _resourceEx.GetStoredSource(resourceDescriptor);
        //        if (stored == null) return null;
        //        receive = new ServerSideFileReceive(identity, stored, _resourceEx);
        //        receive.OnAbort += new EventHandler(receive_OnAbort);
        //        //receive.OnComplete += new EventHandler(receive_OnComplete);
        //        _internalReceiveDic[identity] = receive;
        //    }
        //    return stored;
        //}

        //public FileSaveStatus SaveSource(UserIdentity identity, ResourceDescriptor descriptor, SourceStatus status)
        //{
        //    if (status == SourceStatus.New && _resourceEx.IsExists(descriptor)) return FileSaveStatus.Exists;
        //    return _resourceEx.SaveSource(identity, descriptor);
        //}

        //public void DoneSourceTransfer(UserIdentity identity, ResourceDescriptor descriptor)
        //{
        //    SendDictionaryItem item;
        //    if (_internalSendDic.TryGetValue(identity, out item))
        //    {
        //        item.Saver.Commit();
        //        RemoveItemFromSendDictionary(item.Saver);
        //    }

        //    ServerSideFileReceive receive;
        //    if (_internalReceiveDic.TryGetValue(identity, out receive))
        //    {
        //        RemoveItemFromReceiveDictionary(receive);
        //    }

        //}

        //private void RemoveItemFromReceiveDictionary(ServerSideFileReceive receive)
        //{
        //    lock (this)
        //    {
        //        receive.OnAbort -= new EventHandler(saver_OnAbort);
        //        _internalReceiveDic.Remove(receive.UserIdentity);
        //    }
        //}

        //private void RemoveItemFromSendDictionary(FileSaver saver)
        //{
        //    lock (this)
        //    {
        //        saver.OnAbort -= new EventHandler(saver_OnAbort);
        //        _internalSendDic.Remove(saver.UserIdentity);
        //    }
        //}

        #endregion


        #region Implementation of IFileTransfer

        //public FileSaveStatus Send(UserIdentity userIdentity, FileTransferObject obj)
        //{
        //    SendDictionaryItem item;
        //    if (!_internalSendDic.TryGetValue(userIdentity, out item))
        //        throw new InvalidOperationException(
        //            string.Format("ServerSideSourceTransfer.Send: в коллекции отсутсвует пользователь: {0}",
        //                          userIdentity));
        //    bool ok = item.Saver.Save(obj);
        //    return FileSaveStatus.Ok;
        //}

        //public FileTransferObject? Receive(UserIdentity userIdentity, string resourceId)
        //{
        //    ServerSideFileReceive receive;
        //    if (!_internalReceiveDic.TryGetValue(userIdentity, out receive)) return null;
        //    return receive.Receive(resourceId);
        //}

        //public void Terminate(UserIdentity userIdentity)
        //{
        //    SendDictionaryItem item;
        //    ServerSideFileReceive receiver;
        //    lock (this)
        //    {
        //        _internalSendDic.TryGetValue(userIdentity, out item);
        //        _internalReceiveDic.TryGetValue(userIdentity, out receiver);
        //    }
        //    if (item != null)
        //        item.Saver.Terminate();
        //    if (receiver != null)
        //        receiver.Terminate();
        //}

        #endregion

        #region Overrides of ServerSideAbstractTransfer<ResourceDescriptor>

        protected override IFileInfoProvider GetFileInfoProvider(ResourceDescriptor resource, ResourceDescriptor stored)
        {
            ResourceFileInfo resourceFileInfo = resource.ResourceInfo as ResourceFileInfo;
            if (resourceFileInfo == null)
                throw new InvalidOperationException(
                    string.Format(
                        "ServerSideSourceTransfer.InitSourceUpload: ResourceInfo должен быть типа ResourceFileInfo. Resource: {0}",
                        resource.ResourceInfo.Name));

            return new SourceInfoProvider(_resourceEx, resource, stored);
        }

        #endregion
    }
}

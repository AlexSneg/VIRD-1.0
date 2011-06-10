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
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Util.FileTransfer
{
    public class ClientSideSourceTransfer : ClientSideAbstractTransfer<ResourceDescriptor, ResourceFileProperty>
    {
        private readonly ISourceTransferCRUD _contract;
        private readonly IResourceEx<ResourceDescriptor> _resourceEx;
        private ResourceDescriptor _workedResource;

        public ClientSideSourceTransfer(ISourceTransferCRUD contract, IResourceEx<ResourceDescriptor> resourceDAL)
        {
            _contract = contract;
            _resourceEx = resourceDAL;
        }

        protected override void DoneDownload(UserIdentity userIdentity)
        {
            _contract.DoneSourceTransfer(userIdentity);
        }

        protected override IFileTransfer FileTransport
        {
            get { return _contract; }
        }

        protected override IFileInfoProvider<ResourceFileProperty> GetFileInfoProvider(
            ResourceDescriptor resource, ResourceDescriptor serverResource)
        {
            return new ClientSideSourceInfoProvider(_resourceEx, resource, serverResource);
        }

        protected override ResourceDescriptor InitDownload(UserIdentity userIdentity,
            ResourceDescriptor resource)
        {
            ResourceFileInfo resourceFileInfo = resource.ResourceInfo as ResourceFileInfo;
            if (resourceFileInfo == null) return null;
            ResourceDescriptor resourceDescriptorfromServer = _contract.InitSourceDownload(userIdentity, resource);
            if (resourceDescriptorfromServer == null || !(resourceDescriptorfromServer.ResourceInfo is ResourceFileInfo)) return null;
            return resourceDescriptorfromServer;
        }

        protected override FileSaveStatus OnCreateSource(ResourceDescriptor resource, out string otherResourceId)
        {
            return Upload(resource, SourceStatus.New, out otherResourceId);
        }

        protected override FileSaveStatus OnSaveSource(ResourceDescriptor resource, out string otherResourceId)
        {
            return Upload(resource, SourceStatus.Update, out otherResourceId);
        }

        //protected override bool OnGetSource(ResourceDescriptor descriptor)
        //{
        //    UserIdentity userIdentity = Thread.CurrentPrincipal as UserIdentity;
        //    ResourceFileInfo resourceFileInfo = descriptor.ResourceInfo as ResourceFileInfo;
        //    if (resourceFileInfo == null) return true;

        //    ResourceDescriptor resourceDescriptorfromServer = _contract.InitSourceDownload(userIdentity, descriptor);
        //    try
        //    {
        //        if (resourceDescriptorfromServer == null) return false;
        //        ResourceFileInfo resourceFileInfofromServer = resourceDescriptorfromServer.ResourceInfo as ResourceFileInfo;
        //        if (resourceFileInfofromServer == null) return true;
        //        int numberOfParts = 0;
        //        int part = 0;
        //        foreach (ResourceFileProperty property in resourceFileInfofromServer.ResourceFileList)
        //        {
        //            numberOfParts += (int)(property.Length / (Constants.PartSize + 1)) + 1;
        //        }
        //        _provider = new SourceInfoProvider(_resourceEx, resourceDescriptorfromServer);
        //        _fileSaver = new FileSaver(userIdentity, _provider, true);

        //        foreach (ResourceFileProperty resourceFileProperty in resourceFileInfofromServer.ResourceFileList)
        //        {
        //            bool isComplete = false;
        //            FileTransferObject? obj;
        //            do
        //            {
        //                obj = _contract.Receive(userIdentity, resourceFileProperty.Id);
        //                if (_terminate.WaitOne(0) || !obj.HasValue)
        //                {
        //                    _contract.Terminate(userIdentity);
        //                    _fileSaver.Terminate();
        //                    return false;
        //                }
        //                isComplete = _fileSaver.Save(obj.Value);
        //                PartTransfer(part, numberOfParts);
        //                part++;
        //            } while (!isComplete);
        //        }
        //        return true;
        //    }
        //    catch
        //    {
        //        if (_fileSaver != null)
        //            _fileSaver.Terminate();
        //        throw;
        //    }
        //    finally
        //    {
        //        _contract.DoneSourceTransfer(userIdentity);
        //    }

        //}


        private FileSaveStatus Upload(ResourceDescriptor resourceDescriptor, SourceStatus status, out string otherResourceId)
        {
            UserIdentity userIdentity = Thread.CurrentPrincipal as UserIdentity;
            ResourceFileInfo resourceFileInfo = resourceDescriptor.ResourceInfo as ResourceFileInfo;
            
            if (resourceFileInfo == null)
            {
                 return _contract.SaveSource(userIdentity, resourceDescriptor, status, out otherResourceId);
            }
            else
            {
                FileSaveStatus fileSaveStatus = _contract.InitSourceUpload(userIdentity, resourceDescriptor,
                                                                     status, out otherResourceId);
                if (fileSaveStatus != FileSaveStatus.Ok) return fileSaveStatus;
                try
                {
                    return Send(userIdentity, resourceDescriptor);
                }
                finally
                {
                    _contract.DoneSourceTransfer(userIdentity);
                }
            }
        }

        private FileSaveStatus Send(UserIdentity userIdentity, ResourceDescriptor resourceDescriptor)
        {
            ResourceFileInfo resourceFileInfo = resourceDescriptor.ResourceInfo as ResourceFileInfo;
            if (resourceFileInfo == null) throw new InvalidOperationException(
                string.Format("ClientSideSourceTransfer.Send: ResourceInfo должен быть типа ResourceFileInfo. Resource: {0}",
                resourceDescriptor.ResourceInfo.Name));

            // формируем список файлов у ресурса для передачи
            IEnumerable<ResourceFileProperty> resourceFileProperties = resourceFileInfo.ResourceFileList.
                Where(rfp => rfp.Newly && File.Exists(rfp.ResourceFullFileName));
            if (0 == resourceFileProperties.Count()) return FileSaveStatus.Ok;

            int numberOfParts = 0, throughPart = 0;
            foreach (ResourceFileProperty property in resourceFileProperties)
            {
                long length = property.Length;
                numberOfParts += (int)(length / (Constants.PartSize + 1)) + 1;
            }

            FileSaveStatus saveStatus = FileSaveStatus.Exists;
            byte[] buffer = new byte[Constants.PartSize];
            foreach (ResourceFileProperty property in resourceFileProperties)
            {
                using (FileStream reader = File.OpenRead(property.ResourceFullFileName))
                {
                    int bufferSize = 0;
                    int numberOfPartsForFile = (int)(property.Length / (Constants.PartSize + 1)) + 1;
                    for (int part = 0; (bufferSize = reader.Read(buffer, 0, buffer.Length)) > 0; part++)
                    {
                        if (_terminate.WaitOne(0))
                        {
                            _contract.Terminate(userIdentity);
                            return FileSaveStatus.Abort;
                        }
                        FileTransferObject obj = new FileTransferObject(bufferSize, part, numberOfPartsForFile,
                                                                        property.Id, buffer);
                        saveStatus = _contract.Send(userIdentity, obj);
                        UploadSpeed(_contract.GetCurrentSpeed(), _contract.GetCurrentFile());
                        if (saveStatus != FileSaveStatus.Ok) break;
                        if (part == 0)
                        {
                            try
                            {
                                int forwardSeek = _contract.ForwardMoveNeeded();
                                if (forwardSeek > 0)
                                {
                                    for (int i = 1; i < forwardSeek-1; i++)
                                    {
                                        reader.Read(buffer, 0, buffer.Length);
                                        part++;
                                    }
                                }
                            }
                            catch(Exception ex)
                            {
                            }
                        }
                        else PartTransfer(throughPart, numberOfParts);
                        throughPart++;
                    }
                }
                property.Newly = false;
            }
            return saveStatus;
        }
    }
}
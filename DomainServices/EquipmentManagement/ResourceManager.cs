using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using DomainServices.EquipmentManagement.AgentCommon;
using TechnicalServices.Common;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.CommonPresentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Util;

namespace DomainServices.EquipmentManagement.AgentManagement
{
    public class ResourceManager : SourceDAL, IResourceManager
    {
        private readonly ServerSideSourceTransfer _serverSideSourceTransfer;
        public ResourceManager(IConfiguration config)
            : base(config, false)
        {
            _serverSideSourceTransfer = new ServerSideSourceTransfer(this);
        }

        private bool IsNewly(ResourceDescriptor resourceDescriptor, string resourceId)
        {
            ResourceFileInfo resourceFileInfo = resourceDescriptor.ResourceInfo as ResourceFileInfo;
            if (resourceFileInfo == null) return false;
            string resourceFileName = GetResourceInfoFullFileName(resourceDescriptor);
            if (!File.Exists(resourceFileName)) return true;
            ResourceInfo stored = resourceDescriptor.ResourceInfo.GetResourceInfo(resourceFileName, ExtraTypes);
            ResourceFileInfo storedResourceFileInfo = stored as ResourceFileInfo;
            if (storedResourceFileInfo == null) return false;
            ResourceFileProperty resourceFileProperty =
                storedResourceFileInfo.ResourceFileList.Find(rfp => rfp.Id.Equals(resourceId));
            return resourceFileProperty == null
                       ? true
                       :
                           resourceFileProperty.ModifiedUtc.CompareTo(
                           resourceFileInfo.ResourceFileList.Find(rfp => rfp.Id.Equals(resourceId)).ModifiedUtc) < 0;
            //if (resourceFileProperty == null)
            //    return true;
            //return resourceFileProperty.ModifiedUtc.CompareTo(resourceFileInfo.ResourceFileDictionary[resourceId].ModifiedUtc) < 0;
        }

        #region Implementation of IResourceManager

        public ResourceDescriptor[] GetResourcesForUpload(ResourceDescriptor[] resourceDescriptors,
            out bool isEnoughFreeSpace)
        {
            List<ResourceDescriptor> resourcesForUpload = new List<ResourceDescriptor>(resourceDescriptors.Length);
            foreach (ResourceDescriptor resourceDescriptor in resourceDescriptors)
            {
                ResourceFileInfo resourceFileInfo = resourceDescriptor.ResourceInfo as ResourceFileInfo;
                if (resourceFileInfo == null) continue;
                //List<ResourceFileProperty> resourceIdList = new List<ResourceFileProperty>(resourceFileInfo.ResourceFileDictionary.Count);
                foreach (ResourceFileProperty property in resourceFileInfo.ResourceFileList)
                {
                    //string resourceFileName = GetRealResourceFileName(resourceDescriptor, pair.Key);
                    if (!IsResourceExists(resourceDescriptor, property.Id) || IsNewly(resourceDescriptor, property.Id))
                    {
                        //resourceIdList.Add(pair.Value);
                        property.Newly = true;
                    }
                }
                if (resourceFileInfo.ResourceFileList.Any(rfp => rfp.Newly))
                    resourcesForUpload.Add(resourceDescriptor);
                //if (resourceIdList.Count > 0)
                //    resourcesForUpload.Add(new ResourceForUpload(resourceDescriptor, resourceIdList));
            }
            long requiredSpaceForLocal = 0;
            long requiredSpaceForGlobal = 0;
            foreach (ResourceDescriptor descriptor in resourcesForUpload)
            {
                ResourceFileInfo resourceFileInfo = descriptor.ResourceInfo as ResourceFileInfo;
                if (resourceFileInfo == null) continue;
                if (descriptor.IsLocal)
                    requiredSpaceForLocal += resourceFileInfo.ResourceFileList.Sum(rfp => rfp.Newly ? rfp.Length : 0);
                else
                    requiredSpaceForGlobal += resourceFileInfo.ResourceFileList.Sum(rfp => rfp.Newly ? rfp.Length : 0);
            }

            //foreach (ResourceForUpload resourceForAgentUpload in resourcesForUpload)
            //{
            //    ResourceDescriptor descriptor = resourceForAgentUpload.ResourceDescriptor;
            //    if (descriptor.IsLocal)
            //        requiredSpaceForLocal += resourceForAgentUpload.ResourcePropertyList.Sum(rfp => rfp.Length);
            //    else
            //        requiredSpaceForGlobal += resourceForAgentUpload.ResourcePropertyList.Sum(rfp => rfp.Length);
            //}
            DriveInfo localDriveInfo = new DriveInfo(
                Path.GetFullPath(_configuration.LocalSourceFolder)[0].ToString());
            DriveInfo globalDriveInfo = new DriveInfo(
                Path.GetFullPath(_configuration.GlobalSourceFolder)[0].ToString());
            if (localDriveInfo.AvailableFreeSpace > requiredSpaceForLocal
                &&
                globalDriveInfo.AvailableFreeSpace > requiredSpaceForGlobal)
                isEnoughFreeSpace = true;
            else
                isEnoughFreeSpace = false;
            return resourcesForUpload.ToArray();
        }

        public void CorrectResourceFileName(ResourceDescriptor descriptor)
        {
            ResourceFileInfo resourceFileInfo = descriptor.ResourceInfo as ResourceFileInfo;
            if (resourceFileInfo == null) return;
            foreach (ResourceFileProperty property in resourceFileInfo.ResourceFileList)
            {
                property.ResourceFullFileName = GetRealResourceFileName(descriptor, property.Id);
            }
        }

        public void DeleteAllSource()
        {
            string[] directories = new string[]
            {
                _configuration.LocalSourceFolder,
                _configuration.GlobalSourceFolder
            };
            foreach (string d in directories)
            {
                try
                {
                    foreach (string file in Directory.GetFiles(d))
                    {
                        File.Delete(file);
                    }
                    foreach (string directory in Directory.GetDirectories(d))
                    {
                        Directory.Delete(directory, true);
                    }
                }
                catch { }
            }
        }

        public void DeleteResourcesUploaded(ResourceDescriptor[] resourceDescriptors)
        {
            List<ResourceDescriptor> resourcesForUpload = new List<ResourceDescriptor>(resourceDescriptors.Length);
            foreach (ResourceDescriptor resourceDescriptor in resourceDescriptors)
            {
                ResourceFileInfo resourceFileInfo = resourceDescriptor.ResourceInfo as ResourceFileInfo;
                if (resourceFileInfo == null) continue;
                foreach (ResourceFileProperty property in resourceFileInfo.ResourceFileList)
                {
                    string fileName = GetResourceFileName(resourceDescriptor, property.Id);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        try
                        {
                            if(File.Exists(fileName)) File.Delete(fileName);
                            if (File.Exists(fileName+".temp")) File.Delete(fileName+".temp");
                            string xmlPath= Path.GetDirectoryName(fileName)+"\\"+resourceDescriptor.Id + ".resource.xml";
                            if (File.Exists(xmlPath)) File.Delete(xmlPath);
                        }
                        catch { }
                    }
                }

            }
        }


        #endregion

        #region Implementation of IFileTransfer

        public FileSaveStatus Send(UserIdentity userIdentity, FileTransferObject obj)
        {
            return _serverSideSourceTransfer.Send(userIdentity, obj);
        }

        public FileTransferObject? Receive(UserIdentity userIdentity, string resourceId)
        {
            return _serverSideSourceTransfer.Receive(userIdentity, resourceId);
        }

        public void Terminate(UserIdentity userIdentity)
        {
            _serverSideSourceTransfer.Terminate(userIdentity);
        }

        #endregion

        #region ISorceDAL overriding

        public override void CreateHardwareSources()
        { }

        public override string GetResourceFileName(ResourceDescriptor resourceDescriptor, string resourceId)
        {
            return GetRealResourceFileName(resourceDescriptor, resourceId);
        }

        public override ResourceDescriptor SaveSource(UserIdentity sender, ResourceDescriptor resourceDescriptor, Dictionary<string, string> fileDic)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                string infoFileName = GetResourceInfoFullFileName(resourceDescriptor);
                ResourceInfo info = resourceDescriptor.ResourceInfo;
                ResourceFileInfo resourceFileInfo = info as ResourceFileInfo;
                if (resourceFileInfo != null)
                    foreach (KeyValuePair<string, string> pair in fileDic)
                    {
                        string resourceFile = GetResourceFileName(resourceDescriptor, pair.Key);
                        string resourceTempFileName = pair.Value;
                        if (!string.IsNullOrEmpty(resourceFile) &&
                            !string.IsNullOrEmpty(resourceTempFileName) &&
                            !resourceFile.Equals(resourceTempFileName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            File.Delete(resourceFile);
                            File.Move(resourceTempFileName, resourceFile);
                        }
                    }
                info.SaveToFile(infoFileName, ExtraTypes);
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
            return resourceDescriptor;
        }

        public override List<ResourceDescriptor> SearchByName(ResourceDescriptor descriptor)
        {
            return null;
        }

        #endregion

        #region Implementation of ISourceTransferCRUD

        public FileSaveStatus InitSourceUpload(UserIdentity userIdentity, ResourceDescriptor resourceDescriptor, SourceStatus status, out string otherResourceId)
        {
            return _serverSideSourceTransfer.InitSourceUpload(userIdentity, resourceDescriptor, status, out otherResourceId);
        }

        public FileSaveStatus SaveSource(UserIdentity userIdentity, ResourceDescriptor resourceDescriptor, SourceStatus status, out string otherResourceId)
        {
            otherResourceId = null;
            return null == SaveSource(userIdentity, resourceDescriptor) ? FileSaveStatus.Abort : FileSaveStatus.Ok;
        }

        public void DoneSourceTransfer(UserIdentity userIdentity)
        {
            _serverSideSourceTransfer.DoneSourceTransfer(userIdentity);
        }

        public ResourceDescriptor InitSourceDownload(UserIdentity identity, ResourceDescriptor resourceDescriptor)
        {
            return _serverSideSourceTransfer.InitSourceDownload(identity, resourceDescriptor);
        }

        public int ForwardMoveNeeded()
        {
            return _serverSideSourceTransfer.ForwardMoveNeeded();
        }
        public double GetCurrentSpeed()
        {
            return _serverSideSourceTransfer.GetCurrentSpeed();
        }

        public string GetCurrentFile()
        {
            return _serverSideSourceTransfer.GetCurrentFile();
        }


        #endregion
    }
}

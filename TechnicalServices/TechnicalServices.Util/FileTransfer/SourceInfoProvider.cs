using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Util.FileTransfer
{
    internal class SourceInfoProvider : IFileInfoProvider<ResourceFileProperty>
    {
        protected readonly IResourceEx<ResourceDescriptor> _resourceEx;
        protected readonly ResourceDescriptor _resourceDescriptor;
        public SourceInfoProvider(IResourceEx<ResourceDescriptor> resourceEx,
            ResourceDescriptor resourceDescriptor, ResourceDescriptor storedResourceDescriptor)
        {
            _resourceEx = resourceEx;
            _resourceDescriptor = resourceDescriptor;
            if (storedResourceDescriptor != null && storedResourceDescriptor.ResourceInfo != null)
                _resourceDescriptor.ResourceInfo.Id = storedResourceDescriptor.ResourceInfo.Id;
        }

        protected virtual ResourceDescriptor WorkingResourceDescriptor
        {
            get
            {
                return _resourceDescriptor;
            }
        }

        public string GetResourceId(ResourceFileProperty property)
        {
            return property.Id;
        }

        public virtual string Identity
        {
            get { return WorkingResourceDescriptor.ResourceInfo.Id; }
        }

        public virtual string UniqueName
        {
            get { return WorkingResourceDescriptor.ResourceInfo.Name; }
        }

        public virtual string GetFileName(string fileId)
        {
            return _resourceEx.GetResourceFileName(WorkingResourceDescriptor, fileId);
        }

        public virtual void SaveFile(UserIdentity userIdentity, Dictionary<string, string> fileDic)
        {
            _resourceEx.SaveSource(userIdentity, WorkingResourceDescriptor, fileDic);
        }

        public virtual int GetNumberOfParts(string resourceId)
        {
            return (int)(((ResourceFileInfo)WorkingResourceDescriptor.ResourceInfo).ResourceFileList.Find(rfp => rfp.Id.Equals(resourceId)).Length / (Constants.PartSize + 1)) + 1;
        }

        public virtual int GetNumberOfParts()
        {
            return (int)((ResourceFileInfo)WorkingResourceDescriptor.ResourceInfo).ResourceFileList.Sum(
                rfp => (rfp.Length) / (Constants.PartSize + 1) + 1);
        }

        #region Implementation of IEnumerable

        public virtual IEnumerator<ResourceFileProperty> GetEnumerator()
        {
            return ((ResourceFileInfo)WorkingResourceDescriptor.ResourceInfo).ResourceFileList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }

    internal class ClientSideSourceInfoProvider : SourceInfoProvider
    {
        protected ResourceDescriptor _storedResourceDescriptor;
        private readonly List<ResourceFileProperty> _downloadedResourceList = new List<ResourceFileProperty>();

        public ClientSideSourceInfoProvider(IResourceEx<ResourceDescriptor> resourceEx,
            ResourceDescriptor resourceDescriptor, ResourceDescriptor storedResourceDescriptor)
            : base(resourceEx, resourceDescriptor, storedResourceDescriptor)
        {
            _storedResourceDescriptor = storedResourceDescriptor;
            // найден только те части которых реально нет на клиенте
            ResourceDescriptor clientDescriptor = _resourceEx.GetStoredSource(resourceDescriptor);
            List<ResourceFileProperty> list =
                ((ResourceFileInfo)storedResourceDescriptor.ResourceInfo).ResourceFileList;
            if (clientDescriptor == null)
            {
                _downloadedResourceList.AddRange(list);
            }
            else
            {
                foreach (ResourceFileProperty property in list)
                {
                    ResourceFileProperty clientProperty =
                        ((ResourceFileInfo)clientDescriptor.ResourceInfo).ResourceFileList.Find(
                            rfp => rfp.Id == property.Id);
                    string resourceFile = _resourceEx.GetResourceFileName(clientDescriptor, property.Id);
                    if (clientProperty == null || clientProperty.ModifiedUtc.CompareTo(property.ModifiedUtc) < 0
                        || !File.Exists(resourceFile))
                    {
                        _downloadedResourceList.Add(property);
                    }
                }
            }
        }

        protected override ResourceDescriptor WorkingResourceDescriptor
        {
            get
            {
                return _storedResourceDescriptor;
            }
        }

        public override int GetNumberOfParts()
        {
            return (int)_downloadedResourceList.Sum(
                rfp => (rfp.Length) / (Constants.PartSize + 1) + 1);
        }

        public override IEnumerator<ResourceFileProperty> GetEnumerator()
        {
            return _downloadedResourceList.GetEnumerator();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using TechnicalServices.Entity;

namespace TechnicalServices.Persistence.SystemPersistence.Resource
{
    [Serializable]
    [DataContract]
    public abstract class ResourceDescriptorAbstract : IEquatable<ResourceDescriptorAbstract>, IId
    {
        protected ResourceDescriptorAbstract(ResourceInfo resourceInfo)
        {
            ResourceInfo = resourceInfo;
        }

        [DataMember]
        public ResourceInfo ResourceInfo
        { get; set; }


        #region Implementation of IEquatable<ResourceDescriptorAbstract>

        public bool Equals(ResourceDescriptorAbstract other)
        {
            return this.ResourceInfo.Equals(other.ResourceInfo);
        }

        #endregion

        #region Implementation of IId

        public string Id
        {
            get { return ResourceInfo.Id; }
            set { ResourceInfo.Id = value; }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using TechnicalServices.Entity;
using System.Xml.Serialization;

namespace TechnicalServices.Persistence.SystemPersistence.Resource
{
    /// <summary>
    /// дескриптор ресурса - используется только для описания и нахождения ресурса на диске
    /// </summary>
    [Serializable]
	[DataContract]
    [TypeConverter("TechnicalServices.Common.TypeConverters.ResourceDescriptorConverter, TechnicalServices.Common")]
    public class ResourceDescriptor : ResourceDescriptorAbstract, IEquatable<ResourceDescriptor>, IComponent, IDisposable
    {
        public const string LocalString = " <Сценарий>";

        /// <summary>
        /// локальный/глобальный источник
        /// </summary>
        [Browsable(false)]
        [DataMember]
        public bool IsLocal
        { get; set; }


        [Browsable(false)]
        public bool Removed
        {
            get;
            set;
        }

        [Browsable(false)]
        public bool Created
        {
            get;
            set;
        }


        //protected ResourceDescriptor(string fileName)
        //{
        //    ResourceFileName = fileName;
        //}

        /// <summary>
        /// для глобального ресурса может быть null!!
        /// </summary>
        //public PresentationInfo PresentationInfo
        //{ get; set; }

        [Browsable(false)]
        [DataMember]
        public string PresentationUniqueName { get; set; }

        //[DataMember]
        //public ResourceInfo ResourceInfo
        //{ get; set; }

        public ResourceDescriptor(bool isLocal, string presentationUniqueName, ResourceInfo resourceInfo)
            : base(resourceInfo)
        {
            IsLocal = isLocal;
            //Type = type;
            //PresentationInfo = presentationInfo;
            //ResourceInfo = resourceInfo;
            PresentationUniqueName = presentationUniqueName;
        }

        public virtual bool Equals(ResourceDescriptor other)
        {
            return IsLocal == other.IsLocal &&
                   ResourceInfo.Equals(other.ResourceInfo)
                   &&
                   (!IsLocal || PresentationUniqueName.Equals(other.PresentationUniqueName, StringComparison.InvariantCultureIgnoreCase));
        }

        //public bool Equals(ResourceDescriptor other)
        //{
        //    return ResourceFileName.Equals(other.ResourceFileName, StringComparison.InvariantCultureIgnoreCase) &&
        //           IsLocal() == other.IsLocal();
        //}

        #region IComponent Members
        // Fields
        private static readonly object EventDisposed = new object();
        [NonSerialized]
        private EventHandlerList events;
        [NonSerialized]
        private ISite site;

        // Events
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Advanced)]
        public event EventHandler Disposed
        {
            add
            {
                this.Events.AddHandler(EventDisposed, value);
            }
            remove
            {
                this.Events.RemoveHandler(EventDisposed, value);
            }
        }

        // Methods
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (this)
                {
                    if ((this.site != null) && (this.site.Container != null))
                    {
                        this.site.Container.Remove(this);
                    }
                    if (this.events != null)
                    {
                        EventHandler handler = (EventHandler)this.events[EventDisposed];
                        if (handler != null)
                        {
                            handler(this, EventArgs.Empty);
                        }
                    }
                }
            }
        }

        ~ResourceDescriptor()
        {
            this.Dispose(false);
        }

        protected virtual object GetService(Type service)
        {
            ISite site = this.site;
            if (site != null)
            {
                return site.GetService(service);
            }
            return null;
        }

        public override string ToString()
        {
            return this.ResourceInfo.Name + (IsLocal ? LocalString : String.Empty);
        }

        // Properties
        protected virtual bool CanRaiseEvents
        {
            get
            {
                return true;
            }
        }

        internal bool CanRaiseEventsInternal
        {
            get
            {
                return this.CanRaiseEvents;
            }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IContainer Container
        {
            get
            {
                ISite site = this.site;
                if (site != null)
                {
                    return site.Container;
                }
                return null;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        protected bool DesignMode
        {
            get
            {
                ISite site = this.site;
                return ((site != null) && site.DesignMode);
            }
        }

        protected EventHandlerList Events
        {
            get
            {
                if (this.events == null)
                {
                    this.events = new EventHandlerList();
                }
                return this.events;
            }
        }

        [XmlIgnore]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual ISite Site
        {
            get
            {
                return this.site;
            }
            set
            {
                this.site = value;
            }
        }

        #endregion
    }

    [Serializable]
    [DataContract]
    public class BackgroundImageDescriptor : ResourceDescriptor
    {
        public BackgroundImageDescriptor(string presentationUniqueName, ResourceFileInfo resourceFileInfo)
            : base(true, presentationUniqueName, resourceFileInfo)
        {}

        public BackgroundImageDescriptor(string fileName, string presentationUniqueName)
            : base(true, presentationUniqueName, new ResourceFileInfo() {Type = Constants.BackGroundImage })
        {
            ((ResourceFileInfo)this.ResourceInfo).SetMasterResource(fileName);
        }

        public override bool Equals(ResourceDescriptor other)
        {
            BackgroundImageDescriptor descriptor = other as BackgroundImageDescriptor;
            if (descriptor == null) return base.Equals(other);
            return this.PresentationUniqueName.Equals(descriptor.PresentationUniqueName, StringComparison.InvariantCultureIgnoreCase)
                   &&
                   (this.ResourceInfo).Id.Equals(
                       (descriptor.ResourceInfo).Id,
                       StringComparison.InvariantCultureIgnoreCase);
        }
    }
}

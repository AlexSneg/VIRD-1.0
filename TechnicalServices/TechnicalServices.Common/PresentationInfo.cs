using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace TechnicalServices.Common
{
    /// <summary>
    /// класс для хранения в памяти инфы о презентациях, чтобы не держать всю презентацию
    /// </summary>
    [Serializable]
    public class PresentationInfo : IEquatable<PresentationInfo>
    {
        public string Name { get; set; }

        public string UniqueName { get; set; }

        //public string PresentationPath { get; set; }

        public PresentationInfo(Presentation presentation)
        {
            //PresentationPath = path;
            Name = presentation.Name;
            UniqueName = presentation.UniqueName;
        }

        public bool Equals(PresentationInfo other)
        {
            return Name.Equals(other.Name, StringComparison.InvariantCultureIgnoreCase)
                && UniqueName.Equals(other.UniqueName);
        }
    }

    [Serializable]
    public class PresentationDescriptor
    {
        private readonly List<ResourceDescriptor> _sourceDescriptors = new List<ResourceDescriptor>();

        public PresentationInfo PresentationInfo { get; set; }

        public string PresentationPath { get; set; }

        public PresentationDescriptor(Presentation presentation, string path)
        {
            PresentationInfo = new PresentationInfo(presentation);
            PresentationPath = path;
            SourceDescriptorList.AddRange(presentation.GetResource());
        }

        public List<ResourceDescriptor> SourceDescriptorList
        {
            get { return _sourceDescriptors; }
        }

        public bool ContainsSoftwareSource(ResourceDescriptor resourceDescriptor)
        {
            return SourceDescriptorList.Contains(resourceDescriptor);
        }

        public static explicit operator PresentationInfo(PresentationDescriptor descriptor)
        {
            return descriptor.PresentationInfo;
        }
    }
}
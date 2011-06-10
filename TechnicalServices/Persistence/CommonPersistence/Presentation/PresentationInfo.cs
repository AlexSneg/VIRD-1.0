using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Persistence.SystemPersistence.Presentation
{
    /// <summary>
    /// класс для хранения в памяти инфы о презентациях, чтобы не держать всю презентацию
    /// </summary>
    [DataContract(IsReference = true)]
    public class PresentationInfo : IEquatable<PresentationInfo>
    {
        [DataMember]
        private readonly List<SlideInfo> _slideInfoList = new List<SlideInfo>();
        [DataMember]
        private readonly List<DisplayGroup> _displayGroupList = new List<DisplayGroup>();
        [DataMember]
        private Dictionary<int, IList<LinkInfo>> _slideLinkInfoList = new Dictionary<int, IList<LinkInfo>>();
        [DataMember]
        private Dictionary<int, Point> _slidePositionList = new Dictionary<int, Point>();
        [DataMember]
        private Dictionary<string, int> _displayPositionList = new Dictionary<string, int>();

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string UniqueName { get; set; }
        [DataMember]
        public string Author { get; set; }
        [DataMember]
        public DateTime CreationDate { get; set; }
        [DataMember]
        public DateTime LastChangeDate { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public int SlideCount { get; set; }
        [DataMember]
        public int StartSlideId { get; set; }
				
        public List<SlideInfo> SlideInfoList
        {
            get { return _slideInfoList; }
        }

        public List<DisplayGroup> DisplayGroupList
        {
            get { return _displayGroupList; }
        }

        public Dictionary<int, IList<LinkInfo>> SlideLinkInfoList
        {
            get { return _slideLinkInfoList; }
        }

        public Dictionary<int, Point> SlidePositionList
        {
            get { return _slidePositionList; }
        }

        public Dictionary<string, int> DisplayPositionList
        {
            get { return _displayPositionList; }
        }


        public PresentationInfo(SystemPersistence.Presentation.Presentation presentation)
        {
            //PresentationPath = path;
            Name = presentation.Name ?? String.Empty;
            UniqueName = presentation.UniqueName;
            Author = presentation.Author ?? String.Empty;
            CreationDate = presentation.CreationDate;
            LastChangeDate = presentation.LastChangeDate;
            Comment = presentation.Comment ?? String.Empty;
            SlideCount = presentation.SlideList.Count;
            StartSlideId = presentation.StartSlideId;
            foreach (Slide slide in presentation.SlideList)
            {
                SlideInfoList.Add(new SlideInfo(this, slide));
                //List<LinkInfo> linkInfoList = new List<LinkInfo>();
                //foreach (Link link in slide.LinkList)
                //{
                //    linkInfoList.Add(new LinkInfo(link));
                //}
                //SlideLinkInfoList[slide.Id] = linkInfoList;
            }
            DisplayGroupList.AddRange(presentation.DisplayGroupList);
            foreach (KeyValuePair<int, Point> pair in presentation.SlidePositionList)
            {
                SlidePositionList[pair.Key] = pair.Value;
            }

            foreach (KeyValuePair<string, int> pair in presentation.DisplayPositionList)
            {
                DisplayPositionList[pair.Key] = pair.Value;
            }

            //линки
            foreach (KeyValuePair<int, SlideLinkList> pair in presentation.LinkDictionary)
            {
                if (pair.Value.LinkList.Count > 0)
                {
                    List<LinkInfo> linkInfoList = new List<LinkInfo>();
                    foreach (Link link in pair.Value.LinkList)
                    {
                        linkInfoList.Add(new LinkInfo(link));
                    }
                    SlideLinkInfoList.Add(pair.Key, linkInfoList);
                }
            }
        }

        public PresentationInfo(PresentationInfo other)
        {
            Name = other.Name;
            UniqueName = other.UniqueName;
            Author = other.Author;
            CreationDate = other.CreationDate;
            LastChangeDate = other.LastChangeDate;
            Comment = other.Comment;
            SlideCount = other.SlideCount;
            SlideInfoList.AddRange(other.SlideInfoList);
            DisplayGroupList.AddRange(other.DisplayGroupList);
            StartSlideId = other.StartSlideId;
            foreach (KeyValuePair<int, IList<LinkInfo>> pair in other.SlideLinkInfoList)
            {
                SlideLinkInfoList[pair.Key] = pair.Value;
            }
            foreach (KeyValuePair<int, Point> pair in other.SlidePositionList)
            {
                SlidePositionList[pair.Key] = pair.Value;
            }
            foreach (KeyValuePair<string, int> pair in other.DisplayPositionList)
            {
                DisplayPositionList[pair.Key] = pair.Value;
            }
        }

        public bool Equals(PresentationInfo other)
        {
            return
                //Name.Equals(other.Name, StringComparison.InvariantCultureIgnoreCase)
                //   && 
                   UniqueName.Equals(other.UniqueName);
        }

        public Presentation CreatePresentationStub()
        {
            Presentation presentation = new Presentation(this.UniqueName)
                {Author = this.Author, Comment = this.Comment,
                CreationDate = this.CreationDate, LastChangeDate = this.LastChangeDate,
                Name = this.Name};
            
            presentation.DisplayGroupList.AddRange(this.DisplayGroupList);
            foreach (KeyValuePair<int, Point> pair in this.SlidePositionList)
            {
                presentation.SlidePositionList.Add(pair.Key,
                    pair.Value);
            }
            foreach (KeyValuePair<string, int> pair in this.DisplayPositionList)
            {
                presentation.DisplayPositionList.Add(pair.Key,
                    pair.Value);
            }

            foreach (SlideInfo slideInfo in this.SlideInfoList)
            {
                presentation.SlideList.Add(slideInfo.CreateSlideStub());
            }

            presentation.StartSlide = presentation.SlideList.Find(
                sl=>sl.Id == this.StartSlideId);

            foreach (KeyValuePair<int, IList<LinkInfo>> pair in this.SlideLinkInfoList)
            {
                SlideLinkList slideLinkList = new SlideLinkList();
                foreach (LinkInfo linkInfo in pair.Value)
                {
                    Link link = linkInfo.CreateLinkStub();
                    link.NextSlide = presentation.SlideList.Find(
                        sl=>sl.Id == link.NextSlideId);
                    if (link.NextSlide == null)
                        throw new Exception(string.Format("PresentationInfo.CreatePresentationStub: Слайд {0} не найден",
                            link.NextSlideId));
                    slideLinkList.LinkList.Add(link);
                }
                presentation.LinkDictionary.Add(pair.Key,
                    slideLinkList);
            }

            return presentation;
        }

        public SlideInfo[] GetNextSlides(SlideInfo slideInfo)
        {
            if (slideInfo == null) return new SlideInfo[] {};
            IList<LinkInfo> linkList;
            if (SlideLinkInfoList.TryGetValue(slideInfo.Id, out linkList))
            {
                return SlideInfoList.Where(si => linkList.Any(li => li.NextSlideId == si.Id)).ToArray();
            }
            return new SlideInfo[] {};
        }

        public SlideInfo[] GetPrevSlides(SlideInfo slideInfo)
        {
            if (slideInfo == null) return new SlideInfo[] { };
            return SlideInfoList.Where(sl=>SlideLinkInfoList.Where(li => li.Value.Where(l => l.NextSlideId == slideInfo.Id).Count() > 0).Select(kv=>kv.Key).Any(id=>id==sl.Id)).ToArray();
        }

        public SlideInfo[] GetNeighboringSlides(SlideInfo slideInfo)
        {
            return GetNextSlides(slideInfo).Union(GetPrevSlides(slideInfo)).ToArray();
        }

    }

    [DataContract(IsReference = true)]
    public class PresentationInfoExt : PresentationInfo
    {
        [DataMember]
        public LockingInfo LockingInfo { get; set; }

        public PresentationInfoExt(PresentationInfo info, LockingInfo lockingInfo)
            : base(info)
        {
            LockingInfo = lockingInfo;
        }
    }

    [Serializable]
    public class PresentationDescriptor
    {
        private readonly List<ResourceDescriptor> _sourceDescriptors = new List<ResourceDescriptor>();
        private readonly List<DeviceResourceDescriptor> _deviceDescriptors = new List<DeviceResourceDescriptor>();

        public PresentationInfo PresentationInfo { get; set; }

        public string PresentationPath { get; set; }

        public PresentationDescriptor(SystemPersistence.Presentation.Presentation presentation, string path)
        {
            PresentationInfo = new PresentationInfo(presentation);
            PresentationPath = path;
            SourceDescriptorList.AddRange(presentation.GetResource());
            DeviceDescriptorList.AddRange(presentation.GetDeviceResource());
        }

			public List<ResourceDescriptor> SourceDescriptorList
        {
            get { return _sourceDescriptors; }
        }

			public List<DeviceResourceDescriptor> DeviceDescriptorList
        {
            get { return _deviceDescriptors; }
        }

        public bool ContainsSoftwareSource(ResourceDescriptor resourceDescriptor)
        {
            return SourceDescriptorList.Contains(resourceDescriptor);
        }

        public bool ContainsLabel(int labelId)
        {
            return PresentationInfo.SlideInfoList.Exists(si => si.LabelId == labelId);
        }

        public static explicit operator PresentationInfo(PresentationDescriptor descriptor)
        {
            return descriptor.PresentationInfo;
        }

        public bool ContainsDeviceSource(DeviceResourceDescriptor descriptor)
        {
            return DeviceDescriptorList.Contains(descriptor);
        }
    }
}
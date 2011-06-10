using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using TechnicalServices.Entity;

namespace TechnicalServices.Persistence.SystemPersistence.Presentation
{
    [Serializable]
    public class SlideInfo : IEquatable<SlideInfo>, IEqualityComparer<SlideInfo>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime Modified { get; set; }
        public string Comment { get; set; }
        public int LabelId { get; set; }
        public TimeSpan Time { get; set; }
        //public bool IsLocked { get; set; }
        public LockingInfo LockingInfo { get; set; }
        public PresentationInfo PresentationInfo { get; set; }

        public SlideInfo(PresentationInfo presentationInfo, Slide slide)
        {
            PresentationInfo = presentationInfo;
            Id = slide.Id;
            Name = slide.Name;
            Author = slide.Author;
            Modified = slide.Modified;
            Comment = slide.Comment;
            LabelId = slide.LabelId;
            Time = slide.Time;
        }

        public SlideInfo(Presentation presentation, Slide slide)
            :this(new PresentationInfo(presentation), slide)
        {
        }

        public SlideInfo(SlideInfo other)
        {
            PresentationInfo = other.PresentationInfo;
            Id = other.Id;
            Name = other.Name;
            Author = other.Author;
            Modified = other.Modified;
            Comment = other.Comment;
            LabelId = other.LabelId;
            Time = other.Time;
            //IsLocked = other.IsLocked;
            LockingInfo = LockingInfo;
        }
        
        public bool Equals(SlideInfo other)
        {
            return this.PresentationInfo.Equals(other.PresentationInfo) &&
                   this.Id == other.Id;
        }

        public Slide CreateSlideStub()
        {
            Slide slide = new Slide()
                {Author = this.Author, Comment = this.Comment, Id = this.Id,
                LabelId = this.LabelId, Name = this.Name, Modified = this.Modified,
                Time = this.Time};
            return slide;
        }

        #region Implementation of IEqualityComparer<SlideInfo>

        public bool Equals(SlideInfo x, SlideInfo y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(SlideInfo slideInfo)
        {
            return string.Format("{0}{1}", slideInfo.PresentationInfo.UniqueName, slideInfo.Id).GetHashCode();
        }

        #endregion
    }
}

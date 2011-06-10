using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;

namespace TechnicalServices.Persistence.SystemPersistence.Presentation
{
    [Serializable]
    public class LinkInfo
    {
        public int NextSlideId { get; set; }
        public bool IsDefault { get; set; }

        public LinkInfo(Link link)
        {
            NextSlideId = link.NextSlideId;
            IsDefault = link.IsDefault;
        }

        public Link CreateLinkStub()
        {
            Link link = new Link()
                            {
                                IsDefault = this.IsDefault,
                                NextSlideId = this.NextSlideId
                            };
            return link;
        }
    }
}

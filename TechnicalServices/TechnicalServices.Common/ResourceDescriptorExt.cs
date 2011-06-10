using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace TechnicalServices.Common
{
    public static class ResourceDescriptorExt
    {
        public static ResourceDescriptor[] GetResource(this Presentation presentation)
        {
            List<ResourceDescriptor> sourceDescriptors = new List<ResourceDescriptor>();
            foreach (Slide slide in presentation.SlideList)
            {
                foreach (Source source in slide.SourceList)
                {
                    SoftwareSource sw = source as SoftwareSource;
                    if (sw != null && !String.IsNullOrEmpty(sw.ContentPath))
                    {
                        sourceDescriptors.Add(new ResourceDescriptor(sw, new PresentationInfo(presentation)));
                        //if (sw.IsLocal)
                        //{
                        //    sourceDescriptors.Add(new LocalSourceDescriptor(sw,
                        //        new PresentationInfo(presentation)));
                        //}
                        //else
                        //{
                        //    sourceDescriptors.Add(new GlobalSourceDescriptor(sw.ContentPath));
                        //}
                    }
                }
                foreach (Display display in slide.DisplayList)
                {
                    ActiveDisplay activeDisplay = display as ActiveDisplay;
                    if (activeDisplay != null && !String.IsNullOrEmpty(activeDisplay.BackgroundImage))
                    {
                        sourceDescriptors.Add(new BackgroundImageDescriptor(activeDisplay.BackgroundImage,
                            new PresentationInfo(presentation)));
                    }
                }
            }
            return sourceDescriptors.ToArray();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Persistence.SystemPersistence.Presentation
{
    public static class PresentationExt
    {
        //[Obsolete]
        //public static Presentation SavePresentationLevelChanges(this Presentation presentation, Presentation other)
        //{
        //    presentation.Name = other.Name;
        //    presentation.StartSlide = other.StartSlide;
        //    presentation.Author = other.Author;
        //    presentation.Comment = other.Comment;
        //    // и раскладку слайдов
        //    foreach (Slide slide in presentation.SlideList)
        //    {
        //        if (slide.State == SlideState.New)
        //        {
        //            slide.State = SlideState.Normal;
        //            continue;
        //        }
        //        slide.LinkList.Clear();
        //        Slide otherSlide = other.SlideList.Find(sl => sl.Id == slide.Id);
        //        foreach (Link otherLink in otherSlide.LinkList)
        //        {
        //            slide.LinkList.Add(
        //                new Link()
        //                {IsDefault = otherLink.IsDefault,
        //                 NextSlide = presentation.SlideList.Find (sl => sl.Id == otherLink.NextSlide.Id)
        //                });
        //        }
        //    }
        //    // группы дисплеев
        //    presentation.DisplayGroupList.Clear();
        //    presentation.DisplayGroupList.AddRange(other.DisplayGroupList);
        //    // позиции слайдов
        //    presentation.SlidePositionList.Clear();
        //    foreach (KeyValuePair<int, Point> keyValuePair in other.SlidePositionList)
        //    {
        //        presentation.SlidePositionList.Add(keyValuePair.Key, keyValuePair.Value);
        //    }
        //    return presentation;
        //}

        //public static void ChangeResourceContentPathWithNewName(this Presentation presentation,
        //    string oldName, string newName)
        //{
        //    foreach (Slide slide in presentation.SlideList)
        //    {
        //        slide.ChangeResourceContentPathWithNewName(oldName,newName);            
        //    }
        //}
       
        public static Presentation SavePresentationLevelChanges(this Presentation presentation,
            PresentationInfo presentationInfoOther)
        {
            presentation.Name = presentationInfoOther.Name;
            presentation.StartSlide = presentation.SlideList.Find(
                sl => sl.Id == presentationInfoOther.StartSlideId);
            if (presentation.StartSlide == null)
                throw new Exception(String.Format("Slide {0} not exists",
                    presentationInfoOther.StartSlideId));
            presentation.Author = presentationInfoOther.Author;
            presentation.Comment = presentationInfoOther.Comment;

            // группы дисплеев
            presentation.DisplayGroupList.Clear();
            presentation.DisplayGroupList.AddRange(presentationInfoOther.DisplayGroupList);

            // позиции слайдов
            presentation.SlidePositionList.Clear();
            foreach (KeyValuePair<int, Point> keyValuePair in presentationInfoOther.SlidePositionList)
            {
                presentation.SlidePositionList.Add(keyValuePair.Key, keyValuePair.Value);
            }
            presentation.DisplayPositionList.Clear();
            foreach (KeyValuePair<string, int> keyValuePair in presentationInfoOther.DisplayPositionList)
            {
                presentation.DisplayPositionList.Add(keyValuePair.Key, keyValuePair.Value);
            }

            // линки
            presentation.LinkDictionary.Clear();
            foreach (KeyValuePair<int, IList<LinkInfo>> pair in presentationInfoOther.SlideLinkInfoList)
            {
                SlideLinkList slideLinkList = new SlideLinkList();
                foreach (LinkInfo linkInfo in pair.Value)
                {
                    Link link = new Link();
                    link.IsDefault = linkInfo.IsDefault;
                    link.NextSlide = presentation.SlideList.Find(
                        sl => sl.Id == linkInfo.NextSlideId);
                    if (link.NextSlide == null) throw new Exception(
                        String.Format("Slide {0} not exists", linkInfo.NextSlideId));
                    slideLinkList.LinkList.Add(link);
                }
                presentation.LinkDictionary.Add(pair.Key, slideLinkList);
            }
            return presentation;
        }

        public static int[] GetUsedLabels(this PresentationInfo presentationInfo)
        {
            return presentationInfo.SlideInfoList.Select(si => si.LabelId).Where(id=>id>0).Distinct().ToArray();
        }

        public static ResourceDescriptor[] GetResource(this Presentation presentation)
        {
            List<ResourceDescriptor> sourceDescriptors = new List<ResourceDescriptor>();
            foreach (Slide slide in presentation.SlideList)
            {
                sourceDescriptors.AddRange(slide.GetResource());
            }
            return sourceDescriptors.ToArray();
        }

        public static DeviceResourceDescriptor[] GetDeviceResource(this Presentation presentation)
        {
            List<DeviceResourceDescriptor> sourceDescriptors = new List<DeviceResourceDescriptor>();
            foreach (Slide slide in presentation.SlideList)
            {
                sourceDescriptors.AddRange(slide.GetDeviceResource(new PresentationInfo(presentation)));
            }
            return sourceDescriptors.ToArray();
        }

        public static ResourceDescriptor[] GetResource(this Slide slide)
        {
            List<ResourceDescriptor> sourceDescriptors = new List<ResourceDescriptor>();

            foreach (Source source in slide.SourceList)
            {
                if (source.ResourceDescriptor != null)
                    sourceDescriptors.Add(source.ResourceDescriptor);
            }
            return sourceDescriptors.ToArray();
        }

        public static DeviceResourceDescriptor[] GetDeviceResource(this Slide slide, PresentationInfo presentationInfo)
        {
            List<DeviceResourceDescriptor> deviceResourceDescriptors = new List<DeviceResourceDescriptor>();

            foreach (Device device in slide.DeviceList)
            {
                if (device.DeviceResourceDescriptor != null)
                    deviceResourceDescriptors.Add(device.DeviceResourceDescriptor);
            }
            return deviceResourceDescriptors.ToArray();
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Persistence.SystemPersistence.Presentation
{
    public static class SlideExt
    {
        public static Slide SaveSlideLevelChanges(this Slide slide, Slide other)
        {
            Slide clone = (Slide)other.Clone();

            slide.LabelId = clone.LabelId;
            slide.Name = clone.Name;
            slide.Time = clone.Time;
            slide.Comment = clone.Comment;
            slide.Author = clone.Author;
            slide.Modified = clone.Modified;

            slide.DeviceList.Clear();
            slide.DeviceList.AddRange(clone.DeviceList);

            slide.SourceList.Clear();
            slide.SourceList.AddRange(clone.SourceList);

            slide.DisplayList.Clear();
            slide.DisplayList.AddRange(clone.DisplayList);

            return slide;
        }

        //public static void ChangeResourceContentPathWithNewName(this Slide slide,
        //    string oldName, string newName)
        //{
        //    foreach (Source source in slide.SourceList)
        //    {
        //        SoftwareSource softwareSource = source as SoftwareSource;
        //        if (softwareSource != null &&
        //            softwareSource.ContentPath.Equals(oldName))
        //        {
        //            softwareSource.ContentPath = newName;
        //        }
        //    }
        //    foreach (Display display in slide.DisplayList)
        //    {
        //        ActiveDisplay activeDisplay = display as ActiveDisplay;
        //        if (activeDisplay != null &&
        //            activeDisplay.BackgroundImage.Equals(oldName))
        //        {
        //            activeDisplay.BackgroundImage = newName;
        //        }
        //    }

        //}
    }
}

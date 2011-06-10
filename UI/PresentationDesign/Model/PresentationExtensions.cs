using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UI.PresentationDesign.DesignUI.Classes.View;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System.Reflection;
using UI.PresentationDesign.DesignUI.Classes.Controller;

namespace UI.PresentationDesign.DesignUI.Classes.Model
{
    public static class PresentationExtensions
    {
        internal static List<Link> LinkList(this Slide slide)
        {
            if (!PresentationController.Instance.Presentation.LinkDictionary.ContainsKey(slide.Id))
                PresentationController.Instance.Presentation.LinkDictionary.Add(slide.Id, new SlideLinkList());
            return PresentationController.Instance.Presentation.LinkDictionary[slide.Id].LinkList;
        }
        

        internal static Slide Copy(this Slide from)
        {
            Slide result = new Slide();
            from.CopyTo(result);
            return result;
        }

        internal static void CopyTo(this Slide from, Slide result)
        {
            //copy everything, but not generics...
            foreach (PropertyInfo info in from.GetType().GetProperties())
            {
                object value = info.GetValue(from, null);
                if (value is ICloneable)
                    value = ((ICloneable)value).Clone();

                if (info.CanWrite)
                    info.SetValue(result, value, null);
            }
        }
    }
}

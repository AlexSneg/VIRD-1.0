using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using dotnetCHARTING.WinForms;
using System.Reflection;
using Hosts.Plugins.BusinessGraphics.UI.Controls;

namespace Hosts.Plugins.BusinessGraphics.SystemModule.Design
{
    public class ChartProperyProvider
    {
        static List<string> propertiesWeNeed =
           new List<string> { "Background",  /*"BackgroundImage", "BackgroundImageLayout", "BorderStyle",*/
            /*"ChartArea",*/ "ClipGauges", "DeafultElement", "Depth", "ExplodedSliceAmount", "PaletteName", 
            "PieLabelMode", "ShadingEffectMode", "Use3D"};

        static List<string> props;

        static ChartProperyProvider()
        {
            props = typeof(CustomChart).GetProperties().Where(p=>propertiesWeNeed.Contains(p.Name)).Select(p => p.Name).ToList();
        }

        public static PropertyDescriptorCollection GetProperties()
        {
            return new PropertyDescriptorCollection(TypeDescriptor.GetProperties(typeof(CustomChart)).OfType<PropertyDescriptor>().Where(pd => props.Contains(pd.Name)).ToArray());
        }

        public static PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return new PropertyDescriptorCollection(TypeDescriptor.GetProperties(typeof(CustomChart), attributes).OfType<PropertyDescriptor>().Where(pd => props.Contains(pd.Name)).ToArray());
        }
    }

    //public static class PropertyExtender
    //{
    //    public static void P()
    //    {
    //        Chart c = new Chart();
    //        c.DefaultSeries.Background.Color = System.Drawing.Color.Coral;
    //        string s = c.SaveState();
    //    }
    //}

}

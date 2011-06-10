using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;
using System.Windows.Forms;
using Hosts.Plugins.BusinessGraphics.SystemModule.Config;
using Hosts.Plugins.BusinessGraphics.SystemModule.Design;
using Hosts.Plugins.BusinessGraphics.UI;
using TechnicalServices.Interfaces.Comparers;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.BusinessGraphics.Visualizator
{
    public sealed class BusinessGraphicsVisualizatorModule : DomainVisualizatorModule<BusinessGraphicsVisualizatorDomain, Window>
    {
        private readonly BusinessGraphicsComparer _comparer = new BusinessGraphicsComparer();

        protected override IEqualityComparer<Window> EquatableComparer
        {
            get
            {
                return _comparer;
            }
        }

        #region Nested
        public class BusinessGraphicsComparer : BaseWindowEqualityComparer
        {
            public override bool Equals(Window x, Window y)
            {
                return false;
            }
        }
        #endregion
    }

    public sealed class BusinessGraphicsVisualizatorDomain : VisualizatorDomainAppl
    {
        public override Form CreateForm(DisplayType display, Window window, out bool needProcessing)
        {
            needProcessing = true;
            BusinessGraphicsSourceDesign sourceDesign = window.Source as BusinessGraphicsSourceDesign;
            if (sourceDesign == null) return null;
            if (window.Source.ResourceDescriptor == null) return null;
            BusinessGraphicsResourceInfo BusinessGraphicsResourceInfo = window.Source.ResourceDescriptor.ResourceInfo as BusinessGraphicsResourceInfo;
            if (BusinessGraphicsResourceInfo == null) return null;

            BusinessGraphicsForm form = new BusinessGraphicsForm(sourceDesign, window);
            needProcessing = false;
            return form;
        }
    }
    //public sealed class BusinessGraphicsVisualizatorModule : MainFormApplicationBased
    //{
    //    public override Form CreateForm(Window window)
    //    {
    //        BusinessGraphicsSourceDesign sourceDesign = window.Source as BusinessGraphicsSourceDesign;
    //        if (sourceDesign == null) return null;
    //        BusinessGraphicsResourceInfo BusinessGraphicsResourceInfo = window.Source.ResourceDescriptor.ResourceInfo as BusinessGraphicsResourceInfo;
    //        if (BusinessGraphicsResourceInfo == null) return null;

    //        BusinessGraphicsForm form = new BusinessGraphicsForm();
    //        form.LoadBusinessGraphics(BusinessGraphicsResourceInfo.ResourceFullFileName, sourceDesign.AspectLock);
    //        return form;
    //    }
    //}
}

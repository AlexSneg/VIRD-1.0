using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;
using System.Windows.Forms;
using Hosts.Plugins.ArcGISMap.SystemModule.Config;
using Hosts.Plugins.ArcGISMap.SystemModule.Design;
using TechnicalServices.Interfaces.Comparers;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Hosts.Plugins.ArcGISMap.UI.Controls;
using Hosts.Plugins.ArcGISMap.UI;

namespace Hosts.Plugins.ArcGISMap.Visualizator
{
    public sealed class ArcGISMapVisualizatorModule : DomainVisualizatorModule<ArcGISMapVisualizatorDomain, Window>
    {
        private readonly ArcGISMapComparer _comparer = new ArcGISMapComparer();

        protected override IEqualityComparer<Window> EquatableComparer
        {
            get
            {
                return _comparer;
            }
        }

        #region Nested
        public class ArcGISMapComparer : BaseWindowEqualityComparer
        {
            public override bool Equals(Window x, Window y)
            {
                return false;
            }
        }
        #endregion
    }

    public sealed class ArcGISMapVisualizatorDomain : VisualizatorDomainAppl
    {
        public override Form CreateForm(DisplayType display, Window window, out bool needProcessing)
        {
            needProcessing = true;
            ArcGISMapSourceDesign sourceDesign = window.Source as ArcGISMapSourceDesign;
            if (sourceDesign == null) return null;
            if (window.Source.ResourceDescriptor == null) return null;
            ArcGISMapResourceInfo ArcGISMapResourceInfo = window.Source.ResourceDescriptor.ResourceInfo as ArcGISMapResourceInfo;
            if (ArcGISMapResourceInfo == null) return null;

            ArcGISMapForm form = new ArcGISMapForm(sourceDesign, window);
            return form;
        }
    }
}

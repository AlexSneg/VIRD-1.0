using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;
using Hosts.Plugins.PowerPointPresentation.SystemModule.Config;
using Hosts.Plugins.PowerPointPresentation.SystemModule.Design;
using Hosts.Plugins.PowerPointPresentation.UI;
using TechnicalServices.Interfaces.Comparers;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.PowerPointPresentation.Visualizator
{
    public sealed class PowerPointPresentationVisualizatorModule : DomainVisualizatorModule<PowerPointVisualizatorDomain, Window>
    {
        private readonly PowerPointEqualityComparer _comparer = new PowerPointEqualityComparer();

        protected override IEqualityComparer<Window> EquatableComparer
        {
            get { return _comparer; }
        }

        #region Nested type: PowerPointEqualityComparer

        private class PowerPointEqualityComparer : BaseWindowEqualityComparer
        {
            public override bool Equals(Window x, Window y)
            {
                if (x == y) return true;

                PowerPointPresentationSourceDesign sourceX = x.Source as PowerPointPresentationSourceDesign;
                PowerPointPresentationSourceDesign sourceY = y.Source as PowerPointPresentationSourceDesign;
                if (sourceX == null || sourceY == null) return false;
                if (sourceX.ResourceDescriptor == null || sourceY.ResourceDescriptor == null) return false;

                PowerPointResourceInfo resourceInfoX = sourceX.ResourceDescriptor.ResourceInfo as PowerPointResourceInfo;
                PowerPointResourceInfo resourceInfoY = sourceY.ResourceDescriptor.ResourceInfo as PowerPointResourceInfo;
                if (resourceInfoX == null || resourceInfoY == null) return false;

                return (base.Equals(x, y))
                       &&
                       (resourceInfoX.MasterResourceProperty.ResourceFileName.Equals(
                            resourceInfoY.MasterResourceProperty.ResourceFileName,
                            StringComparison.InvariantCultureIgnoreCase) &&
                        resourceInfoX.MasterResourceProperty.ModifiedUtc.Equals(
                            resourceInfoY.MasterResourceProperty.ModifiedUtc));
            }
        }

        #endregion

    }

    public sealed class PowerPointVisualizatorDomain : VisualizatorDomainAppl
    {
        public override Form CreateForm(DisplayType display, Window window, out bool needProcessing)
        {
            needProcessing = true;
            PowerPointPresentationSourceDesign sourceDesign = window.Source as PowerPointPresentationSourceDesign;
            if (sourceDesign == null) return null;
            PowerPointResourceInfo powerPointResourceInfo = window.Source.ResourceDescriptor.ResourceInfo as PowerPointResourceInfo;
            if (powerPointResourceInfo == null) return null;

            PowerPointForm form = new PowerPointForm();
            if (!form.Init(display, window)) return null;
            form.LoadPresentation(powerPointResourceInfo.ResourceFullFileName);

            needProcessing = false;
            return form;
        }
    }
}

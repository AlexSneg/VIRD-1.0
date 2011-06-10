using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;
using Hosts.Plugins.WordDocument.SystemModule.Config;
using Hosts.Plugins.WordDocument.SystemModule.Design;
using Hosts.Plugins.WordDocument.UI;
using TechnicalServices.Interfaces.Comparers;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.WordDocument.Visualizator
{
    public sealed class WordDocumentVisualizatorModule : DomainVisualizatorModule<WordVisualizatorDomain, Window>
    {
        private readonly WordEqualityComparer _comparer = new WordEqualityComparer();

        protected override IEqualityComparer<Window> EquatableComparer
        {
            get { return _comparer; }
        }

        #region Nested type: WordEqualityComparer

        private class WordEqualityComparer : BaseWindowEqualityComparer
        {
            public override bool Equals(Window x, Window y)
            {
                if (x == y) return true;

                WordDocumentSourceDesign sourceX = x.Source as WordDocumentSourceDesign;
                WordDocumentSourceDesign sourceY = y.Source as WordDocumentSourceDesign;
                if (sourceX == null || sourceY == null) return false;
                if (sourceX.ResourceDescriptor == null || sourceY.ResourceDescriptor == null) return false;

                WordResourceInfo resourceInfoX = sourceX.ResourceDescriptor.ResourceInfo as WordResourceInfo;
                WordResourceInfo resourceInfoY = sourceY.ResourceDescriptor.ResourceInfo as WordResourceInfo;
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

    public sealed class WordVisualizatorDomain : VisualizatorDomainAppl
    {
        public override Form CreateForm(DisplayType display, Window window, out bool needProcessing)
        {
            needProcessing = true;
            WordDocumentSourceDesign sourceDesign = window.Source as WordDocumentSourceDesign;
            if (sourceDesign == null) return null;
            WordResourceInfo WordResourceInfo = window.Source.ResourceDescriptor.ResourceInfo as WordResourceInfo;
            if (WordResourceInfo == null) return null;
            WordForm form = new WordForm(sourceDesign.StartPage, sourceDesign.StartLine, sourceDesign.StartZoom);
            if (!form.Init(display, window)) return null;
            form.LoadPresentation(WordResourceInfo.ResourceFullFileName);

            needProcessing = false;
            return form;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows.Forms;

using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;

using Hosts.Plugins.Image.SystemModule.Config;
using Hosts.Plugins.Image.SystemModule.Design;
using Hosts.Plugins.Image.UI;

using TechnicalServices.Interfaces.Comparers;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.Image.Visualizator
{
    public sealed class ImageVisualizatorModule : DomainVisualizatorModule<ImageVisualizatorDomain, Window>
    {
        private readonly ImageWindowEqualityComparer _comparer = new ImageWindowEqualityComparer();

        protected override IEqualityComparer<Window> EquatableComparer
        {
            get { return _comparer; }
        }

        protected override IEnumerable<string> GetChangedState(Source source1, Source source2)
        {
            return new string[] {};
        }

        #region Nested type: ImageWindowEqualityComparer

        private class ImageWindowEqualityComparer : BaseWindowEqualityComparer
        {
            public override bool Equals(Window x, Window y)
            {
                if (x == y) return true;

                ImageSourceDesign sourceX = x.Source as ImageSourceDesign;
                ImageSourceDesign sourceY = y.Source as ImageSourceDesign;
                if (sourceX == null || sourceY == null) return false;
                if (sourceX.ResourceDescriptor == null || sourceY.ResourceDescriptor == null) return false;

                ImageResourceInfo resourceInfoX = sourceX.ResourceDescriptor.ResourceInfo as ImageResourceInfo;
                ImageResourceInfo resourceInfoY = sourceY.ResourceDescriptor.ResourceInfo as ImageResourceInfo;
                if (resourceInfoX == null || resourceInfoY == null) return false;

                return (base.Equals(x, y))
                       &&
                       (sourceX.ContentPath.Equals(sourceY.ContentPath, StringComparison.InvariantCultureIgnoreCase))
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

    public sealed class ImageVisualizatorDomain : VisualizatorDomainAppl
    {
        public override Form CreateForm(DisplayType display, Window window, out bool needProcessing)
        {
            needProcessing = true;
            ImageSourceDesign sourceDesign = window.Source as ImageSourceDesign;
            if (sourceDesign == null) return null;
            ImageResourceInfo imageResourceInfo = window.Source.ResourceDescriptor.ResourceInfo as ImageResourceInfo;
            if (imageResourceInfo == null) return null;

            ImageForm form = new ImageForm();
            if (!form.Init(display, window)) return null;
            form.LoadImage(imageResourceInfo.ResourceFullFileName, sourceDesign.AspectLock);
            needProcessing = false;

            return form;
        }
    }

    //public sealed class ImageVisualizatorModule : MainFormApplicationBased
    //{
    //    public override Form CreateForm(Window window)
    //    {
    //        ImageSourceDesign sourceDesign = window.Source as ImageSourceDesign;
    //        if (sourceDesign == null) return null;
    //        ImageResourceInfo imageResourceInfo = window.Source.ResourceDescriptor.ResourceInfo as ImageResourceInfo;
    //        if (imageResourceInfo == null) return null;

    //        ImageForm form = new ImageForm();
    //        form.LoadImage(imageResourceInfo.ResourceFullFileName, sourceDesign.AspectLock);
    //        return form;
    //    }
    //}
}
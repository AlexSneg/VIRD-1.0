using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;
using Hosts.Plugins.IEDocument.SystemModule.Config;
using Hosts.Plugins.IEDocument.SystemModule.Design;
using Hosts.Plugins.IEDocument.UI;
using TechnicalServices.Interfaces.Comparers;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Hosts.Plugins.IEDocument.Common;
using System.Reflection;

namespace Hosts.Plugins.IEDocument.Visualizator
{
    public sealed class IEDocumentVisualizatorModule : DomainVisualizatorModule<IEVisualizatorDomain, Window>
    {
        private readonly IEEqualityComparer _comparer = new IEEqualityComparer();

        protected override IEqualityComparer<Window> EquatableComparer
        {
            get { return _comparer; }
        }

        #region Nested type: IEEqualityComparer

        //private class IEEqualityComparer : BaseWindowEqualityComparer
        //{
        //    public override bool Equals(Window x, Window y)
        //    {
        //        if (x == y) return true;

        //        IEDocumentSourceDesign sourceX = x.Source as IEDocumentSourceDesign;
        //        IEDocumentSourceDesign sourceY = y.Source as IEDocumentSourceDesign;
        //        if (sourceX == null || sourceY == null) return false;
        //        if (sourceX.ResourceDescriptor == null || sourceY.ResourceDescriptor == null) return false;

        //        IEResourceInfo resourceInfoX = sourceX.ResourceDescriptor.ResourceInfo as IEResourceInfo;
        //        IEResourceInfo resourceInfoY = sourceY.ResourceDescriptor.ResourceInfo as IEResourceInfo;
        //        if (resourceInfoX == null || resourceInfoY == null) return false;

        //        return (base.Equals(x, y))
        //               &&
        //               (resourceInfoX.MasterResourceProperty.ResourceFileName.Equals(
        //                    resourceInfoY.MasterResourceProperty.ResourceFileName,
        //                    StringComparison.InvariantCultureIgnoreCase) &&
        //                resourceInfoX.MasterResourceProperty.ModifiedUtc.Equals(
        //                    resourceInfoY.MasterResourceProperty.ModifiedUtc));
        //    }
        //}

        #endregion

        #region Nested type: New IEEqualityComparer

        private class IEEqualityComparer : ActiveWindowEqualityComparer
        {
            public override bool Equals(Window x, Window y)
            {
                if (x == y) return true;

                IEDocumentSourceDesign sourceX = x.Source as IEDocumentSourceDesign;
                IEDocumentSourceDesign sourceY = y.Source as IEDocumentSourceDesign;
                if (sourceX == null || sourceY == null) return false;
                if (sourceX.ResourceDescriptor == null || sourceY.ResourceDescriptor == null) return false;

                IEResourceInfo resourceInfoX = sourceX.ResourceDescriptor.ResourceInfo as IEResourceInfo;
                IEResourceInfo resourceInfoY = sourceY.ResourceDescriptor.ResourceInfo as IEResourceInfo;
                if (resourceInfoX == null || resourceInfoY == null) return false;

                return (base.Equals(x, y))
                        //&&
                        //(sourceX.RemoteControl == sourceY.RemoteControl)
                        &&
                        (sourceX.Url == sourceY.Url)
                        &&
                        (resourceInfoX.Id == resourceInfoY.Id);
                       //(resourceInfoX.MasterResourceProperty.ResourceFileName.Equals(
                       //     resourceInfoY.MasterResourceProperty.ResourceFileName,
                       //     StringComparison.InvariantCultureIgnoreCase) &&
                       // resourceInfoX.MasterResourceProperty.ModifiedUtc.Equals(
                       //     resourceInfoY.MasterResourceProperty.ModifiedUtc));
            }
        }

        #endregion

    }

    public sealed class IEVisualizatorDomain : VisualizatorDomainAppl
    {
        IEForm form;
        public override Form CreateForm(DisplayType display, Window window, out bool needProcessing)
        {
            needProcessing = true;
            IEDocumentSourceDesign sourceDesign = window.Source as IEDocumentSourceDesign;
            if (sourceDesign == null) return null;
            IEResourceInfo IEResourceInfo = window.Source.ResourceDescriptor.ResourceInfo as IEResourceInfo;
            if (IEResourceInfo == null) return null;

            //if (form != null)
            //{
            //    form.Dispose();
            //    //Tell the WidowsInterop to Unhook
            //    System.GC.Collect();
            //    Hosts.Plugins.IEDocument.Common.WindowsInterop.Unhook();
            //}
            
            //Tell the WidowsInterop to Hook messages
            WindowsInterop.Hook();
            form = new IEForm(IEResourceInfo.Login, IEResourceInfo.Password, sourceDesign.Zoom, IEResourceInfo.PostParams, IEResourceInfo.PostParamsEncoding);
            if (!form.Init(display, window)) return null;
            form.LoadPresentation(IEResourceInfo.Url);

            needProcessing = false;
            return form;
        }

        protected void Dispose(bool disposing)
        {
            if (disposing && (form != null))
            {
                form.Dispose();
            }
            //Tell the WidowsInterop to Unhook
            Hosts.Plugins.IEDocument.Common.WindowsInterop.Unhook();
        }
    }
}

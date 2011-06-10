using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;
using Hosts.Plugins.VNC.Common;
using Hosts.Plugins.VNC.SystemModule.Config;
using Hosts.Plugins.VNC.SystemModule.Design;
using Hosts.Plugins.VNC.UI;
using TechnicalServices.Interfaces.Comparers;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.VNC.Visualizator
{
    public sealed class VNCVisualizatorModule : DomainVisualizatorModule<VNCVisualizatorDomain, Window>
    {
        private class VNCWindowEqualityComparer : ActiveWindowEqualityComparer
        {
            public override bool Equals(Window x, Window y)
            {
                if (x == y) return true;

                VNCSourceDesign sourceX = x.Source as VNCSourceDesign;
                VNCSourceDesign sourceY = y.Source as VNCSourceDesign;
                if (sourceX == null || sourceY == null) return false;
                if (sourceX.ResourceDescriptor == null || sourceY.ResourceDescriptor == null) return false;

                VNCResourceInfo resourceInfoX = sourceX.ResourceDescriptor.ResourceInfo as VNCResourceInfo;
                VNCResourceInfo resourceInfoY = sourceY.ResourceDescriptor.ResourceInfo as VNCResourceInfo;
                if (resourceInfoX == null || resourceInfoY == null) return false;

                return (base.Equals(x,y))
                    &&
                    (sourceX.RemoteControl == sourceY.RemoteControl)
                    &&
                    (resourceInfoX.Id == resourceInfoY.Id);
            }
        }

        private readonly VNCWindowEqualityComparer _comparer = new VNCWindowEqualityComparer();

        protected override IEqualityComparer<Window> EquatableComparer
        {
            get { return _comparer; }
        }

        protected override IEnumerable<string> GetChangedState(Source source1, Source source2)
        {
            List<string> commandList = new List<string>(1);
            VNCSourceDesign sourceX = source1 as VNCSourceDesign;
            VNCSourceDesign sourceY = source2 as VNCSourceDesign;
            if (sourceX == null || sourceY == null) return commandList;
            if (sourceX.ConnectionStatus != sourceY.ConnectionStatus)
                commandList.Add(sourceY.ConnectionStatus == ConnectionStatus.Connected ?
                    VNCConnect.Instance.Command : VNCDisconnect.Instance.Command);
            return commandList;
        }

    }

    public sealed class VNCVisualizatorDomain : VisualizatorDomainAppl
    {
        public override Form CreateForm(DisplayType display, Window window, out bool needProcessing)
        {
            needProcessing = true;
            VNCSourceDesign source = window.Source as VNCSourceDesign;
            if (source == null) return null;

            VNCResourceInfo resourceInfo = source.ResourceDescriptor.ResourceInfo as VNCResourceInfo;
            if (resourceInfo == null) return null;

            VNCForm vncForm = new VNCForm();
            if (!vncForm.Init(display, window)) return null;
            vncForm.Init(resourceInfo.Uri, resourceInfo.Password,
                source.ConnectionStatus == ConnectionStatus.Connected,
                source.RemoteControl == RemoteControl.Enable);
            needProcessing = false;
            return vncForm;
        }
    }
    //public sealed class VNCVisualizatorModule : MainFormApplicationBased
    //{
    //    #region Overrides of MainFormApplicationBased

    //    public override Form CreateForm(Window window)
    //    {
    //        VNCSourceDesign source = window.Source as VNCSourceDesign;
    //        if (source == null) return null;

    //        VNCResourceInfo resourceInfo = source.ResourceDescriptor.ResourceInfo as VNCResourceInfo;
    //        if (resourceInfo == null) return null;

    //        VNCForm vncForm = new VNCForm();
    //        vncForm.Init(resourceInfo.Uri, resourceInfo.Password,
    //            source.ServerSideScale, source.ConnectionStatus == ConnectionStatus.Connected,
    //            source.RemoteControl == RemoteControl.Enable);

    //        return vncForm;
    //    }

    //    #endregion
    //}
}

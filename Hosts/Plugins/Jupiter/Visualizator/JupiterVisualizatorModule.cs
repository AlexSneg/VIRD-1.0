using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;

using Hosts.Plugins.Jupiter.SystemModule.Config;
using Hosts.Plugins.Jupiter.SystemModule.Design;
using Hosts.Plugins.Jupiter.UI;

using Microsoft.Win32;

using TechnicalServices.Interfaces.Comparers;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

//using CPAPI;

namespace Hosts.Plugins.Jupiter.Visualizator
{
    public sealed class JupiterVisualizatorModule :
        DomainVisualizatorModule<JupiterVisualizatorDomain, JupiterWindow>
    {
        private readonly JupiterWindowEqualityComparer _comparer = new JupiterWindowEqualityComparer();

        protected override IEqualityComparer<JupiterWindow> EquatableComparer
        {
            get { return _comparer; }
        }

        protected override IEnumerable<string> GetChangedState(Source source1, Source source2)
        {
            return
                source2.CommandList.Select(com2 => com2.command).Except(source1.CommandList.Select(com1 => com1.command));
        }

        #region Nested type: JupiterWindowEqualityComparer

        private class JupiterWindowEqualityComparer : ActiveWindowEqualityComparer, IEqualityComparer<JupiterWindow>
        {
            #region IEqualityComparer<JupiterWindow> Members

            public bool Equals(JupiterWindow x, JupiterWindow y)
            {
                if (x == y) return true;

                Source sourceX = x.Source;
                Source sourceY = y.Source;
                if (sourceX == null || sourceY == null) return false;

                return base.Equals(x, y) &&
                       sourceX.Type.UID == sourceY.Type.UID;
            }

            public int GetHashCode(JupiterWindow jupiterWindow)
            {
                return jupiterWindow.Source.Type.UID.GetHashCode();
            }

            #endregion
        }

        #endregion
    }

    public sealed class JupiterVisualizatorDomain : VisualizatorDomainAppl
    {
        protected override bool OnInit()
        {
            // Проверяем наличие ActiveX ProgID = "Galileo.GalileoCtrl.1"
            Guid guid = new Guid("884da716-8b6b-11d4-a820-009027a36ef3");
            string keyName = @"CLSID\{" + guid + "}";
            using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(keyName))
                if (key == null) return false;
            return base.OnInit();
        }

        public override Form CreateForm(DisplayType display, Window window, out bool needProcessing)
        {
            needProcessing = true;
            JupiterDisplayConfig wallDisplay = display as JupiterDisplayConfig;
            JupiterWindow wnd = window as JupiterWindow;
            if (wallDisplay == null || wnd == null) return null;
            if (!wnd.Source.Type.IsHardware) return null;

            JupiterForm videoForm = new JupiterForm();
            if (!videoForm.Init(wallDisplay, wnd)) return null;
            needProcessing = false;
            return videoForm;
        }
    }
}
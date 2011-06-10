using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

using TechnicalServices.Configuration.Configurator;
using TechnicalServices.Configuration.LoadModules;
using TechnicalServices.Logging.FileLogging;
using TechnicalServices.Logging.StubLogging;

using UI.Common.CommonUI.Forms;
using UI.PresentationDesign.ConfiguratorUI.Forms;

namespace Hosts.Configurator.ConfiguratorHost
{
    public class ConfiguratorHostImpl : IDisposable
    {
        private MainForm _mainForm;

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        public void Run()
        {
            using (StubLogging logging = new StubLogging())
            //using (ModuleLoader loader = new ModuleLoader(logging))
            {
                try
                {
                    ConfiguratorConfiguration config = new ConfiguratorConfiguration(logging);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU", false);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    using (_mainForm = new MainForm(config))
                        Application.Run(_mainForm);
                }
                catch (Exception ex)
                {
                    logging.WriteError(ex.ToString());
                    using (MessageBoxForm dlg = new MessageBoxForm())
                    {
                        dlg.ShowForm(null, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error,
                                     new[] {"OK"});
                    }
                }
            }
        }
    }
}
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;

using Hosts.Plugins.BusinessGraphics.SystemModule.Design;

using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

using Timer=System.Threading.Timer;

namespace Hosts.Plugins.BusinessGraphics.UI
{
    public partial class BusinessGraphicsForm : Form, IDoCommand
    {
        private readonly BusinessGraphicsSourceDesign _sourceCopy;
        private Timer refreshTimer;
        private BusinessGraphicsSourceDesign sourceDesign;
        private ActiveWindow wnd;

        //static long nextTick = DateTime.Now.Ticks;

        public BusinessGraphicsForm(BusinessGraphicsSourceDesign sourceDesign, Window window)
        {
            InitializeComponent();
            if (window is ActiveWindow)
            {
                wnd = (ActiveWindow)window;
                BackColor = wnd.BorderColorFrienly;
            }
            sourceDesign.Wnd = wnd;
            sourceDesign.IsPlayerMode = true;
            _sourceCopy = SourceDesignClone(sourceDesign);
            this.sourceDesign = sourceDesign;
            this.sourceDesign.InitializeChart(true);

            Controls.Clear();
            this.sourceDesign.AddChartToContainer(this, wnd);

            FormClosing += BusinessGraphicsForm_FormClosing;

            if (sourceDesign.ODBCRefreshInterval > 0)
            {
                int time = sourceDesign.ODBCRefreshInterval*1000;
                if (((BusinessGraphicsResourceInfo) sourceDesign.ResourceDescriptor.ResourceInfo).ProviderType ==
                    ProviderTypeEnum.ODBC)
                    refreshTimer = new Timer(RefreshChart, "Timer", time, time);
            }
            //начал делать тут проверку, а она оказывается не нужна//
            //nextTick += time; 
        }

        #region IDoCommand Members

        public string DoCommand(string command)
        {
            try
            {
                string[] cmdData = command.Split(',');
                switch (cmdData[0])
                {
                    case "getInteractiveState":
                        {
                            return sourceDesign.AllowUserInteraction.ToString();
                        }
                    case "interactive":
                        {
                            bool b;
                            if (bool.TryParse(cmdData[1], out b))
                                sourceDesign.AllowUserInteraction = b;
                            break;
                        }
                    case "default":
                        {
                            RefreshChart("default");
                            break;
                        }
                }
            }
            catch
            {
            }

            return String.Empty;
        }

        #endregion

        private BusinessGraphicsSourceDesign SourceDesignClone(BusinessGraphicsSourceDesign source)
        {
            BusinessGraphicsSourceDesign result;
            BinaryFormatter _serializer = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                _serializer.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                result = (BusinessGraphicsSourceDesign) _serializer.Deserialize(stream);
            }
            return result;
        }


        private void RefreshChart(object state)
        {
            Invoke(new MethodInvoker(() =>
                                         {
                                             if ((state is string) && ((state as string).Equals("default")))
                                                 sourceDesign = SourceDesignClone(_sourceCopy);
                                             if (sourceDesign != null)
                                             {
                                                 //Останавливаем перерисовку
                                                 this.SetStyle(ControlStyles.UserPaint |
                                                 ControlStyles.AllPaintingInWmPaint |
                                                 ControlStyles.OptimizedDoubleBuffer, true);
                                                 this.UpdateStyles();
                                                 
                                                 bool clearState = true;
                                                 if ((state is string) && ((state as string).Equals("Timer")))
                                                     clearState = false;
                                                 sourceDesign.InitializeData(true);
                                                 sourceDesign.InitializeChart(clearState);
                                                 Controls.Clear();
                                                 sourceDesign.AddChartToContainer(this, wnd);
                                                 //Возобновляем перерисовку
                                                 this.Visible = true;
                                                 this.Update();
                                             }
                                         }));
        }

        private void BusinessGraphicsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (refreshTimer != null)
            {
                refreshTimer.Dispose();
                refreshTimer = null;
            }
            sourceDesign.RemoveChartFromContainer(this);
        }
    }
}
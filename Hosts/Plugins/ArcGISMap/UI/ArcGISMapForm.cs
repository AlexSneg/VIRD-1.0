using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;
using Hosts.Plugins.ArcGISMap.SystemModule.Design;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.ArcGISMap.UI
{
    public partial class ArcGISMapForm : Form, IDoCommand
    {
        public ArcGISMapForm()
        {
            InitializeComponent();
        }
        private ActiveWindow wnd;

        public ArcGISMapForm(ArcGISMapSourceDesign sourceDesign, Window window)
        {
            InitializeComponent();
            if (window is ActiveWindow)
            {
                wnd = (ActiveWindow)window;
                BackColor = wnd.BorderColorFrienly;
            }
            //sourceDesign.Wnd = wnd;
            //sourceDesign.IsPlayerMode = true;
            //_sourceCopy = SourceDesignClone(sourceDesign);
            //this.sourceDesign = sourceDesign;
            //this.sourceDesign.InitializeChart(true);

            //Controls.Clear();
            //this.sourceDesign.AddChartToContainer(this, wnd);

        }

        #region IDoCommand Members

        string IDoCommand.DoCommand(string command)
        {
            string[] cmdData = command.Split(':');
            switch (cmdData[0])
            {
                case "MoveUp": map.MoveUp(); break;
                case "MoveDown": map.MoveDown(); break;
                case "MoveLeft": map.MoveLeft(); break;
                case "MoveRight": map.MoveRigth(); break;
                case "Scale": 
                    {

                        double scale=1;
                        if(double.TryParse(cmdData[1], out scale))
                        {
                          map.SetScale(scale);
                        }
                        break;
                    }
                default: throw new ArgumentException("Неверная команда");
            }
            return "Ok";
        }

        #endregion
    }
}

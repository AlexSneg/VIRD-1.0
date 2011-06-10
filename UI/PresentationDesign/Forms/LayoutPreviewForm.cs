using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;
using UI.PresentationDesign.DesignUI.Views;
using UI.PresentationDesign.DesignUI.Controls.Utils;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace UI.PresentationDesign.DesignUI.Forms
{
    public partial class LayoutPreviewForm : Office2007Form
    {
        Image img;

        public LayoutPreviewForm(Image AImage, Display display, float magnification)
        {
            InitializeComponent();
            img = AImage;
            model1.DocumentSize = new Syncfusion.Windows.Forms.Diagram.PageSize(display.Width, display.Height);
            model1.AppendChild(new ImageNode(img));

            zoomCombo.ConnectToView(this.diagram1.View);
            this.diagram1.View.Magnification = magnification;
            //zoomCombo.Text = ZoomComboHelper.GetTextForZoom(magnification);
            //this.diagram1.View.Magnification = Math.Min(((float)diagram1.Width - 40f) / (float)img.Width, ((float)diagram1.Height - 40f) / (float)img.Height) * 100f;
        }

        private void moveButton_Click(object sender, EventArgs e)
        {
            if (!moveButton.Checked)
                this.diagram1.Controller.ActivateTool(ToolDescriptor.PanTool);
            else
                this.diagram1.Controller.ActivateTool(ToolDescriptor.SelectTool);

            moveButton.Checked = !moveButton.Checked;
        }
      
    }
}

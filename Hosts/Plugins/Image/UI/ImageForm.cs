using System.Drawing;
using System.Windows.Forms;

using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;

using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

using Label=System.Windows.Forms.Label;

namespace Hosts.Plugins.Image.UI
{
    public partial class ImageForm : Form
    {
        public ImageForm()
        {
            InitializeComponent();
        }

        public void LoadImage(string imageLocation, bool aspectLock)
        {
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Image = System.Drawing.Image.FromFile(imageLocation);
        }

        internal bool Init(DisplayType display, Window window)
        {
            this.InitBorderTitle(window, pictureBox);

            return true;
        }
    }
}
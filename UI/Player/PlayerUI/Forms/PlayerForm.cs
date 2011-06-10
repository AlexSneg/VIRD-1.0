using Syncfusion.Windows.Forms.Tools;

namespace UI.Player.PlayerUI.Forms
{
    public partial class PlayerForm : RibbonForm
    {
        public PlayerForm()
        {
            InitializeComponent();
        }

        private void closeMenuButton_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void displaysMenuButton_CheckedChanged(object sender, System.EventArgs e)
        {
            this.dockingManager.SetDockVisibility(this.displayListControl, this.displaysMenuButton.Checked);
            this.dockingManager.SetDockVisibility(this.sourcesControl, this.sourcesMenuButton.Checked);
            this.dockingManager.SetDockVisibility(this.sourcesCommandControl, this.sourcesControlMenuButton.Checked);
            this.dockingManager.SetDockVisibility(this.equipmentControl, this.equipmentMenuButton.Checked);
            this.dockingManager.SetDockVisibility(this.equipmentCommandControl, this.equipmentControlMenuButton.Checked);
        }
    }
}
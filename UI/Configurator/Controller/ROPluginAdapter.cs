using System.ComponentModel;
using System.Windows.Forms;

namespace UI.PresentationDesign.ConfiguratorUI.Controller
{
    public class ROPluginAdapter : IPluginAdapter
    {
        public readonly PluginAdapter InnerAdapter;

        public ROPluginAdapter(TreeNode node)
        {
            InnerAdapter = new PluginAdapter(node);
        }

        #region IPluginAdapter Members

        [ReadOnly(true)]
        [DisplayName("Плагин включен")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.YesNoConverter, TechnicalServices.Common")]
        public bool Enabled
        {
            get { return InnerAdapter.Enabled; }
            set { }
        }

        #endregion
    }
}
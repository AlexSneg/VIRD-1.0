using System.ComponentModel;
using System.Windows.Forms;

namespace UI.PresentationDesign.ConfiguratorUI.Controller
{
    public class PluginAdapter : IPluginAdapter
    {
        private readonly TreeNode _node;
        private bool _enabled;

        public PluginAdapter(TreeNode node)
        {
            _node = node;
            _enabled = NodeHasChildren(_node);
        }

        #region IPluginAdapter Members

        [DisplayName("Плагин включен")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.YesNoConverter, TechnicalServices.Common")]
        public bool Enabled
        {
            get
            {
                return _enabled = _enabled || NodeHasChildren(_node);
                //var aggregate = _node.Tag as SoftwareSourceAggregate;
                //return _enabled = _enabled || NodeHasChildren(_node) || (null != aggregate && null != aggregate.Source);
            }
            set { _enabled = value; }
        }

        #endregion

        internal static bool NodeHasChildren(TreeNode nd)
        {
            return nd.Nodes.Count > 0;
        }
    }
}
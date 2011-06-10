using System;
using System.Windows.Forms;

namespace UI.PresentationDesign.ConfiguratorUI.Controller
{
    internal class PluginTagObject
    {
        private readonly TreeNode _node;
        private readonly ROPluginAdapter _roAdapter;
        public readonly Type Type;

        public PluginTagObject(Type tp, TreeNode node)
        {
            Type = tp;
            _node = node;
            _roAdapter = new ROPluginAdapter(_node);
        }

        public IPluginAdapter Adapter
        {
            get { return PluginAdapter.NodeHasChildren(_node) ? _roAdapter : (IPluginAdapter) _roAdapter.InnerAdapter; }
        }
    }
}
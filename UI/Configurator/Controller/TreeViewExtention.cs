using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace UI.PresentationDesign.ConfiguratorUI.Controller
{
    public static class TreeViewExtention
    {
        public static TreeNode FindNodeByPath(this TreeView tree, string[] path)
        {
            TreeNode found = null;
            TreeNodeCollection nodes = tree.Nodes;
            if (null != path)
                foreach (string text in path)
                {
                    TreeNode[] nodesArray = new TreeNode[nodes.Count];
                    nodes.CopyTo(nodesArray, 0);
                    IEnumerable<TreeNode> foundNodes = from node in nodesArray where node.Text == text select node;
                    found = null;
                    if (0 == foundNodes.Count()) break;
                    found = foundNodes.ElementAt(0);
                    nodes = found.Nodes;
                }
            return found;
        }

        public static string[] GetCorrectFullPath(this TreeNode node)
        {
            Stack<string> names = new Stack<string>();
            TreeNode curNode = node;
            do
            {
                names.Push(curNode.Text);
                curNode = curNode.Parent;
            } while (null != curNode);
            return names.ToArray();
        }
    }
}
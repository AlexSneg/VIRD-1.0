using System.Collections.Generic;
using Syncfusion.Windows.Forms.Tools;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Diagram;
using System.Drawing;
using System.Linq;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System;
using UI.PresentationDesign.DesignUI.Helpers;

namespace UI.PresentationDesign.DesignUI.Controls.SourceTree
{
    public class SourceResourcesView : SwitchableGroupView
    {
        SuperToolTip toolTip = new SuperToolTip();
        bool isDrag = false;
        SortedDictionary<ISourceNode, GroupViewItem> items;

        public ISourceNode SelectedSource
        {
            get
            {
                ISourceNode node = null;
                if ((base.SelectedItem >= 0) && (base.SelectedItem < items.Keys.Count))
                {
                    node = this.items.Keys.ToList()[base.SelectedItem];
                }
                return node;
            }
        }

        SourceGroupBar _parent;

        class nodeComparer : IComparer<ISourceNode>
        {
            #region IComparer<ISourceNode> Members

            public int Compare(ISourceNode x, ISourceNode y)
            {
                return String.Compare(x.Mapping.ResourceInfo.Name, y.Mapping.ResourceInfo.Name);
            }

            #endregion
        }

        public SourceResourcesView(SourceGroupBar Parent)
        {
            _parent = Parent;

            items = new SortedDictionary<ISourceNode, GroupViewItem>(new nodeComparer());
            this.SmallImageList = new ImageList();
            this.SmallImageView = true;
            this.ShowToolTips = false;
            this.ButtonView = true;
            this.ThemesEnabled = true;
        }

        public void AddResourceNode(ISourceNode node, bool selectAfterOperation)
        {
            GroupViewItem item;
            bool contains = items.ContainsKey(node);

            if (!contains)
            {
                if (node.Image != null)
                {
                    SmallImageList.Images.Add(node.Image);
                    item = new GroupViewItem(node.Mapping.ResourceInfo.Name, SmallImageList.Images.Count - 1);
                }
                else
                    item = new GroupViewItem(node.Mapping.ResourceInfo.Name, -1);

                items.Add(node, item);

                //if (!node.IsOnline && node.Mapping.ResourceInfo.IsHardware)
                //    MarkedItems.Add(item);
                MarkedItems[item] = !node.Mapping.ResourceInfo.IsHardware ? (bool?)null : node.IsOnline;

                if (items.Count == 1)
                    this.GroupViewItems.Add(item);
                else
                {
                    int i = items.Keys.ToList().IndexOf(node);
                    this.GroupViewItems.Insert(i, item);
                }
            }
            else
            {
                item = items[node];
            }

            item.ToolTipText = node.Mapping.ResourceInfo.Comment;
            item.Tag = node;

            if (selectAfterOperation)
            {
                this.SelectedItem = this.GroupViewItems.IndexOf(item);
                _parent.SelectItem(node);
            }
        }

        public void RefreshSourceName(ISourceNode source)
        {
            if (items.ContainsKey(source))
                items[source].Text = source.Mapping.ResourceInfo.Name;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            ISourceNode selectedNode = this.SelectedSource;
            if (selectedNode != null)
            {
                _parent.SelectItem(selectedNode);
            }

            isDrag = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            isDrag = false;
        }

        Point mousePoint;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (isDrag)
            {
                ISourceNode selectedNode = this.SelectedSource;
                if (selectedNode != null)
                {
                    Node node = selectedNode.Clone() as Node;
                    try
                    {
                        this.DoDragDrop(new DragDropData(node), DragDropEffects.Copy);
                    }
                    catch
                    {
                        //DragDrop registration failed
                    }
                    this.isDrag = false;
                    return;
                }
            }
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);

            GroupViewItem node = this.GetItemAt(PointToClient(Cursor.Position));

            if (node != null)
            {
                toolTip.Hide();

                if (!String.IsNullOrEmpty(node.ToolTipText))
                {
                    ToolTipInfo info = new ToolTipInfo();
                    info.Header.Text = node.Text;
                    info.Header.Font = new Font(info.Header.Font, System.Drawing.FontStyle.Bold);
                    info.Body.Text = node.ToolTipText;

                    Point p = Cursor.Position;
                    p.Offset(5, 5);
                    toolTip.Show(info, p, 2000);
                }
            }
        }

        public void RemoveNode(ISourceNode node)
        {
            GroupViewItems.Remove(items[node]);
            items.Remove(node);
            SelectedItem = this.GroupViewItems.Count - 1;
            if (SelectedItem > -1)
                _parent.SelectItem(this.GroupViewItems[SelectedItem].Tag as ISourceNode);
            else
                _parent.SelectItem(null);
        }

        public void SelectFirstItem()
        {
            if (this.GroupViewItems.Count > 0)
            {
                SelectedItem = 0;
                _parent.SelectItem(this.GroupViewItems[0].Tag as ISourceNode);
            }
            else
                _parent.SelectItem(null);
        }

        public void OnHardwareStateChanged(ISourceNode node, bool? online)
        {
            if (items.ContainsKey(node))
            {
                GroupViewItem item = items[node];

                MarkedItems[item] = online;
                //if (!online)
                //    MarkedItems.Add(item);
                //else
                //{
                //    if (MarkedItems.Contains(item))
                //        MarkedItems.Remove(item);
                //}

                this.Invalidate();
            }
        }
    }
}

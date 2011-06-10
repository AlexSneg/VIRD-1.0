using System.Collections.Generic;
using Syncfusion.Windows.Forms.Tools;
using System.Windows.Forms;
using System.Drawing;
using System;
using System.Linq;

namespace UI.PresentationDesign.DesignUI.Controls.SourceTree
{
    public delegate void ResourceNodeSelected(ISourceNode node);

    public class SourceGroupBar : GroupBar
    {
        readonly Dictionary<SourceCategory, SourceResourcesView> categories;
        Dictionary<ISourceNode, SourceResourcesView> itemsByViews;

        public event ResourceNodeSelected OnResourceSelected;
        SourceResourcesView first;

        public SourceGroupBar()
        {
            categories = new Dictionary<SourceCategory, SourceResourcesView>();
            this.ShowItemImageInHeader = true;
            this.ShowPopupGripper = false;
            itemsByViews = new Dictionary<ISourceNode, SourceResourcesView>();
        }

        protected override void OnGroupBarItemSelected(EventArgs arg)
        {
            base.OnGroupBarItemSelected(arg);

            SourceResourcesView v = GroupBarItems[SelectedItem].Client as SourceResourcesView;
            if (v != null)
                v.SelectFirstItem();
        }

        public void AddCategory(SourceCategory category, bool selectFirstItem)
        {
            GroupBarItem item = new GroupBarItem { Text = category.Type.Type };
            item.Icon = category.Icon;
            this.GroupBarItems.Add(item);
            SourceResourcesView view = new SourceResourcesView(this);

            if (first == null)
                first = view;

            view.Text = category.Comment;
            item.Client = view;
            categories.Add(category, view);
            
            category.Resources.ForEach(n => AddResource(category, n, false));

            if(selectFirstItem && category.Resources.Count>0)
                SelectItem(category.Resources.First());
        }

        public void AddResource(SourceCategory category, ISourceNode node, bool selectAfterOperation)
        {
            if (category.Resources != null)
                if (!category.Resources.Contains(node))
                    category.Resources.Add(node);

            if (categories.ContainsKey(category))
            {
                categories[category].AddResourceNode(node, selectAfterOperation);
                if(!itemsByViews.ContainsKey(node))
                itemsByViews.Add(node, categories[category]);
            }
        }

        public string SelectedItemText
        {
            get
            {
                int id = SelectedItem;
                if (id != -1)
                {
                    return this.GroupBarItems[id].Text;
                }

                return String.Empty;
            }
        }

        public void SetSelectedItem(ISourceNode node)
        {
            for (int i = 0; i < this.GroupBarItems.Count; i++)
            {
                GroupView view = (this.GroupBarItems[i].Client as GroupView);
                for(int j = 0; j < view.GroupViewItems.Count; j++)
                    if (view.GroupViewItems[j].Text == node.Name)
                    {
                        this.SelectedItem = i;
                        view.SelectedItem = j;
                    }
            }
        }

        internal void SelectItem(ISourceNode node)
        {
            if (OnResourceSelected != null)
                OnResourceSelected(node);
        }

        public void RefreshSourceName(ISourceNode source)
        {
            if (itemsByViews.ContainsKey(source))
                itemsByViews[source].RefreshSourceName(source);
        }


        public void RemoveNode(SourceCategory sourceCategory, ISourceNode node)
        {
            categories[sourceCategory].RemoveNode(node);
            sourceCategory.Resources.Remove(node);
        }

        public void SelectFirstItem()
        {
            if (first != null)
                first.SelectFirstItem();
        }

        public void OnHardwareStateChanged(ISourceNode node, bool? online)
        {
            if(itemsByViews.ContainsKey(node))
                itemsByViews[node].OnHardwareStateChanged(node, online);
        }
    }
}

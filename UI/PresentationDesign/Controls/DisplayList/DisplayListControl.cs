using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using UI.PresentationDesign.DesignUI.Controllers;
using Syncfusion.Windows.Forms;
using Domain.PresentationDesign.Client;
using UI.PresentationDesign.DesignUI.Helpers;

namespace UI.PresentationDesign.DesignUI.Controls.DisplayList
{
    public partial class DisplayListControl : UserControl, IDisposable
    {
        public DisplayTreeView DisplayTree
        {
            get
            {
                return displayTree;
            }
        }

        bool locked = false;

        string oldGroupName = String.Empty;

        bool isPlayerMode = false;

        public bool CreateDisplayGroupEnabled
        {
            get { return createDisplayGroup.Enabled; }
        }

        public bool RemoveDisplayGroupEnabled
        {
            get { return removeDisplayGroup.Enabled; }
        }


        public DisplayListControl()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                displayTree.AfterSelect += new EventHandler(displayTree_AfterSelect);
                Init();
            }
        }

        public void Init()
        {
            PresentationController.Instance.OnPresentationLockChanged += new PresentationLockChanged(Instance_OnPresentationLockChanged);
            PresentationController.Instance.OnPresentationChangedExternally += new PresentationDataChanged(Instance_OnPresentationChangedExternally);

            DisplayController.CreateDisplayController();
            DisplayController.Instance.InitDisplayController();

            DisplayController.Instance.OnDisplayGroupCreated += new DisplayGroupCreated(Instance_OnDisplayGroupCreated);
            DisplayController.Instance.OnEnableCheckboxes += new EnableCheckboxesChanged(Instance_OnEnableCheckboxes);
            DisplayController.Instance.OnDisplayForceChecked += new DisplayChecked(Instance_OnDisplayForceChecked);
            DisplayController.Instance.OnDisplayStateChanged += new DisplayStateChanged(Instance_OnDisplayStateChanged);
            DisplayController.Instance.OnSelectedDisplayChanged += Instance_OnSelectedDisplayChanged;

            locked = PresentationController.Instance.PresentationLocked || DesignerClient.Instance.IsStandAlone;
            Instance_OnPresentationLockChanged(locked);
            LoadDisplays(false);
        }

        void Instance_OnSelectedDisplayChanged(Display obj)
        {
            for(int i = 0; i < displayTree.Nodes.Count; i++)
            {
                if(displayTree.Nodes[i].Text == obj.Name)
                {
                    displayTree.SelectedNode = displayTree.Nodes[i];
                    return;
                }
                for(int j = 0; j < displayTree.Nodes[i].Nodes.Count; j++)
                    if(displayTree.Nodes[i].Nodes[j].Text == obj.Name)
                    {
                        displayTree.SelectedNode = displayTree.Nodes[i].Nodes[j];
                        return;
                    }
            }
        }

        void Instance_OnDisplayStateChanged(Display disp, bool? isOnline)
        {
            foreach (TreeNodeAdv node in this.displayTree.Nodes)
            {
                if (node is DisplayGroupNode)
                {
                    foreach (TreeNodeAdv n in node.Nodes)
                        if (n is DisplayNode && (n as DisplayNode).Display == disp)
                            n.TextColor = isOnline.HasValue ? (isOnline.Value ? Color.Green : Color.Red) : Color.Black;
                            //n.TextColor = isOnline ? Color.Green : Color.Red;
                }
                else if (node is DisplayNode)
                {
                    if ((node as DisplayNode).Display == disp)
                        node.TextColor = isOnline.HasValue ? (isOnline.Value ? Color.Green : Color.Red) : Color.Black;
                        //node.TextColor = isOnline ? Color.Green : Color.Red;
                }
            }
        }

        private void LoadDisplays(bool rememberLastDisplay)
        {
            displayTree.Nodes.Clear();
            DisplayNode d1 = null;
            DisplayGroupNode d2 = null;

            TreeNodeAdv tNode = displayTree.SelectedNode;
            // Выбираем дисплеи в том порядке, в котором их отсортировал пользователь
            var query = from d in DisplayController.Instance.UngrouppedDisplays()
                        join ord in PresentationController.Instance.Presentation.DisplayPositionList
                        on d.Name equals ord.Key
                        into d_ord
                        from sub in d_ord.DefaultIfEmpty()
                        orderby (sub.Key == null ? -1 : sub.Value)
                        select d;

            foreach (Display disp in query/*DisplayController.Instance.UngrouppedDisplays()*/)
            {
                DisplayNode node = new DisplayNode(disp);
                //if (DisplayController.IsPlayerMode)
                bool? state = DisplayController.Instance.DisplayStates[disp];
                node.TextColor = state.HasValue ? (state.Value ? Color.Green : Color.Red) : Color.Black;
                //node.TextColor = DisplayController.Instance.DisplayStates[disp] ? Color.Black : Color.Red;
                if (d1 == null)
                {
                    d1 = node;
                    displayTree.Nodes.Add(d1);
                }
                else
                    displayTree.Nodes.Add(node);
            }

            foreach (var dd in DisplayController.Instance.GrouppedDisplays())
            {
                DisplayGroupNode groupNode = new DisplayGroupNode(dd.Key);

                if (d2 == null)
                    d2 = groupNode;

                foreach (Display display in dd.Value)
                {
                    DisplayNode node = new DisplayNode(display);
                    //if (DisplayController.IsPlayerMode)
                    bool? state = DisplayController.Instance.DisplayStates[display];
                    node.TextColor = state.HasValue ? (state.Value ? Color.Green : Color.Red) : Color.Black;

                        //node.TextColor = DisplayController.Instance.DisplayStates[display] ? Color.Black : Color.Red;
                    groupNode.Nodes.Add(node);
                }

                displayTree.Nodes.Add(groupNode);
            }

            if (d1 != null && !rememberLastDisplay)
            {
                DisplayController.Instance.SelectDisplay(d1.Display);
                this.displayTree.SelectedNode = d1;
                return;
            }

            if (d2 != null && !rememberLastDisplay)
            {
                DisplayController.Instance.SelectDisplayGroup(d2.DisplayGroup);
                this.displayTree.SelectedNode = d2;
            }

            if (rememberLastDisplay && tNode != null)
            {
                if (displayTree.Nodes.Contains(tNode))
                    displayTree.SelectedNode = tNode;
            }

            this.displayTree.SelectedNodeForeColor = this.displayTree.SelectedNode.TextColor;
            this.displayTree.InactiveSelectedNodeForeColor = this.displayTree.SelectedNode.TextColor;
        }

        void Instance_OnPresentationChangedExternally()
        {
            PresentationController.Instance.SuppressLayoutChanging = true;
            this.Invoke(new MethodInvoker(() =>
            {
                LoadDisplays(true);
            }));
            PresentationController.Instance.SuppressLayoutChanging = false;
        }

        private bool findNode(TreeNodeAdvCollection collection, Display display, out TreeNodeAdv node)
        {
            foreach (TreeNodeAdv n in collection)
                if (n is DisplayNode && (n as DisplayNode).Display.Type.Name == display.Type.Name)
                {
                    node = n;
                    return true;
                }
            foreach (TreeNodeAdv n in collection)
                if (findNode(n.Nodes, display, out node))
                    return true;
            node = null;
            return false;
        }

        bool Instance_OnDisplayForceChecked(Display disp, bool isChecked)
        {
            TreeNodeAdv node = null;
            if (findNode(this.displayTree.Nodes, disp, out node))
                node.Checked = isChecked;
            return true;
        }

        void Instance_OnEnableCheckboxes(bool isEnabled)
        {
            this.displayTree.ShowCheckBoxes = isEnabled;
            this.isPlayerMode = isEnabled;
            this.toolStripEx1.Visible = !isEnabled;
            this.displayTree.RegroupDisabled = true;
        }

        void Instance_OnPresentationLockChanged(bool IsLocked)
        {
            locked = IsLocked;
            try
            {
                createDisplayGroup.Enabled = IsLocked;
                removeDisplayGroup.Enabled = IsLocked && this.displayTree.SelectedNode != null && this.displayTree.SelectedNode is DisplayGroupNode;
                TreeNodeAdv node = this.displayTree.SelectedNode;
                excludeFromGroupButton.Enabled = IsLocked && node is DisplayNode && node.Parent != null && node.Parent is DisplayGroupNode;
            }
            catch { }
        }

        void Instance_OnDisplayGroupCreated(DisplayGroup newGroup)
        {
            TreeNodeAdv node = new DisplayGroupNode(newGroup);
            displayTree.Nodes.Add(node);
            displayTree.BeginEdit(node);
        }

        private void DisplayListControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && isPlayerMode && this.displayTree.GetNodeAtPoint(e.Location) != null)
            {
                this.contextMenuStripEx1.Show(this.displayTree.PointToScreen(e.Location));
            }
        }

        private void displayTree_BeforeEdit(object sender, TreeNodeAdvBeforeEditEventArgs e)
        {
            if (e.Node is DisplayNode)
            {
                e.Cancel = true;
                return;
            }

            if (e.Node is DisplayGroupNode)
            {
                oldGroupName = e.Node.Text;
                e.TextBox.MaxLength = 100; //максимальная длина вводимого текста 
            }
        }

        private void createDisplayGroup_Click(object sender, EventArgs e)
        {
            CreateDisplayGroup();
        }

        private void removeDisplayGroup_Click(object sender, EventArgs e)
        {
            RemoveDisplayGroup();
        }

        public void CreateDisplayGroup()
        {
            DisplayController.Instance.CreateDisplayGroup();
        }


        public void ShowProperties()
        {
            if (displayTree.SelectedNode != null)
            {
                DisplayController.Instance.ShowProperties(displayTree.SelectedNode);
            }
        }

        public void RemoveDisplayGroup()
        {
            if (displayTree.SelectedNode is DisplayGroupNode)
            {
                if (MessageBoxExt.Show(String.Format("Удалить группу {0} и разгруппировать дисплеи?", displayTree.SelectedNode.Text), Properties.Resources.Confirmation, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, new string[] { "Да", "Нет" }) == DialogResult.OK)
                {
                    DisplayController.Instance.RemoveDisplayGroup((displayTree.SelectedNode as DisplayGroupNode).DisplayGroup);
                    List<DisplayNode> displays = new List<DisplayNode>();

                    //перенести дисплеи наружу
                    foreach (TreeNodeAdv node in displayTree.SelectedNode.Nodes)
                    {
                        displays.Add(node as DisplayNode);
                    }

                    displayTree.SelectedNode.Remove();
                    displays.ForEach(d =>
                    {
                        d.Remove();
                    });

                    displayTree.Nodes.AddRange(displays);
                }
            }
        }

        private void displayTree_NodeEditorValidated(object sender, TreeNodeAdvEditEventArgs e)
        {
            if (e.Node is DisplayGroupNode)
            {
                string newGroupName = e.Label.Trim();
                bool errors = false;
                if (newGroupName == String.Empty)
                {
                    MessageBoxExt.Show("Название группы не должно быть пустым!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    errors = true;
                }
                else
                {
                    if (PresentationController.Instance.Presentation.DisplayGroupList.Except(new[] { ((DisplayGroupNode)e.Node).DisplayGroup }).Any(d => d.Name == newGroupName))
                    {
                        MessageBoxExt.Show(String.Format("{0} уже существует. Введите другое название", newGroupName), "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        errors = true;
                    }
                }

                if (errors)
                {
                    string tmp = oldGroupName;
                    displayTree.EndEdit(true);
                    displayTree.BeginEdit(e.Node);
                    oldGroupName = tmp;
                }
                else
                {
                    (e.Node as DisplayGroupNode).DisplayGroup.Name = newGroupName;
                }
            }
        }

        private void displayTree_EditCancelled(object sender, TreeNodeAdvEditEventArgs e)
        {
            e.Node.Text = oldGroupName;
        }

        private void propertiesButton_Click(object sender, EventArgs e)
        {
            ShowProperties();
        }

        private void displayTree_MouseDown(object sender, MouseEventArgs e)
        {
            //nop
        }

        private void refreshDisplayButton_Click(object sender, EventArgs e)
        {
            DisplayController.Instance.CreateDisplayList();
            LoadDisplays(true);
        }

        private void findDisplayButton_Click(object sender, EventArgs e)
        {
            FindItemController.Instance.ShowSearchForm(ItemToSearch.Display);
        }

        void displayTree_AfterSelect(object sender, EventArgs e)
        {
            if (displayTree.DragDropOperationActive) return;

            TreeNodeAdv node = displayTree.SelectedNode;
            createDisplayGroup.Enabled = locked;
            removeDisplayGroup.Enabled = node != null && node is DisplayGroupNode && locked;
            excludeFromGroupButton.Enabled = locked && node is DisplayNode && node.Parent != null && node.Parent is DisplayGroupNode;

            if (node != null)
            {
                DisplayController.Instance.OnSelectedDisplayChanged -= Instance_OnSelectedDisplayChanged;
                if (node is DisplayGroupNode)
                    DisplayController.Instance.SelectDisplayGroup((node as DisplayGroupNode).DisplayGroup);
                else
                    if (node is DisplayNode && !(node.Parent != null && node.Parent is DisplayGroupNode))
                        DisplayController.Instance.SelectDisplay((node as DisplayNode).Display);
                    else
                    {
                        DisplayGroupNode n = node.Parent as DisplayGroupNode;
                        if (n != null)
                        {
                            DisplayController.Instance.SelectDisplayGroup(n.DisplayGroup);
                        }
                    }
                DisplayController.Instance.OnSelectedDisplayChanged += Instance_OnSelectedDisplayChanged;
            }
        }


        private void displayTree_AfterCheck(object sender, TreeNodeAdvEventArgs e)
        {
            if (e.Node is DisplayNode && e.Action == TreeViewAdvAction.ByMouse)
            {
                if (!DisplayController.Instance.DisplayChecked((e.Node as DisplayNode).Display, e.Node.Checked) && e.Node.Checked)
                    e.Node.Checked = false;
            }
            else if (e.Node is DisplayGroupNode && e.Action == TreeViewAdvAction.ByMouse)
            {
                if (e.Node.Checked && (e.Node as DisplayGroupNode).DisplayGroup.DisplayNameList.Count == 0)
                    e.Node.Checked = false;
            }
        }

        private void propsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayController.Instance.ShowProperties(this.displayTree.SelectedNode);
        }

        private void excludeFromGroupButton_Click(object sender, EventArgs e)
        {
            if (this.displayTree.SelectedNode is DisplayNode)
            {
                DisplayNode node = (DisplayNode)this.displayTree.SelectedNode;
                DisplayController.Instance.ExcludeFromGroup(node);
                node.Remove();
                this.displayTree.Nodes.Insert(0, node);
            }
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                if (PresentationController.Instance != null)
                {
                    PresentationController.Instance.OnPresentationLockChanged -= new PresentationLockChanged(Instance_OnPresentationLockChanged);
                    PresentationController.Instance.OnPresentationChangedExternally -= new PresentationDataChanged(Instance_OnPresentationChangedExternally);
                }
                if (DisplayController.Instance != null)
                {
                    DisplayController.Instance.OnDisplayGroupCreated -= new DisplayGroupCreated(Instance_OnDisplayGroupCreated);
                    DisplayController.Instance.OnEnableCheckboxes -= new EnableCheckboxesChanged(Instance_OnEnableCheckboxes);
                    DisplayController.Instance.OnDisplayForceChecked -= new DisplayChecked(Instance_OnDisplayForceChecked);
                }
            }
        }

        #endregion

        private void displayTree_AfterSelect_1(object sender, EventArgs e)
        {
            this.displayTree.SelectedNodeForeColor = this.displayTree.SelectedNode.TextColor;
            this.displayTree.InactiveSelectedNodeForeColor = this.displayTree.SelectedNode.TextColor;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Tools;
using System.Drawing;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using System.ComponentModel;
using System.Windows.Forms;
using Syncfusion.Drawing;

namespace UI.PresentationDesign.DesignUI.Controls.Equipment
{
    class EquipmentTreeView : TreeViewAdv
    {

        #region fields & properties
        private SuperToolTip superToolTip1;
        TreeNodeAdv toolTipped = null;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool LockBeforeReorder
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RegroupDisabled
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DragDropOperationActive
        {
            get;
            set;
        }

        #endregion

        #region ctor
        public EquipmentTreeView()
            : base()
        {
            InitializeComponent();
            superToolTip1.MaxWidth = 150;
            LockBeforeReorder = true;
            //this.KeepDottedSelection = false;
            this.AllowDrop = true;
            this.AllowMouseBasedSelection = true;
        }

        private BrushInfo _selectedGroupBack = new BrushInfo(Color.FromArgb(100, Color.LightBlue));
        private BrushInfo _groupBack = new BrushInfo();
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            ChangeBackground();
        }

        private void ChangeBackground()
        {
            if (this.SelectedNodes != null && this.SelectedNodes.Count > 0)
            {
                foreach (TreeNodeAdv item in this.Nodes)
                {
                    ChangeBackgroundSelectedGroup(item, _groupBack);
                }
            }
        }

        private void ChangeBackgroundSelectedGroup(TreeNodeAdv grp, BrushInfo brush)
        {
            if (grp != null)
            {
                grp.Background = brush;
                if (grp.Expanded)
                    foreach (TreeNodeAdv n in grp.Nodes)
                        n.Background = brush;
            }
        }

        #endregion

        #region Drag & Drop
        protected override void OnItemDrag(System.Windows.Forms.ItemDragEventArgs e)
        {
            ToolTipInfo info = new ToolTipInfo();
            info.Header.Font = new Font(info.Header.Font, FontStyle.Bold);
            info.Separator = true;
            info.Body.Image = Properties.Resources.error;
            info.Body.TextImageRelation = ToolTipTextImageRelation.TextBeforeImage;
            info.Body.Text = this.SelectedNode.Text;

            //if (!PresentationController.Instance.PresentationLocked & LockBeforeReorder)
            //{
            //    info.Header.Text = "Требуется блокировка сценария";
            //    info.Footer.Text = "Заблокируйте сценарий перед настройкой группировки.";
            //}

            //else
            //{
            TreeNodeAdv[] nodes = e.Item as TreeNodeAdv[];
            if (nodes != null)
            {
                TreeNodeAdv node = nodes[0];
                if (node is DeviceNode)
                {
                    DragDropOperationActive = true;
                    this.DoDragDrop(node, DragDropEffects.All);
                    return;
                }
                else
                    throw new InvalidOperationException("Неизвестный элемент списка оборудования");
            }
            //}

            Point p = this.PointToScreen(this.SelectedNode.Bounds.Location);
            p.Offset(-superToolTip1.MaxWidth, 0);
            superToolTip1.Show(info, p, 3000);
            toolTipped = this.SelectedNode;
        }

        bool prevMoveToGroup = false;
        TreeNodeAdv prevHoverNode = null;

        protected override void OnDragOver(DragEventArgs e)
        {
            Point clientPoint = this.PointToClient(new Point(e.X, e.Y));
            TreeNodeAdv hoveringNode = this.GetNodeAtPoint(clientPoint);

            if (hoveringNode != prevHoverNode)
            {
                this.Invalidate();
                prevHoverNode = hoveringNode;
            }

            TreeNodeAdv draggingNode = e.Data.GetData(typeof(DeviceNode)) as DeviceNode;

            if (hoveringNode != null && draggingNode != null && hoveringNode != draggingNode)
            {
                Graphics g = this.CreateGraphics();

                //temporary removed

                //if (toolTipped != hoveringNode)
                //{
                //    superToolTip1.Hide();
                //    toolTipped = null;
                //}

                e.Effect = DragDropEffects.Move;
                Color m_lineColor = Color.Red;

                this.SelectedNode = hoveringNode;


                g.DrawLine(new Pen(m_lineColor, 2), new Point(hoveringNode.Bounds.X, hoveringNode.Bounds.Y), new Point(hoveringNode.Bounds.X + this.Bounds.Width, hoveringNode.Bounds.Y));
                g.FillPolygon(new SolidBrush(m_lineColor), new Point[] { new Point(hoveringNode.Bounds.X, hoveringNode.Bounds.Y - 5), new Point(hoveringNode.Bounds.X + 5, hoveringNode.Bounds.Y), new Point(hoveringNode.Bounds.X, hoveringNode.Bounds.Y + 5) });
                g.FillPolygon(new SolidBrush(m_lineColor), new Point[] { new Point(this.Bounds.Width, hoveringNode.Bounds.Y - 5), new Point(this.Bounds.Width - 5, hoveringNode.Bounds.Y), new Point(this.Bounds.Width, hoveringNode.Bounds.Y + 5) });
            }
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNodeAdv)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                DragDropOperationActive = false;
            }
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            superToolTip1.Hide();
            toolTipped = null;
            DragDropOperationActive = false;

            if (e.Effect == DragDropEffects.Move)
            {
                TreeNodeAdv hoveringNode = this.GetNodeAtPoint(this.PointToClient(new Point(e.X, e.Y)));
                DeviceNode draggingNode = e.Data.GetData(typeof(DeviceNode)) as DeviceNode;

                bool excludeFromGroup = false;

                DeviceNode hoveredDisplay = hoveringNode as DeviceNode;

                //reorder items if parents are equal
                if (hoveredDisplay != null && draggingNode.Parent == hoveredDisplay.Parent /*&& (hoveringNode.Parent != null && (hoveringNode.Parent is DisplayGroupNode*/ || RegroupDisabled)//))
                {
                    TreeNodeAdv parent = draggingNode.Parent;
                    draggingNode.Remove();
                        Nodes.Insert(hoveringNode.Index, draggingNode);
                }

                this.SelectedNode = draggingNode;
                ChangeBackground();
                this.Invalidate();
            }

            Domain.PresentationDesign.Client.DesignerClient.Instance.ClientConfiguration.DevicePositions.Clear();
            RenumberNodesRecursive(Nodes);
            Domain.PresentationDesign.Client.DesignerClient.Instance.ClientConfiguration.SaveUserSettings();
            //PresentationController.Instance.PresentationChanged = true;
        }

        /// <summary>
        /// Перенумеровать узла.
        /// </summary>
        /// <param name="nodes">Узлы текущего уровня.</param>
        private void RenumberNodesRecursive(TreeNodeAdvCollection nodes)
        {
            if (nodes == null) return;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] is DeviceNode)
                {
                    Domain.PresentationDesign.Client.DesignerClient.Instance.ClientConfiguration.DevicePositions.Add
                        ((nodes[i] as DeviceNode).Device.Name,
                        Domain.PresentationDesign.Client.DesignerClient.Instance.ClientConfiguration.DevicePositions.Count);
                }
            }
        }
        #endregion

        private void InitializeComponent()
        {
            Syncfusion.Windows.Forms.Tools.TreeNodeAdvStyleInfo treeNodeAdvStyleInfo1 = new Syncfusion.Windows.Forms.Tools.TreeNodeAdvStyleInfo();
            this.superToolTip1 = new Syncfusion.Windows.Forms.Tools.SuperToolTip(null);
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // superToolTip1
            // 
            this.superToolTip1.UseFading = Syncfusion.Windows.Forms.Tools.SuperToolTip.FadingType.System;
            // 
            // DisplayTreeView
            // 
            treeNodeAdvStyleInfo1.EnsureDefaultOptionedChild = true;
            this.BaseStylePairs.AddRange(new Syncfusion.Windows.Forms.Tools.StyleNamePair[] {
            new Syncfusion.Windows.Forms.Tools.StyleNamePair("Standard", treeNodeAdvStyleInfo1)});
            // 
            // 
            // 
            this.HelpTextControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.HelpTextControl.Location = new System.Drawing.Point(0, 0);
            this.HelpTextControl.Name = "helpText";
            this.HelpTextControl.Size = new System.Drawing.Size(49, 15);
            this.HelpTextControl.TabIndex = 0;
            this.HelpTextControl.Text = "help text";
            // 
            // 
            // 
            this.ToolTipControl.BackColor = System.Drawing.SystemColors.Info;
            this.ToolTipControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ToolTipControl.Location = new System.Drawing.Point(0, 0);
            this.ToolTipControl.Name = "toolTip";
            this.ToolTipControl.Size = new System.Drawing.Size(41, 15);
            this.ToolTipControl.TabIndex = 1;
            this.ToolTipControl.Text = "toolTip";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

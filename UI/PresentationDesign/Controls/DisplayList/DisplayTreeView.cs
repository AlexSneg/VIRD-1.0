using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Tools;
using System.Windows.Forms;
using System.Drawing;
using UI.PresentationDesign.DesignUI.Controllers;
using Syncfusion.Windows.Forms;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using System.ComponentModel;
using Syncfusion.Drawing;

namespace UI.PresentationDesign.DesignUI.Controls.DisplayList
{
    public class DisplayTreeView : TreeViewAdv
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
        public DisplayTreeView()
            : base()
        {
            InitializeComponent();
            superToolTip1.MaxWidth = 150;
            LockBeforeReorder = true;
            //this.KeepDottedSelection = false;
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
                TreeNodeAdv node = this.SelectedNode;
                if (node == null) return;
                if (node.Parent is DisplayGroupNode) node = node.Parent;
                if (node is DisplayGroupNode)
                {
                    ChangeBackgroundSelectedGroup(node, _selectedGroupBack);
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

            if (!PresentationController.Instance.PresentationLocked & LockBeforeReorder)
            {
                info.Header.Text = "Требуется блокировка сценария";
                info.Footer.Text = "Заблокируйте сценарий перед настройкой группировки.";
            }
            else
            {
                TreeNodeAdv[] nodes = e.Item as TreeNodeAdv[];
                if (nodes != null)
                {
                    TreeNodeAdv node = nodes[0];

                    if (node is DisplayGroupNode)
                    {
                        info.Header.Text = "Перетаскивание группы запрещено";
                        info.Footer.Text = "Вы не можете перетаскивать группы.";
                    }
                    else if (node is DisplayNode)
                    {
                        DragDropOperationActive = true;
                        this.DoDragDrop(node, DragDropEffects.All);
                        return;
                    }
                    else
                        throw new InvalidOperationException("Неизвестный элемент");
                }
            }

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

            TreeNodeAdv draggingNode = e.Data.GetData(typeof(DisplayNode)) as DisplayNode;

            if (hoveringNode != null && draggingNode != null && hoveringNode != draggingNode && (!RegroupDisabled || hoveringNode is DisplayNode))
            {
                Graphics g = this.CreateGraphics();

                //temporary removed

                //if (toolTipped != hoveringNode)
                //{
                //    superToolTip1.Hide();
                //    toolTipped = null;
                //}

                e.Effect = DragDropEffects.Move;
                bool moveToGroup = false;
                Color m_lineColor = Color.Red;
                DisplayGroupNode groupNode = null;

                if (hoveringNode is DisplayGroupNode)
                {
                    groupNode = hoveringNode as DisplayGroupNode;

                    if (!hoveringNode.Expanded)
                        hoveringNode.Expand();

                    moveToGroup = true;
                }
                else
                {
                    if (hoveringNode.Parent != null && hoveringNode.Parent is DisplayGroupNode && draggingNode.Parent != hoveringNode.Parent)
                    {
                        groupNode = hoveringNode.Parent as DisplayGroupNode;
                        moveToGroup = true;
                    }
                    else
                    {
                        this.SelectedNode = hoveringNode;
                    }
                }

                if (moveToGroup)
                {
                    if (draggingNode.Parent == hoveringNode & (clientPoint.Y - hoveringNode.Bounds.Y) < hoveringNode.Bounds.Height / 2f)
                    {
                        moveToGroup = false;
                    }
                }

                if (moveToGroup != prevMoveToGroup)
                {
                    this.Invalidate();
                }

                prevMoveToGroup = moveToGroup;

                if (moveToGroup & groupNode != null)
                {
                    g.DrawLine(new Pen(m_lineColor, 2), new Point(groupNode.Bounds.X, groupNode.Bounds.Y + groupNode.Bounds.Height), new Point(groupNode.Bounds.X + this.Bounds.Width, groupNode.Bounds.Y + groupNode.Bounds.Height));
                    g.FillPolygon(new SolidBrush(m_lineColor), new Point[] { new Point(groupNode.Bounds.X, groupNode.Bounds.Y + groupNode.Bounds.Height - 5), new Point(groupNode.Bounds.X + 5, groupNode.Bounds.Y + groupNode.Bounds.Height), new Point(groupNode.Bounds.X, groupNode.Bounds.Y + groupNode.Bounds.Height + 5) });
                    g.FillPolygon(new SolidBrush(m_lineColor), new Point[] { new Point(this.Bounds.Width, groupNode.Bounds.Y + groupNode.Bounds.Height - 5), new Point(this.Bounds.Width - 5, groupNode.Bounds.Y + groupNode.Bounds.Height), new Point(this.Bounds.Width, groupNode.Bounds.Y + groupNode.Bounds.Height + 5) });
                }
                else
                {
                    g.DrawLine(new Pen(m_lineColor, 2), new Point(hoveringNode.Bounds.X, hoveringNode.Bounds.Y), new Point(hoveringNode.Bounds.X + this.Bounds.Width, hoveringNode.Bounds.Y));
                    g.FillPolygon(new SolidBrush(m_lineColor), new Point[] { new Point(hoveringNode.Bounds.X, hoveringNode.Bounds.Y - 5), new Point(hoveringNode.Bounds.X + 5, hoveringNode.Bounds.Y), new Point(hoveringNode.Bounds.X, hoveringNode.Bounds.Y + 5) });
                    g.FillPolygon(new SolidBrush(m_lineColor), new Point[] { new Point(this.Bounds.Width, hoveringNode.Bounds.Y - 5), new Point(this.Bounds.Width - 5, hoveringNode.Bounds.Y), new Point(this.Bounds.Width, hoveringNode.Bounds.Y + 5) });
                }


                if (moveToGroup & toolTipped != groupNode & groupNode != null)
                {
                    ToolTipInfo info = new ToolTipInfo();
                    info.Header.Font = new Font(info.Header.Font, FontStyle.Bold);
                    info.Separator = true;

                    if (DisplayController.Instance.CanMoveDisplayToGroup(groupNode.DisplayGroup, ((DisplayNode)draggingNode).Display))
                    {
                        info.Header.Text = "Перемещение в группу";
                        info.Body.Image = Properties.Resources.move;
                    }
                    else
                    {
                        if (draggingNode.Parent == hoveringNode)
                        {
                            info.Header.Text = "Дисплей уже находится в этой группе";
                            info.Footer.Text = "Дисплей уже добавлен в эту группу, добавление не требуется";
                        }
                        else
                        {
                            info.Header.Text = "Перемещение в эту группу невозможно";
                            info.Footer.Text = "Возможно, параметры дисплея не соответствуют параметрам группы";
                        }

                        info.Body.Image = Properties.Resources.error;
                        e.Effect = DragDropEffects.None;
                    }

                    info.Body.TextImageRelation = ToolTipTextImageRelation.TextBeforeImage;
                    info.Body.Text = groupNode.Text;

                    Point p = this.PointToScreen(groupNode.Bounds.Location);
                    p.Offset(-superToolTip1.MaxWidth, 0);
                    superToolTip1.Show(info, p, 3000);

                    toolTipped = groupNode;
                }
                else
                {
                    //nop
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
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
                //хз, но вроде помогло
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
                DisplayNode draggingNode = e.Data.GetData(typeof(DisplayNode)) as DisplayNode;

                DisplayGroupNode groupNode = hoveringNode is DisplayGroupNode ? (DisplayGroupNode)hoveringNode : hoveringNode.Parent is DisplayGroupNode ? (DisplayGroupNode)hoveringNode.Parent : null;

                if (groupNode != null && groupNode != draggingNode.Parent) //move to group ?
                {
                    if (draggingNode != null)
                    {
                        if (draggingNode.Parent != null && draggingNode.Parent is DisplayGroupNode && draggingNode.Parent != groupNode)
                        {
                            DisplayController.Instance.ExcludeFromGroup(draggingNode);
                        }

                        if (!DisplayController.Instance.AddDisplayToGroup(groupNode.DisplayGroup, draggingNode.Display))
                        {
                            //nop
                        }
                        else
                        {
                            draggingNode.Remove();
                            groupNode.Nodes.Add(draggingNode);

                            groupNode.OpenImgIndex = 1;
                            groupNode.NoChildrenImgIndex = 1;
                        }
                    }
                }
                else
                {
                    bool excludeFromGroup = false;
                    if (groupNode == null) //it's not old group ?
                    {
                        DisplayNode hoveredDisplay = hoveringNode as DisplayNode; //we know, it's not a group and parent is not a group !

                        //reorder items if parents are equal
                        if (hoveredDisplay != null && draggingNode.Parent == hoveredDisplay.Parent /*&& (hoveringNode.Parent != null && (hoveringNode.Parent is DisplayGroupNode*/ || RegroupDisabled)//))
                        {
                            TreeNodeAdv parent = draggingNode.Parent;
                            draggingNode.Remove();
                            if (parent == null || !(parent is DisplayGroupNode))
                                Nodes.Insert(hoveringNode.Index, draggingNode);
                            else
                                parent.Nodes.Insert(hoveringNode.Index, draggingNode);
                        }

                        //exclude from group if drag to unparented display
                        if (draggingNode != null && (draggingNode.Parent != null && draggingNode.Parent is DisplayGroupNode) & (hoveringNode.Parent == null || hoveringNode.Parent as DisplayGroupNode == null))
                        {
                            excludeFromGroup = true;
                        }
                    }
                    else
                    {
                        //check if we are moving out of the group?
                        Point clientPoint = this.PointToClient(new Point(e.X, e.Y));
                        if ((clientPoint.Y - groupNode.Bounds.Y) < groupNode.Bounds.Height / 2f)
                        {
                            excludeFromGroup = true;
                        }
                    }

                    if (excludeFromGroup)
                    {
                        DisplayController.Instance.ExcludeFromGroup(draggingNode);
                        draggingNode.Remove();
                        this.Nodes.Insert(hoveringNode.Index, draggingNode);
                        DisplayController.Instance.SelectDisplay(draggingNode.Display);
                    }
                }

                this.SelectedNode = draggingNode;
                ChangeBackground();
                this.Invalidate();
            }

            PresentationController.Instance.Presentation.DisplayPositionList.Clear();
            RenumberNodesRecursive(Nodes);
            PresentationController.Instance.PresentationChanged = true;
        }

        /// <summary>
        /// Перенумеровать узла.
        /// </summary>
        /// <param name="nodes">Узлы текущего уровня.</param>
        private void RenumberNodesRecursive(TreeNodeAdvCollection nodes)
        {
            if(nodes==null) return;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] is DisplayGroupNode)
                {
                    RenumberNodesRecursive(nodes[i].Nodes);
                }
                if (nodes[i] is DisplayNode)
                {
                    PresentationController.Instance.Presentation.DisplayPositionList.Add((nodes[i] as DisplayNode).Display.Type.Name, PresentationController.Instance.Presentation.DisplayPositionList.Count);
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

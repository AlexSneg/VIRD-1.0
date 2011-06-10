using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Syncfusion.Windows.Forms.Tools;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using UI.PresentationDesign.DesignUI.Controls.SourceTree;
using UI.PresentationDesign.DesignUI.Classes.View;
using Syncfusion.Windows.Forms.Diagram;
using Domain.PresentationDesign.Client;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using Syncfusion.Windows.Forms;
using UI.PresentationDesign.DesignUI.Controllers;
using UI.PresentationDesign.DesignUI.Forms;
using UI.PresentationDesign.DesignUI.Helpers;

namespace UI.PresentationDesign.DesignUI.Controls
{
    public partial class SourcesControl : UserControl
    {
        #region Fields and Properties
        PresentationInfo m_presentationInfo;
        SourcesController m_controller;
        ISourceNode localSelectedNode;
        ISourceNode globalSelectedNode;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool PresentationSourceOperationsEnabled
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool PresentationSourceAddOperationEnable
        {
            get { return presentationAddSourceToolButton.Enabled; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool GlobalSourceOperationsEnabled
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool GlobalSourceAddOperationEnable
        {
            get { return addGlobalSourceStripButton.Enabled; }
        }

        #endregion

        #region ctor
        public SourcesControl()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                presentationSourceGroupBar.OnResourceSelected += new ResourceNodeSelected(Local_OnNodeSelected);
                globalSourceGroupBar.OnResourceSelected += new ResourceNodeSelected(Global_OnNodeSelected);
                mainTabControl.SelectedIndex = 1;
                Init();
            }
        }

        #endregion

        public void SelectSource(ISourceNode node, bool isLocal)
        {
            if (isLocal)
            {
                mainTabControl.SelectedIndex = 0;
                SelectLocalSource();
            }
            else
            {
                mainTabControl.SelectedIndex = 1;
                SelectGlobalSource();
            }
            SourceGroupBar bar = isLocal ? presentationSourceGroupBar : globalSourceGroupBar;
            bar.SetSelectedItem(node);
        }

        #region event handlers
        void Instance_OnPresentationLockChanged(bool IsLocked)
        {
        }

        void Local_OnNodeSelected(ISourceNode node)
        {
            localSelectedNode = node;
            EnablePresentationSourceOperations(node != null);

            toolStripExPresentation.Invoke(new MethodInvoker(() =>
            {
                previewPresentationSourceStripButton.Enabled = node != null && node.SourceType.IsSupportPreview;
            }));
        }


        void Global_OnNodeSelected(ISourceNode node)
        {
            globalSelectedNode = node;
            SourceCategory c = SourcesController.Instance.GetSourceCategory(globalSourceGroupBar.SelectedItemText, false);
            EnableGlobalSourceOperations(node != null);

            if (node != null)
            {
                globalSourcePropertyButton.Enabled = true;
                globalSourceRemoveStripButton.Enabled = !c.IsHardware;
                copyToPresentationStripButton.Enabled = !c.IsHardware;
                GlobalSourceOperationsEnabled = !c.IsHardware;
            }

            toolStripExGlobal.Invoke(new MethodInvoker(() =>
            {
                if (c != null)
                    addGlobalSourceStripButton.Enabled = !c.IsHardware;

                globalPreviewStripButton.Enabled = node != null && node.SourceType.IsSupportPreview;
            }));
        }
        #endregion

        #region Commands
        public void AddPresentationSource()
        {
            string s = presentationSourceGroupBar.SelectedItemText;
            if (!String.IsNullOrEmpty(s))
            {
                SourcesController.Instance.CreateSourceInstance(s, false);
            }
        }

        public void AddGlobalSource()
        {
            string s = globalSourceGroupBar.SelectedItemText;
            if (!String.IsNullOrEmpty(s))
            {
                SourcesController.Instance.CreateSourceInstance(s, true);
            }
        }

        public void RemovePresentationSource()
        {
            if (localSelectedNode != null)
            {
                if (MessageBoxExt.Show(String.Format("Удалить источник {0} из хранилища?", localSelectedNode.Name), Properties.Resources.Confirmation, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, new string[] { "Да", "Нет" }) == DialogResult.OK)
                    SourcesController.Instance.RemoveResource(localSelectedNode, false);
            }
        }

        public void RemoveGlobalSource()
        {
            if (globalSelectedNode != null)
            {
                if (MessageBoxExt.Show(String.Format("Удалить источник {0} из хранилища?", globalSelectedNode.Name), Properties.Resources.Confirmation, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, new string[] { "Да", "Нет" }) == DialogResult.OK)
                    SourcesController.Instance.RemoveResource(globalSelectedNode, true);
            }
        }

        public void CopySourceToGlobal()
        {
            if (localSelectedNode != null)
            {
                string name = localSelectedNode.Mapping.ResourceInfo.Name;
                if (MessageBoxExt.Show(String.Format("Копировать источник {0} в Общие источники?\r\n(Перед копированием будет выполнено сохранение всех локальных источников!)", name), Properties.Resources.Confirmation, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, new string[] { "Да", "Нет" }) == DialogResult.OK)
                {
                    SourcesController.Instance.CopySourceToGlobal(localSelectedNode);
                }
            }
        }

        public void CopySourceToPresentation()
        {
            if (globalSelectedNode != null)
            {
                string name = globalSelectedNode.Mapping.ResourceInfo.Name;
                if (MessageBoxExt.Show(String.Format("Копировать источник {0} в источники сценария?\r\n(Перед копированием будет выполнено сохранение всех локальных источников!)", name), Properties.Resources.Confirmation, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, new string[] { "Да", "Нет" }) == DialogResult.OK)
                {
                    SourcesController.Instance.CopySourceToPresentation(globalSelectedNode);
                }
            }
        }

        public void RemoveSourceFromCategory(SourceCategory sourceCategory, ISourceNode node, bool Global)
        {
            if (Global)
                globalSourceGroupBar.RemoveNode(sourceCategory, node);
            else
                presentationSourceGroupBar.RemoveNode(sourceCategory, node);
        }

        public void AddSourceCategory(SourceCategory category, bool Global)
        {
            if (!Global)
                presentationSourceGroupBar.AddCategory(category, false);
            else
                globalSourceGroupBar.AddCategory(category, false);
        }

        public void AddResourceToCategory(SourceCategory category, ISourceNode resource, bool Global)
        {
            if (!Global)
                presentationSourceGroupBar.AddResource(category, resource, true);
            else
            {
                globalSourceGroupBar.AddResource(category, resource, true);
            }
        }

        public void RefreshSourceInfo(List<ISourceNode> resources)
        {
            resources.ForEach(presentationSourceGroupBar.RefreshSourceName);
            resources.ForEach(globalSourceGroupBar.RefreshSourceName);
        }

        public void Init()
        {
            m_presentationInfo = PresentationController.Instance.PresentationInfo;
            SourcesController.CreateSourceController(this, m_presentationInfo);
            m_controller = SourcesController.Instance;
            PresentationController.Instance.OnPresentationLockChanged += new PresentationLockChanged(Instance_OnPresentationLockChanged);
        }

        public void SelectLocalSource()
        {
            Local_OnNodeSelected(localSelectedNode);
        }

        public void SelectGlobalSource()
        {
            Global_OnNodeSelected(globalSelectedNode);
        }

        private void EnablePresentationSourceOperations(bool Enabled)
        {
            PresentationSourceOperationsEnabled = Enabled;

            if (toolStripExGlobal.IsHandleCreated)
                toolStripExGlobal.Invoke(new MethodInvoker(() =>
                {
                    copyToGlobalStripButton.Enabled = Enabled;
                    removePresentationSourceStripButton.Enabled = Enabled;
                    presentationSourceProperties.Enabled = Enabled;
                }));
        }

        private void EnableGlobalSourceOperations(bool Enabled)
        {
            GlobalSourceOperationsEnabled = Enabled;

            if (toolStripExGlobal.IsHandleCreated)
                toolStripExGlobal.Invoke(new MethodInvoker(() =>
                    {
                        copyToPresentationStripButton.Enabled = Enabled;
                        globalSourceRemoveStripButton.Enabled = Enabled;
                        globalSourcePropertyButton.Enabled = Enabled;
                    }));
        }

        internal void ShowPresentationSourceProperties()
        {
            //Показать свойства источника сценария
            using (SourcePropertiesForm sf = new SourcePropertiesForm(localSelectedNode.Mapping, false))
            {
                if (sf.ShowDialog() == DialogResult.OK && sf.Changed())
                {
                    presentationSourceGroupBar.RefreshSourceName(localSelectedNode);
                    SourcesController.Instance.SaveSource(localSelectedNode.Mapping);
                    SourcePropertiesControl.Instance.RefreshProperties();
                }
            }
        }


        internal void ShowGlobalSourceProperties()
        {
            //Показать свойства общего источника
            using (SourcePropertiesForm sf = new SourcePropertiesForm(globalSelectedNode.Mapping, false))
            {
                object[] resourceContexts = null;

                ISupportCustomSaveState customObject = globalSelectedNode.Mapping.ResourceInfo as ISupportCustomSaveState;
                if (customObject != null)
                {
                    customObject.GetState(out resourceContexts);
                }

                if (sf.ShowDialog() == DialogResult.OK && sf.Changed())
                {
                    globalSourceGroupBar.RefreshSourceName(globalSelectedNode);
                    SourcesController.Instance.SaveSource(globalSelectedNode.Mapping);
                    SourcePropertiesControl.Instance.RefreshProperties();
                    customObject = globalSelectedNode.Mapping.ResourceInfo as ISupportCustomSaveState;
                    if (customObject != null)
                    {
                        ((ISupportCustomSaveState)customObject).SetState(resourceContexts);
                    }

                }
            }

        }

        internal void UpdateSources()
        {
            //TODO: Источники - Обновить
        }

        internal void FindSource()
        {
            //TODO : Источники - Поиск
        }

        internal void FindSource(bool GlobalScope)
        {
            //TODO
            if (GlobalScope)
            {
                SourceCategory c = SourcesController.Instance.GetSourceCategory(globalSourceGroupBar.SelectedItemText,
                                                                                false);
                if (c.IsHardware)
                    FindItemController.Instance.ShowSearchForm(ItemToSearch.HardwareSource);
                else
                    FindItemController.Instance.ShowSearchForm(ItemToSearch.GlobalSource);
            }
            else
            {
                FindItemController.Instance.ShowSearchForm(ItemToSearch.LocalSource);
            }
        }

        internal void PresentationSourcePreview()
        {
            if (localSelectedNode != null)
                m_controller.PreviewSource(localSelectedNode);
            else
                MessageBoxExt.Show("Источник сценария не выбран", "Ошибка просмотра", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        internal void GlobalSourcePreview()
        {
            if (globalSelectedNode != null)
                m_controller.PreviewSource(globalSelectedNode);
            else
                MessageBoxExt.Show("Общий источник не выбран", "Ошибка просмотра", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        #endregion

        #region User action handlers
        private void presentationSourceProperties_Click(object sender, EventArgs e)
        {
            ShowPresentationSourceProperties();
        }

        private void presentationAddSourceToolButton_Click(object sender, EventArgs e)
        {
            AddPresentationSource();
        }

        private void addGlobalSourceStripButton_Click(object sender, EventArgs e)
        {
            AddGlobalSource();
        }

        private void copyToPresentationStripButton_Click(object sender, EventArgs e)
        {
            CopySourceToPresentation();
        }

        private void removePresentationSourceStripButton_Click(object sender, EventArgs e)
        {
            RemovePresentationSource();
        }

        private void globalSourceRemoveStripButton_Click(object sender, EventArgs e)
        {
            RemoveGlobalSource();
        }


        private void copyToGlobalStripButton_Click(object sender, EventArgs e)
        {
            CopySourceToGlobal();
        }

        private void presentationSourceGroupBar_GroupBarItemSelected(object sender, EventArgs e)
        {
            EnablePresentationSourceOperations(false);
        }


        private void globalSourceGroupBar_GroupBarItemSelected(object sender, EventArgs e)
        {
            EnableGlobalSourceOperations(false);
        }

        private void commonSourcePropertyButton_Click(object sender, EventArgs e)
        {
            ShowGlobalSourceProperties();
        }

        private void mainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SourcesController.Instance != null)
            {
                switch (mainTabControl.SelectedIndex)
                {
                    case 0:
                        {
                            SelectLocalSource();
                            break;
                        }

                    case 1:
                        {
                            SelectGlobalSource();
                            break;
                        }
                }
            }
        }

        internal void SelectFirstGlobalSource()
        {
            this.globalSourceGroupBar.SelectFirstItem();
        }

        #endregion

        public void OnHardwareStateChanged(ISourceNode node, bool? online)
        {
            if (node.Mapping.IsLocal)
                presentationSourceGroupBar.OnHardwareStateChanged(node, online);
            else
                globalSourceGroupBar.OnHardwareStateChanged(node, online);
        }

        private void globalPreviewStripButton_Click(object sender, EventArgs e)
        {
            if (globalSelectedNode != null)
                this.m_controller.PreviewSource(globalSelectedNode);
            else
                MessageBoxExt.Show("Источник не выбран", "Ошибка просмотра", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void previewPresentationSourceStripButton_Click(object sender, EventArgs e)
        {
            if (localSelectedNode != null)
                this.m_controller.PreviewSource(localSelectedNode);
            else
                MessageBoxExt.Show("Источник не выбран", "Ошибка просмотра", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            FindItemController.Instance.ShowSearchForm(ItemToSearch.LocalSource);
        }

        private void globalSearchButton_Click(object sender, EventArgs e)
        {
            SourceCategory c = SourcesController.Instance.GetSourceCategory(globalSourceGroupBar.SelectedItemText, false);
            if (c.IsHardware)
                FindItemController.Instance.ShowSearchForm(ItemToSearch.HardwareSource);
            else
                FindItemController.Instance.ShowSearchForm(ItemToSearch.GlobalSource);
        }
    }
}

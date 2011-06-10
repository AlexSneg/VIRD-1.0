using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Hosts.Plugins.AudioMixer.SystemModule.Config;
using Hosts.Plugins.ArcGISMap.SystemModule.Config;
using Hosts.Plugins.BusinessGraphics.SystemModule.Config;
using Hosts.Plugins.Computer.SystemModule.Config;
using Hosts.Plugins.DVDPlayer.SystemModule.Config;
using Hosts.Plugins.GangSwitch.SystemModule.Config;
using Hosts.Plugins.Image.SystemModule.Config;
using Hosts.Plugins.Jupiter.SystemModule.Config;
using Hosts.Plugins.Light.SystemModule.Config;
using Hosts.Plugins.Monitor.SystemModule.Config;
using Hosts.Plugins.PowerPointPresentation.SystemModule.Config;
using Hosts.Plugins.StandardSource.SystemModule.Config;
using Hosts.Plugins.VDCServer.SystemModule.Config;
using Hosts.Plugins.VDCTerminal.SystemModule.Config;
using Hosts.Plugins.Video.SystemModule.Config;
using Hosts.Plugins.VideoCamera.SystemModule.Config;
using Hosts.Plugins.VNC.SystemModule.Config;

using Syncfusion.Windows.Forms;

using TechnicalServices.Configuration.Configurator;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

using UI.PresentationDesign.ConfiguratorUI.Controller;
using Hosts.Plugins.WordDocument.SystemModule.Config;
using Hosts.Plugins.IEDocument.SystemModule.Config;

namespace UI.PresentationDesign.ConfiguratorUI.Forms
{
    public partial class MainForm : Office2007Form
    {
        private readonly ConfiguratorConfiguration _configuration;
        private readonly List<string> _expandedNodeList = new List<string>();
        private readonly List<Type> deviceTypeList = new List<Type>();
        private readonly List<Type> displayTypeList = new List<Type>();
        private readonly List<Type> mappingTypeList = new List<Type>();
        private readonly List<Type> sourceTypeList = new List<Type>();

        private bool _ChangeDetected;
        private int _nextId;

        private string _workingDirectory;

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(ConfiguratorConfiguration configuration)
        {
            InitializeComponent();

            _configuration = configuration;

            displayTypeList.Add(typeof (ComputerDisplayConfig));
            displayTypeList.Add(typeof (JupiterDisplayConfig));
            displayTypeList.Add(typeof (MonitorDisplayConfig));

            sourceTypeList.Add(typeof (ImageSourceConfig));
            sourceTypeList.Add(typeof (VideoSourceConfig));
            sourceTypeList.Add(typeof (VNCSourceConfig));
            sourceTypeList.Add(typeof (BusinessGraphicsSourceConfig));
            sourceTypeList.Add(typeof (DVDPlayerSourceConfig));
            sourceTypeList.Add(typeof (StandardSourceSourceConfig));
            sourceTypeList.Add(typeof (VDCTerminalSourceConfig));
            sourceTypeList.Add(typeof (VideoCameraSourceConfig));
            sourceTypeList.Add(typeof (PowerPointPresentationSourceConfig));
            /*sourceTypeList.Add(typeof (ArcGISMapSourceConfig));*/
            sourceTypeList.Add(typeof(WordDocumentSourceConfig));
            sourceTypeList.Add(typeof(IEDocumentSourceConfig));

            deviceTypeList.Add(typeof (DVDPlayerDeviceConfig));
            deviceTypeList.Add(typeof (StandardSourceDeviceConfig));
            deviceTypeList.Add(typeof (VDCTerminalDeviceConfig));
            deviceTypeList.Add(typeof (VideoCameraDeviceConfig));
            deviceTypeList.Add(typeof (JupiterDeviceConfig));
            deviceTypeList.Add(typeof (GangSwitchDeviceConfig));
            deviceTypeList.Add(typeof (LightDeviceConfig));
            deviceTypeList.Add(typeof (VDCServerDeviceConfig));
            deviceTypeList.Add(typeof (AudioMixerDeviceConfig));

            mappingTypeList.Add(typeof (JupiterMapping));

            Reload();
        }

        private bool ChangeDetected
        {
            get { return _ChangeDetected; }
            set { menuItemCancel.Enabled = menuItemSave.Enabled = _ChangeDetected = value; }
        }

        private void menuItemRemove_Click(object sender, EventArgs e)
        {
            Remove();
        }

        private void menuItemCreate_Click(object sender, EventArgs e)
        {
            Create();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuItemQuit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuItemSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void menuItemCancel_Click(object sender, EventArgs e)
        {
            Reload();
            treeView.SelectedNode = treeView.Nodes[0];
        }

        private void Save()
        {
            try
            {
                if (!string.IsNullOrEmpty(_workingDirectory))
                    Environment.CurrentDirectory = _workingDirectory;
                if (!Validate()) return;
                IEnumerable<Type> listType = GetTypeList();
                _configuration.ModuleConfiguration.Check(listType);
                _configuration.ModuleConfiguration.ApplayModelPreset();
                IEnumerable<string> modules = GetModuleList(listType);
                _configuration.Save(displayTypeList, deviceTypeList, sourceTypeList, mappingTypeList, modules);
                _ChangeDetected = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void BackRecurseTree(TreeNodeCollection nodes, Action<TreeNode> act)
        {
            foreach (TreeNode node in nodes)
            {
                BackRecurseTree(node.Nodes, act);
                act(node);
            }
        }


        private void SetModuleList(string[] list)
        {
            BackRecurseTree(treeView.Nodes, node =>
                                                {
                                                    if (node.Tag is PluginTagObject)
                                                    {
                                                        PluginTagObject pto = (PluginTagObject) node.Tag;
                                                        string assemblyName =
                                                            Path.GetFileNameWithoutExtension(pto.Type.Assembly.CodeBase);
                                                        pto.Adapter.Enabled = list.Contains(assemblyName);
                                                    }
                                                });
            BackRecurseTree(treeView.Nodes, node =>
                                                {
                                                    if (node.Tag is SoftwareSourceAggregate)
                                                    {
                                                        SoftwareSourceAggregate ssa = (SoftwareSourceAggregate) node.Tag;
                                                        string assemblyName =
                                                            Path.GetFileNameWithoutExtension(
                                                                ssa.Source.GetType().Assembly.CodeBase);
                                                        ssa.Enabled = list.Contains(assemblyName);
                                                    }
                                                });
        }

        private IEnumerable<Type> GetTypeList()
        {
            List<Type> list = new List<Type>();
            BackRecurseTree(treeView.Nodes, node =>
                                                {
                                                    if (node.Tag is PluginTagObject)
                                                    {
                                                        PluginTagObject pto = (PluginTagObject) node.Tag;
                                                        if (pto.Adapter.Enabled)
                                                        {
                                                            list.Add(pto.Type);
                                                        }
                                                    }
                                                });
            BackRecurseTree(treeView.Nodes, node =>
                                                {
                                                    if (node.Tag is SoftwareSourceAggregate)
                                                    {
                                                        SoftwareSourceAggregate ssa = (SoftwareSourceAggregate) node.Tag;
                                                        if (ssa.Enabled)
                                                            list.Add(ssa.Source.GetType());
                                                    }
                                                });
            return list.Intersect(list);
        }

        private static IEnumerable<string> GetModuleList(IEnumerable<Type> list)
        {
            List<string> modulrList = new List<string>();
            foreach (Type type in list)
                modulrList.Add(Path.GetFileNameWithoutExtension(type.Assembly.CodeBase));
            return modulrList.Intersect(modulrList);
        }

        private void LoadTree(IConfiguration config)
        {
            string[] initialPath = null != treeView.SelectedNode ? treeView.SelectedNode.GetCorrectFullPath() : null;

            //Зачистим дерево
            BackRecurseTree(treeView.Nodes, node => { if (node.Tag is PluginTagObject) node.Nodes.Clear(); });

            //Набьем его заново

            treeView.Nodes.Find("Root", false)[0].Tag = _configuration.ModuleConfiguration;

            FillNode(treeView.Nodes.Find("Display", true)[0], config.ModuleConfiguration.DisplayList, displayTypeList,
                     (e, n) =>
                         {
                             JupiterDisplayConfig jupiter = e as JupiterDisplayConfig;
                             if (null != jupiter)
                                 foreach (JupiterInOutConfig inout in jupiter.InOutConfigList)
                                     n.Nodes.Add(new TreeNode(inout.ToString()) {Tag = inout});
                         });

            //Софтверные источники - особый случай  
            List<SourceType> srcList = new List<SourceType>();
            foreach (Type type in sourceTypeList)
            {
                SourceType source = (SourceType) Activator.CreateInstance(type, "NewInstance");
                if (source.IsHardware) continue;
                source.UID = -1;
                source.Name = source.Type;
                srcList.Add(source);
            }

            FillNode(treeView.Nodes.Find("SoftwareSource", true)[0], srcList /*config.ModuleConfiguration.SourceList*/,
                     sourceTypeList,
                     (e, n) =>
                         {
                             TreeNode parent = n.Parent;
                             parent.Nodes.Clear();
                             parent.Tag = new SoftwareSourceAggregate
                                              {Source = n.Tag, Plugin = ((PluginTagObject) parent.Tag).Adapter};
                         });

            FillNode(treeView.Nodes.Find("HardwareSource", true)[0], config.ModuleConfiguration.SourceList,
                     sourceTypeList);

            FillNode(treeView.Nodes.Find("Equipment", true)[0], config.ModuleConfiguration.DeviceList, deviceTypeList);
            //treeView.ExpandAll();
            treeView.SelectedNode = treeView.FindNodeByPath(initialPath) ?? treeView.Nodes[0];

            // Подпишемся на изменения списка входов видеостены
            var JupiterConfigList = from display in config.ModuleConfiguration.DisplayList
                                    where display.GetType() == typeof (JupiterDisplayConfig)
                                    select display;
            foreach (JupiterDisplayConfig jupiterConfig in JupiterConfigList)
            {
                jupiterConfig.InOutConfigListChanged += config_InOutConfigListChanged;
            }
        }

        private void config_InOutConfigListChanged(object sender, EventArgs e)
        {
            //var s=treeView.Nodes.Find("Display",true).Select().Where(d=>d.Tag.GetType()==typeof(JupiterDisplayConfig));
            var disp = treeView.Nodes.Find("Display", true);
            foreach (TreeNode node in treeView.Nodes.Find("Display", true))
            {
                foreach (TreeNode child in node.Nodes)
                {
                    foreach (TreeNode grandchild in child.Nodes)
                    {
                        if (grandchild.Tag == sender)
                        {
                            grandchild.Nodes.Clear();
                            foreach (JupiterInOutConfig inOutConfig in ((JupiterDisplayConfig) sender).InOutConfigList)
                            {
                                TreeNode newNode = new TreeNode(inOutConfig.ToString());
                                newNode.Tag = inOutConfig;
                                grandchild.Nodes.Add(newNode);
                            }
                        }
                    }
                }
            }
        }

        private static void FillNode<T>(TreeNode localRoot, IEnumerable<T> list, IEnumerable<Type> types)
            where T : EquipmentType
        {
            FillNode(localRoot, list, types, (e, n) => { });
        }

        private static void FillNode<T>(TreeNode localRoot, IEnumerable<T> list, IEnumerable<Type> types,
                                        Action<T, TreeNode> act) where T : EquipmentType
        {
            foreach (Type type in types)
            {
                TreeNode[] nodeList = localRoot.Nodes.Find(type.Name, false);
                if (nodeList == null || nodeList.Length == 0) continue;
                TreeNode parent = nodeList[0];
                parent.Tag = new PluginTagObject(type, parent);
                foreach (T equipment in list)
                    //if (type.IsInstanceOfType(equipment))
                    if (type.FullName == equipment.GetType().FullName)
                    {
                        TreeNode node;
                        parent.Nodes.Add(node = new TreeNode(equipment.Name) {Name = equipment.Name, Tag = equipment});
                        act(equipment, node);
                    }
            }
        }

        private static bool IsInstance(object obj)
        {
            return (obj is EquipmentType || obj is JupiterInOutConfig) &&
                   !(obj is SourceType && !(obj is HardwareSourceType));
        }

        private void SetEnableControls(object value)
        {
            PluginTagObject plugin = value as PluginTagObject;
            menuItemCreate.Enabled = value is JupiterDisplayConfig ||
                                     (null != plugin && plugin.Adapter.Enabled);
            menuItemRemove.Enabled = IsInstance(value);
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //EquipmentType equipment = e.Node.Tag as EquipmentType;
            PluginTagObject plugin = e.Node.Tag as PluginTagObject;
            SoftwareSourceAggregate aggregate = e.Node.Tag as SoftwareSourceAggregate;

            pgParams.SelectedObject = (null != plugin) ? plugin.Adapter : e.Node.Tag;

            SetEnableControls(e.Node.Tag);


            bool showRelations = e.Node.Tag is PassiveDisplayType || e.Node.Tag is HardwareSourceType ||
                                 e.Node.Tag is JupiterInOutConfig,
                 showParams = e.Node.Tag is EquipmentType || e.Node.Tag is JupiterInOutConfig ||
                              e.Node.Tag is ModuleConfiguration || null != plugin || null != aggregate,
                 hideParamsTab = e.Node.Tag is ModuleConfiguration;

            if (showRelations)
            {
                gdRelations.Columns.Clear();
                PassiveDisplayType passiveDisp = e.Node.Tag as PassiveDisplayType;
                JupiterInOutConfig jupiterInOut = e.Node.Tag as JupiterInOutConfig;
                JupiterDisplayConfig jupiter = e.Node.Parent.Tag as JupiterDisplayConfig;
                HardwareSourceType hardwareSource = e.Node.Tag as HardwareSourceType;
                if (null != passiveDisp || null != jupiterInOut)
                {
                    IEnumerable<CompatibilityDispToSourcePair> pairs = (null != jupiterInOut)
                                                                           ?
                                                                               //Вариант запроса для входа видеостены
                                                                       from hSource in
                                                                           _configuration.ModuleConfiguration.SourceList
                                                                       where hSource is HardwareSourceType
                                                                       select new CompatibilityDispToSourcePair(
                                                                           jupiter,
                                                                           hSource,
                                                                           (m, s) =>
                                                                           m.Source.UID == s.UID && m is JupiterMapping &&
                                                                           ((JupiterMapping) m).VideoIn ==
                                                                           jupiterInOut.VideoIn,
                                                                           () =>
                                                                           new JupiterMapping
                                                                               {
                                                                                   Source = hSource,
                                                                                   VideoIn = jupiterInOut.VideoIn
                                                                               })
                                                                           :
                                                                               //Вариант запроса для пассивного дисплея
                                                                       from hSource in
                                                                           _configuration.ModuleConfiguration.SourceList
                                                                       where hSource is HardwareSourceType
                                                                       select new CompatibilityDispToSourcePair(
                                                                           passiveDisp,
                                                                           hSource,
                                                                           (m, s) => m.Source.UID == s.UID,
                                                                           () => new Mapping {Source = hSource});
                    gdRelations.DataSource = pairs.ToArray();
                }
                else
                    gdRelations.DataSource = ((
                                                  from pDisplay in _configuration.ModuleConfiguration.DisplayList
                                                  where pDisplay is PassiveDisplayType
                                                  select new CompatibilitySourceToDispPair(
                                                      pDisplay,
                                                      hardwareSource,
                                                      (m, s) => m.Source == s,
                                                      () => new Mapping {Source = hardwareSource})
                                              ).Concat(
                        from jupiterConfig in _configuration.ModuleConfiguration.DisplayList
                        where jupiterConfig is JupiterDisplayConfig
                        from jupiterInOutConfig in ((JupiterDisplayConfig) jupiterConfig).InOutConfigList
                        select new CompatibilitySourceToDispPair(
                            jupiterConfig,
                            jupiterInOutConfig,
                            hardwareSource,
                            (m, s) =>
                            m.Source == s && m is JupiterMapping &&
                            ((JupiterMapping) m).VideoIn == jupiterInOutConfig.VideoIn,
                            () => new JupiterMapping {Source = hardwareSource, VideoIn = jupiterInOutConfig.VideoIn})
                        )).ToArray();

                foreach (DataGridViewColumn column in gdRelations.Columns)
                    if (column.DataPropertyName == "Compatible")
                        column.DisplayIndex = 0;
                    else
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (showParams)
                tabPageAdv1.Show();
            else
                tabPageAdv1.Hide();

            if (showRelations)
                tabPageAdv2.Show();
            else
                tabPageAdv2.Hide();

            if (hideParamsTab) tabPageAdv1.Hide();

            if (showRelations)
                gdRelations.Show();
            else
                gdRelations.Hide();

            if (showParams)
                pgParams.Show();
            else
                pgParams.Hide();
        }

        private void pgParams_Click(object sender, EventArgs e)
        {
        }

        private void pgParams_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            ChangeDetected = true;
            if (treeView.SelectedNode.Tag is EquipmentType)
                treeView.SelectedNode.Text = (treeView.SelectedNode.Tag as EquipmentType).Name;
            else if (treeView.SelectedNode.Tag is JupiterInOutConfig)
                treeView.SelectedNode.Text = (treeView.SelectedNode.Tag as JupiterInOutConfig).ToString();
            if (treeView.SelectedNode.Tag is SoftwareSourceAggregate)
            {
                SoftwareSourceAggregate ssa = (SoftwareSourceAggregate) treeView.SelectedNode.Tag;
                SourceType source = (SourceType) ssa.Source;
                if (!source.IsHardware)
                {
                    if (!ssa.Enabled)
                    {
                        source =
                            _configuration.ModuleConfiguration.SourceList.First(
                                delegate(SourceType src) { return source.Equals(src); });
                        foreach (DisplayType displayType in _configuration.ModuleConfiguration.DisplayList)
                        {
                            for (int i = displayType.MappingList.Count - 1; i >= 0; i--)
                            {
                                Mapping mapping = displayType.MappingList[i];
                                if (mapping.Source.UID == source.UID)
                                    displayType.MappingList.RemoveAt(i);
                            }
                        }
                        _configuration.ModuleConfiguration.SourceList.Remove(source);
                    }
                    else _configuration.ModuleConfiguration.SourceList.Add(source);
                }
            }

            if (treeView.SelectedNode.Tag is PluginTagObject)
                SetEnableControls(treeView.SelectedNode.Tag);
            if (treeView.SelectedNode.Tag is SoftwareSourceAggregate)
                SetEnableControls(treeView.SelectedNode.Tag);
        }

        private void Reload()
        {
            treeView.BeginUpdate();

            SaveTreeNodeState();
            string[] list = _configuration.Load(displayTypeList, deviceTypeList, sourceTypeList, mappingTypeList);
            LoadTree(_configuration);
            SetModuleList(list);
            LoadTreeNodeState(treeView.Nodes);

            treeView.EndUpdate();

            ChangeDetected = false;
            _nextId = 0;
            if (_configuration.ModuleConfiguration.DeviceList.Count > 0)
            {
                int n = _configuration.ModuleConfiguration.DeviceList.Max(dev => dev.UID);
                _nextId = (n > _nextId) ? n : _nextId;
            }
            if (_configuration.ModuleConfiguration.DisplayList.Count > 0)
            {
                int n = _configuration.ModuleConfiguration.DisplayList.Max(dis => dis.UID);
                _nextId = (n > _nextId) ? n : _nextId;
            }
            if (_configuration.ModuleConfiguration.SourceList.Count > 0)
            {
                int n = _configuration.ModuleConfiguration.SourceList.Max(src => src.UID);
                _nextId = (n > _nextId) ? n : _nextId;
            }
            _nextId++;
        }

        private void LoadTreeNodeState(TreeNodeCollection list)
        {
            foreach (TreeNode node in list)
            {
                if (node.GetNodeCount(true) > 0) LoadTreeNodeState(node.Nodes);
                if (_expandedNodeList.Contains(node.Name))
                    node.Expand();
                //else
                //    node.Collapse();
            }
        }

        private void SaveTreeNodeState()
        {
            _expandedNodeList.Clear();
            saveTreeNodeStateRecurcive(treeView.Nodes);
        }

        private void saveTreeNodeStateRecurcive(TreeNodeCollection list)
        {
            foreach (TreeNode node in list)
            {
                if (node.GetNodeCount(true) > 0) saveTreeNodeStateRecurcive(node.Nodes);
                if (node.IsExpanded) _expandedNodeList.Add(node.Name);
            }
        }

        private void Remove()
        {
            EquipmentType equipment = treeView.SelectedNode.Tag as EquipmentType;
            JupiterInOutConfig inoutConfig = treeView.SelectedNode.Tag as JupiterInOutConfig;
            if (null == equipment && null == inoutConfig) return;
            string name = null != equipment ? equipment.Name : inoutConfig.ToString();
            if (DialogResult.No ==
                MessageBox.Show(string.Format("Подтвердите удаление {0}", name), "Подтверждение",
                                MessageBoxButtons.YesNo)) return;
            DeviceType device = equipment as DeviceType;
            if (null != device)
                _configuration.ModuleConfiguration.DeviceList.Remove(device);
            DisplayType display = equipment as DisplayType;
            if (null != display)
                _configuration.ModuleConfiguration.DisplayList.Remove(display);
            SourceType source = equipment as SourceType;
            if (null != source)
            {
                if (source.IsHardware)
                {
                    if (source.DeviceType != null)
                        _configuration.ModuleConfiguration.DeviceList.Remove(source.DeviceType);
                    foreach (DisplayType displayType in _configuration.ModuleConfiguration.DisplayList)
                    {
                        for (int i = displayType.MappingList.Count - 1; i >= 0; i--)
                        {
                            Mapping mapping = displayType.MappingList[i];
                            if (mapping.Source.UID == source.UID)
                                displayType.MappingList.RemoveAt(i);
                        }
                    }
                }
                _configuration.ModuleConfiguration.SourceList.Remove(source);
            }
            if (null != inoutConfig)
                ((JupiterDisplayConfig) treeView.SelectedNode.Parent.Tag).InOutConfigList.Remove(inoutConfig);
            treeView.SelectedNode.Nodes.Remove(treeView.SelectedNode);
            ChangeDetected = true;
        }

        private void pgParams_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            //if (e.OldSelection is ISupportValidation)
            //{
            //    string message;
            //    bool correct = ((ISupportValidation)e.OldSelection).EnsureValidate(out message);
            //    if (!correct)
            //    {
            //        MessageBox.Show(this,
            //                        message,
            //                        "Ошибка",
            //                        MessageBoxButtons.OK,
            //                        MessageBoxIcon.Warning);

            //    }
            //}
        }

        private void Create()
        {
            object tag = treeView.SelectedNode.Tag;
            if (null == tag) throw new Exception("Wrong tag value");
            PluginTagObject plugin = tag as PluginTagObject;
            TreeNode targetNode = treeView.SelectedNode;
            if (plugin == null && tag.GetType() == typeof (JupiterDisplayConfig))
            {
                if (((JupiterDisplayConfig) tag).InOutConfigList.Count == 100)
                {
                    MessageBox.Show(this, "Нельзя для видеостены задать более 100 входов", "Ошибка",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Вход видеостены добавляется отдельным образом
                JupiterInOutConfig newItem = new JupiterInOutConfig();
                newItem.VideoIn = Convert.ToInt16(targetNode.Nodes.Count + 1);
                ((JupiterDisplayConfig) tag).InOutConfigList.Add(newItem);
                targetNode.Nodes.Add(new TreeNode(newItem.ToString()) {Tag = newItem});
            }
            else if (plugin != null)
            {
                Type creatingType = plugin.Type;
                EquipmentType equipment = (EquipmentType) Activator.CreateInstance(creatingType, "New Instance");
                if (equipment.UID == 0) equipment.UID = _nextId++;
                equipment.Name = GetNewItemName(equipment, targetNode.Nodes);
                DeviceType device = equipment as DeviceType;
                if (null != device)
                    _configuration.ModuleConfiguration.DeviceList.Add(device);
                DisplayType display = equipment as DisplayType;
                if (null != display)
                    _configuration.ModuleConfiguration.DisplayList.Add(display);
                SourceType source = equipment as SourceType;
                if (null != source)
                {
                    if (source.IsHardware)
                    {
                        source.DeviceType = source.CreateDeviceType();
                        source.DeviceType.UID = source.UID;
                        _configuration.ModuleConfiguration.DeviceList.Add(source.DeviceType);
                    }
                    _configuration.ModuleConfiguration.SourceList.Add(source);
                }
                targetNode.Nodes.Add(new TreeNode(equipment.Name) {Tag = equipment});
            }
            _ChangeDetected = true;
        }

        private static string GetNewItemName(EquipmentType eq, IEnumerable nodes)
        {
            string prefix = eq.Type + '_';
            int tmp;
            IEnumerable<int> ids = from TreeNode node in nodes
                                   where
                                       (node.Tag is EquipmentType && ((EquipmentType) node.Tag).Name.StartsWith(prefix) &&
                                        Int32.TryParse(((EquipmentType) node.Tag).Name.Substring(prefix.Length), out tmp))
                                   select Int32.Parse(((EquipmentType) node.Tag).Name.Substring(prefix.Length));
            int maxId = ids.Count() > 0 ? ids.Max() + 1 : 1;
            return prefix + maxId;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ChangeDetected)
                switch (MessageBox.Show("Сохранить изменения?", "Подтверждение", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        Save();
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
        }

        private void SaveToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            SaveStripButton.Enabled = saveConfigToolStripMenuItem.Enabled;
        }

        private void CancelToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            CancelStripButton.Enabled = cancelConfigToolStripMenuItem.Enabled;
        }

        private void CreateToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            AddStripButton.Enabled = menuItemCreate.Enabled;
        }

        private void RemoveToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            RemoveStripButton.Enabled = menuItemRemove.Enabled;
        }

        private void pgParams_Validating(object sender, CancelEventArgs e)
        {
            object selectedObject = ((PropertyGrid) sender).SelectedObject;
            // Проверка, что нет двух аппаратных источников c одинаковым входом.
            /*https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-1630
            if (selectedObject is HardwareSourceType)
            {
                HardwareSourceType config = (HardwareSourceType)selectedObject;
                if (config.Input != 0)
                {
                    IEnumerable<SourceType> t = from source in _configuration.ModuleConfiguration.SourceList
                                                where source.GetType() == config.GetType()
                                                      && ((HardwareSourceType)source).Input == config.Input
                                                select source;
                    int res = t.Count();
                    if (res > 1)
                    {
                        MessageBox.Show(this,
                                        string.Format(
                                            "В списке уже имеется устройство со входом коммутатора равным {0}.",
                                            config.Input),
                                        "Ошибка",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        e.Cancel = true;
                        return;
                    }
                }
            }*/

            // Проверка, что нет двух дисплеев на одном выходе коммутатора.
            if (selectedObject is PassiveDisplayType)
            {
                PassiveDisplayType config = (PassiveDisplayType) selectedObject;
                if (config.Output != 0)
                {
                    IEnumerable<DisplayType> t = from source in _configuration.ModuleConfiguration.DisplayList
                                                 where source.GetType() == config.GetType()
                                                       && ((PassiveDisplayType) source).Output == config.Output
                                                 select source;
                    int res = t.Count();
                    if (res > 1)
                    {
                        MessageBox.Show(this,
                                        string.Format("В списке уже имеется дисплей на выходе коммутатора №{0}.",
                                                      config.Output),
                                        "Ошибка",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        e.Cancel = true;
                        return;
                    }
                }
            }
            if (selectedObject is ISupportValidation)
            {
                // Валидация для редактируемого значения
                string message;
                bool correct = ((ISupportValidation) selectedObject).EnsureValidate(out message);
                if (!correct)
                {
                    MessageBox.Show(this,
                                    string.Format(CultureInfo.InvariantCulture,
                                                  "{0}{1}{1}{2}",
                                                  message,
                                                  Environment.NewLine,
                                                  String.Empty),
                                    "Ошибка",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }
            }
            if (treeView.SelectedNode.Parent != null && treeView.SelectedNode.Parent.Tag is ISupportValidation)
            {
                // Валидация для родительского элемента
                string message;
                bool correct = ((ISupportValidation) treeView.SelectedNode.Parent.Tag).EnsureValidate(out message);
                if (!correct)
                {
                    MessageBox.Show(this,
                                    string.Format(CultureInfo.InvariantCulture,
                                                  "{0}{1}{1}{2}",
                                                  message,
                                                  Environment.NewLine,
                                                  String.Empty),
                                    "Ошибка",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }
            }

            if (treeView.SelectedNode.Parent != null)
            {
                for (int i = 0; i < treeView.SelectedNode.Parent.Nodes.Count; i++)
                {
                    if (treeView.SelectedNode.Parent.Nodes[i].Index != treeView.SelectedNode.Index
                        && treeView.SelectedNode.Parent.Nodes[i].Text == treeView.SelectedNode.Text)
                    {
                        MessageBox.Show(this,
                                        string.Format("В этой ветке уже есть элемент с таким названием."),
                                        "Ошибка",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            _workingDirectory = Environment.CurrentDirectory;
        }
    }
}
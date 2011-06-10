using System;
using System.ComponentModel.Design;
using System.Windows.Forms;
using Hosts.Plugins.Jupiter.SystemModule.Design;
using System.Reflection;

namespace Hosts.Plugins.Jupiter.SystemModule.Config
{
    public class JupiterInOutConfigCollectionEditor : CollectionEditor
    {
        private PropertyGrid propertyGrid;
        private ListBox listBox;
        private object deleted;

        public JupiterInOutConfigCollectionEditor(Type type) : base(type)
        {
        }

        ListBox listbox;

        protected override CollectionForm CreateCollectionForm()
        {
            CollectionForm collectionForm = base.CreateCollectionForm();

            if (collectionForm.Controls.Find("propertyBrowser", true).Length > 0)
            {
                propertyGrid = (PropertyGrid) collectionForm.Controls.Find("propertyBrowser", true)[0];
                propertyGrid.PropertyValueChanged += propertyGrid_PropertyValueChanged;
            }

            if (collectionForm.Controls.Find("listbox", true).Length > 0)
                listBox = (ListBox) collectionForm.Controls.Find("listbox", true)[0];

            if (collectionForm.Controls.Find("removeButton", true).Length > 0)
            {
                Button removeButton = (Button) collectionForm.Controls.Find("removeButton", true)[0];
                removeButton.Click += removeButton_Click;
            }
            if (collectionForm.Controls.Find("upButton", true).Length > 0)
            {
                Button button = (Button)collectionForm.Controls.Find("upButton", true)[0];
                button.Click += new EventHandler(button_Click);
            }

            if (collectionForm.Controls.Find("downButton", true).Length > 0)
            {
                Button button = (Button)collectionForm.Controls.Find("downButton", true)[0];
                button.Click += new EventHandler(button_Click);
            }

            if (collectionForm.Controls.Find("listbox", true).Length > 0)
            {
                listbox = (ListBox)(collectionForm.Controls.Find("listbox", true)[0]);
            }

            deleted = null;
            return collectionForm;
        }

        void button_Click(object sender, EventArgs e)
        {
            JupiterDisplayConfig config = (JupiterDisplayConfig)Context.Instance;
            for (int i = 0; i < listbox.Items.Count; i++)
            {
                FieldInfo val = listbox.Items[i].GetType().GetField("value", BindingFlags.Instance | BindingFlags.NonPublic);
                config.InOutConfigList[i]=(JupiterInOutConfig) val.GetValue(listbox.Items[i]);
            }
            Renumber();
        }

        protected override object CreateInstance(Type type)
        {
            JupiterInOutConfig result = new JupiterInOutConfig();
            if (Context != null)
            {
                JupiterDisplayConfig config = (JupiterDisplayConfig)Context.Instance;
                config.VerifyInOutConfig(result);
            }
            return result;
        }

        protected override void DestroyInstance(object instance)
        {
        }

        protected override bool CanRemoveInstance(object value)
        {
            if (propertyGrid.SelectedObject == null && value != null) deleted = propertyGrid.SelectedObject;
            if (deleted == null || deleted != propertyGrid.SelectedObject) deleted = propertyGrid.SelectedObject;
            return true;
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if ((deleted != null) && (Context != null))
            {
                JupiterDisplayConfig config = (JupiterDisplayConfig)Context.Instance;
                config.InOutConfigList.Remove((JupiterInOutConfig)deleted);
                Renumber();
                deleted = null;
            }
        }

        /// <summary>
        /// Перенумеровать входы в списке.
        /// </summary>
        private void Renumber()
        {
            JupiterDisplayConfig config = (JupiterDisplayConfig)Context.Instance;
            for (short i = 0; i < config.InOutConfigList.Count; i++)
            {
                if (config.InOutConfigList[i].VideoIn != i + 1)
                {
                    config.InOutConfigList[i].VideoIn = (short)(i + 1);
                }
            }
        }

        private void propertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if ((Context == null) ||
                (e.ChangedItem.PropertyDescriptor.Name != "VideoIn") ||
                (listBox == null) || (listBox.SelectedIndex < 0))
                return;

            int index = listBox.SelectedIndex;
            JupiterDisplayConfig config = (JupiterDisplayConfig)((JupiterDisplayDesign)Context.Instance).Type;
            JupiterInOutConfig item = config.InOutConfigList[index];
            config.InOutConfigList.RemoveAt(index);
            config.VerifyInOutConfig(item);
            config.InOutConfigList.Insert(index, item);
            listBox.Refresh();
            propertyGrid.Refresh();
        }

        protected override object SetItems(object editValue, object[] value)
        {
            object obj=base.SetItems(editValue, value);
            Renumber();
            ((JupiterDisplayConfig)Context.Instance).FireInOutConfigListChanged();
            return obj;
        }
    }
}
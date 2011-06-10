using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Util;
using TechnicalServices.Interfaces;
using Syncfusion.Windows.Forms.Tools;
using UI.PresentationDesign.DesignUI.Properties;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using UI.PresentationDesign.DesignUI.Helpers;

namespace UI.PresentationDesign.DesignUI.Forms
{
    public partial class SourcePropertiesForm : PropertyDialog
    {
        object source = null;
        object destInfo = null;
        bool _changed = false;
        bool _isNewSource = false;

        object[] resourceContexts = null;

        public SourcePropertiesForm(object descr, bool isNewSource)
        {
            InitializeComponent();
            source = descr;
            _isNewSource = isNewSource;
            this.Text = descr is ResourceDescriptor ? (descr as ResourceDescriptor).ResourceInfo.Name : (descr as Source).ResourceDescriptor.ResourceInfo.Name;
            this.Text += " - Свойства";
            if (descr is ResourceDescriptor)
                CloneResourceInfo((descr as ResourceDescriptor).ResourceInfo, out destInfo);
            else
                destInfo = descr;
            propertyGrid.AssignedObject = destInfo;
            propertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(propertyGrid_PropertyValueChanged);
            propertyGrid.IsEnabled = true;

            // DSidorov - у аппаратных источников м.б. редактируемые свойства
            // Например, список абонентов в терминале ВКС
            //if (descr is ResourceDescriptor && (descr as ResourceDescriptor).ResourceInfo.IsHardware)
            //{
            //    okButton.Visible = false;
            //    cancelButton.Text = "OK";
            //}
        }

        public SourcePropertiesForm(object descr, bool ReadOnly, bool isNewSource)
            : this(descr, isNewSource)
        {
            propertyGrid.IsEnabled = !ReadOnly;
            propertyGrid.IsReadOnly = ReadOnly;
            okButton.Enabled = !ReadOnly;
        }


        void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            _changed = true;
        }

        private void CloneResourceInfo(object resourceInfo, out object result)
        {
            //ISupportCustomSaveState customObject = resourceInfo as ISupportCustomSaveState;
            //if (customObject!=null)
            //{
            //    customObject.GetState(out resourceContexts);
            //}
            
            MemoryStream ms = new MemoryStream();
            DataContractSerializer ds = new DataContractSerializer(resourceInfo.GetType(), KnownTypeProvider.GetAllKnownTypes(null));
            ds.WriteObject(ms, resourceInfo);
            ms.Flush();
            byte[] bytes = ms.ToArray();
            ms.Dispose();
            ms = new MemoryStream(bytes);
            result = ds.ReadObject(ms);
        }


        private void okButton_Click(object sender, EventArgs e)
        {
            this.CanClose = AcceptChanges();
            if (CanClose)
                CloseMe();
        }

        bool NameEquals(object target)
        {
            if (!PrevNameEquals(target))
                return (target as ResourceDescriptor).ResourceInfo.NameEquals(destInfo as ResourceInfo);
            return false;
        }

        bool PrevNameEquals(object target)
        {
            if (target is ResourceDescriptor)
                return (target as ResourceDescriptor).ResourceInfo.NameEquals((source as ResourceDescriptor).ResourceInfo);
            else if (target is Source)
                return (target as Source).ResourceDescriptor.ResourceInfo.NameEquals((source as Source).ResourceDescriptor.ResourceInfo);
            return false;
        }


        public override bool Changed()
        {
            return _changed;
        }

        public override bool AcceptChanges()
        {
            if (!(source is ResourceDescriptor))
                return true;

            ToolTipInfo t_info = new ToolTipInfo();
            t_info.Body.Image = Resources.error;
            t_info.Header.Text = "Ошибка";

            Point p = this.PointToScreen(propertyGrid.Location);
            p.Offset(propertyGrid.Width, 0);

            var res = SourcesController.Instance.GetResources((source as ResourceDescriptor).IsLocal);
            if (res.Count() > 0)
                if (res.Any(NameEquals))
                {
                    t_info.Body.Text = String.Format("Источник с названием {0} уже есть в хранилище. Укажите другое имя", (destInfo as ResourceInfo).Name);
                    superToolTip1.Show(t_info, p);
                    return false;
                }

            ISupportValidation validation = destInfo as ISupportValidation;
            string message;
            if (validation != null)
            {
                if (!validation.EnsureValidate(out message))
                {
                    t_info.Body.Text = message;
                    superToolTip1.Show(t_info, p);
                    return false;
                }
                else if (!(string.IsNullOrEmpty(message) || message.Equals("OK")) && (!_isNewSource))
                {
                    //валидация прошла но вернулось какое то предупреждающее сообщение, 
                    //надо дать выбор пользователю, продолжить или нет
                    DialogResult dRes = MessageBoxExt.Show(message, "Предупреждение", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (dRes == DialogResult.Cancel)
                        return false;
                }
            }

            (source as ResourceDescriptor).ResourceInfo = destInfo as ResourceInfo;
            //ISupportCustomSaveState customObject = destInfo as ISupportCustomSaveState;
            //if (customObject!=null)
            //{
            //    ((ISupportCustomSaveState)customObject).SetState(resourceContexts);
            //}
            
            DialogResult = DialogResult.OK;

            CloseMe();
            return true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            CancelChanges();
        }

        public override void CancelChanges()
        {
            DialogResult = DialogResult.Cancel;
            CloseMe();
        }
    }
}

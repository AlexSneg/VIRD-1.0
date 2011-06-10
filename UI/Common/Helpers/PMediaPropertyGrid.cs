using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections;
using TechnicalServices.Common.ReadOnly;
using System.Globalization;
using System.Windows.Forms.Design;

namespace UI.PresentationDesign.DesignUI.Helpers
{

    public class PMediaPropertyGrid : PropertyGrid
    {
        bool _readonly = true;

        public event CancelEventHandler PropertyGridValidating;

        public bool IsEnabled
        {
            get
            {
                return !IsReadOnly;
            }
            set
            {
                IsReadOnly = !value;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return _readonly;
            }
            set
            {
                _readonly = value;
                WrapObject();
            }
        }

        Object obj;

        public object AssignedObject
        {
            get
            {
                return obj;
            }

            set
            {
                obj = value;
                WrapObject();

                RefreshProviders();
            }
        }

        private void RefreshProviders()
        {
            foreach (Control c in this.Controls)
            {
                if (string.Compare(c.GetType().FullName, "System.Windows.Forms.PropertyGridInternal.PropertyGridView", true, CultureInfo.InvariantCulture) == 0)
                {
                    FieldInfo errorDialogField = c.GetType().GetField("serviceProvider", BindingFlags.Instance | BindingFlags.NonPublic);
                    errorDialogField.SetValue(c, new MyServiceProvider(this));
                }
            }
        }

        public PMediaPropertyGrid()
            : base()
        {
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            if (PropertyGridValidating != null)
            {
                try { PropertyGridValidating(this, e); }
                catch { }
            }
        }

        private void WrapObject()
        {
            if (_readonly && obj != null)
                this.SelectedObject = new ReadOnlyObject(obj);
            else
                this.SelectedObject = obj;
        }

        protected override object GetService(Type service)
        {
            return base.GetService(service);
        }
    }

    #region custom error handling

    internal class MyServiceProvider : IServiceProvider
    {
        private MyUIService service;

        internal MyServiceProvider(PropertyGrid grid)
        {
            this.service = new MyUIService(grid);
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IUIService))
            {
                return this.service;
            }

            return null;
        }
    }

    internal class MyUIService : IUIService
    {
        private PropertyGrid propertyGrid;
        internal MyUIService(PropertyGrid grid)
        {
            this.propertyGrid = grid;

        }

        public bool CanShowComponentEditor(object component)
        {
            return false;
        }

        public IWin32Window GetDialogOwnerWindow()
        {
            return this.propertyGrid;
        }

        public void SetUIDirty()
        {
            //throw new NotImplementedException("The method SetUIDirty or operation is not implemented.");
        }

        public bool ShowComponentEditor(object component, IWin32Window parent)
        {
            //throw new NotImplementedException("The method ShowComponentEditor or operation is not implemented.");
            return false;
        }

        public DialogResult ShowDialog(Form form)
        {
            if (form != null)
            {
                if (string.Compare(form.GetType().FullName, "System.Windows.Forms.PropertyGridInternal.GridErrorDlg", true, CultureInfo.InvariantCulture) == 0)
                {
                    PropertyDescriptor property = this.propertyGrid.SelectedGridItem.PropertyDescriptor;

                    FieldInfo detailsTextBoxField = form.GetType().GetField("details", BindingFlags.NonPublic | BindingFlags.Instance);
                    TextBox detailsTextBox = detailsTextBoxField.GetValue(form) as TextBox;

                    return MessageBoxExt.Show(this.propertyGrid,
                        string.Format(CultureInfo.InvariantCulture,
                                      "{0}{1}{1}{2}",
                                      detailsTextBox.Text,
                                      Environment.NewLine,
                                      String.Empty),
                        property.DisplayName,
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning, new string[] { });
                }
                else
                {
                    return form.ShowDialog(this.propertyGrid);
                }
            }

            return DialogResult.Cancel;
        }

        public void ShowError(Exception ex, string message)
        {
            MessageBox.Show(string.Format("Возникла ошибка {2}, '{0}'\n{1}", ex.Message, ex.StackTrace, message), "Ошибка");
        }

        public void ShowError(Exception ex)
        {
            MessageBox.Show(string.Format("Возникла ошибка '{0}'\n{1}",ex.Message, ex.StackTrace), "Ошибка");
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка");
        }

        public DialogResult ShowMessage(string message, string caption, MessageBoxButtons buttons)
        {
            //throw new NotImplementedException();
            return DialogResult.Cancel;
        }

        public void ShowMessage(string message, string caption)
        {
            //throw new NotImplementedException();
        }

        public void ShowMessage(string message)
        {
            //throw new NotImplementedException();
        }

        public bool ShowToolWindow(Guid toolWindow)
        {
            //throw new NotImplementedException();
            return false;
        }

        public IDictionary Styles
        {
            get
            {
                return new Hashtable();
            }
        }
    }

    #endregion

}

using System.Windows.Forms;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using UI.PresentationDesign.DesignUI.Services;
using System.ComponentModel;
using System.Collections.Generic;
using UI.PresentationDesign.DesignUI.Controls.SourceTree;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using System.ComponentModel.Design;
using UI.PresentationDesign.DesignUI.Controls.SourceProperties;
using System.Linq;
using Domain.PresentationDesign.Client;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using Syncfusion.Windows.Forms.Tools;
using System.Drawing;
using UI.PresentationDesign.DesignUI.Properties;
using System;

namespace UI.PresentationDesign.DesignUI.Controls
{
    public partial class SourcePropertiesControl : UserControl
    {
        //LayoutController m_LayoutController;
        //SourcesController m_SourceController;

        static SourcePropertiesControl _instance = null;

        public static SourcePropertiesControl Instance
        {
            get
            {
                return _instance;
            }
        }

        public SourcePropertiesControl()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                _instance = this;
                Init();
            }

            this.propertyGrid1.PropertyGridValidating+=new CancelEventHandler(propertyGrid1_PropertyGridValidating);
        }

        void propertyGrid1_PropertyGridValidating(object sender, CancelEventArgs e)
        {
            ToolTipInfo t_info = new ToolTipInfo();
            t_info.Body.Image = Resources.error;
            t_info.Header.Text = "Ошибка";

            Point p = this.PointToScreen(propertyGrid1.Location);
            p.Offset(-propertyGrid1.Width, 0);

            ISupportValidation validation = propertyGrid1.SelectedObject as ISupportValidation;
            string message;
            if (validation != null)
            {
                if (!validation.EnsureValidate(out message))
                {
                    t_info.Body.Text = message;
                    superToolTip1.Show(t_info, p);
                    propertyGrid1.Focus();
                    throw new Exception();
                }
            }
        }

        public void Init()
        {
            //m_SourceController = SourcesController.Instance;
            //m_LayoutController = LayoutController.Instance;
            PresentationController.Instance.OnSelectedResourceChanged += new SelectedResourceChanged(m_controller_OnSelectedSourceChanged);
            PresentationController.Instance.OnSlideChangedExternally += new SlideChanged(Instance_OnSlideChangedExternally);
            PresentationController.Instance.OnSlideLockChanged += new SlideLockChanged(Instance_OnSlideLockChanged);
            UndoService.Instance.OnHistoryChanged += new HistoryChanged(OnHistoryChanged);
        }

        void Instance_OnSlideLockChanged(Slide slide, bool IsLocked, LockingInfo info)
        {
            if (PresentationController.Instance.SelectedSlide.Id == slide.Id)
            {
                this.Invoke(new MethodInvoker(() =>
                    {
                        propertyGrid1.IsEnabled = IsLocked && PresentationController.Instance.CanUnlockSlide(slide);
                    }));
            }
        }

        void Instance_OnSlideChangedExternally(Slide slide)
        {
            if (this.propertyGrid1.AssignedObject != null)
            {
                Source src = ((Window)this.propertyGrid1.AssignedObject).Source;
                if (slide.SourceList.Any(s => s.Equals(src)))
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        propertyGrid1.Refresh();
                    }));
                }
            }
        }

        //void m_SourceController_OnResourceSelected(ResourceDescriptor target)
        //{
        //    if (target != null)
        //        AssignObject(target.ResourceInfo);
        //    else
        //        AssignObject(null);
        //}

        void m_controller_OnSelectedSourceChanged(SourceWindow node)
        {
            Slide slide = null;
            if (node != null)
            {
                slide = PresentationController.Instance.CurrentSlideLayout.Slide;
                AssignObject(node.Window);
                bool enabled = DesignerClient.Instance.IsStandAlone || (slide.IsLocked && PresentationController.Instance.CanUnlockSlide(slide));
                if (enabled != propertyGrid1.IsEnabled) propertyGrid1.IsEnabled = enabled;
            }
            else
                AssignObject(null);

            //propertyGrid1.IsEnabled = DesignerClient.Instance.IsStandAlone || (slide.IsLocked && PresentationController.Instance.CanUnlockSlide(slide));
        }

        //void m_SourceController_OnResourceCreated(ResourceDescriptor target)
        //{
        //    AssignObject(target.ResourceInfo);
        //}

        void OnHistoryChanged(object Target)
        {
            if(Target is Window)
                AssignObject(Target);
        }

        void AssignObject(object obj)
        {
            //if (obj == this.propertyGrid1.SelectedObject)
            //    this.propertyGrid1.Refresh();
            //else
            if (this.propertyGrid1.AssignedObject != obj) 
                this.propertyGrid1.AssignedObject = obj;
        }

        public void RefreshProperties()
        {
            if (this.IsHandleCreated)
                this.Invoke(new MethodInvoker(propertyGrid1.Refresh));
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            object target = propertyGrid1.AssignedObject;
            List<PropertyDescriptor> props = new List<PropertyDescriptor>();
            props.Add(e.ChangedItem.PropertyDescriptor);

            if (e.ChangedItem.Parent.Value != null) //nested?
            {
                GridItem parent = e.ChangedItem;
                for (; ; )
                {
                    parent = parent.Parent;
                    if (parent.Value == null)
                        break;

                    props.Add(parent.PropertyDescriptor);
                }
            }

            UndoService.Instance.PushAction(new PropertyChangedHistoryEntry(target, props, e.OldValue));
            PresentationController.Instance.PresentationChanged = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Domain.Administration.AdministrationClient;
using Syncfusion.Styles;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Tools;
using TechnicalServices.Configuration.Common;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using UI.Administration.AdministrationUI.Controls;
using UI.Administration.AdministrationUI.Forms;
using UI.PresentationDesign.DesignUI.Controls.Config;

namespace UI.Administration.AdministrationUI.Controllers
{
    public delegate void SystemParametersCellChenged(bool changed);

    public class SystemParametersController : CommonAdministationController
    {
        public event SystemParametersCellChenged OnSystemParametersCellChenged; 

        private readonly AdministrationForm view;
        private static SystemParametersController _instance;
        private GridControl _gridControl;
        private bool _changed;
        public SystemParametersController(AdministrationForm form, GridControl gridControl)
        {
            view = form;
            _gridControl = gridControl;
            _instance = this;
            //identity = Thread.CurrentPrincipal as UserIdentity;//TODO
        }

        private void AdministrationForm_OnSystemParametersCellChenged(bool changed)
        {
            
        }

        public bool Changed
        {
            get { return _changed;}
            set 
            { 
                _changed = value;
                OnSystemParametersCellChenged(_changed);
            }

        }

        public ISystemParameters SystemParameters
        {
            get { return AdministrationClient.Instance.LoadSystemParameters(); }
        }

        public PresentationInfoExt[] PresentationWithOneSlide
        {
            get
            {
                return AdministrationClient.Instance.LoadPresentationWithOneSlide();
            }
        }

        public static SystemParametersController Instance
        {
            get { return _instance; }
        }

        void WindowSize_OnSizeChanged(WindowSizeSetter sender)
        {
            Changed = true;
        }

        void FileNameControll_OnFileNameChanged(FileNameControl sender)
        {
            Changed = true;
        }

        private void SystemParametersController_Changed(object sender, StyleChangedEventArgs e)
        {
            Changed = true;
        }

        private void _gridControl_CurrentCellChanged(object sender, EventArgs e)
        {
            Changed = true;
        }
        
        public void LoadSystemParameters()
        {
            _gridControl.Clear(true);
            Type clsType = typeof(SystemParameters);
            MemberInfo[] mInfo;
            string propertyName;
            string propertyComment;
            //_gridControl[3, 2].CellType = "Control";
            _gridControl[3, 2].CellType = "TextBox";
            //_gridControl[3, 2].Control = new FileNameControl { SelectedFileName = SystemParameters.ReloadImage };
            _gridControl[3, 2].Text = SystemParameters.ReloadImage;
            _gridControl[3, 2].FormattedText = SystemParameters.ReloadImage;
            //((FileNameControl)(_gridControl[3, 2].Control)).OnFileNameChanged += new FileNameChanged(FileNameControll_OnFileNameChanged);
            mInfo = clsType.GetMember("ReloadImage");
            propertyName = ((DisplayNameAttribute)Attribute.GetCustomAttribute(mInfo[0], typeof(DisplayNameAttribute))).DisplayName;
            propertyComment = ((DescriptionAttribute)Attribute.GetCustomAttribute(mInfo[0], typeof(DescriptionAttribute))).Description;
            _gridControl[3, 1].Text = propertyName;
            _gridControl[3, 3].Text = propertyComment;
            
            //_gridControl[4, 2].CellType = "TextBox";
            //_gridControl[4, 2].Text =  SystemParameters.SystemName;
            //_gridControl[4, 2].FormattedText = SystemParameters.SystemName;
            //mInfo = clsType.GetMember("SystemName");
            //propertyName = ((DisplayNameAttribute)Attribute.GetCustomAttribute(mInfo[0], typeof(DisplayNameAttribute))).DisplayName;
            //propertyComment = ((DescriptionAttribute)Attribute.GetCustomAttribute(mInfo[0], typeof(DescriptionAttribute))).Description;
            //_gridControl[4, 1].Text = propertyName;
            //_gridControl[4, 3].Text = propertyComment;

            //_gridControl[4, 2].CellType = "MaskedEdit";
            //_gridControl[4, 2].MaskEdit.Mask = "999999999";
            //_gridControl[4, 2].MaskEdit.MinValue = 1;
            //_gridControl[4, 2].MaskEdit.MaxValue = uint.MaxValue;
            //_gridControl[4, 2].CellValueType = typeof (uint);
            //_gridControl[4, 2].ValidateValue = new GridCellValidateValueInfo(true, 1, Double.MaxValue, "ВВеденное значение недопустимо");
            //_gridControl. += new ValidationErrorEventHandler(SystemParametersController_ValidationError);
            _gridControl[4, 2].Text = SystemParameters.BackgroundPresentationRestoreTimeout.ToString();
            //_gridControl[4, 2].FormattedText = SystemParameters.BackgroundPresentationRestoreTimeout.ToString();
            mInfo = clsType.GetMember("BackgroundPresentationRestoreTimeout");
            propertyName = ((DisplayNameAttribute)Attribute.GetCustomAttribute(mInfo[0], typeof(DisplayNameAttribute))).DisplayName;
            propertyComment = ((DescriptionAttribute)Attribute.GetCustomAttribute(mInfo[0], typeof(DescriptionAttribute))).Description;
            _gridControl[4, 1].Text = propertyName;
            _gridControl[4, 3].Text = propertyComment;

            _gridControl[1, 2].DataSource = PresentationWithOneSlide;
            _gridControl[1, 2].DisplayMember= "Name";
            _gridControl[1, 2].ValueMember = "UniqueName"; 
            _gridControl[1, 2].CellType = "ComboBox";
            
            mInfo = clsType.GetMember("BackgroundPresentationUniqueName");
            
            PresentationInfo presentation = PresentationWithOneSlide.ToList().Find(x => x.UniqueName == SystemParameters.BackgroundPresentationUniqueName);
            if (presentation != null)
            {
                _gridControl[1, 2].FormattedText = presentation.Name;
                _gridControl[1, 2].Text = presentation.UniqueName;
            }
            else
            {
                _gridControl[1, 2].FormattedText = "";
                _gridControl[1, 2].Text = "";
            }
            propertyName =((DisplayNameAttribute)Attribute.GetCustomAttribute(mInfo[0], typeof(DisplayNameAttribute))).DisplayName;
            propertyComment = ((DescriptionAttribute)Attribute.GetCustomAttribute(mInfo[0], typeof(DescriptionAttribute))).Description;
            _gridControl[1, 1].Text = propertyName;
            _gridControl[1, 3].Text = propertyComment;
            
            _gridControl.CurrentCellChanged+=new EventHandler(_gridControl_CurrentCellChanged);
            _gridControl[1, 3].Changed +=new StyleChangedEventHandler(SystemParametersController_Changed);

            _gridControl[2, 2].CellType = "Control";
            //_gridControl[2, 2].CellType = "TextBox";
            //_gridControl[2, 2].Text = SystemParameters.ReloadImage;
            //_gridControl[2, 2].FormattedText = SystemParameters.ReloadImage;
            _gridControl[2, 2].Control = new WindowSizeSetter(SystemParameters.DefaultWndsize);
            ((WindowSizeSetter)(_gridControl[2, 2].Control)).OnSizeChanged += new SizeChanged(WindowSize_OnSizeChanged);
            mInfo = clsType.GetMember("DefaultWndsize");
            propertyName = ((DisplayNameAttribute)Attribute.GetCustomAttribute(mInfo[0], typeof(DisplayNameAttribute))).DisplayName;
            propertyComment = ((DescriptionAttribute)Attribute.GetCustomAttribute(mInfo[0], typeof(DescriptionAttribute))).Description;
            _gridControl[2, 1].Text = propertyName;
            _gridControl[2, 3].Text = propertyComment;

        }

        public void SaveSystemParameters()
        {
            if(Changed)
            {   
                SystemParameters systemParametersUpdate = new SystemParameters();
                _gridControl.ConfirmChanges();
                systemParametersUpdate.ReloadImage = _gridControl[3, 2].Text;     //((FileNameControl)_gridControl[3, 2].Control).SelectedFileName;
                //systemParametersUpdate.SystemName = _gridControl[4, 2].Text;
                systemParametersUpdate.BackgroundPresentationUniqueName = _gridControl[1, 2].Text;
                systemParametersUpdate.DefaultWndsize = ((WindowSizeSetter)_gridControl[2, 2].Control).WindowSize;
                int time;
                if (Int32.TryParse(_gridControl[4, 2].Text, out time))
                    systemParametersUpdate.BackgroundPresentationRestoreTimeout = time;
                systemParametersUpdate.IsDirty = true;
                Changed = false;
                AdministrationClient.Instance.SaveSystemParameters(systemParametersUpdate); 
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Windows.Forms.Diagram;
using UI.PresentationDesign.DesignUI.Controls.Utils;
using UI.PresentationDesign.DesignUI.Classes.View;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing;

namespace UI.PresentationDesign.DesignUI.Helpers
{
    public class ZoomComboBox : ToolStripComboBoxEx
    {
        DiagramViewBase _view;
        bool magnificationChanging = false;
        SuperToolTip tip;

        public ZoomComboBox()
            : base()
        {
            tip = new SuperToolTip();
            tip.UseFading = SuperToolTip.FadingType.System;
        }

        protected override void OnSelectedIndexChanged(EventArgs ea)
        {
            base.OnSelectedIndexChanged(ea);

            if (_view != null && base.SelectedIndex > 0)
            {
                magnificationChanging = true;
                _view.Magnification = (base.Items[base.SelectedIndex] as ZoomComboHelper).Value;
            }
        }

        public void ConnectToView(Syncfusion.Windows.Forms.Diagram.View view)
        {
            this.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            _view = view as DiagramViewBase;
            base.Items.AddRange(ZoomComboHelper.CreateList());
            //base.SelectedIndex = ZoomComboHelper.GetIndexForPercentage(100);

            if(_view!=null)
                _view.OnViewMagnifincationChanged += new MagnificationChanged(_view_OnViewMagnifincationChanged);
        }

        void _view_OnViewMagnifincationChanged(ViewMagnificationEventArgs e)
        {
            if (magnificationChanging)
            {
                magnificationChanging = false;
                return;
            }

            if (e.NewMagnification != e.OriginalMagnification)
            {
                this.Text = ZoomComboHelper.GetTextForZoom(e.NewMagnification);
            }
        }


        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (byte)Keys.Enter)
            {
                float value;
                if (ValidateInput(out value))
                {
                    if (value < ZoomComboHelper.MinValue)
                        value = ZoomComboHelper.MinValue;

                    if (value > ZoomComboHelper.MaxValue)
                        value = ZoomComboHelper.MaxValue;

                    this.Text = ZoomComboHelper.GetTextForZoom(value);
                    _view.Magnification = value;
                }
               
                e.Handled = true;
            }

            if(!e.Handled)
                base.OnKeyPress(e);

        }


        bool ValidateInput(out float value)
        {
            value = 0;
            float f;
            if (float.TryParse(Text, out f))
            {
                value = f;
                return true;
            }
            else
            {
                //вернуть первоначальное значение
                if (_view != null)
                    Text = ZoomComboHelper.GetTextForZoom(_view.Magnification);
                else
                    base.SelectedIndex = ZoomComboHelper.GetIndexForPercentage(100);
               
                //show error
                ToolTipInfo t_info = new ToolTipInfo();
                t_info.Body.Image = Properties.Resources.error;
                t_info.Header.Text = "Ошибка";
                t_info.Body.Text = String.Format("Для масштаба требуется числовое значение (десятичный разделитель: \"{0}\")", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                t_info.Footer.Text = "Значение масштаба указывается в процентах";
                tip.Show(t_info, this.Parent.PointToScreen(new Point(this.Bounds.Right, this.Bounds.Bottom)), 1000);
                this.ComboBox.Focus();

                return false;
            }
        }
    }
}

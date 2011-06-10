using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;

using GALILEOLib;

using Hosts.Plugins.Jupiter.SystemModule.Config;
using Hosts.Plugins.Jupiter.SystemModule.Design;

using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.Jupiter.UI
{
    public partial class JupiterForm : Form, IDoCommand
    {
        public JupiterForm()
        {
            InitializeComponent();
        }

        internal bool Init(JupiterDisplayConfig display, JupiterWindow wnd)
        {
            this.InitBorderTitle(wnd, axGalileoCtrl);

            Text = wnd.TitleText;
            try
            {
                if (wnd.VideoIn < 0 || wnd.VideoIn >= display.InOutConfigList.Count)
                    throw new IndexOutOfRangeException("Указан не существующий вход видеостены");

                axGalileoCtrl.SuspendLayout();
                //axGalileoCtrl.BeginInit();
                try
                {
                    JupiterInOutConfig configWindow = display.InOutConfigList[wnd.VideoIn - 1];
                    switch (configWindow.WindowType)
                    {
                        case WindowTypeEnum.RGB:
                            CreateRGB(display, wnd, configWindow);
                            break;
                        case WindowTypeEnum.Video:
                            CreateLive(display, wnd, configWindow);
                            break;
                        default:
                            throw new ApplicationException(String.Format(
                                                               "Данный тип окна: {0}, не поддерживается системой ",
                                                               configWindow.WindowType));
                    }
                    // Бага в OCX, чтобы работало контрол должен получить команду Move
                    axGalileoCtrl.Width = 0;
                    axGalileoCtrl.Height = 0;
                    axGalileoCtrl.Width = Width;
                    axGalileoCtrl.Height = Height;

                    SetCropping(wnd);
                    axGalileoCtrl.Start();
                }
                finally
                {
                    //axGalileoCtrl.EndInit();
                    axGalileoCtrl.ResumeLayout();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return true;
        }

        public void CreateRGB(JupiterDisplayConfig display, JupiterWindow wnd, JupiterInOutConfig configWindow)
        {
            axGalileoCtrl.RGBChannel = configWindow.VideoIn;
            SetRGBTiming(wnd);
        }

        public void CreateLive(JupiterDisplayConfig display, JupiterWindow wnd, JupiterInOutConfig configWindow)
        {
            axGalileoCtrl.LVChannel = configWindow.VideoIn;
        }

        private void SetRGBTiming(ActiveWindow wnd)
        {
            if (wnd.Source.ResourceDescriptor == null || wnd.Source.ResourceDescriptor.ResourceInfo == null)
                return;
            ResourceInfoForHardwareSource resInfo =
                wnd.Source.ResourceDescriptor.ResourceInfo as ResourceInfoForHardwareSource;
            if (resInfo == null) return;

            if (!resInfo.RGBParam.ManualSetting)
            {
                axGalileoCtrl.DetectRGBTiming();
                return;
            }

            SetRGBParam(resInfo.RGBParam);
        }

        private void SetRGBParam(RGBParam rgbParam)
        {
            IRGBTimingControl control = axGalileoCtrl.Interface as IRGBTimingControl;
            if (control == null) return;

            RGBTimingClass timing = new RGBTimingClass();

            timing.Width = rgbParam.HWidth;
            timing.HOffset = rgbParam.HOffset;
            timing.HTotal = rgbParam.HTotal;

            timing.Height = rgbParam.VHeight;
            timing.VOffset = rgbParam.VOffset;
            timing.VTotal = rgbParam.VTotal;

            timing.Phase = rgbParam.Phase;
            timing.SyncType = 0;
            timing.VFreq = rgbParam.VFreq;

            timing.HSyncNeg = rgbParam.HSyncNeg;
            timing.VSyncNeg = rgbParam.VSyncNeg;

            timing.Valid = true;

            control.Timing = timing;
        }

        private RGBParam GetRGBParam()
        {
            IRGBTimingControl control = axGalileoCtrl.Interface as IRGBTimingControl;
            if (control == null) return null;

            //if (!control.Detect()) return null;
            RGBParam rgbParam = new RGBParam();
            RGBTiming timing = control.Timing;

            rgbParam.HWidth = timing.Width;
            rgbParam.HOffset = timing.HOffset;
            rgbParam.HTotal = timing.HTotal;

            rgbParam.VHeight = timing.Height;
            rgbParam.VOffset = timing.VOffset;
            rgbParam.VTotal = timing.VTotal;

            rgbParam.Phase = (short)timing.Phase;
            rgbParam.VFreq = timing.VFreq;

            rgbParam.HSyncNeg = timing.HSyncNeg;
            rgbParam.VSyncNeg = timing.VSyncNeg;

            return rgbParam;

        }

        private void SetCropping(ActiveWindow wnd)
        {
            if ((wnd.CroppingLeft | wnd.CroppingRight | wnd.CroppingTop | wnd.CroppingBottom) == 0) return;
            axGalileoCtrl.Crop(wnd.CroppingLeft, wnd.CroppingTop,
                               wnd.Width - wnd.CroppingRight,
                               wnd.Height - wnd.CroppingBottom);
        }

        #region Implementation of IDoCommand

        public string DoCommand(string command)
        {
            try
            {
                if (String.IsNullOrEmpty(command))
                {
                    if (axGalileoCtrl.RGBChannel >= 0)
                    {
                        RGBParam param = GetRGBParam();
                        if (param != null) return param.ToString();
                    }
                }
                else
                {
                    RGBParam rgbParam;
                    if (RGBParam.TryParse(command, out rgbParam))
                        SetRGBParam(rgbParam);
                }
            }
            catch
            {
            }
            return string.Empty;
        }

        #endregion
    }
}
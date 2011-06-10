using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using Timer=System.Timers.Timer;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.Player
{
    public abstract class PlayerPlaginsRGBController<TDevice, TView>
        : PlayerPlaginsController<TDevice, TView>
            where TDevice : DeviceAsSource 
            where TView : IPlayerPlaginRGBBaseView
    {
        private const int CheckRgbInterval = 1000;
        private readonly static Random _rnd = new Random(100);

        private readonly System.Timers.Timer _rgbTimer;
        private readonly object _syncObject = new object();

        protected IPresentationClient _presClient;
        private ResourceDescriptor _resource;
        private HardwareSource _hardwareSource;

        public PlayerPlaginsRGBController(IPresentationClient presClient,
            /*ResourceDescriptor resource,*/ HardwareSource hardwareSource,
            TDevice device, IPlayerCommand playerCommand, 
            IEventLogging logging, TView view) 
            : base(device, playerCommand, logging, view)
        {
            // Разносим вызовы по времени, что все источники не ломились одновременно
            _rgbTimer = new Timer(CheckRgbInterval + _rnd.Next(100));
            _rgbTimer.BeginInit();
            _rgbTimer.Elapsed += new ElapsedEventHandler(_rgbTimer_Elapsed);
            _rgbTimer.AutoReset = true;
            _rgbTimer.EndInit();
            _rgbTimer.Start();

            _hardwareSource = hardwareSource;
            _resource = _hardwareSource.ResourceDescriptor;
            _presClient = presClient;
            ResourceInfoForHardwareSource res = (ResourceInfoForHardwareSource) _resource.ResourceInfo;
            View.UpdateRGBSettings(res);
            View.OnRGBSettingsChanged += ViewOnRGBSettingsChanged;
        }

        private void _rgbTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!Monitor.TryEnter(_syncObject)) return;
            try
            {
                if (!IsShow) return;

                ResourceInfoForHardwareSource res = (ResourceInfoForHardwareSource) _resource.ResourceInfo;
                if (!res.RGBParam.ManualSetting)
                {
                    string rgb = PlayerCommand.DoSourceCommand(_hardwareSource, String.Empty);
                    if (!String.IsNullOrEmpty(rgb))
                    {
                        RGBParam param;
                        if (RGBParam.TryParse(rgb, out param))
                        {
                            res.RGBParam.HWidth = param.HWidth;
                            res.RGBParam.HOffset = param.HOffset;
                            res.RGBParam.HTotal = param.HTotal;

                            res.RGBParam.VHeight = param.VHeight;
                            res.RGBParam.VOffset = param.VOffset;
                            res.RGBParam.VTotal = param.VTotal;

                            res.RGBParam.Phase = (short) param.Phase;
                            res.RGBParam.VFreq = param.VFreq;

                            res.RGBParam.HSyncNeg = param.HSyncNeg;
                            res.RGBParam.VSyncNeg = param.VSyncNeg;
                            View.UpdateRGBSettings(res);
                        }
                    }
                }
                _rgbTimer.Stop();
            }
            finally
            {
                Monitor.Exit(_syncObject);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            if (_rgbTimer != null)
            {
                _rgbTimer.Dispose();
            }
            View.OnRGBSettingsChanged -= ViewOnRGBSettingsChanged;
        }

        private void ViewOnRGBSettingsChanged(ResourceInfoForHardwareSource settings)
        {
            string otherResourceId;
            settings.RGBParam.ManualSetting = true;
            _resource.ResourceInfo = settings;
            // и команду визуализатору
            PlayerCommand.DoSourceCommand(_hardwareSource, settings.RGBParam.ToString());
            _presClient.GetResourceCrud().SaveSource(_resource, out otherResourceId);
        }
    }
}

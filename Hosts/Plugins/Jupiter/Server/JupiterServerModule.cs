using System;
using System.Collections.Generic;
using System.Linq;

using DomainServices.EnvironmentConfiguration.ConfigModule.Server;

using Hosts.Plugins.Jupiter.SystemModule.Config;
using Hosts.Plugins.Jupiter.SystemModule.Design;

using TechnicalServices.ActiveDisplay.Util;
using TechnicalServices.HardwareEquipment.Util;
using TechnicalServices.Interfaces;
using TechnicalServices.Interfaces.ConfigModule;
using TechnicalServices.Licensing;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.Jupiter.Server
{
    // Jupiter - хитрый модуль он должен содержать сервеную часть отвечающую за управление софтварными источниками но так же он сам может быть как хардварный источник и так же хардварным девайсом
    public sealed class JupiterServerModule : ComputerServerModule<JupiterModule, JupiterDisplayConfig>
    {
        private readonly JupiterHardwareServerModule _jupiterHardwareServerModule = new JupiterHardwareServerModule();

        #region Nested class - отвечающий за поддержку Юпитера как хардварного источника и девайса

        //тут пишем DisplayType так как с точки зрения дисплея - Юпитер софтовый дисплей а не хардварный

        private class JupiterHardwareServerModule :
            HardwareEquipmentServerModule<SourceType, JupiterDeviceConfig, JupiterDisplayConfig>
        {
            public override void CheckLicense()
            {
                LicenseChecker checker = new LicenseChecker();
                checker.CheckFeature((int)Feature.Jupiter);
            }

            protected override CommandDescriptor[] GetCommand(Display display1, Display display2)
            {
                List<CommandDescriptor> result = new List<CommandDescriptor>();
                JupiterDisplayDesign disp1 = display1 as JupiterDisplayDesign;
                JupiterDisplayDesign disp2 = display2 as JupiterDisplayDesign;

                if (disp1 == null && disp2 != null)
                {
                    foreach (JupiterWindow window in disp2.WindowList.Where(d => d.Source.Type is HardwareSourceType))
                    {
                        result.Add(LogicSetTie((JupiterDisplayConfig)disp2.Type, window, 1));
                    }
                }
                if (disp1 != null && disp2 == null)
                {
                    foreach (JupiterWindow window in disp1.WindowList.Where(d => d.Source.Type is HardwareSourceType))
                    {
                        result.Add(LogicSetTie((JupiterDisplayConfig)disp1.Type, window, 0));
                    }
                }
                if (disp1 != null && disp2 != null)
                {
                    IEnumerable<JupiterWindow> list1 =
                        disp1.WindowList.Where(w => w.Source.Type is HardwareSourceType).Cast<JupiterWindow>();
                    IEnumerable<JupiterWindow> list2 =
                        disp2.WindowList.Where(w => w.Source.Type is HardwareSourceType).Cast<JupiterWindow>();

                    foreach (JupiterWindow jupiterWindow in list1)
                    {
                        int VideoIn = jupiterWindow.VideoIn;
                        JupiterWindow windows = list2.FirstOrDefault(jw => jw.VideoIn == VideoIn);
                        if (windows == null)
                        {
                            result.Add(LogicSetTie((JupiterDisplayConfig)disp1.Type, jupiterWindow, 0));
                        }
                        else     
                        {
                            //https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-1941
                            // if (!jupiterWindow.Source.Type.Equals(windows.Source.Type))
                            result.Add(LogicSetTie((JupiterDisplayConfig)disp1.Type, jupiterWindow, 0));
                            result.Add(LogicSetTie((JupiterDisplayConfig)disp1.Type, windows, 1));
                        }
                    }
                    foreach (JupiterWindow jupiterWindow in list2)
                    {
                        int VideoIn = jupiterWindow.VideoIn;
                        JupiterWindow windows = list1.FirstOrDefault(jw => jw.VideoIn == VideoIn);
                        if (windows == null)
                        {
                            result.Add(LogicSetTie((JupiterDisplayConfig)disp1.Type, jupiterWindow, 1));
                        }
                    }
                }

                return result.ToArray();
            }

            private static CommandDescriptor LogicSetTie(JupiterDisplayConfig disp, JupiterWindow window, int value)
            {
                int videoIn = window.VideoIn;
                JupiterInOutConfig conf =
                    disp.InOutConfigList.First(io => io.VideoIn == videoIn);
                int x = ((HardwareSourceType)window.Source.Type).Input;
                int y = conf.SwitchOut;
                return new CommandDescriptor(0, "LogicSetTie", x, y, value);
            }

            protected override CommandDescriptor[] GetCommand(Device device1, Device device2)
            {
                List<CommandDescriptor> result = new List<CommandDescriptor>();
                //JupiterDeviceDesign dev1 = device1 as JupiterDeviceDesign;
                JupiterDeviceDesign dev2 = device2 as JupiterDeviceDesign;
                if (dev2 != null)
                {
                    result.Add(new CommandDescriptor(dev2.Type.UID, GetCommandByName(dev2, "SetPower").Command,
                                                     Convert.ToInt32(dev2.OnOffState)));
                    if (dev2.OnOffState)
                    {
                        result.Add(new CommandDescriptor(dev2.Type.UID, GetCommandByName(dev2, "SetPicMute").Command,
                                                         Convert.ToInt32(dev2.PictureMute)));
                        result.Add(new CommandDescriptor(dev2.Type.UID, GetCommandByName(dev2, "SetBrightness").Command,
                                                         Convert.ToInt32(dev2.Brightness)));
                    }
                }
                return result.ToArray();
            }
        }

        #endregion

        public override CommandDescriptor[] GetCommand(Slide slide1, Slide slide2, EquipmentType[] freezedEquipment)
        {
            return _jupiterHardwareServerModule.GetCommand(slide1, slide2, freezedEquipment);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="config"></param>
        /// <param name="module"></param>
        /// <param name="controller"></param>
        public override void Init(IConfiguration config, IModule module, IControllerChannel controller)
        {
            base.Init(config, module, controller);
            _jupiterHardwareServerModule.Init(config, module, controller);
        }

        public override void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckFeature((int)Feature.Jupiter);
        }
    }
}
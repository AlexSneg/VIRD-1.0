using System;
using System.Collections.Generic;

using DomainServices.EnvironmentConfiguration.ConfigModule.Server;

using Hosts.Plugins.DVDPlayer.SystemModule.Config;
using Hosts.Plugins.DVDPlayer.SystemModule.Design;

using TechnicalServices.HardwareEquipment.Util;
using TechnicalServices.Interfaces;
using TechnicalServices.Licensing;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.DVDPlayer.Server
{
    public sealed class DVDPlayerServerModule :
        HardwareEquipmentServerModule<DVDPlayerSourceConfig, DVDPlayerDeviceConfig, DisplayType>
    {
        public override void CheckLicense()
        {
            LicenseChecker checker = new LicenseChecker();
            checker.CheckFeature((int) Feature.DVDPlayer);
        }

        //нужно переопределить для корректной работы устройства при переходе на следующий слайд
        protected override CommandDescriptor[] GetCommand(Device device1, Device device2)
        {
            List<CommandDescriptor> result = new List<CommandDescriptor>();
            DVDPlayerDeviceDesign dev1 = device1 as DVDPlayerDeviceDesign;
            DVDPlayerDeviceDesign dev2 = device2 as DVDPlayerDeviceDesign;
            if (dev2 != null)
            {
                //if ((dev1 == null) || (dev1.IsPlayerOn != dev2.IsPlayerOn))
                {
                    //если на пред. слайде устройства нет или если статус изменился, то выполнить команду вкл/выкл
                    result.Add(new CommandDescriptor(dev2.Type.UID, GetCommandByName(dev2, "Power").Command,
                                                     Convert.ToInt32(dev2.IsPlayerOn)));
                }
                if (dev2.IsPlayerOn)
                {
                    //есили устройство включено, можно провести другие команды
                    //if ((dev1 == null) || (dev1.SceneAssociatedCommand != dev2.SceneAssociatedCommand))
                    {
                        //если на пред. слайде устройства нет или если команда изменилась, выполним команду
                        if (dev2.SceneAssociatedCommand != SceneAssociatedCommandEnum.None)
                        {
                            //будем выполнять команду если она задана    
                            result.Add(new CommandDescriptor(dev2.Type.UID,
                                                             GetCommandByName(dev2,
                                                                              dev2.SceneAssociatedCommand.ToString()).
                                                                 Command));
                        }
                    }
                    if (dev2.MediumType == MediumTypeEnum.DVD)
                    {
                        //главу можно ставить только для DVD
                        //if ((dev1 == null) || (dev1.DVDChapter != dev1.DVDChapter))
                        {
                            //если на пред. слайде устройства нет или если глава изменился, то установим главу
                            result.Add(new CommandDescriptor(dev2.Type.UID, GetCommandByName(dev2, "Chapter").Command,
                                                             dev2.DVDChapter));
                        }
                    }
                    //if ((dev1 == null) || (dev1.Track != dev1.Track))
                    {
                        //если на пред. слайде устройства нет или если трек изменился, то установим трек
                        result.Add(new CommandDescriptor(dev2.Type.UID, GetCommandByName(dev2, "Track").Command,
                                                         dev2.Track));
                    }
                }
            }
            return result.ToArray();
        }
    }
}
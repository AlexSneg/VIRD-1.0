using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Hosts.Plugins.AudioMixer.SystemModule.Config;
using Hosts.Plugins.Jupiter.SystemModule.Config;

using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace UI.PresentationDesign.ConfiguratorUI
{
    public static class ModuleConfigurationExt
    {
        public static void Check(this ModuleConfiguration config, IEnumerable<Type> list)
        {
            // Синхронизируем источники и связанняе с ним устройствами
            foreach (SourceType sourceType in config.SourceList)
            {
                DeviceType deviceType = sourceType.DeviceType;
                if (deviceType == null) continue;
                if (config.DeviceList.IndexOf(deviceType) == -1)
                    throw new IndexOutOfRangeException(
                        String.Format("Устройство \"{0}\" не найдено в коллекции DeviceList", deviceType.Name));
                deviceType.UID = sourceType.UID;
                deviceType.Name = sourceType.Name;
                deviceType.Type = sourceType.Type;
                deviceType.Model = sourceType.Model;
                deviceType.Comment = sourceType.Comment;
            }

            // Делаем мапинг для софтварных источников для активных дисплеев
            foreach (DisplayTypeUriCapture displayType in config.DisplayList.Where(d => d is DisplayTypeUriCapture))
            {
                foreach (SourceType sourceType in config.SourceList.Where(src => !src.IsHardware))
                {
                    string name = sourceType.Name;
                    Mapping item = displayType.MappingList.FirstOrDefault(m => m.Source.Name == name);
                    if (item != null) continue;

                    if (displayType is JupiterDisplayConfig)
                        displayType.MappingList.Add(new JupiterMapping {Source = sourceType});
                    else
                        displayType.MappingList.Add(new Mapping {Source = sourceType});
                }
            }

            // Синхронизируем поля, так как наименование могли поменять
            foreach (DisplayType displayType in config.DisplayList)
                foreach (Mapping mapping in displayType.MappingList)
                    mapping.SourceName = mapping.Source.Name;

            // Проверка на корректность UID у HardwareSource
            //CheckUID(config.SourceList.Where(src => src.IsHardware),
            CheckUID(config.SourceList.Where(src => src is HardwareSourceType),
                     "Перечисленные аппаратные источники имеют одинаковый UID:{0}",
                     "Перечисленные аппаратные источники имеют неправильный UID:{0}");

            // Проверка на корректность UID у Device
            CheckUID(config.DeviceList,
                     "Перечисленные устройства имеют одинаковый UID:{0}",
                     "Перечисленные устройства имеют неправильный UID:{0}");

            /*https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-1630
            // Проверка на корректность Input у HardwareSourceType
            foreach (
                IGrouping<int, HardwareSourceType> grouping in
                    config.SourceList.Where(src => src is HardwareSourceType).Cast<HardwareSourceType>().GroupBy(
                        item => item.Input))
            {
                if (grouping.Key == 0) continue;
                if (grouping.Count() > 1)
                {
                    string message = String.Empty;
                    foreach (HardwareSourceType type in grouping)
                    {
                        message += Environment.NewLine;
                        message += String.Format("UID:{0}, Name:{1}, Type:{2}", type.UID, type.Name,
                                                 type.Type);
                    }
                    throw new ApplicationException(
                        String.Format("Перечисленные аппаратные источники имеют одинаковое значение поля \"Вход коммутатора\":{0}", message));
                }
            }*/

            // Проверка на корректность Output у PassiveDisplayType
            foreach (IGrouping<int, PassiveDisplayType> grouping in 
                config.DisplayList.Where(disp => disp is PassiveDisplayType).Cast<PassiveDisplayType>().GroupBy(
                    item => item.Output))
            {
                if (grouping.Key == 0) continue;
                if (grouping.Count() > 1)
                {
                    string message = String.Empty;
                    foreach (PassiveDisplayType type in grouping)
                    {
                        message += Environment.NewLine;
                        message += String.Format("UID:{0}, Name:{1}, Type:{2}", type.UID, type.Name,
                                                 type.Type);
                    }
                    throw new ApplicationException(
                        String.Format("Перечисленные дисплеи имеют одинаковое значение поля\"Выход коммутатора\":{0}", message));
                }
            }


            foreach (SourceType sourceType in config.SourceList.Where(src => !src.IsHardware))
            {
                if (sourceType.UID == -1) continue;
                throw new ApplicationException(
                    String.Format("Программный источник{0}UID:{1}, Name:{2}, Type:{3}{0}должен иметь UID = -1",
                                  Environment.NewLine, sourceType.UID, sourceType.Name, sourceType.Type));
            }

            // Проверяем наличие "потерянных" устройств
            foreach (DeviceType deviceType in config.DeviceList)
            {
                DeviceType item = deviceType;
                if (item.Visible) continue;
                if (config.SourceList.Exists(src => src.UID == item.UID)) continue;
                throw new ApplicationException(
                    String.Format(
                        "Обнаружено устройство{0}UID:{1}, Name:{2}, Type:{3}{0} не связвнное с аппаратным источником",
                        Environment.NewLine, item.UID, item.Name, item.Type));
            }

            // Проверяем наличие битых мапингов в дисплеях
            foreach (DisplayType displayType in config.DisplayList)
            {
                foreach (Mapping mapping in displayType.MappingList)
                {
                    if (!mapping.Source.IsHardware) continue;
                    int sourceUid = mapping.Source.UID;
                    SourceType sourceType = config.SourceList.FirstOrDefault(s => s.UID == sourceUid);
                    if (sourceType != null) continue;
                    throw new ApplicationException(
                        String.Format("Обнаружен дисплей \"{0}\" с битым мапингом аппаратных источников",
                                      displayType.Name));
                }
            }

            foreach (
                DisplayTypeUriCapture displayType in
                    config.DisplayList.Where(d => d is DisplayTypeUriCapture).Cast<DisplayTypeUriCapture>())
            {
                if (displayType.Address.StartsWith(@"net.tcp://"))
                {
                    if (displayType.Address.EndsWith(@":790/Agent"))
                        continue;
                }
                throw new ApplicationException(String.Format("Для компьютера {0} неверно задано значение поля URI",
                                                             displayType.Name));
            }

            foreach (
                IGrouping<string, DisplayTypeUriCapture> grouping in
                    config.DisplayList.Where(d => d is DisplayTypeUriCapture).Cast<DisplayTypeUriCapture>().GroupBy(
                        disp => disp.Address))
            {
                if (grouping.Count() > 1)
                {
                    string message = String.Empty;
                    foreach (DisplayTypeUriCapture type in grouping)
                    {
                        message += Environment.NewLine;
                        message += String.Format("UID:{0}, Name:{1}, Type:{2}", type.UID, type.Name, type.Type);
                    }
                    throw new ApplicationException(String.Format(
                                                       "Данные дисплеи имеют одинаковое значение поля Uri:{0}", message));
                }
            }
            foreach (
                IGrouping<string, DisplayTypeUriCapture> grouping in
                    config.DisplayList.Where(d => d is DisplayTypeUriCapture).Cast<DisplayTypeUriCapture>().GroupBy(
                        disp => disp.AgentUID))
            {
                if (grouping.Count() > 1)
                {
                    string message = String.Empty;
                    foreach (DisplayTypeUriCapture type in grouping)
                    {
                        message += Environment.NewLine;
                        message += String.Format("UID:{0}, Name:{1}, Type:{2}", type.UID, type.Name, type.Type);
                    }
                    throw new ApplicationException(
                        String.Format("Данные дисплеи имеют одинаковое значение поля AgentUID:{0}", message));
                }
            }

            foreach (
                JupiterDisplayConfig displayConfig in
                    config.DisplayList.Where(d => d is JupiterDisplayConfig).Cast<JupiterDisplayConfig>())
            {
                if (displayConfig.Height != displayConfig.SegmentRows*displayConfig.SegmentHeight ||
                    displayConfig.Width != displayConfig.SegmentColumns*displayConfig.SegmentWidth)
                    throw new ApplicationException(
                        String.Format(
                            "Дисплей: UID:{0}, Name:{1}, Type:{2}, имеет неверно заполненые поля \"Разрешение сегмента (px)\", \"Количество сегментов\"",
                            displayConfig.UID, displayConfig.Name, displayConfig.Type));
            }

            foreach (IGrouping<int, Label> grouping in config.LabelList.GroupBy(l => l.Id))
            {
                if (grouping.Count() > 1)
                {
                    string message = String.Empty;
                    foreach (Label label in grouping)
                    {
                        message += Environment.NewLine;
                        message += String.Format("Id:{0}, Name:{1}", label.Id, label.Name);
                    }
                    throw new ApplicationException(String.Format("Данные метки имеют одинаковое значение поля Id:{0}",
                                                                 message));
                }
            }
            foreach (IGrouping<string, Label> grouping in config.LabelList.GroupBy(l => l.Name))
            {
                if (grouping.Count() > 1)
                {
                    string message = String.Empty;
                    foreach (Label label in grouping)
                    {
                        message += Environment.NewLine;
                        message += String.Format("Id:{0}, Name:{1}", label.Id, label.Name);
                    }
                    throw new ApplicationException(String.Format(
                                                       "Данные метки имеют одинаковое значение поля Name:{0}", message));
                }
            }

            foreach (AudioMixerDeviceConfig device in
                config.DeviceList.Where(d => d is AudioMixerDeviceConfig).Cast<AudioMixerDeviceConfig>())
            {
                for (int i = 0; i < device.FaderGroupList.Count; i++)
                {
                    for (int j = 0; j < device.FaderGroupList[i].FaderList.Count; j++)
                        for (int k = 0; k < device.FaderGroupList.Count; k++)
                        {
                            if (i == k) continue;
                            for (int l = 0; l < device.FaderGroupList[k].FaderList.Count; l++)
                            {
                                if (device.FaderGroupList[i].FaderList[j].InstanceID == device.FaderGroupList[k].FaderList[l].InstanceID)
                                {
                                    throw new ApplicationException(
                                        String.Format(
                                            "У аудиомикшера \"{5}\", в списке есть фейдеры с одинаковым InstanceID {4} (группа '{0}', фейдер '{1}' и группа '{2}', фейдер '{3}').",
                                            device.FaderGroupList[i].Name, device.FaderGroupList[i].FaderList[j].Name,
                                            device.FaderGroupList[k].Name, device.FaderGroupList[k].FaderList[l].Name,
                                            device.FaderGroupList[i].FaderList[j].InstanceID,
                                            device.Name));
                                }
                            }
                        }
                }
            }
        }

        public static void ApplayModelPreset(this ModuleConfiguration config)
        {
            Dictionary<Type, Preset[]> listPreset = new Dictionary<Type, Preset[]>();
            foreach (DeviceType deviceType in config.DeviceList)
            {
                Type type = deviceType.GetType();
                string model = deviceType.Model;

                if (!listPreset.ContainsKey(type)) listPreset.Add(type, Preset.GetPresetting(type.Assembly));
                
                Preset preset = listPreset[type].FirstOrDefault(i => i.Name == model);
                if (preset == null)
                    preset = listPreset[type].First(i => String.IsNullOrEmpty(i.Name));
                deviceType.CommandList.Clear();
                deviceType.CommandList.AddRange(preset.CommandList);

                //if (preset.PropertyList.Count == 0) continue;
                //int deviceUid = deviceType.UID;
                //PropertyInfo[] propertiesDevice = deviceType.GetType().GetProperties();
                //foreach (NameValuePair pair in preset.PropertyList)
                //{
                //    PropertyInfo pInfo = propertiesDevice.FirstOrDefault(p => p.Name == pair.Name);
                //    if (pInfo == null) continue;
                //    if (!pInfo.CanWrite || pInfo.PropertyType.IsClass) continue;
                //    if (pInfo.PropertyType.IsEnum)
                //    {
                //        //FieldInfo fi = pInfo.PropertyType.GetField(Enum.GetName(pInfo.PropertyType, pair.Value));
                //        object value = Enum.Parse(pInfo.PropertyType, pair.Value);
                //        if (value != null) pInfo.SetValue(deviceType, value, new object[] { });
                //    }
                //    else if (pInfo.PropertyType.IsValueType)
                //    {
                //        object value = Convert.ChangeType(pair.Value, pInfo.PropertyType);
                //        pInfo.SetValue(deviceType, value, new object[] { });
                //    }
                //}
            }
        }

        /// <summary>
        /// Проверка на корректность UID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="errMessage1"></param>
        /// <param name="errMessage2"></param>
        private static void CheckUID<T>(IEnumerable<T> list, string errMessage1, string errMessage2)
            where T : EquipmentType
        {
            IEnumerable<IGrouping<int, T>> itemList = list.GroupBy(item => item.UID);
            foreach (IGrouping<int, T> grouping in itemList)
            {
                if (grouping.Count() > 1)
                {
                    string message = String.Empty;
                    foreach (T type in grouping)
                    {
                        message += Environment.NewLine;
                        message += String.Format("UID:{0}, Name:{1}, Type:{2}", type.UID, type.Name, type.Type);
                    }
                    throw new ApplicationException(String.Format(errMessage1, message));
                }
                if (grouping.Key < 1)
                {
                    string message = String.Empty;
                    foreach (T type in grouping)
                    {
                        message += Environment.NewLine;
                        message += String.Format("UID:{0}, Name:{1}, Type:{2}", type.UID, type.Name, type.Type);
                    }
                    throw new ApplicationException(string.Format(errMessage2, message));
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using DomainServices.EnvironmentConfiguration.ConfigModule.Server;

using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Exceptions;

namespace TechnicalServices.HardwareEquipment.Util
{
    public abstract class HardwareEquipmentServerModule<TSourceType, TDeviceType, TDisplayType> : ServerModule
        where TSourceType : SourceType
        where TDeviceType : DeviceType
        where TDisplayType : DisplayType
    {
        #region override

        public override CommandDescriptor[] GetCommand(Slide slide1, Slide slide2, EquipmentType[] freezedEquipment)
        {
            List<CommandDescriptor> commandDescriptors = new List<CommandDescriptor>();
            commandDescriptors.AddRange(GetCommandForSource(slide1, slide2, freezedEquipment.Where(eq=>eq is SourceType).Cast<SourceType>().ToArray()));
            commandDescriptors.AddRange(GetCommandForDevice(slide1, slide2, freezedEquipment.Where(eq => eq is DeviceType).Cast<DeviceType>().ToArray()));
            commandDescriptors.AddRange(GetCommandForDisplay(slide1, slide2, freezedEquipment.Where(eq => eq is DisplayType).Cast<DisplayType>().ToArray()));

            return commandDescriptors.Distinct().ToArray();
        }

        #endregion

        #region private

        private CommandDescriptor[] GetCommandForSource(Slide slide1, Slide slide2, SourceType[] freezedSource)
        {
            // из конфигарации достанем интерисующие нас сорсы
            IEnumerable<SourceType> configSourceTypes = _config.ModuleConfiguration.SourceList.Where(
                st => st.GetType() == typeof(TSourceType) && st.IsHardware);
            if (0 == configSourceTypes.Count()) return new CommandDescriptor[] {};
            List<CommandDescriptor> commandDescriptors = new List<CommandDescriptor>(configSourceTypes.Count());

            // для каждого типа сорса - ищем пары сорсов в презентации
            foreach (SourceType sourceType in configSourceTypes)
            {
                // на прямое использование sourceType в linq ругается Решарпер
                SourceType st = sourceType;
                if (freezedSource.Any(freezed => freezed.Equals(st))) continue;
                Debug.WriteLine(String.Format("sourceType:{0}", sourceType.Name));
                Source source1 = null;
                Source source2 = null;
                if (slide1 != null)
                {
                    source1 = slide1.SourceList.Where(
                        s => s.Type.Name.Equals(st.Name, StringComparison.InvariantCultureIgnoreCase) &&
                             s.Type.Type.Equals(st.Type, StringComparison.InvariantCultureIgnoreCase) &&
                             s.Type.UID == st.UID).FirstOrDefault();
                }
                if (slide2 != null)
                {
                    source2 = slide2.SourceList.Where(
                        s => s.Type.Name.Equals(st.Name, StringComparison.InvariantCultureIgnoreCase) &&
                             s.Type.Type.Equals(st.Type, StringComparison.InvariantCultureIgnoreCase) &&
                             s.Type.UID == st.UID).FirstOrDefault();
                }
                commandDescriptors.AddRange(GetCommand(source1, source2));
            }
            return commandDescriptors.ToArray();
        }

        private CommandDescriptor[] GetCommandForDevice(Slide slide1, Slide slide2, DeviceType[] freezedDevice)
        {
            // из конфигарации достанем интерисующие нас девайсы
            IEnumerable<DeviceType> configDeviceTypes = _config.ModuleConfiguration.DeviceList.Where(
                dev => dev.GetType() == typeof(TDeviceType) && dev.IsHardware);
            if (0 == configDeviceTypes.Count()) return new CommandDescriptor[] {};
            List<CommandDescriptor> commandDescriptors = new List<CommandDescriptor>(configDeviceTypes.Count());
            // для каждого типа сорса - ищем пары сорсов в презентации
            foreach (DeviceType deviceType in configDeviceTypes)
            {
                // на прямое использование deviceType в linq ругается Решарпер
                DeviceType dt = deviceType;
                if (freezedDevice.Any(freezed => freezed.Equals(dt))) continue;
                Debug.WriteLine(String.Format("deviceType:{0}", deviceType.Name));
                Device device1 = null;
                Device device2 = null;
                if (slide1 != null)
                {
                    device1 = slide1.DeviceList.Where(
                        d => d.Type.Name.Equals(dt.Name, StringComparison.InvariantCultureIgnoreCase) &&
                             d.Type.Type.Equals(dt.Type, StringComparison.InvariantCultureIgnoreCase) &&
                             d.Type.UID == dt.UID).SingleOrDefault();
                }
                if (slide2 != null)
                {
                    device2 = slide2.DeviceList.Where(
                        d => d.Type.Name.Equals(dt.Name, StringComparison.InvariantCultureIgnoreCase) &&
                             d.Type.Type.Equals(dt.Type, StringComparison.InvariantCultureIgnoreCase) &&
                             d.Type.UID == dt.UID).SingleOrDefault();
                }
                commandDescriptors.AddRange(GetCommand(device1, device2));
            }
            return commandDescriptors.ToArray();
        }

        private CommandDescriptor[] GetCommandForDisplay(Slide slide1, Slide slide2, DisplayType[] freezedDisplay)
        {
            // из конфигарации достанем интерисующие нас дисплеи
            IEnumerable<DisplayType> configDisplayType = _config.ModuleConfiguration.DisplayList.Where(
                dis => dis.GetType() == typeof(TDisplayType) /* && dis.IsHardware*/);
            if (0 == configDisplayType.Count()) return new CommandDescriptor[] {};
            List<CommandDescriptor> commandDescriptors = new List<CommandDescriptor>(configDisplayType.Count());
            // для каждого типа сорса - ищем пары сорсов в презентации
            foreach (DisplayType displayType in configDisplayType)
            {
                // на прямое использование displayType в linq ругается Решарпер
                DisplayType dt = displayType;
                if (freezedDisplay.Any(freezed => freezed.Equals(dt))) continue;
                Debug.WriteLine(String.Format("displayType:{0}", displayType.Name));
                Display display1 = null;
                Display display2 = null;
                if (slide1 != null)
                {
                    display1 = slide1.DisplayList.Where(
                        d => d.Type.Name.Equals(dt.Name, StringComparison.InvariantCultureIgnoreCase) &&
                             d.Type.Type.Equals(dt.Type, StringComparison.InvariantCultureIgnoreCase) &&
                             d.Type.UID == dt.UID).SingleOrDefault();
                }
                if (slide2 != null)
                {
                    display2 = slide2.DisplayList.Where(
                        d => d.Type.Name.Equals(dt.Name, StringComparison.InvariantCultureIgnoreCase) &&
                             d.Type.Type.Equals(dt.Type, StringComparison.InvariantCultureIgnoreCase) &&
                             d.Type.UID == dt.UID).SingleOrDefault();
                }
                commandDescriptors.AddRange(GetCommand(display1, display2));
            }
            return commandDescriptors.ToArray();
        }


        /// <summary>
        /// возвращает комманды которые нужно пихнуть контроллеру - это дефолтовое поведение
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="equipment">состояние на предыдущем слайде</param>
        /// <returns></returns>
        private static CommandDescriptor[] GetCommand<T>(Equipment<T> equipment) where T : EquipmentType
        {
            if (equipment == null || equipment.Type == null || !equipment.Type.IsHardware)
                return new CommandDescriptor[] {};
            List<CommandDescriptor> commandDescriptors = new List<CommandDescriptor>(equipment.CommandList.Count);
            foreach (Command command in equipment.CommandList)
            {
                commandDescriptors.Add(new CommandDescriptor(equipment.Type.UID, command.command));
            }
            return commandDescriptors.ToArray();
        }

        #endregion

        #region protected

        /// <summary>
        /// команды которые необходимо кинуть контроллеру при переходе на след слайд.
        /// </summary>
        /// <param name="device1">предыдущее состояние</param>
        /// <param name="device2">текущее состояние</param>
        /// <returns></returns>
        protected virtual CommandDescriptor[] GetCommand(Device device1, Device device2)
        {
            return GetCommand(device2);
        }

        /// <summary>
        /// команды которые необходимо кинуть контроллеру при переходе на след слайд.
        /// </summary>
        /// <param name="source1">предыдущее состояние</param>
        /// <param name="source2">текущее состояние</param>
        /// <returns></returns>
        protected virtual CommandDescriptor[] GetCommand(Source source1, Source source2)
        {
            return GetCommand(source2);
        }

        /// <summary>
        /// команды которые необходимо кинуть контроллеру при переходе на след слайд.
        /// </summary>
        /// <param name="display1">предыдущее состояние</param>
        /// <param name="display2">текущее состояние</param>
        /// <returns></returns>
        protected virtual CommandDescriptor[] GetCommand(Display display1, Display display2)
        {
            return GetCommand(display2);
        }

        protected EquipmentCommand GetCommandByName(Device device, string commandName)
        {
            EquipmentCommand cmd = device.Type.CommandList.Find(delegate(EquipmentCommand item) { return item.Name.Equals(commandName); });
            if (cmd == null)
                throw new WrongHardwareCommand(string.Format("Wrong command name {0} for device {1}", commandName, device.Type.UID));
            return cmd;
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace TechnicalServices.Interfaces.ConfigModule.Player
{
    public interface IPlayerCommand
    {
        String DoSourceCommand(Source source, String command);
        String DoEquipmentCommand(CommandDescriptor cmd);
        bool? IsOnLine(EquipmentType equipmentType);
        bool IsShow { get;  }
        void FreezeEquipmentSetting(EquipmentType equipmentType, FreezeStatus status);
        FreezeStatus GetFreezedEquipment(EquipmentType equipmentType);
    }
}

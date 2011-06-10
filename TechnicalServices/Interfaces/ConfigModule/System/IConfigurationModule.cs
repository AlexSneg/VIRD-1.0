using System;

namespace TechnicalServices.Interfaces.ConfigModule.System
{
    public interface IConfigurationModule
    {
        Type[] GetDevice();
        Type[] GetDisplay();
        Type[] GetSource();
        Type[] GetMappingType();
        // На данный момент наследники ResourceInfo
        Type[] GetExtensionType();
    }
}
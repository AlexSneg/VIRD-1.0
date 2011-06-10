using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace TechnicalServices.Persistence.CommonPersistence.Presentation
{
    public interface IResourceProvider
    {
        /// <summary>
        /// получаем список ресурсов для данного типа сорса
        /// </summary>
        /// <param name="Type">тип</param>
        /// <param name="checkMapping">использовать маппинг для текущего дисплея или не использовать</param>
        /// <returns></returns>
        ResourceDescriptor[] GetResourcesByType(string Type, bool checkMapping);
        Device GetDeviceByName(DeviceType deviceType);
    }
}
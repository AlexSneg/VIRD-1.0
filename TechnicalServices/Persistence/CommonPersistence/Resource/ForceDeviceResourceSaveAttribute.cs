using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Persistence.CommonPersistence.Resource
{
    /// <summary>
    /// атрибут следует навешивать на свойства девайсов при изменеии которых следует принудительно сохранять DeviceResourceDescriptor, если он конечно есть у девайса
    /// в обычном случае DeviceResourceDescriptor сохраняется при сохранении изменений у слайда
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ForceDeviceResourceSaveAttribute : Attribute
    {
    }
}

using TechnicalServices.Interfaces;

namespace TechnicalServices.Configuration.Client
{
    public interface IClientConfiguration : IConfiguration
    {
        bool IsStandalone { get; }

        /// <summary>
        /// Позиции оборудования в списке.
        /// </summary>
        TechnicalServices.Entity.XmlSerializableDictionary<string, int> DevicePositions {get; set;}
    }
}
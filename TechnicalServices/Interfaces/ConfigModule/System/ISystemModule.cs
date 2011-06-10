namespace TechnicalServices.Interfaces.ConfigModule.System
{
    public interface ISystemModule
    {
        string Name { get; set; }
        IConfigurationModule Configuration { get; }
        IPresentationModule Presentation { get; }
    }
}
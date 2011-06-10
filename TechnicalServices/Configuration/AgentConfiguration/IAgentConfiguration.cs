using TechnicalServices.Interfaces;

namespace TechnicalServices.Configuration.Agent
{
    public interface IAgentConfiguration : IConfiguration
    {
        string AgentUID { get; }
        string Temp { get; }
        string RestoreImagePath { get; }
    }
}
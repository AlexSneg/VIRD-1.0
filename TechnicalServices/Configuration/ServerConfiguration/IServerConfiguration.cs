using System;

using TechnicalServices.Interfaces;

namespace TechnicalServices.Configuration.Server
{
    public interface IServerConfiguration : IConfiguration
    {
        UserStorageAdapter UserStorageAdapter { get; }
        string ControllerLibrary { get; }
        Uri ControllerURI { get; }
        int ControllerReceiveTimeout { get; }
        int ControllerCheckTimeout { get; }
        Uri ExternalSystemControllerUri { get; }
        string ExternalSystemControllerLibrary { get; }
        string[] GetConfigurationFiles();
        string[] GetPresentationSchemaFiles();
    }
}
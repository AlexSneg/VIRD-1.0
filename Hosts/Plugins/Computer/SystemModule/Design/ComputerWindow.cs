using System;
using System.Xml.Serialization;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.Computer.SystemModule.Design
{
    [Serializable]
    [XmlType("ComputerWindow")]
    public class ComputerWindow : ActiveWindow
    {
    }
}
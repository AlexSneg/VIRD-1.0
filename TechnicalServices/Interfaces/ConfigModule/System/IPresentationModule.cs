using System;
using System.Xml.Serialization;

namespace TechnicalServices.Interfaces.ConfigModule.System
{
    public interface IPresentationModule
    {
        Type[] GetDevice();
        Type[] GetDisplay();
        Type[] GetSource();
        Type[] GetWindow();

        // На данный момент не используется
        Type[] GetExtensionType();
    }
}
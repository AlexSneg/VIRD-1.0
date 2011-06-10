using System;

namespace TechnicalServices.Persistence.SystemPersistence.Configuration
{
    [Serializable]
    public abstract class DisplayTypeCapture : DisplayType
    {
        public override bool SupportsCaptureScreen
        {
            get { return true; }
        }
    }
}
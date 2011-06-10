using System;

namespace TechnicalServices.Exceptions
{
    public class WrongAgentDesktopResolution : ApplicationException
    {
        public WrongAgentDesktopResolution()
            : base("Текущее разрешение экрана не соответствует конфигурационным настройкам")
        {
        }
    }
}
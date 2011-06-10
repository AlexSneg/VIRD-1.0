using System;

namespace TechnicalServices.Exceptions
{
    public class WrongModuleVersion : ApplicationException
    {
        public WrongModuleVersion()
            : base("Версии сервера и клиента не совпадают.")
        {
        }
    }
}
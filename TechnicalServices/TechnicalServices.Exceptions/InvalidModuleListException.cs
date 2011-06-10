using System;

namespace TechnicalServices.Exceptions
{
    public class InvalidModuleListException : ApplicationException
    {
        public InvalidModuleListException()
            : base("Не удалось загрузить плагины." +
                Environment.NewLine +
                "Возможно нет установленых плагинов или " +
                Environment.NewLine +
                "в конфигурации указан не правильный путь к плагинам.")
        {
        }
    }
}
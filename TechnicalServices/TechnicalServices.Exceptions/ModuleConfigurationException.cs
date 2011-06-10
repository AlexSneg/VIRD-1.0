using System;

namespace TechnicalServices.Exceptions
{
    public class ModuleConfigurationException : ApplicationException
    {
        private const string FmtMessage = "Файл относящийся к конфигурации{1}\"{0}\"{1}не найден или поврежден,{1}система не может без него функционировать.{1}Необходимо задать текущую конфигурацию";
        public ModuleConfigurationException(string fileName)
            : base(String.Format(FmtMessage, fileName, Environment.NewLine))
        {

        }

        public ModuleConfigurationException(string fileName, Exception ex)
            : base(String.Format(FmtMessage, fileName, Environment.NewLine), ex)
        {

        }
    }

    public class ModuleConfigurationLoadException : ApplicationException
    {
        public ModuleConfigurationLoadException()
            : base("Не удалось загрузить конфигурацию системы")
        {

        }
    }
}
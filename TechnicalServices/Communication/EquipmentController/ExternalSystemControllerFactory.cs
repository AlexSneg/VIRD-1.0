using System;
using System.Reflection;

using TechnicalServices.Interfaces;

namespace TechnicalServices.Communication.EquipmentController
{
    public class ExternalSystemControllerFactory
    {
        public static IExternalSystemCommand CreateController(string fullClassName, Uri connectionString,
                                                              IEventLogging logging)
        {
            string[] list = fullClassName.Split(',');
            if (list.Length != 2)
                throw new ApplicationException(
                    "Неправильно указана библиотека для работы с контроллером внешней системы");
            Assembly asm = AppDomain.CurrentDomain.Load(list[0]);
            Type classType = asm.GetType(list[1]);
            IExternalSystemCommand result =
                (IExternalSystemCommand) Activator.CreateInstance(classType, connectionString, logging);
            return result;
        }
    }
}
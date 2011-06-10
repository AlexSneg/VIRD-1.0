using System;
using System.Reflection;

using TechnicalServices.Interfaces;

namespace TechnicalServices.Communication.EquipmentController
{
    public class ControllerFactory
    {
        public static IControllerChannel CreateController(IEventLogging logging, string fullClassName, Uri parameter, int receiveTimeout,
                                                          int checkTimeout)
        {
            string[] list = fullClassName.Split(',');
            if (list.Length != 2)
                throw new ApplicationException("Неправильно указана библиотека для работы с контроллером");
            Assembly asm = AppDomain.CurrentDomain.Load(list[0]);
            Type classType = asm.GetType(list[1]);
            IControllerChannel result =
                (IControllerChannel) Activator.CreateInstance(classType, logging, parameter, receiveTimeout, checkTimeout);
            return result;
        }
    }
}
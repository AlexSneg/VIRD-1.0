using System;
using System.Drawing;
using TechnicalServices.Exceptions;
using System.Text.RegularExpressions;

namespace TechnicalServices.Persistence.CommonPersistence.Configuration
{
    public class ValidationHelper
    {
        public static string CheckLength(string value, int length, string valueName)
        {
            if (value == null) return null;

            string result = value.Trim();
            if (result.Length > length)
                throw new InvalidParameterException(String.Format("Длина {0} не может превышать {1} символов", valueName,
                                                                  length));
            return result;
        }

        public static string CheckIsNullOrEmpty(string value, string valueName)
        {
            if (string.IsNullOrEmpty(value))
                throw new NullReferenceException(String.Format("Значение поля:{0}, не может быть пустым", valueName));
            return value;
        }

        /// <summary>
        /// Проверка на вхождение значения в диапазон.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <param name="min">Минимальное допустимое значение.</param>
        /// <param name="max">Максимальное допустимое значение.</param>
        /// <param name="valueName">Имя проверяемого параметра.</param>
        /// <returns>Проверенное значение.</returns>
        public static int CheckRange(int value, int min, int max, string valueName)
        {
            if(value<min || value>max)
                throw new InvalidParameterException(String.Format("Значение поля {0} должно быть в диапазоне от {1} до {2}.", valueName, min, max));
            return value;
        }

        public static decimal CheckRange(decimal value, decimal min, decimal max, string valueName)
        {
            if (value < min || value > max)
                throw new InvalidParameterException(String.Format("Значение поля {0} должно быть в диапазоне от {1} до {2}.", valueName, min, max));
            return value;
        }

        public static bool CheckODBCProcedureName(string name)
        {
            return !Regex.IsMatch(name, "[^a-zA-Z_0-9]");
        }

        public static Size CheckSize(Size value, Size maxSize, string valueName)
        {
            if (value.Width > maxSize.Width || value.Height > maxSize.Height)
                throw new InvalidParameterException(String.Format("Разрешение {0} должно быть не больше {1}x{2}.", valueName, maxSize.Width, maxSize.Height));
            return value;
        }
    }
}
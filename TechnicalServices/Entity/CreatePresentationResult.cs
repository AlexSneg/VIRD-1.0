using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Entity
{
    [Serializable]
    public enum CreatePresentationResult
    {
        /// <summary>
        /// презентация успешно создана
        /// </summary>
        Ok,
        /// <summary>
        /// существует презентация с таким же уникальным именем - это означает что попытка создать туже самую презентацию
        /// </summary>
        SameUniqueNameExists,
        /// <summary>
        /// Существует презентация с таким же именем. По требованиям имя презентации - должно быть уникально
        /// </summary>
        SameNameExists,
        /// <summary>
        /// использованные метки не найдены в системе
        /// </summary>
        LabelNotExists
    }
}

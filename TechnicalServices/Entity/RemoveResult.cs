using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Entity
{
    [Serializable]
    public enum RemoveResult
    {
        /// <summary>
        /// успешно удален
        /// </summary>
        Ok,
        /// <summary>
        /// сорса не существует
        /// </summary>
        NotExists,
        /// <summary>
        /// сорс заюзан в презентации
        /// </summary>
        LinkedToPresentation
    }
}

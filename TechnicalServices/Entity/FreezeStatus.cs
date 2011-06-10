using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Entity
{
    [Serializable]
    public enum FreezeStatus
    {
        /// <summary>
        /// заморозить состояние
        /// </summary>
        Freeze,
        /// <summary>
        /// разморозить состояние
        /// </summary>
        UnFreeze
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Entity
{
    [Serializable]
    public enum FileSaveStatus
    {
        /// <summary>
        /// Ok
        /// </summary>
        Ok,
        /// <summary>
        /// Resource Exists
        /// </summary>
        Exists,
        /// <summary>
        /// есть другой ресурс с таким же именем
        /// </summary>
        ExistsWithSameName,
        /// <summary>
        /// operation aborted
        /// </summary>
        Abort,
        /// <summary>
        /// загрузка в процессе и таким образом сейчас невозможна
        /// </summary>
        LoadInProgress
    }
}

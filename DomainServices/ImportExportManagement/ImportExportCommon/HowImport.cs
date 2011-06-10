using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainServices.ImportExportCommon
{
    /// <summary>
    /// как импортировать сценарий
    /// </summary>
    public enum HowImport
    {
        /// <summary>
        /// заменить
        /// </summary>
        Replace,
        /// <summary>
        /// создать новый
        /// </summary>
        New,
        /// <summary>
        /// отмена
        /// </summary>
        Cancel
    }
}

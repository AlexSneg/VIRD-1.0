using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TechnicalServices.Persistence.SystemPersistence.Presentation
{
    /// <summary>
    /// Интерфейс установки содержимого источника.
    /// Используется в БГ.
    /// </summary>
    public interface ISourceContentSize 
    {
        void SetContentSize(Size newSize);
    }
}

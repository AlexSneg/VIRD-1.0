using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Entity
{
    /// <summary>
    /// предполагается здесь складировать всякие полезные и не очень константы
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// для нетипизированных ресурсов - сейчас используется для backgroundImage
        /// </summary>
        public const string BackGroundImage = "BackGroundImage";

        /// <summary>
        /// размер части файла для передачи по wcf
        /// </summary>
        public const int PartSize = 1048576; // 1 метров

        /// <summary>
        /// идентификатор контроллера
        /// </summary>
        public const int ControllerUID = 0;
    }
}

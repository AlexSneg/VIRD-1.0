using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TechnicalServices.Interfaces;

namespace TechnicalServices.Interfaces
{
    [Serializable]
    public class SystemParameters : ISystemParameters
    {
        public bool IsDirty{ get; set; }

        //[DisplayName("SYSTEM_NAME")]
        //[Description("Название системы")]
        //public string SystemName { get; set; }

        [DisplayName("RELOAD_IMAGE")]
        [Description("Файл заставки восстановления")]
        public string ReloadImage { get; set; }
        
        [DisplayName("BACKGROUND_PRESENTATION")]
        [Description("Фоновая презентация")]
        public string BackgroundPresentationUniqueName { get; set; }
        
        [DisplayName("DEFAULT_WNDSIZE")]
        [Description("Размер окна по умолчанию")]
        public string DefaultWndsize { get; set; }

        [DisplayName("BGRD_PRESENTATION_TIMEOUT")]
        [Description("Таймаут запуска фонового сценария (сек)")]
        public int BackgroundPresentationRestoreTimeout {get; set;}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UI.PresentationDesign.DesignUI.Helpers
{
    public static class Description
    {
        public static String GetModeDescription(bool standalone)
        {
            return standalone ? "Автономный режим" : "Подключено к серверу";
        }

        public const string DisplayGroup = "Группа дисплеев ";
    }
}

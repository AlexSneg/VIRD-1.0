using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UI.PresentationDesign.DesignUI.Controls.Utils
{
    internal class ZoomComboHelper
    {
        public const int MaxValue = 500;
        public const int MinValue = 10;

        static List<ZoomComboHelper> lst = new List<ZoomComboHelper>();

        static ZoomComboHelper()
        {
            lst.Add(new ZoomComboHelper { Value = 10 });
            lst.Add(new ZoomComboHelper { Value = 20 });
            lst.Add(new ZoomComboHelper { Value = 50 });
            lst.Add(new ZoomComboHelper { Value = 100 });
            lst.Add(new ZoomComboHelper { Value = 150 });
            lst.Add(new ZoomComboHelper { Value = 200 });
            lst.Add(new ZoomComboHelper { Value = 300 });
            lst.Add(new ZoomComboHelper { Value = 500 });
        }

        public int Value
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Value + "%";
        }


        public static ZoomComboHelper[] CreateList()
        {

            return lst.ToArray();
        }

        public static int GetIndexForPercentage(int percent)
        {
            var l = lst.Find(z => z.Value == percent);
            if (l != null)
                return lst.IndexOf(l);
            return -1;
        }

        public static string GetTextForZoom(float m)
        {
            return String.Format("{0}%", Math.Round(m));
        }
    }
}

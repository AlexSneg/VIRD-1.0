using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Hosts.Plugins.BusinessGraphics.UI
{
    public partial class SeriesEditControl : UserControl
    {
        public SeriesEditControl()
        {
            InitializeComponent();
        }

        public SeriesEditControl(ICollection series, List<String> visible)
        {
            InitializeComponent();
            List<string> strs = series.OfType<String>().ToList();
            checkedListBox1.Items.AddRange(strs.ToArray());

            foreach (string s in visible)
            {
                checkedListBox1.SetItemChecked(strs.IndexOf(s), true);
            }
        }

        public String VisibleSeries
        {
            get
            {
                if (checkedListBox1.CheckedItems.Count > 0)
                    return checkedListBox1.CheckedItems.OfType<String>().Aggregate((a, b) => a + "," + b);
                else
                    return String.Empty;
            }


        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Tools;
using UI.PresentationDesign.DesignUI.Controls;
using System.Windows.Forms;

namespace UI.PresentationDesign.DesignUI.Helpers
{
    public class ToolStripTabEx : ToolStripEx
    {
        public event SwitchToNext OnSwitchToNext;
        public event SwitchToNext OnSwitchToPrev;

        protected override bool ProcessDialogKey(System.Windows.Forms.Keys keyData)
        {
            if (keyData == Keys.Tab && (Items.Count == 0 || Items[Items.Count - 1].Selected))
            {
                if (OnSwitchToNext != null)
                    OnSwitchToNext();

                return true;
            }
            else
            {
                //if ((keyData & Keys.Shift) == Keys.Shift & (keyData & Keys.Tab) == Keys.Tab)
                if ((int)keyData == 65545)
                {
                    if (Items.Count == 0 || Items[0].Selected)
                    {
                        if (OnSwitchToPrev != null)
                            OnSwitchToPrev();

                        return true;
                    }
                }
            }

            return base.ProcessDialogKey(keyData);
        }

        public void SelectFirstItem()
        {
            if (Items.Count > 0)
                Items[0].Select();
            this.Focus();
        }

        public void SelectLastItem()
        {
            if (Items.Count > 0)
                Items[Items.Count - 1].Select();
            this.Focus();
        }
    }
}

using System.Windows.Forms;

using UI.PresentationDesign.DesignUI.Controls;

namespace UI.PresentationDesign.DesignUI.Helpers
{
    public class GridViewTabEx : DataGridView
    {
        public event SwitchToNext OnSwitchToNext;
        public event SwitchToNext OnSwitchToPrev;

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Tab)
            {
                if (OnSwitchToNext != null)
                    OnSwitchToNext();

                return true;
            }
            else
            {
                //if ((keyData & Keys.Shift) == Keys.Shift & (keyData & Keys.Tab) == Keys.Tab)
                if ((int) keyData == 65545)
                {
                    if (OnSwitchToPrev != null)
                        OnSwitchToPrev();

                    return true;
                }
            }

            return base.ProcessDialogKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if ((e.KeyData & Keys.KeyCode) == Keys.Enter)
            {
                if (CurrentRow != null)
                    OnCellDoubleClick(new DataGridViewCellEventArgs(0, CurrentRow.Index));
                return;
            }
            base.OnKeyDown(e);
        }
    }
}
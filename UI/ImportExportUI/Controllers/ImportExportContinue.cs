using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DomainServices.ImportExportCommon;
using Syncfusion.Windows.Forms;
using UI.PresentationDesign.DesignUI.Helpers;

namespace UI.ImportExport.ImportExportUI.Controllers
{
    internal class ImportExportContinue : IContinue
    {
        private readonly bool _onlyOne;
        public ImportExportContinue(bool onlyOne)
        {
            _onlyOne = onlyOne;
        }
        #region Implementation of IContinue

        public bool Continue(string message)
        {
            if (_onlyOne)
            {
                MessageBoxExt.Show(message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true; //чтобы не было создано эксепшена и не выведено второе сообщение
            }
            else
            {
                return DialogResult.OK ==
                    MessageBoxExt.Show(message, "Внимание", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information, new string[] { "Продолжить", "Отмена" });
            }
        }

        #endregion

    }
}

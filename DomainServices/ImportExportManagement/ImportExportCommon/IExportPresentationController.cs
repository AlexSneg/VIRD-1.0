using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainServices.ImportExportCommon
{
    public interface IExportPresentationController
    {
        bool ConfirmExport(string directory, string filter, IEnumerable<string> presentationNames, out string newPresentationFileName);
        IContinue GetUserInteractive(bool onlyOnePresentation);
        void ErrorMessage(string errorMessage);
        void SuccessMessage(string message);
    }
}

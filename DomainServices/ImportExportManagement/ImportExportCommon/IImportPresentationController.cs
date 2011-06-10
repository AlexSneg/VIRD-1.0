using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainServices.ImportExportCommon
{
    public enum SameSourceAction
    {
        /// <summary>
        /// сделать копию
        /// </summary>
        Copy,
        /// <summary>
        /// заменить существующий
        /// </summary>
        Replace
    }


    public interface IImportPresentationController
    {
        void ErrorMessage(string errorMessage);
        void SuccessMessage(string successMessage);
        IContinue GetUserInterActive(bool onlyOnePresentation);
        bool ExistsSameSource(string message, out SameSourceAction action);
        HowImport HowToImportRequest(string message);
    }
}

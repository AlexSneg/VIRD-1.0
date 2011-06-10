using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainServices.ImportExportCommon
{
    public interface IExportSlideController
    {
        IContinue GetUserInterActive(bool onlyOneSlide);
        void ErrorMessage(string errorMessage);
        void SuccessMessage(string successMessage);
    }
}

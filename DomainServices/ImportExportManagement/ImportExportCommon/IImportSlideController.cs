using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace DomainServices.ImportExportCommon
{

    public interface IImportSlideController
    {
        void ErrorMessage(string errorMessage);
        void SuccessMessage(string successMessage);
        void AddSlide(Slide slide);
        void AddLink(Slide slideFrom, Slide slideTo);
        bool GetNewName(string message, out string newName);
        bool IsSlideUniqueName(string name, string exceptOne);
    }
}

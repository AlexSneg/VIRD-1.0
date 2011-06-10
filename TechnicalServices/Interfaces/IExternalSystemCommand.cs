using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Interfaces
{
    public interface IExternalSystemCommand : IDisposable
    {
        event Func<string, bool> OnGoToLabel;
        event Func<string, bool> OnGoToSlideById;
        event Func<bool> OnGoToNextSlide;
        event Func<bool> OnGoToPrevSlide;
    }
}

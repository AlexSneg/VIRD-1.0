using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace UI.PresentationDesign.DesignUI.Controllers.Interfaces
{
    public delegate void ImageChanged();

    /// <summary>
    /// Интерфейс для получения скриншота дисплея
    /// </summary>
    public interface IDisplayViewer
    {
        Display Display { get; }
        bool IsPresentationShow { get; }
        System.Drawing.Image getSceenshot();
        String Name { get; }
        System.Drawing.Rectangle Pos { get; set; }
        event ImageChanged OnImageLoaded;
        bool HasImage { get; set; }
        bool HasLayout { get; set; }
        bool IsTransformed { get; set; }

        event ImageChanged OnSourceSelected;
        System.Drawing.RectangleF? SelectedSource { get; set; }
        void NotifyUserClicked(float x, float y);
    }
}

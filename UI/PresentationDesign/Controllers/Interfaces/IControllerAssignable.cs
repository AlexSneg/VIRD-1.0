using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UI.PresentationDesign.DesignUI.Classes.Controller
{
    /// <summary>
    /// Интерфейс для элементов управления, которым можно назначать контроллеры
    /// </summary>
    public interface IControllerAssignable
    {
        /// <summary>
        /// Назначает контроллер сценария для элемента управления
        /// </summary>
        /// <param name="controller">Назначаемый контроллер</param>
        void AssignController(PresentationDiagramControllerBase controller);
    }
}

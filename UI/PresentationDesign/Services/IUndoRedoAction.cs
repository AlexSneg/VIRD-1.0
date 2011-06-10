using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Interfaces;

namespace UI.PresentationDesign.DesignUI.Services
{
    public interface IUndoRedoAction: ISimpleUndoRedoAction
    {
        string Tag { get; }
        object Target { get; }
    }
}

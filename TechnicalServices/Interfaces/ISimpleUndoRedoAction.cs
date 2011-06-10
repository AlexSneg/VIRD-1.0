using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Interfaces
{
    public interface ISimpleUndoRedoAction
    {
        void Undo();
        void Redo();

        bool CanUndo();
        bool CanRedo();
    }
}

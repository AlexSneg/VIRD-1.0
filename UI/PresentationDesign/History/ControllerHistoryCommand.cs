using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UI.PresentationDesign.DesignUI.Services;
using TechnicalServices.Interfaces;

namespace UI.PresentationDesign.DesignUI.History
{
    public class ControllerHistoryCommand : IUndoRedoAction
    {
        string descr;
        ISimpleUndoRedoAction actionController;

        public ControllerHistoryCommand(string descr, ISimpleUndoRedoAction actionController)
        {
            this.descr = descr;
            this.actionController = actionController;
        }

        #region IUndoRedoAction Members

        public void Undo()
        {
            if(actionController!=null && actionController.CanUndo())
                actionController.Undo();
        }

        public void Redo()
        {
            if(actionController!=null && actionController.CanRedo())
                actionController.Redo();
        }

        public string Tag
        {
            get { return descr; }
        }

        public object Target
        {
            get { return actionController; }
        }

        #endregion

        #region ISimpleUndoRedoAction Members


        public bool CanUndo()
        {
            return actionController != null && actionController.CanUndo();
        }

        public bool CanRedo()
        {
            return actionController != null && actionController.CanRedo();
        }

        #endregion
    }
}

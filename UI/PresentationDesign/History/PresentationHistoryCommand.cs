using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace UI.PresentationDesign.DesignUI.Classes.History
{
    public abstract class PresentationHistoryCommand
    {
        protected Presentation Presentation;

        public PresentationHistoryCommand(Presentation APresentation)
        {
            Presentation = APresentation;
        }

        public abstract void Undo();
        public abstract void Redo();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System.ComponentModel;
using UI.PresentationDesign.DesignUI.Classes.View;

namespace UI.PresentationDesign.DesignUI.Classes.Model
{
    public class PresentationModel: Syncfusion.Windows.Forms.Diagram.Model
    {
        Presentation m_Presentation;

        public PresentationModel(Presentation APresentation, IContainer container)
            : base(container)
        {
            m_Presentation = APresentation;
        }
    }
}

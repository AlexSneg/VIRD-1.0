using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Syncfusion.Windows.Forms.Tools;
using UI.PresentationDesign.DesignUI.Controllers;

namespace UI.PresentationDesign.DesignUI.Controls.DisplayList
{
    public class DisplayNode : TreeNodeAdv
    {
        Display _display;
        public Display Display
        {
            get
            {
                return DisplayController.Instance.FindDisplay(_display);
            }

            set
            {
                _display = value;
            }
        }

        public DisplayNode(Display ADisplay)
            : base(ADisplay.Type.Name)
        {
            _display = ADisplay;
            OpenImgIndex = 1;
            NoChildrenImgIndex = 0;
            EnabledButtons = !(Display is PassiveDisplay);
        }
    }
}

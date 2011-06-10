using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Syncfusion.Windows.Forms.Tools;
using UI.PresentationDesign.DesignUI.Controllers;

namespace UI.PresentationDesign.DesignUI.Controls.DisplayList
{
    public class DisplayGroupNode : TreeNodeAdv
    {
        public List<Display> DisplayList
        {
            get
            {
                return DisplayController.Instance.DisplayByGroup(DisplayGroup);
            }
        }

        public DisplayGroup DisplayGroup { get; set; }

        public DisplayGroupNode(DisplayGroup ADisplayGroup)
            : base(ADisplayGroup.Name)
        {
            this.DisplayGroup = ADisplayGroup;

            OpenImgIndex = 1;
            NoChildrenImgIndex = 0;
            EnabledButtons = !(DisplayList.FirstOrDefault() is PassiveDisplay);
        }
    }
}

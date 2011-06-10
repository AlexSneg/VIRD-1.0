using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System.Drawing;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Common.Utils;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace UI.PresentationDesign.DesignUI.Controls.SourceTree
{
    public interface ISourceNode: ICloneable
    {
        bool CreateSourceWindow(Identity id, Display display);

        SourceType SourceType
        {
            get;
            set;
        }

        Window Window
        {
            get;
            set;
        }

        ResourceDescriptor Mapping
        {
            get;
            set;
        }

        Image Image
        {
            get;
            set;
        }

        string Name
        {
            get;
            set;
        }

        bool? IsOnline
        {
            get;
            set;
        }
    }
}

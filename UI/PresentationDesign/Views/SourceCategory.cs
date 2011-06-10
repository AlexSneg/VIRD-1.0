using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using TechnicalServices.Persistence.CommonPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace UI.PresentationDesign.DesignUI.Controls.SourceTree
{
    public class SourceCategory
    {
        SourceType _type;
        bool _isGlobal;

        public SourceCategory(SourceType type, bool IsGlobal)
        {
            Resources = new List<ISourceNode>();
            _type = type;
            _isGlobal = IsGlobal;
        }

        public bool Global
        {
            get { return _isGlobal; }
        }

        public string Comment
        {
            get { return _type.Comment; }
        }


        public Icon Icon
        {
            get;
            set;
        }

        public string Name
        {
            get { return _type.Name; }
        }

        public SourceType Type
        {
            get
            {
                return _type;
            }
        }

        public List<ISourceNode> Resources
        {
            get;
            set;
        }

        public bool IsHardware
        {
            get { return Type.IsHardware; }
        }

        public bool IsSupportPreview
        {
            get { return Type.IsSupportPreview; }
        }

    }
}

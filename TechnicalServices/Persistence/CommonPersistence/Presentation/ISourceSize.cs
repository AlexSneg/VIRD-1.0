using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TechnicalServices.Persistence.SystemPersistence.Presentation
{
    public interface ISourceSize : IAspectLock
    {
        int Width { get; }
        int Height { get; }
        void SetSize(Size newSize);
    }

    public interface IAspectLock
    {
        bool AspectLock { get; set; }
    }
}

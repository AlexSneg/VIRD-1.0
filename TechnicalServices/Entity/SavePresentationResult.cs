using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Entity
{
    [Flags]
    public enum SavePresentationResult
    {
        Ok = 0,
        LabelNotExists = 1,
        PresentationNotLocked = 2,
        PresentationNotExists = 4,
        ResourceNotExists = 8,
        SlideLocked = 16,
        SlideAlreadyExists = 32,
        Unknown = 64
    }
}

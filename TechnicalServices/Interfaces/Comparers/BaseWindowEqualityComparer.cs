using System.Collections.Generic;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace TechnicalServices.Interfaces.Comparers
{
    public class BaseWindowEqualityComparer : IEqualityComparer<Window>
    {
        public virtual bool Equals(Window x, Window y)
        {
            if (x == y) return true;
            return (x.Height == y.Height && x.Left == y.Left && x.Top == y.Top && x.Width == y.Width);
        }

        public virtual int GetHashCode(Window window)
        {
            if (window.Source == null || window.Source.Type == null) return 0;
            return window.Source.Type.Type.GetHashCode();
        }
    }

    public class ActiveWindowEqualityComparer : BaseWindowEqualityComparer, IEqualityComparer<ActiveWindow>
    {
        public bool Equals(ActiveWindow x, ActiveWindow y)
        {
            return base.Equals(x, y) &&
                x.CroppingLeft == y.CroppingLeft && x.CroppingRight == y.CroppingRight && x.CroppingTop == y.CroppingTop && x.CroppingBottom == y.CroppingBottom /*&&
                x.HOffset == y.HOffset && x.HSyncNeg == y.HSyncNeg && x.HTotal == y.HTotal && x.HWidth == y.HWidth &&
                x.Phase == y.Phase && x.VFreq == y.VFreq && x.VHeight == y.VHeight && x.VOffset == y.VOffset && x.VSyncNeg == y.VSyncNeg &&
                x.VTotal == y.VTotal*/;
        }

        public override bool Equals(Window x, Window y)
        {
            if ((x is ActiveWindow) && (y is ActiveWindow))
                return Equals((ActiveWindow)x, (ActiveWindow)y);
            return false;
        }

        public int GetHashCode(ActiveWindow obj)
        {
            return base.GetHashCode(obj);
        }
    }
}
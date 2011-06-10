using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Entity
{
    public class ValuePair<T1, T2> : IEquatable<ValuePair<T1, T2>>
    {
        public T1 Value1;
        public T2 Value2;
        public ValuePair(T1 value1, T2 value2)
        {
            Value1 = value1;
            Value2 = value2;
        }
        public bool Equals(ValuePair<T1, T2> other)
        {
            if (other == null) return false;
            if (this.Value1.Equals(other.Value1) && this.Value2.Equals(other.Value2))
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            long tmp = ((long)Value1.GetHashCode() << 32) + (long)Value2.GetHashCode();
            return tmp.GetHashCode();
        }
    }
}

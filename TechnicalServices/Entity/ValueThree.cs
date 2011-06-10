using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Entity
{
    public class ValueThree<T1, T2, T3> : IEquatable<ValueThree<T1, T2, T3>>
    {
        public T1 Value1;
        public T2 Value2;
        public T3 Value3;

        public ValueThree(T1 value1, T2 value2, T3 value3)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }
        public bool Equals(ValueThree<T1, T2, T3> other)
        {
            if (other == null) return false;
            if (this.Value1.Equals(other.Value1) && 
                this.Value2.Equals(other.Value2) &&
                this.Value3.Equals(other.Value3))
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            decimal tmp =
                (decimal)Value1.GetHashCode() * uint.MaxValue * uint.MaxValue +
                (decimal)Value2.GetHashCode() * uint.MaxValue +
                (decimal)Value3.GetHashCode();
            return tmp.GetHashCode();
        }
    }
}

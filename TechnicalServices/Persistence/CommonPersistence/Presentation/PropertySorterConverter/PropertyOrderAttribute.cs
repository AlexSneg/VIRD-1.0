using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Persistence.CommonPersistence.Presentation.PropertySorterConverter
{
    /// <summary>
    /// Атрибут для задания сортировки
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyOrderAttribute : Attribute
    {
        private int _order;
        public PropertyOrderAttribute(int order)
        {
            _order = order;
        }

        public int Order
        {
            get { return _order; }
        }
    }

}

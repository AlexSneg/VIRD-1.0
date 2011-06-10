using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Persistence.CommonPersistence.Presentation
{
    /// <summary>
    /// Атрибут для задания заголовка формы редактирования коллекции.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CollectionFormNameAttribute : Attribute
    {
        public CollectionFormNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}

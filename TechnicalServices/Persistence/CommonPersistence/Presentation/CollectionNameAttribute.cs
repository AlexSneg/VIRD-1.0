using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Persistence.CommonPersistence.Presentation
{
    /// <summary>
    /// Атрибут для задания имени коллекции.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CollectionNameAttribute : Attribute
    {
        public CollectionNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}

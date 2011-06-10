using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Persistence.CommonPersistence.Presentation
{
    /// <summary>
    /// Атрибут для задания заголовка свойств.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertiesNameAttribute : Attribute
    {
        public PropertiesNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}

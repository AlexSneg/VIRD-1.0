using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Persistence.SystemPersistence.Resource
{
    /// <summary>
    /// Аттрибут должен навешиваться на каждое свойство класса ResourceFileInfo - суть полное имя к файловому ресурсу
    /// кол-во аттрибутов в классе будет означать количество поддерживаемых файловых ресурсов
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ResourceFileAttribute : Attribute
    {
        private readonly string _id;
        private readonly bool _master;
        private readonly bool _required;
        /// <summary>
        /// Имя уникально внутри данного ResourceFileInfo и оно будет использховаться в качетсве расширения у файлового ресурса - поэтому запрещено использовать имена которые могут быть расширениями данного источника - на совести разработчика!!!!
        /// </summary>
        /// <param name="id">Имя, для одного ресурса по умолчанию resource</param>
        /// <param name="master">данный файл определяет имя всего ресурса. один мастерный ресурс должен быть обязательно!</param>
        /// <param name="required">данный файловый ресурс необходим</param>
        public ResourceFileAttribute(string id, bool master, bool required)
        {
            _id = id;
            _master = master;
            _required = required;
        }

        public string Id { get { return _id; } }
        public bool Master { get { return _master; } }
        public bool Required { get { return _required; } }
    }
}

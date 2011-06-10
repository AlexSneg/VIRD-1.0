using System;
using System.Diagnostics;
using TechnicalServices.Interfaces.ConfigModule;

namespace DomainServices.EnvironmentConfiguration.ConfigModule
{
    public sealed class ModuleAttribute : Attribute
    {
        private readonly string _name;
        private readonly Type _type;

        public ModuleAttribute(string name, Type moduleType)
        {
            _name = name;
            _type = moduleType;
            InterfaceCheck();
        }

        public string Name
        {
            get { return _name; }
        }

        public Type ModuleType
        {
            get { return _type; }
        }

        [Conditional("DEBUG")]
        private void InterfaceCheck()
        {
            Debug.Assert(_type != null, "moduleType не может быть пустым");

            foreach (Type item in _type.GetInterfaces())
            {
                if (typeof (IModule).Equals(item)) return;
            }
            Debug.Fail("Не верный тип передан в атрибут moduleType" +
                       Environment.NewLine +
                       _type.Assembly.FullName +
                       Environment.NewLine +
                       _type.Assembly.CodeBase);
        }
    }
}
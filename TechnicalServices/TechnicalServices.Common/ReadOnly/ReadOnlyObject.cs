using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections;

using TechnicalServices.Common.Editor;
using TechnicalServices.Common.TypeConverters;
using TechnicalServices.Interfaces;
using System.Reflection;

namespace TechnicalServices.Common.ReadOnly
{
    public class ReadOnlyObject : ICustomTypeDescriptor, IReadOnlyWrapper
    {
        // Fields
        private ICustomTypeDescriptor customWrapped;
        private object wrapped;

        public object Wrapped
        {
            get { return wrapped; }
            set { wrapped = value; }
        }

        // Methods
        public ReadOnlyObject(object objectToWrap)
        {
            this.wrapped = RuntimeHelpers.GetObjectValue(objectToWrap);
            if (this.wrapped is ICustomTypeDescriptor)
            {
                this.customWrapped = (ICustomTypeDescriptor)this.wrapped;
            }
        }

        public override string ToString()
        {
            return Wrapped.ToString();
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(RuntimeHelpers.GetObjectValue(this.wrapped), false);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(RuntimeHelpers.GetObjectValue(this.wrapped), false);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(RuntimeHelpers.GetObjectValue(this.wrapped), false);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(RuntimeHelpers.GetObjectValue(this.wrapped), false);
            //return new ReadOnlyConverter(this);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(RuntimeHelpers.GetObjectValue(this.wrapped), false);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            PropertyDescriptor defaultProperty = TypeDescriptor.GetDefaultProperty(RuntimeHelpers.GetObjectValue(this.wrapped), false);
            if (defaultProperty == null)
            {
                return null;
            }
            return this.WrapProperty(defaultProperty);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(RuntimeHelpers.GetObjectValue(this.wrapped), editorBaseType, false);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(RuntimeHelpers.GetObjectValue(this.wrapped), false);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(RuntimeHelpers.GetObjectValue(this.wrapped), attributes, false);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return this.WrapProperties(TypeDescriptor.GetProperties(RuntimeHelpers.GetObjectValue(this.wrapped), false));
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return this.WrapProperties(TypeDescriptor.GetProperties(RuntimeHelpers.GetObjectValue(this.wrapped), attributes, false));
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        private PropertyDescriptorCollection WrapProperties(PropertyDescriptorCollection props)
        {
            IEnumerator e = null;
            PropertyDescriptorCollection newProps = new PropertyDescriptorCollection(null);
            try
            {
                e = props.GetEnumerator();
                while (e.MoveNext())
                {
                    PropertyDescriptor prop = (PropertyDescriptor)e.Current;
                    newProps.Add(this.WrapProperty(prop));
                }
            }
            finally
            {
                if (e is IDisposable)
                {
                    (e as IDisposable).Dispose();
                }
            }
            return newProps;
        }

        private PropertyDescriptor WrapProperty(PropertyDescriptor prop)
        {
            return new ReadOnlyPropertyDescriptor(this, prop);
        }

        // Nested Types
        private class ReadOnlyConverter : TypeConverter
        {
            // Fields
            private ReadOnlyObject parent;

            // Methods
            public ReadOnlyConverter(ReadOnlyObject parent)
            {
                this.parent = parent;
            }

            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                return this.parent.GetProperties(attributes);
            }
        }

        private class ReadOnlyPropertyDescriptor : PropertyDescriptor
        {
            // Fields
            private ReadOnlyObject parent;
            private PropertyDescriptor prop;

            // Methods
            public ReadOnlyPropertyDescriptor(ReadOnlyObject parent, PropertyDescriptor propToWrap)
                : base(GetPropName(propToWrap), GetPropAttributes(propToWrap))
            {
                if (parent == null)
                {
                    throw new ArgumentNullException("parent");
                }
                this.parent = parent;
                this.prop = propToWrap;
            }

            public override bool CanResetValue(object component)
            {
                return false;
            }

            private static Attribute[] GetPropAttributes(PropertyDescriptor prop)
            {
                Attribute[] attributes = new Attribute[(prop.Attributes.Count - 1) + 1];
                prop.Attributes.CopyTo(attributes, 0);
                return attributes;
            }

            private static string GetPropName(PropertyDescriptor prop)
            {
                if (prop == null)
                {
                    throw new ArgumentNullException("propToWrap");
                }
                return prop.Name;
            }

            public override object GetValue(object component)
            {
                object value;
                if (component is ReadOnlyObject)
                    value = prop.GetValue(RuntimeHelpers.GetObjectValue(((ReadOnlyObject)component).Wrapped));
                else
                    value = prop.GetValue(RuntimeHelpers.GetObjectValue(component));

                //TypeConverterAttribute converter = (TypeConverterAttribute)prop.Attributes[typeof(TypeConverterAttribute)];
                //if (converter.ConverterTypeName == typeof(ExpandableObjectConverter).AssemblyQualifiedName)
                //    return new ReadOnlyObject(value);

                if (value != null && (!(value is ReadOnlyObject)) && prop.Converter is ExpandableObjectConverter)
                    return new ReadOnlyObject(value);

                if ((value is IList) && (!(value is ReadOnlyCollection)))
                    return new ReadOnlyCollection((IList)value);

                return value;
            }

            public override void ResetValue(object component)
            {
                throw new NotSupportedException("This property is read-only");
            }

            public override void SetValue(object component, object value)
            {
                throw new NotSupportedException("This property is read-only");
            }

            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }

            // Properties
            public override Type ComponentType
            {
                get
                {
                    //return typeof(ReadOnlyObject);
                    return this.prop.GetType();
                }
            }

            public override TypeConverter Converter
            {
                get
                {
                    if (PropertyType.GetInterface("IList") != null)
                        return new CollectionNameConverter();
                    //return this.parent.GetConverter();
                    return this.prop.Converter;
                }
            }

            public override object GetEditor(Type editorBaseType)
            {
                if (PropertyType.GetInterface("IList") != null)
                {
                    AttributeCollection list = Attributes;
                    EditorReadonlyAttribute attibute = list[typeof(EditorReadonlyAttribute)] as EditorReadonlyAttribute;
                    if (attibute != null)
                    {
                        UITypeEditor editor =(UITypeEditor )Activator.CreateInstance(attibute.EditorType);
                        return editor;
                    }
                    return new ReadOnlyCollectionEditor(typeof (ReadOnlyCollection));
                }
                return base.GetEditor(editorBaseType);
            }

            //public override AttributeCollection Attributes
            //{
            //    get
            //    {
            //        return base.Attributes;
            //    }
            //}

            public override bool IsReadOnly
            {
                get
                {
                    return true;
                }
            }

            public override Type PropertyType
            {
                get
                {
                    return this.prop.PropertyType;
                }
            }
        }
    }

    public class ReadOnlyPropertyDescriptor : PropertyDescriptor
    {
        // Fields
        private ReadOnlyObject parent;
        private PropertyDescriptor prop;

        // Methods
        public ReadOnlyPropertyDescriptor(ReadOnlyObject parent, PropertyDescriptor propToWrap)
            : base(GetPropName(propToWrap), GetPropAttributes(propToWrap))
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            this.parent = parent;
            this.prop = propToWrap;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        private static Attribute[] GetPropAttributes(PropertyDescriptor prop)
        {
            Attribute[] attributes = new Attribute[(prop.Attributes.Count - 1) + 1];
            prop.Attributes.CopyTo(attributes, 0);
            return attributes;
        }

        private static string GetPropName(PropertyDescriptor prop)
        {
            if (prop == null)
            {
                throw new ArgumentNullException("propToWrap");
            }
            return prop.Name;
        }

        public override object GetValue(object component)
        {
            return this.prop.GetValue(RuntimeHelpers.GetObjectValue(((ReadOnlyObject)component).Wrapped));
        }

        public override void ResetValue(object component)
        {
            throw new NotSupportedException("This property is read-only");
        }

        public override void SetValue(object component, object value)
        {
            throw new NotSupportedException("This property is read-only");
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        // Properties
        public override Type ComponentType
        {
            get
            {
                return typeof(ReadOnlyObject);
            }
        }

        public override TypeConverter Converter
        {
            get
            {
                return this.parent.GetConverter();
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public override Type PropertyType
        {
            get
            {
                return this.prop.PropertyType;
            }
        }
    }

    [TypeConverter(typeof(CollectionNameConverter))]
    [Editor(typeof(ReadOnlyCollectionEditor), typeof(UITypeEditor))]
    public class ReadOnlyCollection : List<ReadOnlyObject>
    {
        public ReadOnlyCollection(IList sourceList)
        {
            foreach (object item in sourceList)
                Add(new ReadOnlyObject(item));
        }
    }
}

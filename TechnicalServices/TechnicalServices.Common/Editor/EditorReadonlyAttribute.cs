using System;

namespace TechnicalServices.Common.Editor
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class EditorReadonlyAttribute : Attribute
    {
        private readonly Type _editorType;

        public EditorReadonlyAttribute(Type editorType)
        {
            _editorType = editorType;
        }

        public Type EditorType
        {
            get { return _editorType; }
        }
    }
}
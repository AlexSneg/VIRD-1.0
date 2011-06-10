using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TechnicalServices.Common.Editor
{
    public abstract class ReadOnlyCollectionEditorAdv<T> : CollectionEditorAdv<T>
    {
        protected ReadOnlyCollectionEditorAdv(Type type) : base(type)
        {
        }

        protected override CollectionForm CreateCollectionForm()
        {
            CollectionForm collectionForm = base.CreateCollectionForm();
            collectionForm.HelpButton = false;
            PropertyInfo pi = collectionForm.GetType().BaseType.
                GetProperty("CollectionEditable", BindingFlags.NonPublic | BindingFlags.Instance);
            if (pi != null)
                pi.SetValue(collectionForm, false, null);

            return collectionForm;
        }

        protected override List<T> CreateCopy(System.ComponentModel.ITypeDescriptorContext context,
            List<T> list)
        {
            return new List<T>(list);
        }

    }
}

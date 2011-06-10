using System;
using System.ComponentModel.Design;
using System.Reflection;
using System.Windows.Forms;

namespace TechnicalServices.Common.ReadOnly
{
    public class ReadOnlyCollectionEditor : CorrectNameCollectionEditor
    {
        public ReadOnlyCollectionEditor(Type type) : base(type)
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


    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TechnicalServices.Common.Editor
{
    public abstract class ClonableObjectCollectionEditorAdv<T> : CollectionEditorAdv<T>
        where T : ICloneable
    {
        protected ClonableObjectCollectionEditorAdv(Type type) : base(type)
        {
        }

        #region Overrides of CollectionEditorAdv<T>

        protected override List<T> CreateCopy(ITypeDescriptorContext context, List<T> list)
        {
            return new List<T>(list.Select(el=>(T)el.Clone()));
        }

        #endregion
    }
}

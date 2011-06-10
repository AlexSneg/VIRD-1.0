using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace TechnicalServices.Common.Editor
{
    /// <summary>
    /// 
    /// </summary>
    public class LabelCollectionEditor : ClonableObjectCollectionEditorAdv<Label>
    {
        public LabelCollectionEditor(Type type) : base(type)
        {
        }

        protected override object CreateInstance(Type itemType)
        {
            ModuleConfiguration conf = (ModuleConfiguration) Context.Instance;
            Label lbl = (Label) base.CreateInstance(itemType);
            lbl.IsSystem = true;
            if (conf.LabelList.Count == 0)
                lbl.Id = 1;
            else
                lbl.Id = conf.LabelList.Max(l => l.Id) + 1;
            lbl.Name += lbl.Id;
            return lbl;
        }
    }
}
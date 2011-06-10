using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
using TechnicalServices.Common.Editor;
using System.ComponentModel;
using System.Windows.Forms;

namespace Hosts.Plugins.BusinessGraphics.SystemModule.Design
{
    public class RangeCollectionEditor : ClonableObjectCollectionEditorAdv<ValueRange>
    {
        public RangeCollectionEditor(Type type)
            : base(type)
        {
        }
        protected override object CreateInstance(Type itemType)
        {
            ValueRange res = new ValueRange();
            BusinessGraphicsSourceDesign design = (this.Context.Instance as BusinessGraphicsSourceDesign);
            if ((design != null) && (design.ValueRanges.Count > 0))
            {
                float max = design.ValueRanges.Max(d => d.MaxValue);
                res.MinValue = max + 1;
                res.MaxValue = max + 11;
            }
            return res;
        }

        protected override object SetItems(object editValue, object[] value)
        {
            object o = base.SetItems(editValue, value);
            
            BusinessGraphicsSourceDesign design = (this.Context.Instance as BusinessGraphicsSourceDesign);
            
            if(design!=null)
                design.InitializeChart(true);

            return o;
        }
        protected override CollectionEditor.CollectionForm CreateCollectionForm()
        {
            CollectionEditor.CollectionForm frm = base.CreateCollectionForm();
            frm.ControlBox = false;
            return frm;
        }
    }
}

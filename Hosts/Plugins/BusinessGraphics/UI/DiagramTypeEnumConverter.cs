using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using TechnicalServices.Common.TypeConverters;
using Hosts.Plugins.BusinessGraphics.SystemModule.Design;

namespace Hosts.Plugins.BusinessGraphics.UI
{
    public class DiagramTypeEnumConverter : CommonEnumConverter
    {
        public DiagramTypeEnumConverter(Type type)
            : base(type)
        {

        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }


        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            bool detailed = ((BusinessGraphicsSourceDesign)context.Instance).AllowSubDiagram;
            List<DiagramTypeEnum> enums = new List<DiagramTypeEnum>();
            if (!detailed)
            {
                return new StandardValuesCollection(Enum.GetValues(typeof(DiagramTypeEnum)));
            }
            else
            {
                enums.AddRange(new[] { DiagramTypeEnum.Map, DiagramTypeEnum.PieGeneral, DiagramTypeEnum.ColumnGeneral });
            }

            return new StandardValuesCollection(enums);
        }
    }
}

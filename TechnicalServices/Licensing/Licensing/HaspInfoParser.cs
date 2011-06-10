using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace TechnicalServices.Licensing
{
    internal class HaspInfoParser
    {
        internal static bool IsFeatureAvailable(int featureId, string haspInfo)
        {
            XElement root = XElement.Parse(haspInfo);

            IEnumerable<XElement> feature =
                from el in root.Elements("feature")
                where (int)el.Attribute("id") == featureId &&
                    !(bool)el.Attribute("disabled") &&
                    (bool)el.Attribute("usable")
                select el;

            return (feature.Count<XElement>() == 1);
        }
    }
}

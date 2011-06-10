using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Hosts.Plugins.BusinessGraphics.SystemModule.Design.Helpers
{
    /// <summary>с помощью этого класса мы преобразуем xml в набор точек, серий и их значений </summary>
    internal static class DataChartXmlReader
    {
        internal static void ReadDataChartFromXml(XmlTextReader source, 
            out Dictionary<string, float> series,
            out Dictionary<string, float> points,
            out Dictionary<Intersection, float> intersections,
            ref string defaultSeries)
        {
            series = new Dictionary<string, float>();
            points = new Dictionary<string, float>();
            intersections = new Dictionary<Intersection, float>();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.None;

            XmlDocument doc = new XmlDocument();
            doc.Load(source);
            try
            {
                // SQL возвращает root в начале документа, а в XML файле вначале идет тег
                // <?xml version="1.0" encoding="utf-8"?>, поэтому ищем root
                XmlNodeList list = doc.GetElementsByTagName("root");
                if (list.Count == 1)
                {
                    XmlNode root = list[0];

                    if (root.ChildNodes.Count > 0)
                    {
                        foreach (XmlNode metaNode in root.ChildNodes)
                        {
                            switch (metaNode.Name)
                            {
                                case "SeriesDefinitions":
                                    foreach (XmlNode n in metaNode.ChildNodes)
                                    {
                                        string name = n.Attributes[0].Value;

                                        if (string.IsNullOrEmpty(n.Attributes[1].Value))
                                            n.Attributes[1].Value = "0";

                                        float val = float.Parse(n.Attributes[1].Value);
                                        if (!series.ContainsKey(name))
                                        {
                                            series.Add(name, val);
                                        }
                                        else
                                        {
                                            series[name] = val;
                                        }
                                    }
                                    if ((defaultSeries == String.Empty) || !(series.ContainsKey(defaultSeries)))
                                        defaultSeries = series.Keys.FirstOrDefault();
                                    break;
                                case "PointsDefinitions":
                                    foreach (XmlNode n in metaNode.ChildNodes)
                                    {
                                        string name = n.Attributes[0].Value;

                                        if (string.IsNullOrEmpty(n.Attributes[1].Value))
                                            n.Attributes[1].Value = "0";

                                        float val = float.Parse(n.Attributes[1].Value);
                                        if (!points.ContainsKey(name))
                                        {
                                            points.Add(name, val);
                                        }
                                        else
                                        {
                                            points[name] = val;
                                        }
                                    }
                                    break;
                                case "DataIntersections":
                                    foreach (XmlNode n in metaNode.ChildNodes)
                                    {
                                        string name1 = n.Attributes[0].Value;
                                        string name2 = n.Attributes[1].Value;

                                        if (string.IsNullOrEmpty(n.Attributes[2].Value))
                                            n.Attributes[2].Value = "0";

                                        float val = float.Parse(n.Attributes[2].Value);

                                        Intersection i = new Intersection(name1, name2);
                                        if (!intersections.ContainsKey(i))
                                        {
                                            intersections.Add(i, val);
                                        }
                                        else
                                        {
                                            intersections[i] = val;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            finally
            {
                source.Close();
            }
        }
    }
}

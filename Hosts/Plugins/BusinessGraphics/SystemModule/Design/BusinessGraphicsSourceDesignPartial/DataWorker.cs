using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotnetCHARTING.WinForms;

namespace Hosts.Plugins.BusinessGraphics.SystemModule.Design
{
    public partial class BusinessGraphicsSourceDesign
    {
        /// <summary>метод который формирует данные для chart-а на основе колекции серий, 
        /// поинтов и их пересечений </summary>
        private SeriesCollection FillData(
            Dictionary<string, float> seriesDef, 
            Dictionary<string, float> pointDef,
            Dictionary<Intersection, float> intersections)
        {
            SeriesCollection result = new SeriesCollection();
            if (!subDiagramMode)
            {
                switch (this.DiagramType)
                {
                    case DiagramTypeEnum.Graph:
                        result.Add(CreateDataForFullDiagram(seriesDef, pointDef, intersections, true));
                        break;
                    case DiagramTypeEnum.ColumnDetail:
                        result.Add(CreateDataForFullDiagram(seriesDef, pointDef, intersections, false));
                        break;
                    case DiagramTypeEnum.ColumnGeneral:
                        result.Add(CreateDataForColumnGeneral(seriesDef));
                        break;
                    case DiagramTypeEnum.PieGeneral:
                        result.Add(CreateDataForPieGeneral(seriesDef));
                        break;
                    case DiagramTypeEnum.Speedometer:
                    case DiagramTypeEnum.TrafficLight:
                        if (seriesDef.Any( s => s.Key.Equals(defaultSeries)))
                            result.Add(FillSeriesData(seriesDef.First(s => s.Key.Equals(defaultSeries))));
                        break;
                    case DiagramTypeEnum.PieDetail:
                        if (seriesDef.Any(s => s.Key.Equals(defaultSeries)))
                            result.Add(FillPointsData(seriesDef.First(s => s.Key.Equals(defaultSeries)),
                                pointDef, intersections, false));
                        break;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(detalizedSeriesName)) detalizedSeriesName = this.defaultSeries;
                if (seriesDef.Any(s => s.Key.Equals(detalizedSeriesName)))
                {
                    switch (this.SubDiagramType)
                    {
                        case SubDiagramTypeEnum.Graph:
                        case SubDiagramTypeEnum.ColumnDetail:
                        case SubDiagramTypeEnum.PieDetail:
                            result.Add(FillPointsData(seriesDef.First(s => s.Key.Equals(detalizedSeriesName)),
                                pointDef, intersections, false));
                            break;
                    }
                }
            }
            FillEmptyVisibleSeries(seriesDef.Keys.ToList());
            return result;
        }

        internal void FillEmptyVisibleSeries(List<string> series)
        {
            if (String.IsNullOrEmpty(visibleSeries))
                visibleSeries = series.Aggregate((a, b) => a + "," + b);
        }

        /// <summary>вспомогательный метод дергается только из FillData, 
        /// для формирование чарта и с сериями и с точками </summary>
        private Series[] CreateDataForFullDiagram(
            Dictionary<string, float> seriesDef, 
            Dictionary<string, float> pointDef, 
            Dictionary<Intersection, float> intersections,
            bool precisionXAxis)
        {
            List<Series> result = new List<Series>();
            foreach (KeyValuePair<string, float> series in seriesDef)
            {
                result.Add(FillPointsData(series, pointDef, intersections, precisionXAxis));
            }
            return result.ToArray();
        }

        /// <summary> вспомогательный метод дергается только из FillData, 
        /// для формирования чарта по одной серии по всем точкам</summary>
        private Series FillPointsData(
            KeyValuePair<string, float> series,
            Dictionary<string, float> pointDef,
            Dictionary<Intersection, float> intersections,
            bool precisionXAxis)
        {
            Series s = new Series(series.Key);
            s.LegendEntry.Value = series.Value.ToString();
            foreach (KeyValuePair<string, float> point in pointDef)
            {
                Element e = new Element();
                if (precisionXAxis)
                    e.XValue = point.Value;
                else
                    e.Name = point.Key;
                Intersection i = new Intersection(series.Key, point.Key);
                if (intersections.ContainsKey(i))
                {
                    e.YValue = intersections[i];
                    //e.ToolTip = intersections[i].ToString();
                }
                s.Elements.Add(e);
            }
            return s;
        }

        /// <summary> вспомогательный метод дергается только из FillData, 
        /// для формирования чарта по обобщенным значениям серии</summary>
        private Series CreateDataForPieGeneral(Dictionary<string, float> seriesDef)
        {
            Series result = new Series();
            foreach (KeyValuePair<string, float> series in seriesDef)
            {
                Element e = new Element(series.Key);
                e.YValue = series.Value;
                result.Elements.Add(e);
                //e.ToolTip = series.Value.ToString();
                
            }
            return result;
        }

        /// <summary> вспомогательный метод дергается только из FillData, 
        /// для формирования чарта по обобщенным значениям серии</summary>
        private Series[] CreateDataForColumnGeneral(Dictionary<string, float> seriesDef)
        {
            List<Series> result = new List<Series>();
            foreach (KeyValuePair<string, float> series in seriesDef)
            {
                result.Add(FillSeriesData(series));
            }
            return result.ToArray();
        }

        /// <summary> вспомогательный метод дергается только из FillData, 
        /// для формирования чарта по обобщенному значению серии</summary>
        private Series FillSeriesData(KeyValuePair<string, float> series)
        {
            Series s = new Series(series.Key);
            s.LegendEntry.Value = series.Value.ToString();
            Element e = new Element(series.Key);
            e.YValue = series.Value;
            //e.ToolTip = series.Value.ToString();
            s.Elements.Add(e);
            return s;
        }
    }
}

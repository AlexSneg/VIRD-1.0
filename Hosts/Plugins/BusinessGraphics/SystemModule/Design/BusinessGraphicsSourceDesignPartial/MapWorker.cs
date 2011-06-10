using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotnetCHARTING.Mapping;
using System.IO;
using System.Drawing;
using dotnetCHARTING.WinForms;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace Hosts.Plugins.BusinessGraphics.SystemModule.Design
{
    public partial class BusinessGraphicsSourceDesign
    {
        /// <summary>пробегает по всем shape-ам и настраивает их (подсказки, расскраска и пр.)</summary>
        private void LinkMapData()
        {
            if (this.DiagramType == DiagramTypeEnum.Map && chart.Mapping.MapLayerCollection.Count > 0)
            {
                string dbfFile = GetFile(FileType.mapdataResource);
                if (!string.IsNullOrEmpty(dbfFile))
                {
                    MapLayer layer = chart.Mapping.MapLayerCollection[0];
                    DbfFile dbf = new DbfFile(dbfFile);
                    int shapeNo = 0;
                    foreach (Shape shape in layer.Shapes)
                    {
                        String name = dbf.Get(shapeNo++, "NAME\0\0\0\0\0\0").Trim();
                        shape.Hotspot.URL = name;
                        shape.Hotspot.ToolTip = name; //всплывающая подскаска при наведении на область
                        shape.Label.Text = string.Empty; //убираем имя, чтобы оно не загромождало карту
                        PaintShape(shape);
                    }
                }
            }
        }

        /// <summary>по точке клика возвращает имя территории (она же имя серии) </summary>
        private string GetShapeName(Point location)
        {
            Shape _currentShape = chart.Mapping.GetShapesAtPoint(location).Cast<Shape>().FirstOrDefault();
            if (_currentShape != null)
                return _currentShape.Hotspot.URL;
            else
                return string.Empty;
        }

        /// <summary>инициализирует БГ картой</summary>
        private void InitializeMap(string mapFile)
        {
            chart.Mapping.MapLayerCollection.Clear();
            MapLayer layer = MapDataEngine.LoadLayer(mapFile);
            layer.DefaultShape.Background.Color = Color.PapayaWhip;
            if (valueRange != null && valueRange.Count > 0)
                chart.Palette = this.ValueRanges.Select(v => v.Color).ToArray();
            else
                chart.PaletteName = Palette.Four;
            chart.Mapping.MapLayerCollection.Add(layer);
        }


        /// <summary>окрашивает область в зависимости от значения серии </summary>
        private void PaintShape(Shape shape)
        {
            if (seriesDef.ContainsKey(shape.Hotspot.URL) && ValueRanges.Count > 0)
            {
                float value = seriesDef[shape.Hotspot.URL];
                shape.Hotspot.ToolTip = shape.Hotspot.ToolTip + ": " + value.ToString();
                ValueRange range = ValueRanges.Where(v => v.MinValue <= value && value <= v.MaxValue).FirstOrDefault();
                if (range == null)
                {
                    if (value < ValueRanges.First().MinValue)
                    {
                        range = ValueRanges.First();
                    }
                    if (value > ValueRanges.Last().MaxValue)
                    {
                        range = ValueRanges.Last();
                    }
                }
                if (range != null)
                    shape.Background.Color = range.Color;
            }
        }

    }
}

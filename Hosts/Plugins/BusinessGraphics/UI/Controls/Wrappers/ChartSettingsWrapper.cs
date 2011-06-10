using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotnetCHARTING.WinForms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Hosts.Plugins.BusinessGraphics.UI.Controls.Wrappers
{
    /// <summary>
    /// Обертка Chart для сохранения свойств.
    /// </summary>
    [Serializable]
    public class ChartSettingsWrapper
    {
        public ChartSettingsWrapper() { }
        public ChartSettingsWrapper(CustomChart chart) 
        {
            ClipGauges = chart.ClipGauges;
            Depth = chart.Depth;
            ExplodedSliceAmount = chart.ExplodedSliceAmount;
            GaugeType = chart.DefaultSeries.GaugeType;
            PaletteName = chart.PaletteName;
            PieLabelMode = chart.PieLabelMode;
            SeriesType = (SeriesType)chart.DefaultSeries.Type;
            ShadingEffectMode = chart.ShadingEffectMode;
            GraphLineWidth = chart.DefaultSeries.Line.Width;
            Use3D = chart.Use3D;

            ChartArea = new ChartAreaSettingsWrapper(chart.ChartArea);

            Background = new BackgroundSettingsWrapper(chart.Background);
            DefaultElement = new DefaultElementSettingsWrapper(chart.ChartArea.DefaultElement);
        }
        
        public bool ClipGauges;
        public int Depth;
        public double ExplodedSliceAmount;
        public GaugeType GaugeType;
        public Palette PaletteName;
        public PieLabelMode PieLabelMode;
        public SeriesType SeriesType;
        public ShadingEffectMode ShadingEffectMode;
        public int GraphLineWidth; // В окне настроек этот параметр находится в группе DefaultElement
        public bool Use3D;

        public BackgroundSettingsWrapper Background;
        public ChartAreaSettingsWrapper ChartArea;
        public DefaultElementSettingsWrapper DefaultElement;

        /// <summary>
        /// Восстановить сохранненые значения.
        /// </summary>
        public void RestoreSettings(CustomChart chart)
        {
            chart.ClipGauges = ClipGauges;
            chart.Depth = Depth;
            chart.ExplodedSliceAmount = ExplodedSliceAmount;
            chart.DefaultSeries.GaugeType = GaugeType;
            chart.PaletteName = PaletteName;
            chart.PieLabelMode = PieLabelMode;
            chart.DefaultSeries.Type = SeriesType;
            chart.ShadingEffectMode = ShadingEffectMode;
            chart.DefaultSeries.Line.Width = GraphLineWidth;
            chart.Use3D = Use3D;

            ChartArea.RestoreSettings(chart.ChartArea);
            Background.RestoreSettings(chart.Background);
            DefaultElement.RestoreSettings(chart.ChartArea.DefaultElement);
        }

    }
}

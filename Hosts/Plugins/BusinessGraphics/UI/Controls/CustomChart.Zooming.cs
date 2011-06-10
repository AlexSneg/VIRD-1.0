using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using dotnetCHARTING.WinForms;
using Syncfusion.Windows.Forms.Tools;
using System.Windows.Forms;
using Hosts.Plugins.BusinessGraphics.SystemModule.Design;
using System.Drawing.Drawing2D;

namespace Hosts.Plugins.BusinessGraphics.UI.Controls
{
    public partial class CustomChart
    {
        private Point _firstClick = Point.Empty;
        private Point _secondClick = Point.Empty;

        //начальная точка в которой началось выделение области
        private PointF _startSelection = PointF.Empty;
        //конечная точка в которой завершилось выделение области
        private PointF _endSelection = PointF.Empty;

        /// <summary>обрабатываем события от мыши, чтобы отловить, выделение области </summary>
        private void ProcessSelectionArea(MouseEventArgs e)
        {
            //выделение области мы ловим только для столбиков и графика
            if (isAllowMouseClickProcess && _allowUserInteraction && !isChartSetupMode)
            {
                if ((DiagramType == DiagramTypeEnum.Graph) ||
                    (DiagramType == DiagramTypeEnum.ColumnGeneral) ||
                    (DiagramType == DiagramTypeEnum.ColumnDetail))
                {
                    //выделение происходит нажатием левой клавиши мыши
                    if (e.Button == MouseButtons.Left)
                    {
                        string coordinat = string.Format("{0},{1}", e.X, e.Y);
                        object x = base.XAxis.GetValueAtX(coordinat);
                        object y = base.YAxis.GetValueAtY(coordinat);
                        base.ExtraChartAreas.Clear();
                        base.XAxis.Markers.Clear();
                        base.YAxis.Markers.Clear();
                        if (_startSelection.Equals(PointF.Empty))
                        {
                            //первое нажатие
                            _firstClick = e.Location;
                            _startSelection = new PointF(Convert.ToSingle(x == null ? 0 : x), Convert.ToSingle(y == null ? 0 : y));
                            if (x != null)
                                base.XAxis.Markers.Add(new AxisMarker("", Color.Orange, _startSelection.X));
                            if (y != null)
                                base.YAxis.Markers.Add(new AxisMarker("", Color.Red, _startSelection.Y));
                        }
                        else if (_endSelection.Equals(PointF.Empty))
                        {
                            //второе нажатие
                            _secondClick = e.Location;
                            _endSelection = new PointF(Convert.ToSingle(x == null ? 0 : x), Convert.ToSingle(y == null ? 0 : y));
                            if (DiagramType == DiagramTypeEnum.Graph)
                            {
                                if ((x != null) && (y != null) && (_startSelection.X != 0) && (_startSelection.Y != 0))
                                {
                                    //для графика вычислим какой zoom нужен по оси X или Y
                                    if (Math.Abs(_firstClick.X - _secondClick.X) >= Math.Abs(_firstClick.Y - _secondClick.Y))
                                        ZoomXAxis(_startSelection, _endSelection);
                                    else
                                        ZoomYAxis(_startSelection, _endSelection);
                                }
                            }
                            else if ((y != null) && (_startSelection.Y != 0))
                            {
                                //для всех остальных масштабирование только по оси Y
                                ZoomYAxis(_startSelection, _endSelection);
                            }
                        }
                        else
                        {
                            //третье нажатие
                            _startSelection = _endSelection = PointF.Empty;
                        }
                        base.RefreshChart();
                    }
                }
            }
            else
            {
                // Нажатие правой кнопкой
                ClearInteractiveInfo();
            }
        }

        /// <summary>
        /// Очистка промежуточных данных, используемых в интерактивных действиях.
        /// </summary>
        public void ClearInteractiveInfo()
        {
            _startSelection = _endSelection = PointF.Empty;
        }

        private void ZoomXAxis(PointF _startSelection, PointF _endSelection)
        {
            ChartArea ca = base.ChartArea.GetXZoomChartArea(
                base.XAxis,
                new ScaleRange(_startSelection.X, _endSelection.X), 
                new Line(Color.LightGreen, DashStyle.Dash));
            base.ExtraChartAreas.Add(ca);
        }

        private void ZoomYAxis(PointF _startSelection, PointF _endSelection)
        {
            ChartArea ca = base.ChartArea.GetYZoomChartArea(
                base.YAxis,
                new ScaleRange(_startSelection.Y, _endSelection.Y), 
                new Line(Color.LightGreen, DashStyle.Dash));
            base.ExtraChartAreas.Add(ca);
        }
    }
}

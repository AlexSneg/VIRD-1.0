using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hosts.Plugins.ArcGISMap.Player;

namespace Hosts.Plugins.ArcGISMap.UI.Controls
{
    public partial class MapControl : UserControl
    {
        /// <summary>
        /// Масштаб карты.
        /// </summary>
        public double Scale { get; set; }
        /// <summary>
        /// Смещение по горизонтали.
        /// </summary>
        public int XOffset { get; set; }
        /// <summary>
        /// Смещение по вертикали.
        /// </summary>
        public int YOffset { get; set; }

        public bool RedLayerVisible=true;
        public bool BlueLayerVisible=true;
        public bool GridVisible;

        /// <summary>
        /// Высота карты.
        /// </summary>
        public int MapHeight
        {
            get
            {
                return image.Height;
            }
        }

        /// <summary>
        /// Ширина карты.
        /// </summary>
        public int MapWidth
        {
            get
            {
                return image.Width;
            }
        }

        public MapControl()
        {
            InitializeComponent();

            UpdateLayers();
            Scale = 1;
            XOffset = 0;
            YOffset = 0;
        }

        /// <summary>
        /// Перерисовать слои.
        /// </summary>
        public void UpdateLayers()
        {
            double R = Math.Sqrt(image.Width * image.Width + image.Height * image.Height);
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    double r = Math.Sqrt(i * i + j * j) / R;
                    Color c = Color.FromArgb
                        (
                        RedLayerVisible ? Convert.ToInt32(255 * r) : 0,
                        0,
                        BlueLayerVisible ? Convert.ToInt32(255 * (1 - r)) : 0);
                    if ((i % 77 == 0 || j % 77 == 0) && GridVisible)
                    {
                        c = Color.Black;
                    }
                    image.SetPixel(i, j, c);
                }
            }
            this.Refresh();
        }
        Bitmap image = new Bitmap(1000, 1000);

        private void MapControl_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.DrawLine(Pens.Blue, new Point(0, 0), new Point(this.Width, this.Height));
            Bitmap crop = new Bitmap(image, new Size(Convert.ToInt32(image.Size.Width * Scale), Convert.ToInt32(image.Size.Height * Scale)));
            Rectangle destinationRectangle = new Rectangle(new Point(0,0), this.Size);
            Rectangle sourceRectangle = new Rectangle(new Point(XOffset, YOffset), 
                this.Size
                );
            e.Graphics.DrawImage(crop, destinationRectangle, sourceRectangle, GraphicsUnit.Pixel );
        }

        #region Управление просмотром
        /// <summary>
        /// Установить масштаб карты.
        /// </summary>
        /// <param name="scale">Значение масштаба</param>
        public void SetScale(double scale)
        {
            this.Scale = scale;
            this.XOffset = 0;
            this.YOffset = 0;
            this.Refresh();
        }

        const int step = 100;
        /// <summary>
        /// Сместить окно просмотра участка карты вверх.
        /// </summary>
        public void MoveUp()
        {
            if (this.YOffset > step) this.YOffset -= step;
            this.Refresh();
        }
        /// <summary>
        /// Сместить окно просмотра участка карты вниз.
        /// </summary>
        public void MoveDown()
        {
            if (this.MapHeight * this.Scale - this.YOffset - this.Height > step) this.YOffset += step;
            else
            {
                this.YOffset = Convert.ToInt32(this.MapHeight * this.Scale - this.Height);
            }
            this.Refresh();
        }
        /// <summary>
        /// Сместить окно просмотра участка карты влево.
        /// </summary>
        public void MoveLeft()
        {
            if (this.XOffset > step) this.XOffset -= step;
            this.Refresh();
        }
        /// <summary>
        /// Сместить окно просмотра участка карты вправо.
        /// </summary>
        public void MoveRigth()
        {
            if (this.MapWidth * this.Scale - this.XOffset - this.Width > step) this.XOffset += step;
            else
            {
                this.XOffset = Convert.ToInt32(this.MapWidth * this.Scale - this.Width);
            }
            this.Refresh();
        }
        #endregion
    }
}

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Syncfusion.Windows.Forms.Diagram;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System;

namespace UI.PresentationDesign.DesignUI.Classes.View
{
    /// <summary>
    /// Отображение сцены
    /// </summary>
    public class SlideView : RoundRect
    {
        public const int MaxWidth = 100;
        public const int MaxHeight = 50;
        public const int Margin = 20;

        private Slide m_Slide = null;

        /// <summary>
        /// Mapping из модели сцены
        /// </summary>
        public Slide Slide
        {
            set
            {
                m_Slide = value;
                Refresh();
            }

            get
            {
                return m_Slide;
            }
        }

        private readonly float _curveRaduis = 15.0f;

        private readonly Color _lineColor = Color.DarkGray;
        private readonly Color _foreColor = Color.LightGreen;
        //private readonly Color _selectionColor = Color.DarkGreen;
        private readonly Color _startColor = Color.Gray;
        private readonly Color _lineLockColor = Color.Red;
        private readonly Color _lineShowColor = Color.YellowGreen;
        private readonly Color nonDefColor = Color.FromArgb(205, 213, 236);
        private readonly PresentationLabel _nameLbl;
        private readonly PresentationLabel _commentLbl;
        private bool _isLocked = false;

        /// <summary>
        /// Устанавливает или возвращает имя сцены
        /// </summary>
        public string SlideName
        {
            get { return Slide.Name; }
            set
            {
                Slide.Name = value;
                this._nameLbl.Text = value;
            }
        }

        public SlideView(SlideView src)
            : this(src.Slide, src.GetPosition())
        {
            this._isLocked = src._isLocked;
            this.EnableShading = true;
        }

        public SlideView(Slide slide, PointF pos)
            : base(pos.X, pos.Y, MaxWidth, MaxHeight, MeasureUnits.Pixel)
        {
            m_Slide = slide;

            this.LineStyle.LineColor = _lineColor;
            this.LineStyle.LineWidth = 1;

            this.FillStyle.ForeColor = nonDefColor;
            this.FillStyle.Color = Color.White;
            this.FillStyle.Type = FillStyleType.LinearGradient;
            this.FillStyle.GradientCenter = 1;
            this.FillStyle.GradientAngle = 0;

            this.ShadowStyle.Visible = true;
            this.ShadowStyle.OffsetX = 6;
            this.ShadowStyle.OffsetY = 3;

            RectangleF rcBounds = this.BoundingRectangle;

            this._nameLbl = new PresentationLabel(this, slide, "Name") { IsMultiline = false, OffsetY = 5 };
            this._nameLbl.FontStyle.Style = System.Drawing.FontStyle.Bold;
            this._nameLbl.FontStyle.Family = "Arial";
            this._nameLbl.FontStyle.Size = 8;

            this._commentLbl = new PresentationLabel(this, slide, "Comment") { IsMultiline = true, OffsetY = 18 };
            this._commentLbl.FontStyle.Family = "Arial";
            this._commentLbl.FontStyle.Size = 8;

            this.Labels.Add(this._commentLbl);
            this.Labels.Add(this._nameLbl);

            this.EditStyle.AllowSelect = true;
            this.EditStyle.AllowDelete = false;
            this.EditStyle.AllowRotate = false;
            this.EditStyle.AllowChangeHeight = false;
            this.EditStyle.AllowChangeWidth = false;
            this.EditStyle.HidePinPoint = true;
            this.EditStyle.HideRotationHandle = true;

            this.CurveRadius = _curveRaduis;
            this.EnableCentralPort = true;
        }

        /// <summary>
        /// Выполняет обновление текстовых меток сцены (заголовок, имя)
        /// </summary>
        public void Refresh()
        {
            this._nameLbl.UpdateReference(Slide);
            this._commentLbl.UpdateReference(Slide);
        }

        /// <summary>
        /// Устанавливает признак Начального сцены сценария
        /// </summary>
        public bool IsStartSlide
        {
            get
            {
                return _isdefault;
            }
            set
            {
                _isdefault = value;
                if (!value)
                {
                    this.LineStyle.LineColor = _lineColor;
                    this.LineStyle.LineWidth = 1;
                    this.CurveRadius = _curveRaduis;
                }
                else
                {
                    this.LineStyle.LineColor = _startColor;
                    this.LineStyle.LineWidth = 3;
                    this.CurveRadius = 1;
                }
            }
        }

        bool _isdefault;

        /// <summary>
        /// Устанавливает флаг принадлежности сцены к дефолтному пути 
        /// </summary>
        public bool IsDefaultPathNode
        {
            get
            {
                return _isdefault;
            }
            set
            {
                
                _isdefault = value;

                if (!_isLocked)
                {
                    if (!value)
                    {
                        this.FillStyle.ForeColor = nonDefColor;
                    }
                    else
                    {
                        this.FillStyle.ForeColor = _foreColor;
                    }
                    this.GetAllLinks().ForEach(l => { l.DefaultPath = value; l.Refresh(); });
                }
            }
        }

        /// <summary>
        /// Возвращает или устанавливает признак блокировки сцены
        /// </summary>
        public bool IsLocked
        {
            get { return _isLocked; }
            set
            {
                if (value && !_isLocked)
                    Lock();
                if (!value && _isLocked)
                    Unlock();
            }
        }

        /// <summary>
        /// Выполняет блокировку сцены
        /// </summary>
        public void Lock(bool forEdit)
        {
            HistoryManager.Pause();
            this.LineStyle.LineColor = forEdit ? _lineLockColor : _lineShowColor;
            this.FillStyle.ForeColor = forEdit ? Color.LightCoral : Color.Yellow;
            HistoryManager.Resume();
            _isLocked = true;
            Slide.IsLocked = true;
        }

         /// <summary>
        /// Выполняет блокировку сцены
        /// </summary>
        public void Lock()
        {
            Lock(true);
        }

        /// <summary>
        /// Выполняет разблокировку сцены
        /// </summary>
        public void Unlock()
        {
            HistoryManager.Pause();
            this.LineStyle.LineColor = _lineColor;

            if (_isdefault)
                this.FillStyle.ForeColor = _foreColor;
            else
                this.FillStyle.ForeColor = nonDefColor;

            HistoryManager.Resume();

            _isLocked = false;
            Slide.IsLocked = false;
        }

        /// <summary>
        /// Возвращает список отображений связей
        /// </summary>
        /// <returns>Cформированный список типа SlideLink</returns>
        public List<SlideLink> GetAllLinks()
        {
            List<SlideLink> slideLinks = new List<SlideLink>();

            foreach (EndPoint e in this.CentralPort.Connections)
            {
                SlideLink link = e.Container as SlideLink;
                if (link != null)
                    slideLinks.Add(link);
            }

            return slideLinks;
        }

        public List<SlideLink> GetOutgoingLinks()
        {
            List<SlideLink> slideLinks = new List<SlideLink>();

            foreach (EndPoint e in this.CentralPort.Connections)
            {
                SlideLink link = e.Container as SlideLink;
                if (link != null && link.FromNode == this)
                    slideLinks.Add(link);
            }

            return slideLinks;
        }

        /// <summary>
        /// Возвращает список входящих связей в данной сцене
        /// </summary>
        /// <returns>Список SlideLink</returns>
        public List<SlideLink> GetIncomingSlideLinks()
        {
            List<SlideLink> slideLinks = new List<SlideLink>();

            foreach (EndPoint e in this.CentralPort.Connections)
            {
                SlideLink link = e.Container as SlideLink;
                if (link != null && link.ToNode == this)
                    slideLinks.Add(link);
            }

            return slideLinks;
        }


        /// <summary>
        /// Возвращает список исходящих отображений сцен, согласно ссылок сцены
        /// </summary>
        /// <returns>Список SlideView</returns>
        public List<SlideView> GetOutgoingSlideViews()
        {
            List<SlideView> slideViews = new List<SlideView>();

            foreach (EndPoint e in this.CentralPort.Connections)
            {
                SlideLink link = e.Container as SlideLink;
                if (link != null && link.FromNode == this && link.ToSlideView != null)
                    slideViews.Add(link.ToSlideView);
            }

            return slideViews;
        }

        /// <summary>
        /// Возвращает следующее по умолчанию отображение сцены
        /// </summary>
        public SlideView GetNextDefaultSlideView()
        {
            var links = GetAllLinks().Where(l => l.ToSlideView != this && l.IsDefault);
            if (links.Count() > 0)
                return links.First().ToNode as SlideView;
            return null;
        }

        public override object Clone()
        {
            return new SlideView(this);
        }

        public Point GetPosition()
        {
            return new Point((int)this.BoundingRectangle.Location.X, (int)this.BoundingRectangle.Location.Y);
        }

        public PointF GetPositionF()
        {
            return this.BoundingRectangle.Location;
        }


        public bool CanLinkTo(SlideView to)
        {
            List<SlideLink> links = this.GetAllLinks();
            if (links.Count == 0) return true;
            return !links.Any(l => l.ToSlideView == to | l.FromSlideView == to);
        }

        public override string ToString()
        {
            if (Slide != null)
                return this.Slide.Name;
            return String.Empty;
        }
    }
}

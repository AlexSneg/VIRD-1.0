using Syncfusion.Windows.Forms.Diagram;
using System.Drawing;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Syncfusion.Windows.Forms.Tools;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using UI.PresentationDesign.DesignUI.Classes.View;
using System;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Common.Utils;
using System.Drawing.Drawing2D;
using UI.PresentationDesign.DesignUI.Views;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using UI.PresentationDesign.DesignUI.Controllers;
using System.Diagnostics;
using TechnicalServices.Interfaces;
using TechnicalServices.Entity;
using Domain.PresentationDesign.Client;
using Syncfusion.Windows.Forms.Diagram;

namespace UI.PresentationDesign.DesignUI.Controls.SourceTree
{
    public class SourceWindow : Syncfusion.Windows.Forms.Diagram.Rectangle, ISourceNode, IUnitIndependent, IBoundsInfo
    {
        private static ValuePair<int, int> _defaultWndsize;

        #region fields and properties
        public static int InitialWidth { get { return _defaultWndsize.Value1; } }
        public static int InitialHeight { get { return _defaultWndsize.Value2; } }

        Window _window;
        PresentationLabel lbl;

        public new String Name
        {
            get
            {
                if (Mapping != null)
                    return Mapping.ResourceInfo.Name;
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }
        #endregion

        #region Ctors
        static SourceWindow()
        {
            _defaultWndsize = DesignerClient.Instance.DefaultWndsize;
        }

        public SourceWindow(SourceWindow src)
            : base(src)
        {
            this.EnableShading = src.EnableShading;
            CheckAspectRatio(src);
        }

        public SourceWindow(Window window)
            : this(window, window.Source.ResourceDescriptor.ResourceInfo.Name)
        {
            ZOrder = window.ZOrder;
            CheckAspectRatio(this);
        }

        public SourceWindow(Window window, string AName)
            : this(AName, window.Left, window.Top, window.Width, window.Height)
        {
            Window = window;
        }

        public SourceWindow(ResourceDescriptor info)
            : this(info.ResourceInfo.Name)
        {
            Mapping = info;
        }

        public SourceWindow(string AName)
            : this(AName, 0, 0, InitialWidth, InitialHeight)
        {
        }

        public SourceWindow(string AName, float x, float y, float width, float height)
            : base(x, y, width, height, MeasureUnits.Pixel)
        {
            this.FillStyle.ForeColor = Color.DarkGreen;
            this.FillStyle.Color = Color.LightYellow;
            this.FillStyle.Type = FillStyleType.LinearGradient;
            this.FillStyle.GradientCenter = 1;
            this.FillStyle.GradientAngle = 45;

            this.ShadowStyle.Visible = true;
            this.ShadowStyle.OffsetX = 6;
            this.ShadowStyle.OffsetY = 3;
            this.LineStyle.LineWidth = 2;


            this.EditStyle.AllowSelect = true;
            this.EditStyle.AllowDelete = false;
            this.EditStyle.AllowRotate = false;

            this.EditStyle.AllowChangeHeight = true;
            this.EditStyle.AllowChangeWidth = true;

            this.EditStyle.HidePinPoint = true;
            this.EditStyle.HideRotationHandle = true;
            this.EnableCentralPort = true;

            Name = AName;
            CreateLabel(this);
        }

        protected override void UpdateReferences(IServiceReferenceProvider provider)
        {
            base.UpdateReferences(provider);
            this.EventSink.SizeChanged += new SizeChangedEventHandler(EventSink_SizeChanged);
            this.EventSink.ZOrderChanged += new ZOrderChangedEventHandler(EventSink_ZOrderChanged);
        }

        #endregion

        #region Render
        protected override void Render(System.Drawing.Graphics gfx)
        {
            base.Render(gfx);

            if (this.Window != null && this.Window.Source != null)
            {
                IDesignRenderSupport support = this.Window.Source as IDesignRenderSupport;
                if (support != null)
                {
                    lbl.IsVisible = false;

                    support.Render(gfx, this.GetBoundsInfo());
                    return;
                }
                else
                    lbl.IsVisible = true;
            }
        }
        #endregion

        #region aspect ration
        private void CheckAspectRatio(SourceWindow src)
        {
            if (src.Window != null && src.Window.Source != null)
            {
                IAspectLock original = (src.Window.Source as IAspectLock);
                if (original != null && original.AspectLock)
                    this.EditStyle.AspectRatio = true;
                else
                    this.EditStyle.AspectRatio = false;
            }
        }
        #endregion

        #region Update and Refresh properties

        private void RefreshProperties()
        {
            if (SourcePropertiesControl.Instance != null)
                SourcePropertiesControl.Instance.RefreshProperties();
        }

        protected void UpdatePositions()
        {
            if (this.Window != null)
            {
                this.Window.Left = GetPosition().X;
                this.Window.Top = GetPosition().Y;
            }

            RefreshProperties();
        }

        protected void UpdateZOrder()
        {
            if (this.Window != null)
            {
                if (this.ZOrder != -1)
                    this.Window.ZOrder = (byte)this.ZOrder;
            }

            RefreshProperties();
        }

        protected void UpdateSize()
        {
            Size size = this.GetSize();
            if (this.Window != null)
            {
                this.Window.Width = size.Width;
                this.Window.Height = size.Height;

                if (this.Window.Source != null)
                {
                    ISourceSize ss = this.Window.Source as ISourceSize;
                    if (ss != null && (ss.Width != size.Width || ss.Height != size.Height))
                    {
                        ss.SetSize(size);
                    }
                }
            }

            RefreshProperties();
        }


        void EventSink_ZOrderChanged(ZOrderChangedEventArgs evtArgs)
        {
            UpdateZOrder();
        }

        void EventSink_SizeChanged(SizeChangedEventArgs evtArgs)
        {
            UpdateSize();
        }


        public void UpdateWindowInfo()
        {
            UpdateSize();
            UpdatePositions();
            UpdateZOrder();

            RefreshProperties();
        }


        protected override void UpdateBoundingRectangle()
        {
            base.UpdateBoundingRectangle();
            UpdatePositions();
        }


        public void Refresh()
        {
            RectangleF rectBounds = new RectangleF(new PointF(_window.Left, _window.Top), new SizeF(_window.Width == 0 ? 1 : _window.Width, _window.Height == 0 ? 1 : _window.Height));
            UpdateBoundsInfo(rectBounds);
            lbl.UpdateReference(this);
        }


        #endregion

        #region helpers

        static void CreateLabel(SourceWindow wnd)
        {
            wnd.lbl = new PresentationLabel(wnd, wnd, "Name", false) { IsMultiline = true };
            wnd.lbl.ForeColor = Color.White;
            wnd.lbl.FontStyle.Style = System.Drawing.FontStyle.Bold;
            wnd.lbl.FontStyle.Family = "Arial";
            wnd.lbl.FontStyle.Size = 18;
            wnd.Labels.Add(wnd.lbl);
        }


        public override object Clone()
        {
            SourceWindow node = new SourceWindow(this);

            node.Image = Image;

            if (Window != null)
                node.Window = this.Window;
            else
                node.Mapping = Mapping;

            node.SourceType = SourceType;
            CreateLabel(node);

            return node;
        }

        public Point GetPosition()
        {
            float x = this.GetUpperLeftPoint(MeasureUnits.Pixel).X;
            float y = this.GetUpperLeftPoint(MeasureUnits.Pixel).Y;

            return new Point((int)x, (int)y);
        }

        public Size GetSize()
        {
            float width = MeasureUnitsConverter.ConvertX(this.GetBoundsInfo().Size.Width, MeasurementUnit, MeasureUnits.Pixel);
            float height = MeasureUnitsConverter.ConvertX(this.GetBoundsInfo().Size.Height, MeasurementUnit, MeasureUnits.Pixel);

            // Stas - round
            return new Size((int)Math.Round(width), (int)Math.Round(height));
        }

        public bool IntersectsWith(SourceWindow wnd)
        {
            PointF pos = wnd.GetPosition();
            RectangleF rect = new RectangleF(pos, wnd.Size);
            return rect.IntersectsWith(new RectangleF(this.GetUpperLeftPoint(MeasurementUnit), this.GetBoundsInfo().Size));
        }
        #endregion

        #region ISourceMapping Members

        public Window Window
        {
            get { return _window; }
            set
            {
                _window = value;
                //Refresh();
            }
        }

        ResourceDescriptor _rd;

        public ResourceDescriptor Mapping
        {
            get
            {
                if (this.Window != null && this.Window.Source != null)
                    return this.Window.Source.ResourceDescriptor;
                else
                    return _rd;
            }
            set
            {
                _rd = value;
                if (this.Window != null && this.Window.Source != null)
                {
                    this.Window.Source.ResourceDescriptor = value;
                }
            }
        }

        public Image Image
        {
            get;
            set;
        }

        public SourceType SourceType
        {
            get;
            set;
        }

        public bool CreateSourceWindow(Identity id, Display display)
        {
            if (SourceType != null)
            {
                //STAS - тут теперь необходимо передавать так же и слайд и еще блин конфигурацию
                Source source = SourceType.CreateNewSource(
                    PresentationController.Instance.SelectedSlide,
                    PresentationController.Configuration.ModuleConfiguration,
                    DesignerClient.Instance.PresentationWorker.GetGlobalDeviceSources(), 
                    display);

                if (source != null)
                {
                    source.Id = id.NextID.ToString();
                    source.ResourceDescriptor = this.Mapping;
                }

                Window wnd = display.CreateWindow(source, PresentationController.Instance.SelectedSlide);
                if (wnd != null)
                {
                    if (wnd.Width == 0)
                        wnd.Width = InitialWidth;
                    if (wnd.Height == 0)
                        wnd.Height = InitialHeight;
                }

                this.Window = wnd;
                return this.Window != null && this.Window.Source != null;
            }
            return false;
        }

        #endregion

        #region IUnitIndependent Members

        RectangleF IUnitIndependent.GetBoundingRectangle(MeasureUnits unit, bool bRelativeToModel)
        {
            if (bRelativeToModel)
            {
                RectangleF pathBounds = this.GetPathBounds();
                PointF[] pts = new PointF[] { pathBounds.Location, new PointF(pathBounds.X + pathBounds.Width, pathBounds.Y), new PointF(pathBounds.Right, pathBounds.Bottom), new PointF(pathBounds.X, pathBounds.Y + pathBounds.Height) };
                HandlesHitTesting.GetParentsTransformations(this, true).TransformPoints(pts);
                return Geometry.CreateRect(pts);
            }
            return MeasureUnitsConverter.FromPixels(this.BoundingRect, unit);

        }

        PointF IUnitIndependent.GetPinPoint(MeasureUnits unit)
        {
            return this.BoundsInfo.GetPinPoint(unit);

        }

        SizeF IUnitIndependent.GetPinPointOffset(MeasureUnits unit)
        {
            return this.BoundsInfo.GetPinOffset(unit);
        }

        SizeF IUnitIndependent.GetSize(MeasureUnits unit)
        {
            return this.BoundsInfo.GetSize(unit);

        }

        void IUnitIndependent.SetPinPoint(PointF ptValue, MeasureUnits unit)
        {
            this.SetPinPoint(ptValue, unit);
        }

        void IUnitIndependent.SetPinPointOffset(SizeF szValue, MeasureUnits unit)
        {
            if (this.CheckNewPinOffset(szValue, unit))
            {
                this.BoundsInfo.SetPinOffset(szValue, unit);
                this.UpdateBoundingRectangle();
            }
        }

        void IUnitIndependent.SetSize(SizeF szValue, MeasureUnits unit)
        {
            if (this.CheckNewSize(szValue, unit))
            {
                this.BoundsInfo.SetSize(szValue, unit);
                this.UpdateBoundingRectangle();
                if (this.Parent != null)
                {
                    this.Parent.UpdateCompositeBounds();
                }
            }
        }

        // https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-733
        protected override void SetPinPoint(PointF ptValue, MeasureUnits unit)
        {
            SizeF size = this.BoundsInfo.GetSize(unit);
            SizeF offset = this.BoundsInfo.GetPinOffset(unit);
            if (ptValue.X - offset.Width < -size.Width) ptValue.X = -size.Width + offset.Width;
            if (ptValue.Y - offset.Height < -size.Height) ptValue.Y = -size.Height + offset.Height;

            Syncfusion.Windows.Forms.Diagram.Model model = this.Root;
            if (model != null)
            {
                size = MeasureUnitsConverter.Convert(model.LogicalSize, model.MeasurementUnits, unit);
                if (ptValue.X - offset.Width > size.Width) ptValue.X = size.Width + offset.Width;
                if (ptValue.Y - offset.Height > size.Height) ptValue.Y = size.Height + offset.Height;
            }
            base.SetPinPoint(ptValue, unit);
        }

        private bool CheckNewSize(SizeF szSize, MeasureUnits unit)
        {
            SizeF size = this.BoundsInfo.GetSize(unit);
            bool flag = size != szSize;
            if (!flag && (szSize.Width <= 0f))
            {
                flag = false;
            }
            if (!flag && (szSize.Height <= 0f))
            {
                flag = false;
            }

            if (flag && (size.Height != szSize.Height))
            {
                flag = EditStyle.CanChangeHeight(this);
            }

            if (flag && (size.Width != szSize.Width))
            {
                flag = EditStyle.CanChangeWidth(this);
            }

            if (flag)
            {
                PointF pinPoint = this.BoundsInfo.GetPinPoint(unit);
                SizeF pinOffset = this.BoundsInfo.GetPinOffset(unit);
                SizeF ef3 = this.BoundsInfo.GetSize(unit);
                float fAngle = Geometry.ConvertToFullCircle(this.RotationAngle);
                float num2 = (ef3.Width != 0f) ? (pinOffset.Width / ef3.Width) : 1f;
                float num3 = (ef3.Height != 0f) ? (pinOffset.Height / ef3.Height) : 1f;
                SizeF szPinOffset = new SizeF(szSize.Width * num2, szSize.Height * num3);
                Matrix matrix = this.GetTransformations(pinPoint, szPinOffset, fAngle);
                this.AppendFlipTransforms(matrix, pinPoint, this.FlipX, this.FlipY);
                flag = this.CheckConstrainingRegion(matrix, pinPoint, szSize);
            }
            return flag;
        }

        private bool CheckNewPinOffset(SizeF szPinOffset, MeasureUnits unit)
        {
            SizeF pinOffset = this.BoundsInfo.GetPinOffset(unit);
            bool flag = pinOffset != szPinOffset;
            if (!flag && (szPinOffset.Width <= 0f))
            {
                flag = false;
            }
            if (!flag && (szPinOffset.Height <= 0f))
            {
                flag = false;
            }
            if (flag && (pinOffset.Width != szPinOffset.Width))
            {
                flag = EditStyle.CanMoveX(this);
            }
            if (flag && (pinOffset.Height != szPinOffset.Height))
            {
                flag = EditStyle.CanMoveY(this);
            }
            if (flag)
            {
                PointF pinPoint = this.BoundsInfo.GetPinPoint(unit);
                SizeF size = this.BoundsInfo.GetSize(unit);
                float fAngle = Geometry.ConvertToFullCircle(this.RotationAngle);
                Matrix matrix = this.GetTransformations(pinPoint, szPinOffset, fAngle);
                this.AppendFlipTransforms(matrix, pinPoint, this.FlipX, this.FlipY);
                flag = this.CheckConstrainingRegion(matrix, pinPoint, size);
            }
            return flag;
        }

        private bool CheckConstrainingRegion(Matrix matrixTransformation, PointF ptPinPoint, SizeF szSize)
        {
            bool flag = true;
            if ((this.Root != null) && this.Root.BoundaryConstraintsEnabled)
            {
                RectangleF bounds = new RectangleF((PointF)Point.Empty, szSize);
                PointF[] pts = new PointF[] { bounds.Location, new PointF(bounds.Right, bounds.Top), new PointF(bounds.Left, bounds.Bottom), new PointF(bounds.Right, bounds.Bottom) };
                PointF[] tfArray2 = new PointF[] { ptPinPoint };
                Matrix parentsTransformations = HandlesHitTesting.GetParentsTransformations(this, false);
                matrixTransformation.Multiply(parentsTransformations, MatrixOrder.Append);
                matrixTransformation.TransformPoints(pts);
                parentsTransformations.TransformPoints(tfArray2);
                bounds = RectangleF.Union(Geometry.CreateRect(pts), new RectangleF(tfArray2[0], SizeF.Empty));
                flag = this.CheckConstrainingRegion(bounds);
            }
            return flag;
        }

        #endregion

        #region IBoundsInfo Members

        public void SetBoundsInfo(RectangleF rect)
        {
            this.UpdateBoundsInfo(rect);
        }

        public RectangleF GetBoundsInfo()
        {
            return this.GetPathBounds();
        }

        public SizeF GetWindowSize()
        {
            return new SizeF(this.Window.Width, this.Window.Height);
        }

        #endregion

        #region ISourceNode Members

        public bool? IsOnline
        {
            get;
            set;
        }

        #endregion
    }

}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Domain.PresentationShow.ShowClient;
using UI.PresentationDesign.DesignUI.Controllers.Interfaces;
using UI.PresentationDesign.DesignUI.Classes.Controller;

namespace UI.PresentationDesign.DesignUI.Controllers
{
    public class DisplayViewer : IDisplayViewer
    {
        private Image m_Image = null;
        private Display m_Display = null;

        private Source m_currentSource = null;

        private bool _isInvoking = false;

        public bool HasImage { get; set; }

        public bool HasLayout { get; set; }

        private bool m_Async = true;

        public event ImageChanged OnImageLoaded;

        public event ImageChanged OnSourceSelected;

        public System.Drawing.Rectangle Pos { get; set; }

        public System.Drawing.RectangleF? SelectedSource { get; set; }

        public bool IsTransformed { get; set; }

        public DisplayViewer(Display ADisplay)
            : this(ADisplay, true)
        {
        }

        public DisplayViewer(Display ADisplay, bool async)
        {
            HasLayout = true;
            m_Display = ADisplay;
            HasImage = true;
            m_Async = async;
            PresentationController.Instance.OnSourceChanged += new CurrentSourceChanged(Instance_OnSourceChanged);
        }

        void Instance_OnSourceChanged(Source newSource)
        {
            if (newSource != m_currentSource)
            {
                Slide currentSlide = PresentationController.Instance.SelectedSlide;
                if (currentSlide == null)
                    return;
                Display disp = currentSlide.DisplayList.Find(d => d.EquipmentType == m_Display.EquipmentType);
                if (disp == null)
                    return;
                Window wnd = disp.WindowList.Where(x => x.Source == newSource).FirstOrDefault();
                if (wnd != null)
                {
                    SelectedSource = new RectangleF(wnd.Left / (float)m_Display.Width, wnd.Top / (float)m_Display.Height, wnd.Width / (float)m_Display.Width, wnd.Height / (float)m_Display.Height);
                    m_currentSource = wnd.Source;
                }
                else
                {
                    SelectedSource = null;
                    m_currentSource = null;
                }
                if (OnSourceSelected != null)
                    OnSourceSelected();
            }
        }

        private delegate Image GetImageDelegate();

        private Image getImageFromServer()
        {
            _isInvoking = true;
            Stream[] streams = ShowClient.Instance.getScreenshot(new DisplayType[] { m_Display.Type });
            if (streams.Length > 0 && streams[0] != null)
                return Image.FromStream(streams[0]);
            return null;
        }

        private void imageLoaded(IAsyncResult res)
        {
            GetImageDelegate gid = (GetImageDelegate)res.AsyncState;
            Image image = gid.EndInvoke(res);
            if (image == null)
            {
                HasImage = false;
                return;
            }
            lock (this)
            {
                m_Image = image;
            }
            if (OnImageLoaded != null)
                OnImageLoaded();
            _isInvoking = false;
        }

        private Image createImage()
        {
            if (!m_Async)
                return getImageFromServer();

            HasLayout = true;
            Slide currSlide = PresentationController.Instance.SelectedSlide;
            Display disp = null;
            if (currSlide != null && (disp = currSlide.DisplayList.Find(x => x.EquipmentType == m_Display.EquipmentType)) == null || disp.WindowList.Count == 0)
            {
                bool background = false;
                if (disp is ActiveDisplay) // Может не быть окон, но быть фон
                {
                    background = !string.IsNullOrEmpty(((ActiveDisplay)disp).BackgroundImage);
                }

                if (!background)
                {
                    m_Image = null;
                    HasLayout = false;
                    return m_Image;
                }
            }
            HasImage = true;
            Image retValue = null;
            GetImageDelegate gid = new GetImageDelegate(this.getImageFromServer);
            gid.BeginInvoke(new AsyncCallback(this.imageLoaded), gid);
            return retValue;
        }

        public Display Display
        {
            get { return m_Display; }
        }

        public void ReloadImage()
        {
            if (m_Image != null)
                m_Image.Dispose(); 
          
            lock (this)
            {
                m_Image = createImage();
            }
        }

        public bool IsPresentationShow
        { 
            get { return ShowClient.Instance.IsShow; } 
        }

        #region IDisplayViewer Members

        public Image getSceenshot()
        {
            lock (this)
            {
                if (HasImage && m_Image == null && !_isInvoking)
                    m_Image = createImage();
                return m_Image;
            }
        }

        public String Name
        {
            get { return m_Display.Type.Name; }
        }

        public void NotifyUserClicked(float x, float y)
        {
            if (x > 1.0f || y > 1.0f || x < 0.0f || y < 0.0f)
                return;
            m_currentSource = null;
            try
            {
                SelectedSource = null;
                x *= this.m_Display.Width;
                y *= this.m_Display.Height;
                Slide currentSlide = PresentationController.Instance.SelectedSlide;
                if (currentSlide == null)
                    return;
                Display disp = currentSlide.DisplayList.Find(d => d.EquipmentType == m_Display.EquipmentType);
                if (disp == null)
                    return;
                byte zOrder = byte.MinValue;
                Window clickedWindow = null;
                foreach (Window wnd in disp.WindowList)
                {
                    if (x >= wnd.Left && x <= wnd.Left + wnd.Width && y >= wnd.Top && y <= wnd.Top + wnd.Height)
                    {
                        if (wnd.ZOrder >= zOrder) // Нашли окно, которое выше ранее найденного
                        {
                            zOrder = wnd.ZOrder;
                            clickedWindow = wnd;
                        }
                    }
                }
                if (clickedWindow != null) // Было найдено какое-то окно
                {
                    SelectedSource = new RectangleF(clickedWindow.Left / (float)m_Display.Width, clickedWindow.Top / (float)m_Display.Height, clickedWindow.Width / (float)m_Display.Width, clickedWindow.Height / (float)m_Display.Height);
                    m_currentSource = clickedWindow.Source;
                }
            }
            finally
            {
                PresentationController.Instance.NotifyCurrentSourceChanged(m_currentSource);
            }
        }
        #endregion
    }
}

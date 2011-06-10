using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using UI.PresentationDesign.DesignUI.Classes.Controller;

namespace UI.PresentationDesign.DesignUI.Model
{
    public class SlideLayout
    {
        List<Display> m_displays;
        Slide m_slide;
        List<Window> m_windowList;
        Display firstDisplay;

        public bool IsEmpty
        {
            get
            {
                return m_slide == null || m_displays == null || m_displays.Count == 0 || firstDisplay == null;
            }
        }

        public Display Display
        {
            get
            {
                return firstDisplay;
            }
        }

        public Slide Slide
        {
            get { return m_slide; }
        }

        public SlideLayout(IEnumerable<Display> ADisplays, Slide ASlide)
        {
            m_slide = ASlide;
            m_displays = new List<Display>();
            foreach (Display d in ADisplays)
            {
                if (!WeContainDisplay(d))
                {
                    AddDisplayToSlide(d);
                }
                else
                {
                    m_displays.Add(FindDisplay(d));
                }
            }

            if (m_displays.Count > 0)
            {
                m_windowList = new List<Window>(m_displays[0].WindowList);
                firstDisplay = m_displays.First();
            }
            else
                m_windowList = new List<Window>();

            UpdateDisplayLayout();
        }


        void AddLayoutToDisplay(Display slideDisplay)
        {
            slideDisplay.WindowList.Clear();
            slideDisplay.WindowList.AddRange(m_windowList);
        }

        Display FindDisplay(Display display)
        {
            return FindDisplay(display, Slide);
        }


        bool WeContainDisplay(Display d)
        {
            return m_slide.DisplayList.Contains(d);
        }

        void AddSourceToSlide(Source source)
        {
            if (!Slide.SourceList.Contains(source))
                Slide.SourceList.Add(source);
        }

        void AddDisplayToSlide(Display d)
        {
            Display newDisplay = d.Type.CreateNewDisplay();
            m_slide.DisplayList.Add(newDisplay);
            m_displays.Add(newDisplay);
        }

        void UpdateDisplayLayout()
        {
            foreach (Display d in m_displays)
            {
                d.WindowList.Clear();
                d.WindowList.AddRange(m_windowList);
            }
        }

        public void ReplaceWindows(Window sourceWnd, Window destWnd)
        {
            int index = m_windowList.IndexOf(sourceWnd);
            if (index > -1)
                m_windowList[index] = destWnd;

            RemoveUnusedSource(sourceWnd.Source);
            AddSourceToSlide(destWnd.Source);
            UpdateDisplayLayout();
        }

        private void RemoveUnusedSource(Source src)
        {
            //if (!m_windowList.Any(w => w.Source.Equals(src)))
            //    Slide.SourceList.Remove(src);
            //необходимо проверить не только для окошек на текущей раскладке, но и то что данный источник не юзается на других дисплеях, так как дисплей может быть перемещенным из группы и иметь общие источники
            List<Display> otherDisplays = new List<Display>(m_slide.DisplayList);
            foreach (Display display in m_displays)
            {
                otherDisplays.RemoveAll(dis => dis.Equals(display));
            }
            if (!m_windowList.Any(w => w.Source.Equals(src)) &&
                !otherDisplays.SelectMany(dis=>dis.WindowList).Any(win=>win.Source.Equals(src)))
            {
                Slide.SourceList.Remove(src);
                RemoveUnusedDevice(src.Device);
            }
        }

        private void RemoveUnusedDevice(Device dev)
        {
            if (dev != null)
            {
                List<Source> srcList = Slide.SourceList.Where(src => src.Device != null && src.Device.Equals(dev)).ToList();
                if (srcList.Count == 0)
                    Slide.DeviceList.Remove(dev);
            }
        }

        public void AppendToLayout(Window window)
        {
            if (!m_windowList.Contains(window))
            {
                m_windowList.Add(window);
                AddSourceToSlide(window.Source);

                UpdateDisplayLayout();
            }
        }

        public void AppendToLayout(Display display, bool clearWindows)
        {
            if (!WeContainDisplay(display))
                AddDisplayToSlide(display);

            display = FindDisplay(display);

            if (!clearWindows)
            {
                foreach (Window w in display.WindowList)
                {
                    AppendToLayout(w);
                }
            }

            AddLayoutToDisplay(display);
            UpdateDisplayLayout();
        }

        public void RemoveFromLayout(Display display)
        {
            display = FindDisplay(display);

            if (WeContainDisplay(display))
                m_displays.Remove(display);

            if (m_displays.Count > 0)
            {
                display.WindowList.Clear();
                foreach (Window wnd in m_windowList)
                {
                    display.WindowList.Add(wnd.Clone());
                }
            }

        }

        public void RemoveWindow(Window window)
        {
            if (m_windowList.Contains(window))
                m_windowList.Remove(window);

            RemoveUnusedSource(window.Source);
            UpdateDisplayLayout();
        }


        #region background

        public static void ApplyDisplayBackground(Slide slide, string path, Display disp)
        {
            foreach (Display d in slide.DisplayList)
            {
                if (d is ActiveDisplay && d.Equals(disp))
                {
                    ((ActiveDisplay)d).BackgroundImage = path;
                }
            }
        }

        public void ApplyDisplayBackground(string path, Display disp)
        {
            foreach (Display d in m_displays)
            {
                if (d is ActiveDisplay && d.Equals(disp))
                {
                    ((ActiveDisplay)d).BackgroundImage = path;
                }
            }
        }
        #endregion

        #region helpers

        public static int GetWindowsCount(Display display, Slide slide)
        {
            Display d = FindDisplay(display, slide);
            if (d == null) return 0;
            return d.WindowList.Count;
        }

        public static Display FindDisplay(Display display, Slide slide)
        {
            var disp = slide.DisplayList.Where(d => d.Equals(display));
            if (disp.Count() > 0)
                return disp.First();
            return null;
        }
        #endregion
    }

    public static class WindowCloner
    {
        public static Window Clone(this Window wnd)
        {
            XmlSerializer cloner = new XmlSerializer(wnd.GetType());
            if (wnd == null) throw new NullReferenceException("wnd");
            StringBuilder sb = new StringBuilder();
            XmlWriter x = XmlWriter.Create(sb);
            cloner.Serialize(x, wnd);
            Window result = (Window)cloner.Deserialize(new StringReader(sb.ToString()));
            result.Source = (Source)wnd.Source.Clone();
            result.Source.Id = result.SourceId = PresentationController.Instance.SourceID.NextID.ToString();

            foreach (Slide slide in PresentationController.Instance.Presentation.SlideList)
            {
                foreach (Display display in slide.DisplayList)
                {
                    foreach (Window window in display.WindowList)
                    {
                        if (!slide.SourceList.Contains(window.Source))
                        {
                            slide.SourceList.Add(window.Source);
                        }

                        if (window == wnd)
                        {
                            if (!slide.SourceList.Contains(result.Source))
                            {
                                slide.SourceList.Add(result.Source);
                            }
                        }
                    }
                }
            }


            return result;
        }

        /// <summary>
        /// простой клон окана - без создания сорсов и т.д - сорс просто присваиватся новому окну.
        /// Clone который сверху создает новый сорс и используется только при исключении дисплея из группы
        /// </summary>
        /// <param name="wnd"></param>
        /// <returns></returns>
        public static Window SimpleClone(this Window wnd)
        {
            XmlSerializer cloner = new XmlSerializer(wnd.GetType());
            if (wnd == null) throw new NullReferenceException("wnd");
            StringBuilder sb = new StringBuilder();
            using (XmlWriter x = XmlWriter.Create(sb))
            {
                cloner.Serialize(x, wnd);
                Window result = (Window)cloner.Deserialize(new StringReader(sb.ToString()));
                result.Source = wnd.Source;
                return result;
            }
        }
    }
}

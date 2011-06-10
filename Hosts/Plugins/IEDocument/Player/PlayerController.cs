using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hosts.Plugins.IEDocument.SystemModule.Design;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Hosts.Plugins.IEDocument.Common;

namespace Hosts.Plugins.IEDocument.Player
{
    internal class PlayerController
    {
        private readonly Source _source = null;
        private readonly IPlayerCommand _playerProvider;

        public PlayerController(Source source, IPlayerCommand playerCommand)
        {
            _source = source;
            _playerProvider = playerCommand;
            IEDocumentSourceDesign ieSource = source as IEDocumentSourceDesign;
            //if (ieSource != null)
            //{
            //    //NumberOfSlides = ieSource.NumberOfSlides;
            //    ZoomProperty = ieSource.Zoom;
            //}
        }

        public int ZoomProperty
        { get; private set; }

        internal int CurrentSlide
        { get; private set; }

        //public int NumberOfSlides
        //{ get; private set; }

        public void UpScroll()
        {
            _playerProvider.DoSourceCommand(_source, IEShowCommand.Up.ToString());
            //string slide = _playerProvider.DoSourceCommand(_source, IEShowCommand.Prev.ToString());
            //CurrentSlide = GetCurrentSlideNumber(slide);
            //return CurrentSlide;
        }

        public void DownScroll()
        {
            _playerProvider.DoSourceCommand(_source, IEShowCommand.Down.ToString());
            //string slide = _playerProvider.DoSourceCommand(_source, IEShowCommand.Next.ToString());
            //CurrentSlide = GetCurrentSlideNumber(slide);
            //return CurrentSlide;
        }

        public void LeftScroll()
        {
            _playerProvider.DoSourceCommand(_source, IEShowCommand.Left.ToString());
        }

        public void RightScroll()
        {
            _playerProvider.DoSourceCommand(_source, IEShowCommand.Right.ToString());
        }

        //public int Goto(int slideNumber)
        //{
        //    string slide = _playerProvider.DoSourceCommand(_source, string.Format("{0}{1}{2}",
        //        IEShowCommand.GoToSlide, Constants.Delimeter, slideNumber));
        //    CurrentSlide = GetCurrentSlideNumber(slide);
        //    return CurrentSlide;
        //}

        //TO DO: Переработать передавать текущую страницу в плагине IE не нужно
        public void GetStatus()
        {
            //string slide = _playerProvider.DoSourceCommand(_source, 
            //    IEShowCommand.Status.ToString());
            //CurrentSlide = GetCurrentSlideNumber(slide);
            string strScale = _playerProvider.DoSourceCommand(_source, IEShowCommand.Zoom.ToString());
            ZoomProperty = GetCurrentZoom(strScale);
        }

        //private int GetCurrentSlideNumber(string slide)
        //{
        //    int slideNumber;
        //    if (Int32.TryParse(slide, out slideNumber))
        //        return slideNumber;
        //    else
        //        return CurrentSlide;
        //}

        public void Zoom(int scale)
        {
            string strScale = _playerProvider.DoSourceCommand(_source, string.Format("{0}{1}{2}",
                IEShowCommand.Zoom, Constants.Delimeter, scale));
        }

        private int GetCurrentZoom(string strScale)
        {
            int scale;
            if (Int32.TryParse(strScale, out scale))
                return scale;
            else
                return ZoomProperty;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hosts.Plugins.WordDocument.Common;
using Hosts.Plugins.WordDocument.SystemModule.Design;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.WordDocument.Player
{
    internal class PlayerController
    {
        private readonly Source _source = null;
        private readonly IPlayerCommand _playerProvider;

        public PlayerController(Source source, IPlayerCommand playerCommand)
        {
            _source = source;
            _playerProvider = playerCommand;
            WordDocumentSourceDesign docSource = source as WordDocumentSourceDesign;
            if (docSource != null)
            {
                NumberOfPages = docSource.NumberOfPages;
                StartZoom = docSource.StartZoom;
            }
        }

        internal int CurrentSlide
        { get; private set; }

        public int NumberOfPages
        { get; private set; }

        public int StartZoom
        { get; private set; }

        public int Prev()
        {
            string slide = _playerProvider.DoSourceCommand(_source, WordShowCommand.Prev.ToString());
            CurrentSlide = GetCurrentSlideNumber(slide);
            return CurrentSlide;
        }

        public int Next()
        {
            string slide = _playerProvider.DoSourceCommand(_source, WordShowCommand.Next.ToString());
            CurrentSlide = GetCurrentSlideNumber(slide);
            return CurrentSlide;
        }

        public int LeftScroll()
        {
            string slide = _playerProvider.DoSourceCommand(_source, WordShowCommand.Left.ToString());
            CurrentSlide = GetCurrentSlideNumber(slide);
            return CurrentSlide;
        }

        public int RightScroll()
        {
            string slide = _playerProvider.DoSourceCommand(_source, WordShowCommand.Right.ToString());
            CurrentSlide = GetCurrentSlideNumber(slide);
            return CurrentSlide;
        }
        
        public int PrevPage()
        {
            string slide = _playerProvider.DoSourceCommand(_source, WordShowCommand.PrevPage.ToString());
            CurrentSlide = GetCurrentSlideNumber(slide);
            return CurrentSlide;
        }

        public int NextPage()
        {
            string slide = _playerProvider.DoSourceCommand(_source, WordShowCommand.NextPage.ToString());
            CurrentSlide = GetCurrentSlideNumber(slide);
            return CurrentSlide;
        }

        public int LastPage()
        {
            string slide = _playerProvider.DoSourceCommand(_source, WordShowCommand.LastPage.ToString());
            CurrentSlide = GetCurrentSlideNumber(slide);
            return CurrentSlide;
        }

        public int FirstPage()
        {
            string slide = _playerProvider.DoSourceCommand(_source, WordShowCommand.FirstPage.ToString());
            CurrentSlide = GetCurrentSlideNumber(slide);
            return CurrentSlide;
        }
        

        public int Goto(int pageNumber)
        {
            string page = _playerProvider.DoSourceCommand(_source, string.Format("{0}{1}{2}",
                WordShowCommand.GoToPage, Constants.Delimeter, pageNumber));
            CurrentSlide = GetCurrentSlideNumber(page);
            return CurrentSlide;
        }

        public void Zoom(int scale)
        {
            string strScale = _playerProvider.DoSourceCommand(_source, string.Format("{0}{1}{2}",
                WordShowCommand.Zoom, Constants.Delimeter, scale));
        }

        public void GetStatus()
        {
            string slide = _playerProvider.DoSourceCommand(_source, 
                WordShowCommand.Status.ToString());
            CurrentSlide = GetCurrentSlideNumber(slide);
          
            string strScale = _playerProvider.DoSourceCommand(_source, WordShowCommand.Zoom.ToString());
            StartZoom = GetCurrentZoom(strScale);
        }

        private int GetCurrentSlideNumber(string slide)
        {
            int slideNumber;
            if (Int32.TryParse(slide, out slideNumber))
                return slideNumber;
            else
                return CurrentSlide;
        }

        private int GetCurrentZoom(string strScale)
        {
            int scale;
            if (Int32.TryParse(strScale, out scale))
                return scale;
            else
                return StartZoom;
        }
    }
}

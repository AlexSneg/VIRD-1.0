using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hosts.Plugins.PowerPointPresentation.Common;
using Hosts.Plugins.PowerPointPresentation.SystemModule.Design;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Hosts.Plugins.PowerPointPresentation.Player
{
    internal class PlayerController
    {
        private readonly Source _source = null;
        private readonly IPlayerCommand _playerProvider;

        public PlayerController(Source source, IPlayerCommand playerCommand)
        {
            _source = source;
            _playerProvider = playerCommand;
            PowerPointPresentationSourceDesign pptSource = source as PowerPointPresentationSourceDesign;
            if (pptSource != null)
            {
                NumberOfSlides = pptSource.NumberOfSlides;
            }
        }

        internal int CurrentSlide
        { get; private set; }

        public int NumberOfSlides
        { get; private set; }

        public int Prev()
        {
            string slide = _playerProvider.DoSourceCommand(_source, PowerPointShowCommand.PrevSlide.ToString());
            CurrentSlide = GetCurrentSlideNumber(slide);
            return CurrentSlide;
        }

        public int Next()
        {
            string slide = _playerProvider.DoSourceCommand(_source, PowerPointShowCommand.NextSlide.ToString());
            CurrentSlide = GetCurrentSlideNumber(slide);
            return CurrentSlide;
        }

        public int Goto(int slideNumber)
        {
            string slide = _playerProvider.DoSourceCommand(_source, string.Format("{0}{1}{2}",
                PowerPointShowCommand.GoToSlide, Constants.Delimeter, slideNumber));
            CurrentSlide = GetCurrentSlideNumber(slide);
            return CurrentSlide;
        }

        public void GetStatus()
        {
            string slide = _playerProvider.DoSourceCommand(_source, 
                PowerPointShowCommand.Status.ToString());
            CurrentSlide = GetCurrentSlideNumber(slide);
        }

        private int GetCurrentSlideNumber(string slide)
        {
            int slideNumber;
            if (Int32.TryParse(slide, out slideNumber))
                return slideNumber;
            else
                return CurrentSlide;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace Domain.PresentationShow.ShowService
{
    internal interface IShowDisplayAndDeviceCommand
    {
        void ComposeCommandAndSendToControllerForAllDevice(Slide prevSlide, Slide currentSlide);
        void ComposeCommandAndSendToControllerForDevice(Device device, Slide prevSlide, Slide currentSlide);
        void ShowDisplays(string presentationUniqueName, Slide slide);
        void ShowDisplay(Display display, string presentationUniqueName);
    }
}

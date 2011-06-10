using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using UI.PresentationDesign.DesignUI.Controls.Equipment;
using UI.PresentationDesign.DesignUI.Services;

namespace UI.PresentationDesign.DesignUI.History
{
    public interface IGetSlide
    {
        Slide GetSlideById(int slideId);
    }

    public class SlideDeviceHistoryCommand : IUndoRedoAction
    {
        private readonly int _slideId;
        private readonly Device _device;
        private readonly IGetSlide _getSlide;
        private readonly bool _added;

        public SlideDeviceHistoryCommand(IGetSlide getSlide, int slideId, Device device, bool wasAdded)
        {
            _slideId = slideId;
            _device = device;
            _getSlide = getSlide;
            _added = wasAdded;
        }

        public int SlideId
        {
            get { return _slideId; }
        }

        public Device Device
        {
            get { return _device; }
        }

        public bool WasAdded
        {
            get
            {
                //if (_wasUndo.HasValue)
                //{
                    
                //}
                return _added;
            }
        }

        #region Implementation of ISimpleUndoRedoAction

        public void Undo()
        {
            Slide slide = _getSlide.GetSlideById(_slideId);
            if (slide == null) return;
            Device device = slide.DeviceList.Find(dev => dev.Equals(_device));

            if (_added)
            {
                if (device == null) return;
                slide.DeviceList.Remove(device);
            }
            else
            {
                if (device != null) return;
                slide.DeviceList.Add(_device);
            }
        }

        public void Redo()
        {
            Slide slide = _getSlide.GetSlideById(_slideId);
            if (slide == null) return;
            Device device = slide.DeviceList.Find(dev => dev.Equals(_device));

            if (_added)
            {
                if (device != null) return;
                slide.DeviceList.Add(_device);
            }
            else
            {
                if (device == null) return;
                slide.DeviceList.Remove(device);
            }
        }

        public bool CanUndo()
        {
            Slide slide = _getSlide.GetSlideById(_slideId);
            if (slide == null) return true;
            return slide.IsLocked;
        }

        public bool CanRedo()
        {
            Slide slide = _getSlide.GetSlideById(_slideId);
            if (slide == null) return true;
            return slide.IsLocked;
        }

        #endregion

        #region Implementation of IUndoRedoAction

        public string Tag
        {
            get { return string.Format("{0} устройства {1}", _added ? "Добавление" : "Удаление", _device.Name); }
        }

        public object Target
        {
            get { return this; }
        }

        #endregion
    }
}

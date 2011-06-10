using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.PresentationDesign.Client;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Domain.PresentationShow.ShowClient;
using UI.PresentationDesign.DesignUI.Controls;
using UI.PresentationDesign.DesignUI.History;

namespace UI.PresentationDesign.DesignUI.Controllers
{
    public class EquipmentController : IGetSlide
    {
        static EquipmentController _instance;
        List<DeviceType> _deviceTypes;

        public List<DeviceType> DeviceTypes
        {
            get { return _deviceTypes; }
        }

        public EquipmentControl Control { get; set; }

        public static EquipmentController Instance
        {
            get
            {
                return _instance;
            }
        }

        public EquipmentController()
        {
            _instance = this;
            _deviceTypes = PresentationController.Configuration.ModuleConfiguration.DeviceList.OrderBy(d => d.Name).ToList();
        }


        public static EquipmentController CreateEquipmentController()
        {
            return new EquipmentController();
        }


        public bool? IsOnline(Device dev)
        {
            return ShowClient.Instance.IsOnLine(dev.Type);
        }

        #region Implementation of IGetSlide

        public Slide GetSlideById(int slideId)
        {
            return PresentationController.Instance.GetAllSlides().SingleOrDefault(sl => sl.Id == slideId);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace TechnicalServices.Persistence.CommonPresentation
{
    public class StandAlonePresentationDAL : PresentationDAL
    {
        public StandAlonePresentationDAL(IConfiguration configuration) : base(configuration)
        {
        }

        #region override
        public override TechnicalServices.Persistence.SystemPersistence.Presentation.PresentationInfo GetPresentationInfo(string uniqueName)
        {
            PresentationInfo presentationInfo = base.GetPresentationInfo(uniqueName);

            if (presentationInfo!=null && IsPresentationExists(presentationInfo))
                return presentationInfo;
            else
                return null;
        }

        public override IList<PresentationInfo> GetPresentationInfoList()
        {
            List<PresentationInfo> newList = new List<PresentationInfo>();
            IList<PresentationInfo> presentationInfos = base.GetPresentationInfoList();
            foreach (PresentationInfo info in presentationInfos)
            {
                if (IsPresentationExists(info))
                {
                    newList.Add(info);
                }
            }
            return newList;
        }

        protected override bool IsPresentationExists(PresentationInfo presentationInfo)
        {
            bool isExists = base.IsPresentationExists(presentationInfo);
            if (!isExists)
            {
                _presentationStorage.Remove(presentationInfo.UniqueName);
                _presentationStorageByName.Remove(presentationInfo.Name);
            }
            return isExists;
        }
        #endregion

        #region public
        #endregion

    }
}

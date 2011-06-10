using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using Domain.PresentationDesign.Client;

namespace UI.PresentationDesign.DesignUI.Classes.Helpers
{
    public class PresentationStatusInfo
    {
        public PresentationStatus Status
        {
            get;
            set;
        }

        public UserIdentity Identity
        {
            get;
            set;
        }

        public static PresentationStatusInfo GetPresentationStatusInfo(string UniqueName)
        {
            UserIdentity uid;
            PresentationStatus s = DesignerClient.Instance.PresentationWorker.GetPresentationStatus(UniqueName, out uid);

            return new PresentationStatusInfo { Identity = uid, Status = s };
        }

        public static string GetPresentationStatusDescr(PresentationInfo info)
        {
            return GetPresentationStatusDescr(info.UniqueName, info.Name);
        }

        public static string GetPresentationStatusDescr(string UniqueName, string Name)
        {
            PresentationStatusInfo info = GetPresentationStatusInfo(UniqueName);
            return GetPresentationStatusDescr(Name, info.Status, info.Identity);
        }

        public static string GetPresentationStatusDescr(Presentation p)
        {
            return GetPresentationStatusDescr(p.UniqueName, p.Name);
        }

        public static string GetPresentationStatusDescr(PresentationInfo info, PresentationStatus status, UserIdentity id)
        {
            return GetPresentationStatusDescr(info.Name, status, id);
        }

        public static string GetPresentationStatusDescr(string Name, PresentationStatus status, UserIdentity id)
        {
            StringBuilder sb = new StringBuilder();
            string userName = string.Empty;
            if ((id != null) && (id.User != null))
                userName = string.IsNullOrEmpty(id.User.FullName) ? id.User.Name : id.User.FullName;
            switch (status)
            {
                case PresentationStatus.Deleted: sb.Append("Сценарий уже удален"); break;
                case PresentationStatus.LockedForEdit: sb.AppendFormat("Сценарий заблокирован пользователем {0} для редактирования", userName); break;
                case PresentationStatus.LockedForShow: sb.AppendFormat("Сценарий заблокирован пользователем {0} для показа", userName); break;
                case PresentationStatus.SlideLocked: sb.Append("Сцена сценария заблокирована"); break;
                case PresentationStatus.Unknown: sb.Append("Неизвестно"); break;
                case PresentationStatus.AlreadyLocallyOpened: sb.Append("Сценарий уже открыт в другом экземпляре Дизайнера"); break;
            }

            return sb.ToString();
        }

        static string GetSlideLockingInfoDescr(string name, LockingInfo info, bool useName)
        {
            StringBuilder sb = new StringBuilder();
            if (useName)
                sb.AppendFormat("Сцена {0}", name);
            else
                sb.Append("Сцена");

            if (info != null)
            {
                sb.Append(" заблокирована ");
                switch (info.RequireLock)
                {
                    case RequireLock.ForEdit: sb.Append("для редактирования"); break;
                    case RequireLock.ForShow: sb.Append("для показа"); break;
                }

                sb.AppendFormat(" пользователем {0}", string.IsNullOrEmpty(info.UserIdentity.User.FullName) ? info.UserIdentity.User.Name : info.UserIdentity.User.FullName);
            }
            else
                sb.Append(" не заблокирована");

            return sb.ToString();
        }

        public static string GetSlideLockingInfoDescr(string name, LockingInfo info)
        {
            return GetSlideLockingInfoDescr(name, info, true);
        }


        public static string GetSlideLockingInfoDescr(LockingInfo info)
        {
            return GetSlideLockingInfoDescr(string.Empty, info, false);
        }
    }
}

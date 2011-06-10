using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Diagram;

namespace UI.PresentationDesign.DesignUI.Classes.History
{
    public static class CommandDescr
    {
        const string movehandle = "Move Handle";
        const string removestr = "Remove";
        const string insertstr = "Insert";
        const string setstr = "Set";
        public const string UpdateDefLinkDescr = "Update default link";
        public const string EditSlideDescr = "Edit slide properties";
        public const string CreateLinkDescr = "Insert link";
        public const string RemoveSelectedDescr = "RemoveSelected";
        public const string PasteElements = "PasteElements";

        public static bool IsUpdateDefLink(string descr)
        {
            return descr.Equals(UpdateDefLinkDescr);
        }

        public static bool IsMoveHandleCmd(string descr)
        {
            return descr.Equals(movehandle);
        }

        public static bool IsRemoveCmd(string descr)
        {
            return descr.Contains(removestr);
        }

        public static bool IsInsertCmd(string descr)
        {
            return descr.Contains(insertstr);
        }

        public static bool IsEditSlide(string descr)
        {
            return descr.Equals(EditSlideDescr);
        }

        private static bool IsPasteElements(string descr)
        {
            return descr.Equals(PasteElements);
        }

        public static bool IsChangeSmth(string undoCmd)
        {
            return undoCmd.Contains(setstr);
        }

        public static bool IsCreateLinkStart(string p)
        {
            return p.Equals(CreateLinkDescr);
        }

        public static bool IsKnownHistoryCommand(string descr)
        {
            return IsUpdateDefLink(descr) || IsRemoveCmd(descr) || IsInsertCmd(descr) || IsEditSlide(descr) || IsPasteElements(descr);
        }

    }
}

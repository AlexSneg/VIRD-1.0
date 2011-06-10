using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using System.Windows.Forms;
using UI.PresentationDesign.DesignUI.Forms;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using UI.PresentationDesign.DesignUI.Helpers;

namespace UI.PresentationDesign.DesignUI.Controllers
{
    public enum ItemToSearch
    {
        Slide,
        Display,
        Device,
        LocalSource,
        GlobalSource,
        HardwareSource
    }

    public class FindItemController
    {
        private static FindItemController _instance = new FindItemController();

        private FindItemController()
        {
        }

        List<object> lastFound = new List<object>();
        int lastPos = -1;

        #region bindable properties
        private String name;
        private String comment;
        private String author;
        //private String[] keywords;
        //private String[] commentKeywords;
        //private String[] authorKeywords;
        private String keywords;
        private String commentKeywords;
        private String authorKeywords;

        public bool FindSlides { get; set; }
        public bool FindSources { get; set; }
        public bool FindDisplays { get; set; }
        public bool FindDevices { get; set; }
        public bool FindLocalSources { get; set; }
        public bool FindGlobalSources { get; set; }
        public bool FindHardwareSources { get; set; }

        public String FindName
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    populateKeywords();
                }
            }
        }
        public String Comment
        {
            get { return comment; }
            set
            {
                if (comment != value)
                {
                    comment = value;
                    populateKeywords();
                }
            }
        }

        public String Author
        {
            get { return author; }
            set
            {
                if (author != value)
                {
                    author = value;
                    populateKeywords();
                }
            }
        }
        #endregion

        private void populateKeywords()
        {
            ClearState();
            if (name != null)
                //keywords = name.Split(new[] { " ", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                keywords = name.Trim().ToLower();
            if (comment != null)
                //commentKeywords = comment.Split(new[] { " ", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                commentKeywords = comment.Trim().ToLower();
            if (author != null)
                //authorKeywords = author.Split(new[] { " ", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                authorKeywords = author.Trim().ToLower();
        }

        public void ClearState()
        {
            lastFound.Clear();
            lastPos = -1;
        }

        public static FindItemController Instance
        {
            get { return _instance; }
        }

        public void ShowSearchForm(ItemToSearch itemToSearch)
        {
            CheckUnSavePresentation();
            FindDevices = FindDisplays = FindGlobalSources = FindHardwareSources = FindLocalSources =
                                                                                   FindSlides = false;
            switch(itemToSearch)
            {
                case ItemToSearch.Device:
                    this.FindDevices = true;
                    break;
                case ItemToSearch.Display:
                    this.FindDisplays = true;
                    break;
                case ItemToSearch.GlobalSource:
                    this.FindGlobalSources = true;
                    break;
                case ItemToSearch.HardwareSource:
                    this.FindHardwareSources = true;
                    break;
                case ItemToSearch.LocalSource:
                    this.FindLocalSources = true;
                    break;
                case ItemToSearch.Slide:
                    this.FindSlides = true;
                    break;
            }
            FindItemForm frm = new FindItemForm(this);
            frm.ShowDialog();
        }

        private void CheckUnSavePresentation()
        {
            if (PresentationController.Instance.PresentationChanged)
            {
                DialogResult dialogResult = MessageBoxExt.Show(
                    "В сценарии есть несохраненные изменения.\r\nСохранить сценарий для корректного выполнения поиска?",
                    "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, new string[] {"Да", "Нет"});
                if (dialogResult == DialogResult.OK)
                    PresentationController.Instance.SavePresentation();
            }
        }

        public bool Find()
        {
            if (this.FindLocalSources)
                return FindLocalSourcesFunc();
            if (this.FindGlobalSources)
                return FindGlobalSourcesFunc();
            if (this.FindHardwareSources)
                return FindHardwareSourcesFunc();
            if (this.FindDisplays)
                return FindDisplaysFunc();
            if (this.FindSlides)
                return FindSlidesFunc();
            if (this.FindDevices)
                return FindDevicesFunc();
            return false;
        }

        public bool FindLocalSourcesFunc()
        {
            if (lastFound.Count == 0)
                foreach (var s in SourcesController.Instance.GetResources(true))
                    if (checkString(s.ResourceInfo.Name) && checkString(s.ResourceInfo.Comment, true))
                        lastFound.Add(s);

            if (lastFound.Count == 0)
                return false;
            if (lastPos < lastFound.Count - 1)
                SourcesController.Instance.SelectSource(lastFound[++lastPos] as ResourceDescriptor, true);
            else
                SourcesController.Instance.SelectSource(lastFound[(lastPos = 0)] as ResourceDescriptor, true);
            return true;
        }

        public bool FindGlobalSourcesFunc()
        {
            if (lastFound.Count == 0)
                foreach (var s in SourcesController.Instance.GetResources(false).Where(x => !x.ResourceInfo.IsHardware))
                    if (checkString(s.ResourceInfo.Name) && checkString(s.ResourceInfo.Comment, true))
                        lastFound.Add(s);
            if (lastFound.Count == 0)
                return false;
            if(lastPos < lastFound.Count - 1)
                SourcesController.Instance.SelectSource(lastFound[++lastPos] as ResourceDescriptor, false);
            else
                SourcesController.Instance.SelectSource(lastFound[(lastPos = 0)] as ResourceDescriptor, false);
            return true;
        }

        public bool FindHardwareSourcesFunc()
        {
            if(lastFound.Count == 0)
                foreach (var s in SourcesController.Instance.GetResources(false).Where(x => x.ResourceInfo.IsHardware))
                    if (checkString(s.ResourceInfo.Name) && checkString(s.ResourceInfo.Comment, true))
                        lastFound.Add(s);
            if(lastFound.Count == 0)
                return false;
            if(lastPos < lastFound.Count - 1)
                SourcesController.Instance.SelectSource(lastFound[++lastPos] as ResourceDescriptor, false);
            else
                SourcesController.Instance.SelectSource(lastFound[(lastPos = 0)] as ResourceDescriptor, false);
            return true;
        }

        public bool FindDevicesFunc()
        {
            if(lastFound.Count == 0)
                foreach (var d in EquipmentController.Instance.DeviceTypes.Where(dv => dv.Visible))
                    if (checkString(d.Name) && checkString(d.Comment, true))
                        lastFound.Add(d);

            if(lastFound.Count == 0)
                return false;
            if(lastPos < lastFound.Count - 1)
                EquipmentController.Instance.Control.SelectDevice(lastFound[++lastPos] as DeviceType);
            else
                EquipmentController.Instance.Control.SelectDevice(lastFound[(lastPos = 0)] as DeviceType);
            return true;
        }

        public bool FindSlidesFunc()
        {
            if (lastFound.Count == 0)
                foreach (var s in PresentationController.Instance.Presentation.SlideList)
                {
                    if (checkAuthor(s.Author) && checkString(s.Name) && checkString(s.Comment, true))
                        lastFound.Add(s);
                }
            if(lastFound.Count == 0)
                return false;
            if(lastPos < lastFound.Count - 1)
                SlideGraphController.Instance.SelectSlide(lastFound[++lastPos] as Slide);
            else
                SlideGraphController.Instance.SelectSlide(lastFound[(lastPos = 0)] as Slide);
            return true;
        }

        public bool FindDisplaysFunc()
        {
            if (lastFound.Count == 0)
            {
                foreach (var d in DisplayController.Instance.UngrouppedDisplays())
                    if (checkString(d.Name) && checkString(d.Comment, true))
                        lastFound.Add(d);
                foreach (var gr in DisplayController.Instance.GrouppedDisplays())
                    foreach (var d in gr.Value)
                        if (checkString(d.Name) && checkString(d.Comment, true))
                            lastFound.Add(d);
            }
            if (lastFound.Count == 0)
                return false;
            if(lastPos < lastFound.Count - 1)
                DisplayController.Instance.ChangeSelectedDisplay(lastFound[++lastPos] as Display);
            else
                DisplayController.Instance.ChangeSelectedDisplay(lastFound[(lastPos = 0)] as Display);
            return true;
        }

        private bool checkAuthor(string source)
        {
            if (string.IsNullOrEmpty(authorKeywords)) //если строка поиска пустая, значит и искать ничего не надо
                return true;
            else
                return source.ToLower().Contains(authorKeywords);
        }

        private bool checkString(String str)
        {
            return checkString(str, false);
        }

        private bool checkString(String source, bool comment)
        {
            String words = comment ? commentKeywords : keywords;
            if (string.IsNullOrEmpty(words)) //если строка поиска пустая, то ничего и не ищем
                return true;
            else
            {
                if (string.IsNullOrEmpty(source)) //если в та строка где ищем пустая, а то что ищем нет, значит и не найдем ничего
                    return false;
                else
                    return source.ToLower().Contains(words);
            }
        }
    }
}

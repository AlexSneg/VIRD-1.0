using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Configuration.Common;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using UI.PresentationDesign.DesignUI.Controls.DisplayList;
using Syncfusion.Windows.Forms;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using UI.PresentationDesign.DesignUI.Forms;
using UI.PresentationDesign.DesignUI.Helpers;
using System.ComponentModel;
using UI.PresentationDesign.DesignUI.Model;
using Domain.PresentationShow.ShowClient;
using TechnicalServices.Persistence.CommonPersistence.Presentation;

namespace UI.PresentationDesign.DesignUI.Controllers
{
    internal delegate void DisplayGroupCreated(DisplayGroup newGroup);

    public delegate void EnableCheckboxesChanged(bool isEnabled);
    public delegate bool DisplayChecked(Display disp, bool isChecked);
    public delegate void DisplayStateChanged(Display disp, bool? isOnline);

    public class DisplayController : IDisposable
    {
        #region events
        internal event DisplayGroupCreated OnDisplayGroupCreated;

        public event Action<Display> OnSelectedDisplayChanged;
        #endregion

        #region Player events
        /// <summary>
        /// Срабатывает, когда разрешается отображение чекбоксов в списке дисплеев
        /// </summary>
        public event EnableCheckboxesChanged OnEnableCheckboxes;
        /// <summary>
        /// Срабатывает, когда пользователь включил/выключил галочку в списке дисплеев. На него подписан MonitoringControl
        /// </summary>
        public event DisplayChecked OnDisplayChecked;
        /// <summary>
        /// Оповещение извне, что нужно поставить/снять галочку у какого-либо дисплея в списке
        /// </summary>
        public event DisplayChecked OnDisplayForceChecked;
        /// <summary>
        /// Оповещение для контрола о том, что доступность дисплея изменилась
        /// </summary>
        public event DisplayStateChanged OnDisplayStateChanged;
        #endregion

        #region fields & properties
        static DisplayController _instance = null;
        CommonConfiguration _config;

        public Display CurrentDisplay { get; set; }
        public DisplayGroup CurrentDisplayGroup { get; set; }

        //все дисплеи
        List<Display> Displays = new List<Display>();
        public static DisplayController Instance
        {
            get
            {
                if (_instance == null)
                    return CreateDisplayController();
                return _instance;
            }
        }
        #endregion

        #region ctor & factory
        public DisplayController()
        {
            _config = PresentationController.Configuration;
            _instance = this;
            //if (IsPlayerMode)
            ShowClient.Instance.OnEquipmentStateChanged += Instance_OnEquipmentStateChanged;

            //PresentationController.Instance.OnPresentationChangedExternally += new PresentationDataChanged(Instance_OnPresentationChangedExternally);
        }

        //void Instance_OnPresentationChangedExternally()
        //{
        //}

        public static DisplayController CreateDisplayController()
        {
            return new DisplayController();
        }
        #endregion

        #region Player

        public static bool IsPlayerMode { get; set; }

        Dictionary<Display, bool?> _displayStates = new Dictionary<Display, bool?>();


        void Instance_OnEquipmentStateChanged(EquipmentType equipmentType, bool? isOnLine)
        {
            //https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-997
            Display disp = this.Displays.Where(x => x.Type.Equals(equipmentType)/*x => x.Type.GetType() == equipmentType.GetType()*/).FirstOrDefault();
            if (OnDisplayStateChanged != null && disp != null)
                OnDisplayStateChanged(disp, isOnLine);
        }

        public Dictionary<Display, bool?> DisplayStates
        {
            get { return _displayStates; }
        }

        /// <summary>
        /// Разрешить отображение чекбоксов в списке дисплеев
        /// </summary>
        /// <param name="isEnabled"></param>
        public void EnableCheckboxes(bool isEnabled)
        {
            if (OnEnableCheckboxes != null)
                OnEnableCheckboxes(isEnabled);
        }
        /// <summary>
        /// Обработчик включения/выключения галочки дисплея пользователем. Оповещает всех своих подписчиков
        /// </summary>
        /// <param name="disp">Выбранный дисплей</param>
        /// <param name="isChecked">Включено/выключено</param>
        /// <returns>true, если выбранный дисплей можно включить, иначе изменение будет отменено</returns>
        public bool DisplayChecked(Display disp, bool isChecked)
        {
            if (OnDisplayChecked != null)
                return OnDisplayChecked(disp, isChecked);
            return true;
        }
        /// <summary>
        /// Обработчик включения/выключения галочки дисплея извне. Оповещает свой контрол
        /// </summary>
        /// <param name="displayName">Дисплей</param>
        /// <param name="isChecked">Новое состояние</param>
        /// <returns>не используется</returns>
        public bool ForceCheckDisplay(String displayName, bool isChecked)
        {
            Display checkedDisp = Displays.Find(x => x.Type.Name == displayName);
            if (OnDisplayForceChecked != null && checkedDisp != null)
                OnDisplayForceChecked(checkedDisp, isChecked);
            return true;
        }
        /// <summary>
        /// Обработчик включения галочки для группы извне. Оповещает свой контол. Возвращает список дисплеев в группе
        /// </summary>
        /// <param name="node">Нода группы</param>
        /// <returns>Список дисплеев в группе</returns>
        public List<Display> ForceCheckGroup(DisplayGroupNode node)
        {
            List<Display> displays = this.DisplayByGroup(node.DisplayGroup);
            if (OnDisplayForceChecked != null)
                foreach (Display disp in displays)
                    OnDisplayForceChecked(disp, true);
            return displays;
        }
        #endregion

        #region Initialization
        public void InitDisplayController()
        {
            CreateDisplayList();
        }
        #endregion

        #region Display routine
        public void SelectDisplay(Display ADisplay)
        {
            if (CurrentDisplay != ADisplay)
            {
                CurrentDisplay = ADisplay;
                if (ADisplay != null)
                    CurrentDisplayGroup = null;

                if (CurrentDisplay != null)
                {
                    //проверить вхождение дисплея в группу
                    var group = GrouppedDisplays().Where(d => d.Value.Contains(CurrentDisplay)).Select(d => d.Key);
                    if (group.Count() > 0)
                    {
                        SelectDisplayGroup(group.First());
                        return;
                    }
                }

                PresentationController.Instance.SelectedDisplay = CurrentDisplay;
                ChangeSelectedDisplay(ADisplay);
            }
        }

        public void SelectDisplayGroup(DisplayGroup ADisplayGroup)
        {
            if (CurrentDisplayGroup != ADisplayGroup)
            {
                CurrentDisplay = null;
                CurrentDisplayGroup = ADisplayGroup;
                PresentationController.Instance.SelectedDisplayGroup = DisplayByGroup(CurrentDisplayGroup);
            }
        }

        public List<Display> CreateDisplayList()
        {
            var result = _config.ModuleConfiguration.DisplayList.Select((d) =>
                {
                    Display res = d.CreateNewDisplay();
                    DisplayGroup group = GetDisplayGroup(res);
                    if (null != group)
                        res.DisplayGroup = group.Name;
                    return res;
                }).ToList();

            Displays = result;
            //if (IsPlayerMode)
            foreach (Display disp in Displays)
            {
                _displayStates[disp] = ShowClient.Instance.IsOnLine(disp.Type);
            }
            return result;
        }

        public string GetNewGroupName()
        {
            int id = 0;
            string name = String.Empty;
            while (true)
            {
                id++;
                name = Description.DisplayGroup + id;
                if (!PresentationController.Instance.Presentation.DisplayGroupList.Any(g => g.Name.Equals(name)))
                    break;
            }

            return name;
        }

        public void CreateDisplayGroup()
        {
            DisplayGroup newGroup = new DisplayGroup { Name = GetNewGroupName() };
            PresentationController.Instance.Presentation.DisplayGroupList.Add(newGroup);

            if (OnDisplayGroupCreated != null)
                OnDisplayGroupCreated(newGroup);

            PresentationController.Instance.PresentationChanged = true;
        }

        public void RemoveDisplayGroup(DisplayGroup displayGroup)
        {
            SlideLayout layout = PresentationController.Instance.CurrentSlideLayout;
            DisplayByGroup(displayGroup).ForEach(layout.RemoveFromLayout);
            DisplayByGroup(displayGroup).ForEach(d => d.DisplayGroup = null);
            PresentationController.Instance.Presentation.DisplayGroupList.Remove(displayGroup);
            PresentationController.Instance.PresentationChanged = true;
        }

        public bool AddDisplayToGroup(DisplayGroup displayGroup, Display display)
        {
            ISegmentationSupport s = display as ISegmentationSupport;

            if (s != null && (s.SegmentColumns > 1 || s.SegmentRows > 1))
            {
                MessageBoxExt.Show("Многосегментный дисплей не может быть помещен в группу", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            List<Slide> changedSlides = new List<Slide>();

            bool clear = false;
            if (displayGroup.DisplayNameList.Count == 0)
            {
                displayGroup.Width = display.Width;
                displayGroup.Height = display.Height;
                displayGroup.Type = display.Type.Type;
                display.DisplayGroup = displayGroup.Name;
                PresentationController.Instance.PresentationChanged = true;
            }
            else
            {
                if (display.Width == displayGroup.Width && display.Height == displayGroup.Height && display.Type.Type == displayGroup.Type)
                {
                    if (displayGroup.DisplayNameList.Contains(display.Type.Name))
                        return false;

                    if (!SlideGraphController.Instance.SaveNewSlides("Имеются несохраненные сцены.\r\nДля продолжения необходимо сохранить изменения.\r\nСохранить новые сцены?"))
                        return false;

                    //https://sentinel2.luxoft.com/sen/issues/browse/PMEDIAINFOVISDEV-861
                    //Если хотя бы одна сцена в сценарии заблокирована другим пользователем, то при попытке поместить дисплей в непустую группу дисплеев, система должна выводить сообщение: «Перемещение дисплея <Название дисплея> в группу <Название группы> невозможно: в сценарии есть заблокированные сцены. Для перемещения дисплея в непустую группу необходимо разблокировать все сцены.» (кнопка «ОК»)
                    //Display anyDisplay = DisplayByGroup(displayGroup).First();
                    foreach (Slide slide in PresentationController.Instance.GetAllSlides())
                    {
                        // НЕ проверяем сцены для которых есть непустая раскладка как на дисплее из группы итак и для дисплея который перемещается в группу
                        //if (SlideLayout.GetWindowsCount(anyDisplay, slide) > 0 || SlideLayout.GetWindowsCount(display, slide) > 0)
                        {
                            changedSlides.Add(slide);
                            LockingInfo lockingInfo = PresentationController.Instance.GetSlideLockingInfo(slide);
                            if (lockingInfo != null && !lockingInfo.UserIdentity.Equals(PresentationController.Instance.UserIdentity))
                            {
                                MessageBoxExt.Show(
                                    string.Format(
                                        "Перемещение дисплея {0} в группу {1} невозможно: в сценарии есть заблокированные сцены. Для перемещения дисплея в непустую группу необходимо разблокировать все сцены.",
                                        display.Name, displayGroup.Name),
                                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return false;
                            }
                        }
                    }

                    Slide[] allSlides = PresentationController.Instance.GetAllSlides();
                    for (int i = 0; i < allSlides.Length; i++)
                    {
                        if (SlideLayout.GetWindowsCount(display, allSlides[i]) > 0)
                        {
                            if (MessageBoxExt.Show(String.Format("У дисплея {0} есть настроенные раскладки сцен.\r\nПри перемещении дисплея ему будут установлены раскладки группы {1}?", display.Type.Name, displayGroup.Name), "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, new string[] { "Продолжить", "Отмена" }) == DialogResult.Cancel)
                                return false;
                            clear = true;
                            break;
                        }
                    }
                    // Это не работает, если выделена пустая сцена. Поэтому написан код выше.
                    //if (SlideLayout.GetWindowsCount(display, PresentationController.Instance.SelectedSlide) > 0)
                    //{
                    //    if (MessageBoxExt.Show(String.Format("У дисплея {0} есть настроенные раскладки сцен.\r\nПри перемещении дисплея ему будут установлены раскладки группы {1}?", display.Type.Name, displayGroup.Name), "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, new string[] { "Продолжить", "Отмена" }) == DialogResult.Cancel)
                    //        return false;

                    //    clear = true;
                    //}

                    display.DisplayGroup = displayGroup.Name;
                    PresentationController.Instance.PresentationChanged = true;
                }
                else
                {
                    MessageBoxExt.Show(String.Format("Разрешение/тип {0} отличаются от разрешения/типа {1}.\r\nПеремещение дисплея в группу невозможно", displayGroup.Name, display.Name), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            bool isSuccess = true;
            List<Display> displayList = DisplayByGroup(displayGroup);
            List<Slide> lockedSlides = new List<Slide>(changedSlides.Count);
            // если все прошло успешно, то теперь для слайдов в changedSlides мы должны нефвно залочить, сохранить и разлочить
            foreach (Slide slide in changedSlides)
            {
                LockingInfo lockingInfo = PresentationController.Instance.GetSlideLockingInfo(slide);
                if (!(lockingInfo != null && lockingInfo.UserIdentity.Equals(PresentationController.Instance.UserIdentity)))
                {
                    if (!PresentationController.Instance.LockSlide(slide))
                    {
                        MessageBoxExt.Show(
                            string.Format(
                                "Перемещение дисплея {0} в группу {1} невозможно: в сценарии есть заблокированные сцены. Для перемещения дисплея в непустую группу необходимо разблокировать все сцены.",
                                display.Name, displayGroup.Name),
                            "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        isSuccess = false;
                        break;
                    }
                    else
                    {
                        lockedSlides.Add(slide);
                    }
                }
            }

            if (isSuccess)
            {
                foreach (Slide slide in changedSlides)
                {
                    SlideLayout sltemp = new SlideLayout(displayList, slide);
                    sltemp.AppendToLayout(display, clear);
                    if (displayList.Count > 0)
                    {
                        Display firstDisplay = slide.DisplayList.First(src => src.Equals(displayList[0]));
                        if (firstDisplay is ActiveDisplay)
                        {
                            string id = ((ActiveDisplay)firstDisplay).BackgroundImage;
                            if (!string.IsNullOrEmpty(id))
                                SlideLayout.ApplyDisplayBackground(slide, id, display);
                        }
                    }
                }
                displayGroup.DisplayNameList.Add(display.Type.Name);
                //PresentationController.Instance.SaveSlideChanges(lockedSlides.ToArray());
                PresentationController.Instance.SavePresentation();
            }
            foreach (Slide slide in lockedSlides)
            {
                PresentationController.Instance.UnlockSlide(slide);
            }
            if (!isSuccess) return false;

            //SlideLayout sl = new SlideLayout(displayList, PresentationController.Instance.SelectedSlide);
            //sl.AppendToLayout(display, clear);

            return true;
        }

        public bool CanMoveDisplayToGroup(DisplayGroup displayGroup, Display display)
        {
            ISegmentationSupport s = display as ISegmentationSupport;

            if (s != null && (s.SegmentColumns > 1 || s.SegmentRows > 1))
            {
                return false;
            }


            if (displayGroup.DisplayNameList.Count == 0)
            {
                return true;
            }
            else
            {
                if (display.Width == displayGroup.Width && display.Height == displayGroup.Height && display.Type.Type == displayGroup.Type)
                {
                    if (displayGroup.DisplayNameList.Contains(display.Type.Name))
                        return false;

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void ExcludeFromGroup(DisplayNode node)
        {
            if (node.Parent != null)
            {
                DisplayGroupNode groupNode = node.Parent as DisplayGroupNode;
                if (groupNode != null)
                {
                    groupNode.DisplayGroup.DisplayNameList.Remove(node.Display.Type.Name);
                    PresentationController.Instance.CurrentSlideLayout.RemoveFromLayout(node.Display);
                    foreach (Slide slide in PresentationController.Instance.GetAllSlides()
                        .Where(sld=> !sld.Name.Equals(PresentationController.Instance.CurrentSlideLayout.Slide.Name)))
                    {
                        IEnumerable<Display> displs = slide.DisplayList.Where(dsp => groupNode.DisplayGroup.DisplayNameList.Contains(dsp.Type.Name));
                        SlideLayout sltemp = new SlideLayout(displs, slide);
                        sltemp.RemoveFromLayout(node.Display);
                    }
                    PresentationController.Instance.PresentationChanged = true;
                    if (groupNode.DisplayGroup.DisplayNameList.Count == 0)
                    {
                        groupNode.DisplayGroup.Width = 0;
                        groupNode.DisplayGroup.Height = 0;
                        groupNode.DisplayGroup.Type = String.Empty;

                        groupNode.OpenImgIndex = 1;
                        groupNode.NoChildrenImgIndex = 0;
                    }

                    node.Display.DisplayGroup = null;
                }
            }
        }

        public void ShowProperties(TreeNodeAdv node)
        {
            if (node is DisplayGroupNode)
            {
                DisplayGroup dg = ((DisplayGroupNode)node).DisplayGroup;
                using (DisplayGroupProperties prop = new DisplayGroupProperties(dg, !PresentationController.Instance.PresentationLocked))
                {
                    if (prop.ShowDialog() == DialogResult.OK)
                    {
                        if (node.Text != dg.Name)
                            PresentationController.Instance.PresentationChanged = true;

                        node.Text = dg.Name;
                    }
                }
            }
            else if (node is DisplayNode)
            {
                DisplayNode n = (DisplayNode)node;
                using (DisplayPropsForm prop = new DisplayPropsForm(n.Display))
                {
                    prop.ShowDialog();
                }
            }
        }

        public Display FindDisplay(Display _display)
        {
            Display result = Displays.Find(d => d.Equals(_display));
            return result;
        }

        public IEnumerable<Display> UngrouppedDisplays()
        {
            List<Display> result = new List<Display>();
            Displays.ForEach(d => { if (GetDisplayGroup(d) == null) result.Add(d); });
            result.Sort((x, y) => x.Type.Name.CompareTo(y.Type.Name));
            return result;
        }

        public DisplayGroup GetDisplayGroup(Display display)
        {
            return PresentationController.Instance.Presentation.DisplayGroupList.Find(dg => dg.DisplayNameList.Contains(display.Type.Name));
        }

        public Dictionary<DisplayGroup, List<Display>> GrouppedDisplays()
        {
            Dictionary<DisplayGroup, List<Display>> result = new Dictionary<DisplayGroup, List<Display>>();
            List<DisplayGroup> groups = new List<DisplayGroup>(PresentationController.Instance.Presentation.DisplayGroupList);
            groups.Sort();
            foreach (DisplayGroup group in groups)
            {
                result.Add(group, DisplayByGroup(group));
            }
            return result;
        }

        public List<Display> DisplayByGroup(DisplayGroup gr)
        {
            var displaysInGroup = (from d in Displays where gr.DisplayNameList.Contains(d.Type.Name) orderby d.Type.Name select d).ToList();
            return displaysInGroup;
        }
        #endregion

        public void ChangeSelectedDisplay(Display disp)
        {
            if (OnSelectedDisplayChanged != null)
                OnSelectedDisplayChanged(disp);
        }

        #region IDisposable
        public void Dispose()
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                ShowClient.Instance.OnEquipmentStateChanged -= Instance_OnEquipmentStateChanged;
                DisplayController._instance = null;
            }
        }
        #endregion
    }
}

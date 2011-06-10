using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using UI.PresentationDesign.DesignUI.Controls.SourceTree;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using UI.PresentationDesign.DesignUI.Forms;
using Domain.PresentationDesign.Client;
using Domain.PresentationShow.ShowClient;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Common.Classes;

namespace UI.PresentationDesign.DesignUI.Controllers
{
    public delegate void SourcesChanged();
    public delegate void CurrentSourceNameChanged(String sourceName);

    public class PlayerSourcesController : IDisposable
    {
        private static PlayerSourcesController _instance;
        private Dictionary<String, List<KeyValuePair<String, object>>> _categories = new Dictionary<String, List<KeyValuePair<String, object>>>();
        private Dictionary<String, Dictionary<String, bool?>> _states = new Dictionary<String, Dictionary<String, bool?>>();
        private IEnumerable<Display> _selectedDisplays = new List<Display>();
        private Slide _selectedSlide = null;
        private object _selectedSource = null;

        public event SourcesChanged OnSourcesChanged;
        public event CurrentSourceNameChanged OnCurrentSourceChanged;

        public static void CreateController()
        {
            _instance = new PlayerSourcesController();
        }

        public static PlayerSourcesController Instance
        {
            get { return _instance; }
        }

        private PlayerSourcesController()
        {
            ShowClient.Instance.OnEquipmentStateChanged += Instance_OnEquipmentStateChanged;
        }

        void Instance_OnEquipmentStateChanged(EquipmentType equipmentType, bool? isOnLine)
        {
            bool needRefresh = false;
            foreach(var cat in _categories)
                foreach (var src in cat.Value)
                {
                    if ((src.Value as Source).Type.Equals(equipmentType))
                        if (isOnLine != _states[cat.Key][src.Key])
                        {
                            needRefresh = true;
                            _states[cat.Key][src.Key] = isOnLine;
                        }
                }
            if (needRefresh && OnSourcesChanged != null)
                OnSourcesChanged();
        }

        public void Initialize()
        {
            PresentationController.Instance.OnSlideSelectionChanged += Instance_OnSlideSelectionChanged;
            PresentationController.Instance.OnPlaySelectionChanged += Instance_OnPlaySelectionChanged;
            PresentationController.Instance.OnMonitorListChanged += Instance_OnMonitorListChanged;
            PresentationController.Instance.OnSourceChanged += Instance_OnSourceChanged;
            PresentationController.Instance.OnSlideChangedExternally += Instance_OnSlideChangedExternally;
            DesignerClient.Instance.PresentationNotifier.OnResourceAdded += PresentationNotifier_OnResourceAdded;
            DesignerClient.Instance.PresentationNotifier.OnResourceDeleted += PresentationNotifier_OnResourceAdded;
        }

        void PresentationNotifier_OnResourceAdded(object sender, NotifierEventArg<ResourceDescriptor> e)
        {
            populateCategories();
            if (OnSourcesChanged != null)
                OnSourcesChanged();
        }

        void Instance_OnSlideChangedExternally(Slide slide)
        {
            if (slide == _selectedSlide)
            {
                populateCategories();
                if (OnSourcesChanged != null)
                    OnSourcesChanged();
            }
        }

        void Instance_OnSourceChanged(Source newSource)
        {
            if (object.ReferenceEquals(_selectedSource, newSource))
                return;
            _selectedSource = newSource;
            if (newSource != null)
            {
                foreach (var cat in _categories)
                {
                    foreach (var s in cat.Value)
                        if (s.Key == newSource.ResourceDescriptor.ResourceInfo.Name)
                        {
                            if (OnCurrentSourceChanged != null)
                                OnCurrentSourceChanged(s.Key);
                            return;
                        }
                }
            }
            if (OnCurrentSourceChanged != null)
                OnCurrentSourceChanged(null);
        }

        void Instance_OnMonitorListChanged(IEnumerable<Display> newList)
        {
            _selectedDisplays = newList;
            populateCategories();
            if (OnSourcesChanged != null)
                OnSourcesChanged();
        }

        void Instance_OnSlideSelectionChanged(IEnumerable<Slide> NewSelection)
        {
            if (PlayerController.Instance.CanPlay)
                return;
            _selectedSlide = NewSelection.FirstOrDefault();
            populateCategories();
            if (OnSourcesChanged != null)
                OnSourcesChanged();
        }

        void Instance_OnPlaySelectionChanged(Slide slide)
        {
            if (!PlayerController.Instance.CanPlay)
                return;
            _selectedSlide = slide;
            populateCategories();
            if (OnSourcesChanged != null)
                OnSourcesChanged();
        }

        private void populateCategories()
        {
            if (_selectedSlide == null)
            {
                _categories.Clear();
                return;
            }
            HashSet<Source> sources = new HashSet<Source>();
            foreach (var ls in (from disp in _selectedSlide.DisplayList where _selectedDisplays.Any(x => x.EquipmentType == disp.EquipmentType) select disp.WindowList))
                foreach (var window in ls)
                    if(!sources.Any(x => x.ResourceId == window.Source.ResourceId))
                        sources.Add(window.Source);
            var cats = (from source in sources group source by source.PluginName);

            _categories.Clear();
            _states.Clear();
            foreach (var category in cats)
            {
                List<KeyValuePair<String, object>> items = new List<KeyValuePair<String, object>>();
                _categories.Add(category.Key, items);
                _states.Add(category.Key, new Dictionary<String, bool?>());
                foreach (var src in category)
                    if (src.ResourceDescriptor != null)
                    {
                        items.Add(new KeyValuePair<string, object>(src.ResourceDescriptor.ResourceInfo.Name, src));
                        _states[category.Key][src.ResourceDescriptor.ResourceInfo.Name] =           //(!src.ResourceDescriptor.ResourceInfo.IsHardware) || ShowClient.Instance.IsOnLine(src.Type);
                            (!src.ResourceDescriptor.ResourceInfo.IsHardware) ? (bool?)null : ShowClient.Instance.IsOnLine(src.Type);
                    }
            }
        }

        public Dictionary<String, List<KeyValuePair<String, object>>> Categories
        {
            get { return _categories; }
        }

        public Dictionary<String, Dictionary<String, bool?>> States
        {
            get { return _states; }
        }

        public void ShowProperties(object p)
        {
            Source source = p as Source;
            if (source == null || source.ResourceDescriptor == null)
                return;
            SourcePropertiesForm sf = new SourcePropertiesForm(source, true, false);
            sf.ShowDialog();
        }

        public void ChangeSelectedItem(object p)
        {
            if (object.ReferenceEquals(p, _selectedSource))
                return;
            _selectedSource = p;
            PresentationController.Instance.NotifyCurrentSourceChanged(p as Source);
        }

        public void Dispose()
        {
            if (ShowClient.Instance != null)
                ShowClient.Instance.OnEquipmentStateChanged -= Instance_OnEquipmentStateChanged;
            if (PresentationController.Instance != null)
            {
                PresentationController.Instance.OnSlideSelectionChanged -= Instance_OnSlideSelectionChanged;
                PresentationController.Instance.OnPlaySelectionChanged -= Instance_OnPlaySelectionChanged;
                PresentationController.Instance.OnMonitorListChanged -= Instance_OnMonitorListChanged;
                PresentationController.Instance.OnSourceChanged -= Instance_OnSourceChanged;
                PresentationController.Instance.OnSlideChangedExternally -= Instance_OnSlideChangedExternally;
            }
            if (DesignerClient.Instance != null)
            {
                DesignerClient.Instance.PresentationNotifier.OnResourceAdded -= PresentationNotifier_OnResourceAdded;
                DesignerClient.Instance.PresentationNotifier.OnResourceDeleted -= PresentationNotifier_OnResourceAdded;
            }
            _instance = null;
        }
    }
}

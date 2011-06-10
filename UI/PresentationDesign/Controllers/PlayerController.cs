using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.PresentationShow.ShowClient;
using Domain.PresentationShow.ShowCommon;
using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using UI.PresentationDesign.DesignUI.Forms;
using UI.PresentationDesign.DesignUI.Classes.Controller;
using System.Windows.Forms;
using Syncfusion.Windows.Forms;
using System.IO;

namespace UI.PresentationDesign.DesignUI.Controllers
{
    /// <summary>
    /// Контроллер для управления показом и подготовкой презентации
    /// </summary>
    public class PlayerController : IDisposable
    {
        private static PlayerController _instance;
        private bool _isPresentationPrepared = false;
        private bool _isPresentationStarted = false;
        private Stack<Slide> _slideHistory = new Stack<Slide>();
        private Slide _currentPlayingSlide = null;
        public static readonly String CrashFile = Path.Combine(Application.StartupPath, "crashlog.log");

        private String _lastPresentationName = null;
        private int _lastSlideNum = -1;

        public bool CanGoNext { get; private set; }
        public bool CanGoPrev { get; private set; }

        public event Action OnPresentationStarted;
        public event Action OnSlideShowed;

        public Slide CurrentPlayingSlide
        {
            get { return _currentPlayingSlide; }
            set { _currentPlayingSlide = value; }
        }

        public Stack<Slide> SlideHistory
        {
            get { return _slideHistory; }
        }

        public static void CreatePlayerController()
        {
            _instance = new PlayerController();
        }

        public bool CanPrepare
        {
            get { return !_isPresentationPrepared; }
        }

        public bool CanStart
        {
            get { return _isPresentationPrepared && !_isPresentationStarted; }
        }

        public bool CanStop
        {
            get { return _isPresentationStarted; }
        }

        public bool CanPlay
        {
            get { return _isPresentationPrepared && _isPresentationStarted; }
        }

        private PlayerController()
        {
            try
            {
                StreamReader reader = new StreamReader(CrashFile);
                _lastPresentationName = reader.ReadLine();
                _lastSlideNum = Convert.ToInt32(reader.ReadLine());
            }
            catch (Exception /*ex*/) { }
            PresentationController.Instance.OnSlideSelectionChanged += Instance_OnSlideSelectionChanged;
            PresentationController.Instance.OnSlideChangedExternally += Instance_OnSlideChangedExternally;
            ShowClient.Instance.OnGoToSlide += Instance_OnGoToSlide;
        }

        void Instance_OnGoToSlide(int obj)
        {
            Slide newSlide = PresentationController.Instance.Presentation.SlideList.Find(sld => sld.Id == obj);
            if (_currentPlayingSlide != null && _currentPlayingSlide != newSlide)
                _slideHistory.Push(newSlide);
            _currentPlayingSlide = newSlide;
        }

        void Instance_OnSlideSelectionChanged(IEnumerable<Slide> NewSelection)
        {
            if (CanPlay)
                return;
            if (_currentPlayingSlide != null && _currentPlayingSlide != NewSelection.FirstOrDefault() && (_slideHistory.Count == 0 || _slideHistory.Peek() != _currentPlayingSlide))
                _slideHistory.Push(_currentPlayingSlide);
            _currentPlayingSlide = NewSelection.FirstOrDefault();
        }

        void Instance_OnSlideChangedExternally(Slide slide)
        {
            if (_isPresentationPrepared)
            {
                ShowClient.Instance.Load(PresentationController.Instance.PresentationInfo, slide.Id, true);
            }
        }

        public static PlayerController Instance
        {
            get { return _instance; }
        }

        private void MarkCurrentSlide(Presentation presentation, Slide slide)
        {
            StreamWriter writer = new StreamWriter(CrashFile, false);
            writer.WriteLine(presentation.UniqueName);
            writer.WriteLine(slide.Id);
            writer.Close();
        }

        private void UnmarkCurrentSlide()
        {
            if (File.Exists(CrashFile))
                File.Delete(CrashFile);
        }

        public bool PlaySlide(Presentation presentation, Slide slide)
        {
            if (slide != null && slide != _currentPlayingSlide && CanPlay)
            {
                bool isPrevEnable, isNextEnable;
                ShowSlideResult result = ShowClient.Instance.ShowSlide(presentation, slide, out isPrevEnable, out isNextEnable);
                if (result.IsSuccess)
                {
                    MarkCurrentSlide(presentation, slide);
                    if (_currentPlayingSlide != null)
                        _slideHistory.Push(_currentPlayingSlide);
                    _currentPlayingSlide = slide;
                }
                else
                {
                    MessageBoxAdv.Show(result.ErrorMessage, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                CanGoNext = isNextEnable;
                CanGoPrev = isPrevEnable;
                return true;
            }
            return false;
        }

        public bool GoToPrevSlide(Presentation presentation, out int slideId)
        {
            slideId = 0;
            if (CanPlay)
            {
                //ShowClient.Instance.ShowSlide(presentation, slide, out isPrevEnable, out isNextEnable);
                bool isPrevEnable, isNextEnable;
                ShowSlideResult result = ShowClient.Instance.GoToPrevSlide(out slideId, out isPrevEnable, out isNextEnable);
                if (result.IsSuccess)
                {
                    int id = slideId;
                    Slide slide = PresentationController.Instance.Presentation.SlideList.Find(x => x.Id == id);
                    MarkCurrentSlide(presentation, slide);
                    _currentPlayingSlide = slide;
                }
                else
                {
                    MessageBoxAdv.Show(result.ErrorMessage, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                CanGoNext = isNextEnable;
                CanGoPrev = isPrevEnable;
                return true;
            }
            return false;
        }

        public void EditSlide(PresentationInfo info, Slide slide)
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE");
            if (key != null)
            {
                key = key.OpenSubKey("PolyMedia");
                if (key != null)
                {
                    key = key.OpenSubKey("PresentationDesigner");
                    if (key != null)
                    {
                        String path = key.GetValue("ExePath").ToString();
                        UserIdentity id = (UserIdentity)System.Threading.Thread.CurrentPrincipal;
                        System.Runtime.Serialization.DataContractSerializer ser = new System.Runtime.Serialization.DataContractSerializer(typeof(UserIdentity));
                        System.IO.MemoryStream stream = new System.IO.MemoryStream();
                        ser.WriteObject(stream, id);
                        stream.Seek(0, System.IO.SeekOrigin.Begin);
                        String args = String.Format("{0} {1} \"{2}\"", info.UniqueName, slide.Id, new System.IO.StreamReader(stream).ReadToEnd().Replace('\"', '\''));

                        System.Diagnostics.Process.Start(path, args);
                        return;
                    }
                }
            }
            MessageBoxAdv.Show("Не удается найти путь запуска модуля подготовки сценариев!", "Редакитрование сцены");
        }

        public void Pause(Presentation presentation)
        {
            ShowClient.Instance.Pause(presentation);
            _currentPlayingSlide = null;
        }

        public void Prepare(PresentationInfo info)
        {
            PreparePresentationController controller = new PreparePresentationController(new PresentationInfo(info));
            PreparePresentationForm form = new PreparePresentationForm(controller);
            form.ShowDialog();
            _isPresentationPrepared = controller.PreparationStatus != ShowClient.PreparationStatus.Error;
        }

        public void Start(PresentationInfo info)
        {
            if (info.UniqueName == _lastPresentationName)
            {
                PresentationController.Instance.SelectedSlide = PresentationController.Instance.Presentation.SlideList.Find(x => x.Id == _lastSlideNum);
            }
            // Первый запуск, закрываем все окна на агентах, 
            // если были от предыдущих запусков
            if (!_isPresentationStarted) 
                ShowClient.Instance.CloseWindows();
            _isPresentationStarted = true;
            ShowClient.Instance.Start(new PresentationInfo(info));
            if (OnPresentationStarted != null)
                OnPresentationStarted();
        }

        public void Stop(PresentationInfo info)
        {
            UnmarkCurrentSlide();
            _isPresentationStarted = false;
            ShowClient.Instance.Stop(new PresentationInfo(info));
        }

        public void Dispose()
        {
            if (PresentationController.Instance != null)
            {
                PresentationController.Instance.OnSlideSelectionChanged -= Instance_OnSlideSelectionChanged;
                PresentationController.Instance.OnSlideChangedExternally -= Instance_OnSlideChangedExternally;
            }
            if (ShowClient.Instance != null)
                ShowClient.Instance.OnGoToSlide -= Instance_OnGoToSlide;
            _instance = null;
        }
    }
}

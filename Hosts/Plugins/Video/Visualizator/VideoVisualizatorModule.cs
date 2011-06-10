using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator;

using Hosts.Plugins.Video.Common;
using Hosts.Plugins.Video.SystemModule.Config;
using Hosts.Plugins.Video.SystemModule.Design;
using Hosts.Plugins.Video.UI;

using TechnicalServices.Interfaces.Comparers;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using System.Threading;

namespace Hosts.Plugins.Video.Visualizator
{
    public sealed class VideoVisualizatorModule : DomainVisualizatorModule<VideoVisualizatorDomain, Window>
    {
        private readonly VideoWindowEqualityComparer _comparer = new VideoWindowEqualityComparer();

        protected override IEqualityComparer<Window> EquatableComparer
        {
            get { return _comparer; }
        }

        protected override IEnumerable<string> GetChangedState(Source source1, Source source2)
        {
            List<string> commandList = new List<string>();
            VideoSourceDesign sourceX = source1 as VideoSourceDesign;
            VideoSourceDesign sourceY = source2 as VideoSourceDesign;
            if (sourceX == null || sourceY == null) return commandList;
            if (sourceX.State != sourceY.State)
                commandList.Add(
                    sourceY.State == PlayState.Play
                        ? PlayCommand.Instance.GetCommand(null)
                        : PauseCommand.Instance.GetCommand(null));
            if (sourceX.StartTimeShift != sourceY.StartTimeShift)
                commandList.Add(SeekCommand.Instance.GetCommand(sourceY.StartTimeShift.ToString()));
            return commandList;
        }

        protected override string GetPauseCommand()
        {
            return PauseCommand.Instance.GetCommand();
        }

        protected override void SetStateToPause(Source source)
        {
            VideoSourceDesign videoSourceDesign = source as VideoSourceDesign;
            if (videoSourceDesign == null)
            {
                base.SetStateToPause(source);
                return;
            }
            videoSourceDesign.State = PlayState.Pause;
        }

        #region Nested type: VideoWindowEqualityComparer

        private class VideoWindowEqualityComparer : BaseWindowEqualityComparer
        {
            public override bool Equals(Window x, Window y)
            {
                if (x == y) return true;

                VideoSourceDesign sourceX = x.Source as VideoSourceDesign;
                VideoSourceDesign sourceY = y.Source as VideoSourceDesign;
                if (sourceX == null || sourceY == null) return false;
                if (sourceX.ResourceDescriptor == null || sourceY.ResourceDescriptor == null) return false;

                VideoResourceInfo resourceInfoX = sourceX.ResourceDescriptor.ResourceInfo as VideoResourceInfo;
                VideoResourceInfo resourceInfoY = sourceY.ResourceDescriptor.ResourceInfo as VideoResourceInfo;
                if (resourceInfoX == null || resourceInfoY == null) return false;

                return (base.Equals(x, y))
                       &&
                       (sourceX.ContentPath.Equals(sourceY.ContentPath, StringComparison.InvariantCultureIgnoreCase))
                       &&
                       (resourceInfoX.MasterResourceProperty.ResourceFileName.Equals(
                            resourceInfoY.MasterResourceProperty.ResourceFileName,
                            StringComparison.InvariantCultureIgnoreCase) &&
                        resourceInfoX.MasterResourceProperty.ModifiedUtc.Equals(
                            resourceInfoY.MasterResourceProperty.ModifiedUtc));
            }
        }

        #endregion
    }

    public sealed class VideoVisualizatorDomain : VisualizatorDomainAppl
    {
        private const string ResourcePrefix = "Hosts.Plugins.Video.Resources.";

        protected override bool OnInit()
        {
            bool onInit = base.OnInit();
            if (onInit)
            {
                VideoPreload(null);
                byte[] data = new byte[1024];
                List<string> fileList = new List<string>();
                Assembly asmbl = GetType().Assembly;
                foreach (string item in asmbl.GetManifestResourceNames())
                {
                    ManifestResourceInfo resInfo = asmbl.GetManifestResourceInfo(item);
                    if (resInfo == null) continue;
                    using (Stream stream = asmbl.GetManifestResourceStream(item))
                    {
                        if (stream == null) continue;
                        if (item.StartsWith(ResourcePrefix) && item.EndsWith(".avi"))
                        {
                            string fileName = item.Remove(0, ResourcePrefix.Length);
                            using (FileStream file = new FileStream(fileName, FileMode.Create))
                            {
                                int count;
                                while ((count = stream.Read(data, 0, data.Length)) > 0)
                                    file.Write(data, 0, count);
                                fileList.Add(fileName);
                                Thread.Sleep(0);
                                VideoPreload(fileName);
                            }
                        }
                    }
                }
                foreach (string fileName in fileList)
                {
                    try
                    {
                        File.Delete(fileName);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return onInit;
        }

        private void VideoPreload(string fileName)
        {
            using (VideoForm form = new VideoForm())
            {
                try
                {
                    //form.WindowState = FormWindowState.Minimized;
                    form.Left = 0;
                    form.Top = 0;
                    form.Width = 10;
                    form.Height = 10;
                    if (!String.IsNullOrEmpty(fileName))
                        form.LoadVideo(Path.GetFullPath(fileName), 0, true, true);
                    form.Show();
                    Thread.Sleep(0);
                }
                catch (Exception)
                {

                }
            }
        }

        public override Form CreateForm(DisplayType display, Window window, out bool needProcessing)
        {
            needProcessing = true;
            VideoSourceDesign source = window.Source as VideoSourceDesign;
            if (source == null) return null;
            VideoResourceInfo videoResourceInfo = window.Source.ResourceDescriptor.ResourceInfo as VideoResourceInfo;
            if (videoResourceInfo == null) return null;

            VideoForm videoForm = new VideoForm();
            if (!videoForm.Init(display, window)) return null;
            videoForm.LoadVideo(videoResourceInfo.ResourceFullFileName,
                                source.StartTimeShift, source.State == PlayState.Play);
            needProcessing = false;
            return videoForm;
        }
    }

    //public sealed class VideoVisualizatorModule : MainFormApplicationBased
    //{
    //    public override Form CreateForm(Window window)
    //    {
    //        VideoSourceDesign source = window.Source as VideoSourceDesign;
    //        if (source == null) return null;
    //        VideoResourceInfo videoResourceInfo = window.Source.ResourceDescriptor.ResourceInfo as VideoResourceInfo;
    //        if (videoResourceInfo == null) return null;

    //        VideoForm videoForm = new VideoForm();
    //        videoForm.LoadVideo(videoResourceInfo.ResourceFullFileName,
    //            source.StartTimeShift, source.State == PlayState.Play,
    //            source.AspectLock);
    //        return videoForm;
    //    }
    //}
}
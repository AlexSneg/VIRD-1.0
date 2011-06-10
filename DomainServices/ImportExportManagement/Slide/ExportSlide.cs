using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DomainServices.ImportExportCommon;
using TechnicalServices.Entity;
using TechnicalServices.Exceptions;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace DomainServices.ImportExportClientManagement.Slide
{
    public class ExportSlide
    {
        private readonly IPresentationClient _remotePresentationWorker;
        private readonly IPresentationClient _standalonePresentationWorker;
        private readonly IExportSlideController _exportSlideController;
        private IContinue _continue;

        public ExportSlide(IPresentationClient remotePresentationWorker,
            IPresentationClient standalonePresentationWorker,
            IExportSlideController exportSlideController)
        {
            _remotePresentationWorker = remotePresentationWorker;
            _standalonePresentationWorker = standalonePresentationWorker;
            _exportSlideController = exportSlideController;
        }

        #region public

        public void Export(string fileName, PresentationInfo presentationInfo,
            TechnicalServices.Persistence.SystemPersistence.Presentation.Slide[] slideToExport)
        {
            try
            {
                if (slideToExport == null || slideToExport.Length == 0) return;
                _continue = _exportSlideController.GetUserInterActive(slideToExport.Length == 1);

                // загружаем сцены
                TechnicalServices.Persistence.SystemPersistence.Presentation.Slide[] slideArr =
                    LoadSlides(presentationInfo, slideToExport);

                // формируем балк для выгрузки
                SlideBulk slideBulk = ComposeSlideBulk(presentationInfo, slideArr);
                // сохраняем на диск
                SaveToDisk(fileName, slideBulk);
                //_exportSlideController.SuccessMessage("Экспорт сцен успешно завершен");
            }
            catch (InterruptOperationException)
            {
                if (slideToExport.Length > 1)
                    _exportSlideController.ErrorMessage(string.Format("Экспорт сцен был прерван пользователем"));
            }
            catch (Exception ex)
            {
                _exportSlideController.ErrorMessage(string.Format("При экспорте сцен произошла неизвестная ошибка: {0}", ex));
            }
        }

        #endregion

        #region private

        private static SlideBulk ComposeSlideBulk(PresentationInfo presentationInfo,
            IEnumerable<TechnicalServices.Persistence.SystemPersistence.Presentation.Slide> slideArr)
        {
            XmlSerializableDictionary<int, SlideLinkList> linkDic = new XmlSerializableDictionary<int, SlideLinkList>();
            XmlSerializableDictionary<int, Point> positionDic = new XmlSerializableDictionary<int, Point>();
            SlideBulk slideBulk = new SlideBulk();
            foreach (TechnicalServices.Persistence.SystemPersistence.Presentation.Slide slide in slideArr)
            {
                slideBulk.SlideList.Add(slide);
                positionDic[slide.Id] = presentationInfo.SlidePositionList[slide.Id];
                IList<LinkInfo> linkInfoList;
                if (!presentationInfo.SlideLinkInfoList.TryGetValue(slide.Id, out linkInfoList)) continue;
                foreach (LinkInfo linkInfo in linkInfoList)
                {
                    if (!slideArr.Any(sl => sl.Id == linkInfo.NextSlideId)) continue;
                    SlideLinkList slideLinkList;
                    if (!linkDic.TryGetValue(slide.Id, out slideLinkList))
                    {
                        linkDic[slide.Id] = slideLinkList = new SlideLinkList();
                    }
                    slideLinkList.LinkList.Add(linkInfo.CreateLinkStub());
                }
            }

            slideBulk.LinkDictionary = linkDic;
            slideBulk.SlidePositionList = positionDic;
            return slideBulk;
        }

        private TechnicalServices.Persistence.SystemPersistence.Presentation.Slide[]
            LoadSlides(PresentationInfo presentationInfo,
            IEnumerable<TechnicalServices.Persistence.SystemPersistence.Presentation.Slide> slideToExport)
        {
            TechnicalServices.Persistence.SystemPersistence.Presentation.Slide[] slideArr =
                _remotePresentationWorker.LoadSlides(presentationInfo.UniqueName,
                slideToExport.Select(sl => sl.Id).ToArray());
            int[] removed = slideToExport.Select(sl => sl.Id).Except(slideArr.Select(sl => sl.Id)).ToArray();

            foreach (int slideId in removed)
            {
                string slideName = slideToExport.Single(sl => sl.Id == slideId).Name;
                if (!_continue.Continue(string.Format("Невозможно экспортировать сцену {0}: сцена уже удалена", slideName)))
                {
                    throw new InterruptOperationException(slideName);
                }
            }

            return slideArr;
        }

        private void SaveToDisk(string fileName, SlideBulk slideBulk)
        {
            _standalonePresentationWorker.SaveSlideBulk(fileName, slideBulk);
        }

        #endregion
    }
}

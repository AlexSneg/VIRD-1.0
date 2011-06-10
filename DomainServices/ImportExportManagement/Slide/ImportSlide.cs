using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DomainServices.ImportExportCommon;
using TechnicalServices.Exceptions;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Persistence.SystemPersistence.Resource;

namespace DomainServices.ImportExportClientManagement.Slide
{
    public class ImportSlide
    {
        private readonly IPresentationClient _standalonePresentationWorker;
        private readonly IImportSlideController _importSlideController;
        private int _indent, _height;
        private Size _gapSize;


        public ImportSlide(IPresentationClient standalonePresentationWorker,
            IImportSlideController importSlideController)
        {
            _standalonePresentationWorker = standalonePresentationWorker;
            _importSlideController = importSlideController;
        }

        #region public

        public void Import(string fileName,
            TechnicalServices.Persistence.SystemPersistence.Presentation.Presentation presentation,
            ResourceDescriptor[] resourceDescriptors, DeviceResourceDescriptor[] deviceResourceDescriptors,
            int indent, int height)
        {
            _indent = indent;
            _height = height;
            try
            {
                SlideBulk slideBulk = LoadFromFile(fileName, resourceDescriptors, deviceResourceDescriptors);
                if (slideBulk == null)
                {
                    throw new InvalideFileException("Содержание файла некорректно. Импорт сцен невозмлжен");
                }
                CheckSlideNames(slideBulk);
                AddSlideAndLink(presentation, slideBulk);
                _importSlideController.SuccessMessage("Импорт сцен успешно завершен");
            }
            catch (InterruptOperationException)
            { }
            catch (InvalideFileException ex)
            {
                _importSlideController.ErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                _importSlideController.ErrorMessage(string.Format("При экспорте сцен произошла неизвестная ошибка: {0}", ex));
            }
        }

        #endregion

        #region private

        private SlideBulk LoadFromFile(string fileName, ResourceDescriptor[] resourceDescriptors,
            DeviceResourceDescriptor[] deviceResourceDescriptors)
        {
            return _standalonePresentationWorker.LoadSlideBulk(fileName, resourceDescriptors, deviceResourceDescriptors);
        }

        private void CheckSlideNames(SlideBulk slideBulk)
        {
            HashSet<string> addedSlideName = new HashSet<string>();     //presentation.SlideList.Select(sl => sl.Name)
            foreach (TechnicalServices.Persistence.SystemPersistence.Presentation.Slide slide in slideBulk.SlideList)
            {
                bool isContinue = true;
                while (isContinue && 
                    (addedSlideName.Any(name => name.Equals(slide.Name, StringComparison.InvariantCultureIgnoreCase))
                    ||
                    !_importSlideController.IsSlideUniqueName(slide.Name, string.Empty)))
                {
                    string newName;
                    isContinue = _importSlideController.GetNewName(
                        string.Format("Сцена с названием {0} уже есть в сценарии. Введите другое название", slide.Name), out newName);
                    slide.Name = newName;
                }
                addedSlideName.Add(slide.Name);
                if (!isContinue) throw new InterruptOperationException(slide.Name);
            }
        }

        private void AddSlideAndLink(TechnicalServices.Persistence.SystemPersistence.Presentation.Presentation presentation,
            SlideBulk slideBulk)
        {
            //SetInitialPosition(presentation.SlidePositionList.Values, slideBulk.SlidePositionList.Values);
            // слайды
            // айдишники меняются, так что запоминаем те которые были
            Dictionary<int, int> oldNewIdMapping = new Dictionary<int, int>(slideBulk.SlideList.Count);
            foreach (TechnicalServices.Persistence.SystemPersistence.Presentation.Slide slide in slideBulk.SlideList)
            {
                int oldId = slide.Id;
                if (slide.LabelId != Label.NullId
                    && presentation.SlideList.Select(sl=>sl.LabelId).Where(lb=>lb != Label.NullId).Any(
                    lb=>lb == slide.LabelId))
                {
                    slide.LabelId = Label.NullId;
                }
                _importSlideController.AddSlide(slide);
                    //, GetPoint(slideBulk.SlidePositionList[slide.Id]));
                oldNewIdMapping[slide.Id] = oldId;
            }
            // линки
            foreach (TechnicalServices.Persistence.SystemPersistence.Presentation.Slide slide in slideBulk.SlideList)
            {
                SlideLinkList slideLinkList;
                if (!slideBulk.LinkDictionary.TryGetValue(oldNewIdMapping[slide.Id], out slideLinkList)) continue;
                foreach (Link link in slideLinkList.LinkList.OrderByDescending(ln=>ln.IsDefault))
                {
                    _importSlideController.AddLink(slide, link.NextSlide);
                }
            }
        }

        private PointF GetPoint(Point point)
        {
            return PointF.Add(point, _gapSize);
        }

        private void SetInitialPosition(IEnumerable<Point> existedPositionList,
            IEnumerable<Point> newPositionList)
        {
            int minX = newPositionList.Select(p => p.X).Min();
            int gapX = _indent - minX;
            int minY = newPositionList.Select(p => p.Y).Min();
            int gapY = _indent + _height + (existedPositionList.Count() == 0 ? 0 : existedPositionList.Select(p => p.Y).Max()) - minY;
            _gapSize = new Size(gapX, gapY);
        }

        #endregion

    }
}

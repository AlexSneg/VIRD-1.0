using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Interfaces;
using TechnicalServices.Persistence.SystemPersistence.Resource;
using System.IO;

namespace Hosts.Plugins.BusinessGraphics.SystemModule.Design
{
    public partial class BusinessGraphicsSourceDesign
    {
        /// <summary>вернет правильный файл </summary>
        private string GetFile(FileType file)
        {
            BusinessGraphicsResourceInfo info = null;
            if (service != null && resourceCRUD != null)
            {
                if (resourceCRUD.GetSource(this.ResourceDescriptor, true))
                {
                    IPresentationClient client = service.GetService(typeof(IPresentationClient)) as IPresentationClient;
                    if (client != null)
                    {
                        ResourceDescriptor rd = client.SourceDAL.GetStoredSource(this.ResourceDescriptor);
                        info = ((BusinessGraphicsResourceInfo)rd.ResourceInfo);
                    }
                }
            }
            else if (this.ResourceDescriptor != null && this.ResourceDescriptor.ResourceInfo != null)
            {
                //сюда попадаем скорее всего только на агенте
                info = this.ResourceDescriptor.ResourceInfo as BusinessGraphicsResourceInfo;
            }
            //если запрашивается сам map файл заодно скопируем и dbf файл, без него компонент не сможет работать
            if (file == FileType.mapResource) PrepareFile(info, FileType.mapdataResource);
            return PrepareFile(info, file);
        }

        /// <summary>подготовим файлы карт</summary>
        private string PrepareFile(BusinessGraphicsResourceInfo info, FileType fType)
        {
            //достанем временной индентивикатор для карт, по нему будем ориентироваться новый файл или нет
            ResourceFileProperty file = info.ResourceFileList.FirstOrDefault(f => f.Id.Equals(FileType.mapResource.ToString()));
            //если файла карты нет, значит БГ без карты, возьмем тогда сам xml
            if (file == null)
                file = info.ResourceFileList.FirstOrDefault(f => f.Id.Equals(FileType.xmlResource.ToString()));
            long dtFile = file.ModifiedUtc;
            //информация об оригинальном файле
            ResourceFileProperty oldMapFileName = info.ResourceFileList.FirstOrDefault(f => f.Id.Equals(fType.ToString()));
            //информация о файле который приехал с сервера в качестве ресурса
            string resFileName = GetFileByType(info, fType);
            if (string.IsNullOrEmpty(resFileName) || !File.Exists(resFileName)) return string.Empty;
            //имя файла как он должен называться
            string newFileName = Path.Combine(
                Path.GetDirectoryName(resFileName),
                Path.GetFileNameWithoutExtension(oldMapFileName.ResourceFileName) +
                    dtFile.ToString() + "." + GetFileExtension(fType));
            if (!File.Exists(newFileName))
            {
                File.Copy(resFileName, newFileName);
            }
            return newFileName;
        }
        /// <summary>достает имя файла по типу </summary>
        private string GetFileByType(BusinessGraphicsResourceInfo info, FileType fType)
        {
            switch (fType)
            {
                case FileType.xmlResource:
                    return info.File;
                case FileType.mapResource:
                    return info.MapFile;
                case FileType.mapdataResource:
                    return info.MapDataFile;
                default:
                    throw new Exception("Unknown file type ");
            }
        }
        /// <summary>возвращает правильное расширение файла для типа </summary>
        private string GetFileExtension(FileType fType)
        {
            switch (fType)
            {
                case FileType.xmlResource:
                    return "xml";
                case FileType.mapResource:
                    return "shp";
                case FileType.mapdataResource:
                    return "dbf";
                default:
                    throw new Exception("Unknown file type ");
            }
        }

    }
}

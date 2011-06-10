using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using TechnicalServices.Common;
using TechnicalServices.Entity;

namespace TechnicalServices.Util.FileTransfer
{
    public class FileSaver
    {
        private readonly Entity.UserIdentity _userIdentity;
        private readonly IFileInfoProvider _fileInfoProvider;
        private ICommunicationObject _communicationObject = null;

        private const string _tempExt = ".temp";
        private int _part = -1;
        private string _currentFileName;
        private string _currentFileId;
        private readonly bool _useTempFile;
        private int _wasTerminated = 0;

        private DateTime _beginTime;
        private long bytesWritten = 0;

        bool _useUploadResume = false;
        /// <summary>
        /// Использовать докачку файлов
        /// </summary>
        public bool UseUploadResume
        {
            get { return _useUploadResume; }
            set { _useUploadResume = value; }
        }

        bool deleteFilesAfterAbort = true;

        public bool DeleteFilesAfterAbort
        {
            get { return deleteFilesAfterAbort; }
            set { deleteFilesAfterAbort = value; }
        }


        private readonly Dictionary<string, string> _fileDic = new Dictionary<string, string>();

        public FileSaver(Entity.UserIdentity userIdentity, IFileInfoProvider fileInfoProvider, bool useTempFile)
        {
            _userIdentity = userIdentity;
            _fileInfoProvider = fileInfoProvider;
            _useTempFile = useTempFile;
            if (OperationContext.Current != null && OperationContext.Current.Channel != null)
            {
                _communicationObject = OperationContext.Current.Channel;
                _communicationObject.Faulted += new EventHandler(Abort);
            }
            _beginTime = DateTime.Now;
        }
        int _partSend = 0;
        public int PartSend
        {
            get
            {
                return _partSend;
            }
        }

        private void WriteContentToFile(byte[] buffer, int countToWrite)
        {
            if (string.IsNullOrEmpty(_currentFileName)) throw new Exception("FileSaver.WriteContentToFile: не задано имя временного файла!");
            using (FileStream file = new FileStream(_currentFileName, FileMode.Append, FileAccess.Write))
            {
                file.Write(buffer, 0, countToWrite);
            }
            bytesWritten += countToWrite;
        }

        /// <summary>
        /// Текущая скорость записи файла.
        /// </summary>
        /// <returns>Скорость записи.</returns>
        public double Speed
        {
            get
            {
                double s;
                double seconds = (DateTime.Now - _beginTime).Seconds;
                if (seconds == 0) return 0;
                s = (bytesWritten / seconds) / 1048576;
                if(double.IsNaN(s)) s = 0;
                if(double.IsPositiveInfinity(s) || double.IsNegativeInfinity(s)) s = 0;
                return s;
            }
        }

        private string _file;
        public string CurrentFile
        {
            get
            {
                return _file;
            }
        }

        public string ResourceName
        {
            get { return _fileInfoProvider.UniqueName; }
        }

        public UserIdentity UserIdentity
        {
            get { return _userIdentity; }
        }

        private void Abort(object sender, EventArgs e)
        {
            lock (this)
            {
                Terminate();
                if (OnAbort != null)
                    OnAbort(this, e);
            }
        }

        private void Done()
        {
            if (_communicationObject != null)
            {
                _communicationObject.Faulted -= Abort;
                _communicationObject = null;
            }
        }

        public event EventHandler OnAbort;

        public bool Save(FileTransferObject obj)
        {
            lock (this)
            {
                if (_wasTerminated == 1) return false;
                int currentPart = obj.Part;
                if (currentPart - _part != 1)
                {
                    if (PartSend == 0) // С докачкой возможно такое, не страшно
                    {
                        //Error();
                        throw new Exception(
                            string.Format(
                                "FileSaver.Save: неверная последовательность частей файла! Должна придти {0} часть, а пришла {1} часть",
                                _part + 1, currentPart));
                    }
                    else return false;
                }
                _part = currentPart;
                if (_part == 0)
                {
                    bool needExit = false;
                    _file =  Path.GetFileName(_fileInfoProvider.GetFileName(obj.FileId));
                    // пришла самая первая часть
                    _currentFileName = _useTempFile
                                           ?
                                               _fileInfoProvider.GetFileName(obj.FileId) + _tempExt
                                           :
                                               _fileInfoProvider.GetFileName(obj.FileId);
                    if (_useUploadResume && obj.NumberOfParts>2)
                    {
                        //Если нужна докачка и файл длинный
                        if(File.Exists(_currentFileName)) 
                        {
                            //Такой файл уже есть, надо докачивать.
                            FileInfo info=new FileInfo(_currentFileName);
                            long fileSize = info.Length;
                            int partSize = obj.CountToWrite;
                            if(fileSize%partSize == 0)
                            {
                                // Размер файла соответствует размеру блока (тупая проверка)
                                _partSend = Convert.ToInt32(fileSize/partSize);
                                _part = _partSend-1;
                                needExit = true;
                                //return false;
                            }
                            else
                            {
                                File.Delete(_currentFileName);
                            }
                        }
                    }
                    else
                    {
                        File.Delete(_currentFileName);
                    }
                    _currentFileId = obj.FileId;
                    _fileDic[_currentFileId] = _currentFileName;
                    if (needExit) return false;
                }
                WriteContentToFile(obj.FileContent, obj.CountToWrite);
                if (_part + 1 == obj.NumberOfParts)
                {
                    _part = -1;
                    return true;
                }
                return false;
            }
        }

        public void Terminate()
        {
            if (Interlocked.CompareExchange(ref _wasTerminated, 1, 0) == 1) return;
            Done();
            foreach (string fileName in _fileDic.Values)
            {
                if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                {
                    if (deleteFilesAfterAbort) File.Delete(_currentFileName);
                    FireAbort(); // Уведомим заинтересованных, что никто ничего уже не качает.
                }
            }
            _fileDic.Clear();
        }

        private void FireAbort()
        {
          if (OnAbort != null)
          OnAbort(this, null);
        }

        public bool WasTerminated
        {
            get { return _wasTerminated == 1; }
        }

        public void Commit()
        {
            if (Interlocked.CompareExchange(ref _wasTerminated, 1, 0) == 1) return;
            Done();
            string newResourceId;
            _fileInfoProvider.SaveFile(UserIdentity, _fileDic);
        }
    }
}

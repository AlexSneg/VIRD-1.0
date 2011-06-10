using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.PresentationShow.ShowCommon;
using TechnicalServices.Interfaces;

namespace Domain.PresentationShow.ShowService
{
    internal enum LogEntry
    {
        NotConneted,
        NotExists,
        NotUploadResource,
        NotUploadBackground,
        Terminated,
        Paused,
        NotEnoughSpace,
        ClearSpace,
        Unknown
    }

    internal class PreparationLog
    {
        #region Nested

        private class LogKey : IEqualityComparer<LogKey>
        {
            public LogKey(bool isError, LogEntry logEntry)
            {
                _isError = isError;
                _logEntry = logEntry;
            }
            private readonly bool _isError;
            private readonly LogEntry _logEntry;

            public bool IsError { get { return _isError; } }
            public LogEntry LogEntry { get { return _logEntry; } }
            public bool Equals(LogKey x, LogKey y)
            {
                return x.IsError == y.IsError && x.LogEntry == y.LogEntry;
            }

            public int GetHashCode(LogKey obj)
            {
                return (obj.IsError ? 1 : 0 + ((int)obj.LogEntry + 1) * 10).GetHashCode();
            }
        }

        #endregion

        private readonly Dictionary<LogKey, List<string>> _logStorage = new Dictionary<LogKey, List<string>>();
        private readonly IEventLogging _eventLogging;

        public Action<string> OnLogMessage;
        private void LogMessage(string message)
        {
            if (OnLogMessage != null)
            {
                OnLogMessage(message);
            }
        }

        public PreparationLog(IEventLogging eventLogging)
        {
            _eventLogging = eventLogging;
        }

        public PreparationResult GetPreparationResult()
        {
            PreparationResult result = new PreparationResult();
            foreach (KeyValuePair<LogKey, List<string>> pair in _logStorage)
            {
                StringBuilder builder = new StringBuilder();

                StringBuilder namesBuilder = new StringBuilder();
                bool first = true;
                foreach (string name in pair.Value)
                {
                    if (!first)
                    {
                        namesBuilder.Append(", ");
                    }
                    else
                    {
                        first = false;
                    }
                    namesBuilder.Append(name);
                }
                string names=namesBuilder.ToString();

                switch (pair.Key.LogEntry)
                {
                    case LogEntry.NotConneted:
                        builder.AppendFormat("Недоступно оборудование: {0}", names);
                        break;
                    case LogEntry.NotExists:
                        builder.AppendFormat("Следующие устройства не существуют: {0}", names);
                        break;
                    case LogEntry.NotUploadResource:
                        builder.AppendFormat("{0}: Загрузка источников не завершена.", names);
                        break;
                    case LogEntry.NotUploadBackground:
                        builder.AppendFormat("{0}: Не удалось скопировать фоновое изображение. ", names);
                        break;
                    case LogEntry.Terminated:
                        builder.AppendFormat("{0}: Загрузка источников не завершена: прервана пользователем. ", names);
                        break;
                    case LogEntry.Paused:
                        builder.AppendFormat("{0}: Загрузка источников не завершена: приостановлена пользователем. ", names);
                        break;
                    case LogEntry.NotEnoughSpace:
                        builder.AppendFormat("{0}: На агенте не хватает свободного места. ", names);
                        break;
                    case LogEntry.ClearSpace:
                        builder.AppendFormat("{0}: Очищено дисковое пространство. ", names);
                        break;
                    case LogEntry.Unknown:
                    default:
                        builder.AppendFormat("{0}: Неизвестная ошибка: ", names);
                        break;
                }

                if (pair.Key.IsError)
                    result.ErrorLog.Add(builder.ToString());
                else
                    result.WarningLog.Add(builder.ToString());
            }
            if (result.ErrorLog.Count > 0)
                result.WithError = true;
            if (result.WarningLog.Count > 0)
                result.WithWarning = true;
            return result;
        }

        /// <summary>
        /// стирает предыдущий лог
        /// </summary>
        public void Clear()
        {
            _logStorage.Clear();
        }

        /// <summary>
        /// запись в лог
        /// </summary>
        /// <param name="logEntry">тип записи</param>
        /// <param name="isError">ошибка или warning</param>
        /// <param name="equipmentName">имя оборудования</param>
        public void AddLog(LogEntry logEntry, bool isError, string equipmentName)
        {
            lock (this)
            {
                string message=ComposeMessage(logEntry, equipmentName);
                if (isError) _eventLogging.WriteError(message);
                else _eventLogging.WriteWarning(message);
                AddToLogDictionary(new LogKey(isError, logEntry), equipmentName);
                LogMessage(message);
            }
        }

        private void AddToLogDictionary(LogKey logKey, string equipmentName)
        {
            List<string> list;
            if (!_logStorage.TryGetValue(logKey, out list))
            {
                _logStorage[logKey] = list = new List<string>();
            }
            list.Add(equipmentName);
        }

        private string ComposeMessage(LogEntry logEntry, string equipmentName)
        {
            switch (logEntry)
            {
                case LogEntry.NotExists:
                    return string.Format("{0} не существует", equipmentName);
                case LogEntry.NotConneted:
                    return string.Format("{0} не удалось законектиться", equipmentName);
                case LogEntry.NotUploadResource:
                    return string.Format("{0}: Загрузка источников не завершена. ",
                                         equipmentName);
                case LogEntry.NotUploadBackground:
                    return string.Format("{0}: Не удалось загрузить фоновое изображение для агента.",
                        equipmentName);
                case LogEntry.Terminated:
                    return  string.Format("{0}: Загрузка источников не завершена: прервана пользователем. ", equipmentName);
                case LogEntry.Paused:
                    return string.Format("{0}: Загрузка источников не завершена: приостановлена пользователем. ", equipmentName);
                case LogEntry.NotEnoughSpace:
                    return string.Format("{0}: На агенте не хватает свободного места. ", equipmentName);
                case LogEntry.ClearSpace:
                    return string.Format("{0}: Очищено дисковое пространство. ", equipmentName);
                default:
                    return "PreparationLog.ComposeMessage: неизвестная ошибка";
            }
        }
    }
}

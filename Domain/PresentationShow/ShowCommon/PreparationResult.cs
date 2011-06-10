using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Domain.PresentationShow.ShowCommon
{
    [DataContract]
    public class PreparationResult
    {
        [DataMember]
        private readonly List<string> _errorLog = new List<string>();
        [DataMember]
        private readonly List<string> _warningLog = new List<string>();
        [DataMember]
        public bool WithError { get; set; }
        [DataMember]
        public bool WithWarning { get; set; }

        public List<string> ErrorLog
        {
            get { return _errorLog; }
        }

        public List<string> WarningLog
        {
            get { return _warningLog; }
        }
    }
}

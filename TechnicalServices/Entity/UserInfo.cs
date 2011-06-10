using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;

namespace TechnicalServices.Entity
{
    [Serializable]
    public class UserInfo
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Алгоритм для хеша = MD5
        /// </summary>
        [XmlAttribute("Hash", DataType = "base64Binary")]
        public byte[] Hash { get; set; }

        [XmlAttribute("FullName")]
        public string FullName { get; set; }

        [XmlAttribute("Date", DataType = "date")]
        public DateTime Date { get; set; }

        [XmlAttribute("Enable")]
        public bool Enable { get; set; }

        [XmlAttribute("IsOperator")]
        public bool IsOperator { get; set; }

        [XmlAttribute("IsAdmin")]
        public bool IsAdmin { get; set; }

        [XmlAttribute("Priority")]
        public int Priority { get; set; }

        [XmlAttribute("Comment")]
        public string Comment { get; set; }

        public static UserInfo Empty
        {
            get
            {
                return new UserInfo
                           {
                               Name = "unknown",
                               FullName = String.Empty,
                               Enable = true,
                               IsOperator = true,
                               IsAdmin = false,
                               Priority = 0
                           };
            }
        }

        [DebuggerStepThrough]
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture,
                                 "Name:{0}, Hash:{1}, FullName:{2}, Enable:{3}, IsOperator:{4}, IsAdmin:{5}",
                                 Name,
                                 Convert.ToBase64String(Hash),
                                 FullName,
                                 Enable,
                                 IsOperator,
                                 IsAdmin);
        }
    }
}
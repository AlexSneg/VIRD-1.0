using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace TechnicalServices.Common
{
    [DataContract]
    public struct FileTransferObject
    {
        public FileTransferObject(int countToWrite, int part, int numberOfParts, string fileId, byte[] fileContent)
        {
            CountToWrite = countToWrite;
            Part = part;
            NumberOfParts = numberOfParts;
            FileId = fileId;
            byte[] buffer = fileContent;
            if (CountToWrite < fileContent.Length)
            {
                buffer = new byte[CountToWrite];
                Array.Copy(fileContent, buffer, CountToWrite);
            }
            FileContent = buffer;
        }

        [DataMember(IsRequired = true)]
        public byte[] FileContent;
        [DataMember(IsRequired = true)]
        public string FileId;
        [DataMember]
        public int NumberOfParts;
        [DataMember]
        public int Part;
        [DataMember]
        public int CountToWrite;
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TechnicalServices.Util
{
    public sealed class FileHelper
    {
        private FileHelper() { }

        public static byte[] GetFileAsByteArray(string fullFileName)
        {
            byte[] buffer = new byte[256];
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (FileStream file = File.OpenRead(fullFileName))
                {
                    int count;
                    while ((count = file.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        memoryStream.Write(buffer, 0, count);
                    }
                }
                return memoryStream.ToArray();
            }
        }

        public static void SaveToFile(string fullFileName, byte[] byteArr)
        {
            using (FileStream file = File.Create(fullFileName))
            {
                file.Write(byteArr, 0, byteArr.Length);
            }
        }
    }
}
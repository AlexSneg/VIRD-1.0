using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace TechnicalServices.Util
{
    public sealed class BinarySerializer
    {
        private BinarySerializer()
        {
        }

        public static byte[] Serialize<T>(T obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        //public static byte[] SerializeAsByte<T>(T obj)
        //{
        //    using(Stream stream = Serialize<T>(obj))
        //    {
        //        return stream.
        //    }
        //}

        public static T Deserialize<T>(byte[] serializedObject)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(serializedObject))
            {
                T obj = (T) formatter.Deserialize(stream);
                return obj;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Hosts.Plugins.BusinessGraphics.SystemModule.Design
{
    class DbfFile
    {

        private DbfFileHeader header;
        private List<Dictionary<String, String>> records = new List<Dictionary<String, String>>();

        public String Get(int rowNo, String column)
        {
            Dictionary<String, String> row = records[rowNo];
            return row.ContainsKey(column) ? row[column] : "";
        }

        public DbfFile(String path)
        {
            Binary stream = new Binary(File.OpenRead(path));
            header = new DbfFileHeader(stream);
            stream.Seek(header.DataAreaOffset);
            for (int i = 0, count = header.RecordCount; i < count; i++)
            {
                Dictionary<String, String> record = header.ReadRecord(stream);
                if (record != null)
                {
                    records.Add(record);
                }
            }
        }


    }

    class Binary
    {

        private FileStream stream;

        public void Seek(int offset)
        {
            stream.Seek(offset, SeekOrigin.Begin);
        }

        public void Close()
        {
            stream.Close();
        }

        public void Read(byte[] bytes, int offset, int len)
        {
            stream.Read(bytes, offset, len);
        }

        public Binary(FileStream stream)
        {
            this.stream = stream;
        }

        public byte readByte()
        {
            byte temp = (byte)stream.ReadByte();
            Console.WriteLine(String.Format("Byte={0:X2}", temp));
            return temp;
        }

        public int readInt2()
        {
            int byte1 = readByte();
            int byte2 = readByte();
            return byte1 + (byte2 << 8);
        }

        public int readInt4()
        {
            int word1 = readInt2();
            int word2 = readInt2();
            return word1 + (word2 << 16);
        }

    }

    class Encoder
    {

        private static Encoding cp1251 = Encoding.GetEncoding("windows-1251");
        private static Encoding utf8 = Encoding.GetEncoding("utf-8");
        public static String fromBytes(byte[] textBytes)
        {
            return cp1251.GetString(textBytes);
        }
        public static void toBytes(byte[] bytes, String text)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(text);
            Array.Clear(bytes, 0, bytes.Length);
            Array.Copy(encoded, bytes, Math.Min(bytes.Length, encoded.Length));
        }
    }

    class DbfFileHeader
    {

        const bool USE_RESERVED_AREA_3 = false;
        const byte LIVE_RECORD_MARKER = 0x20;

        private byte typeSignature;
        private byte lastUpdateMonth;
        private byte lastUpdateYear;
        private byte lastUpdateDay;
        private int recordCount;
        private int recordSize;
        private int dataAreaOffset;
        private byte[] reservedArea1 = new byte[16];
        private byte[] reservedArea2 = new byte[2];
        private byte[] reservedArea3 = new byte[263];
        private byte flags;
        private byte codepage;
        private List<DbfSubrecord> subrecords = new List<DbfSubrecord>();

        public int DataAreaOffset
        {
            get
            {
                return dataAreaOffset;
            }
        }
        public int RecordCount
        {
            get
            {
                return recordCount;
            }
        }

        public DbfFileHeader(Binary stream)
        {
            typeSignature = stream.readByte();
            lastUpdateYear = stream.readByte();
            lastUpdateMonth = stream.readByte();
            lastUpdateDay = stream.readByte();
            recordCount = stream.readInt4();
            dataAreaOffset = stream.readInt2();
            recordSize = stream.readInt2();
            stream.Read(reservedArea1, 0, reservedArea1.Length);
            flags = stream.readByte();
            codepage = stream.readByte();
            stream.Read(reservedArea2, 0, reservedArea2.Length);
            DbfSubrecord subrecord = DbfSubrecord.read(stream);
            while (subrecord != null)
            {
                subrecords.Add(subrecord);
                subrecord = DbfSubrecord.read(stream);
            }
        }

        public Dictionary<String, String> ReadRecord(Binary stream)
        {
            byte marker = stream.readByte();
            if (marker != LIVE_RECORD_MARKER)
            {
                return null;
            }
            Console.WriteLine(marker);
            Dictionary<String, String> record = new Dictionary<String, String>();
            foreach (DbfSubrecord sr in subrecords)
            {
                String value = sr.ReadValue(stream);
                record.Add(sr.Name, value);
            }
            return record;
        }

    }

    class DbfSubrecord
    {
        private byte[] nameBytes = new byte[11];
        private String name;
        private byte type;
        private int offsetInRecord;
        private int size;
        private int decimals;
        private byte flags;
        private int autoincNextValue;
        private int autoincStep;
        private byte[] reservedArea = new byte[8];
        private int utf8Size;

        public String Name
        {
            get { return name; }
        }

        public static DbfSubrecord read(Binary stream)
        {
            // Console.WriteLine("-Subrecord");
            byte nameFirstByte = stream.readByte();
            if (nameFirstByte == 0x0D)
            {
                return null;
            }
            else
            {
                DbfSubrecord sr = new DbfSubrecord();
                sr.nameBytes[0] = nameFirstByte;
                for (int i = 1; i < sr.nameBytes.Length; i++)
                {
                    sr.nameBytes[i] = stream.readByte();
                }
                sr.name = Encoder.fromBytes(sr.nameBytes);
                if (sr.name[sr.name.Length - 1] == '\0')
                {
                    sr.name = sr.name.Substring(0, sr.name.Length - 1);
                }
                sr.type = stream.readByte();
                sr.offsetInRecord = stream.readInt4();
                sr.size = stream.readByte();
                sr.decimals = stream.readByte();
                sr.flags = stream.readByte();
                sr.autoincNextValue = stream.readInt4();
                sr.autoincStep = stream.readByte();
                stream.Read(sr.reservedArea, 0, sr.reservedArea.Length);
                sr.utf8Size = (sr.type == (byte)'C') ? sr.size * 2 : sr.size;
                return sr;
            }
        }

        public String ReadValue(Binary stream)
        {
            byte[] bytes = new byte[size];
            stream.Read(bytes, 0, size);
            return Encoder.fromBytes(bytes);
        }

    }
}

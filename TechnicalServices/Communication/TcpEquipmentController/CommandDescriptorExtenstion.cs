using System;
using System.Globalization;
using System.Text;

using TechnicalServices.Interfaces;

namespace TechnicalServices.Communication.TcpEquipmentController
{
    public static class CommandDescriptorExtenstion
    {
        public const string CommandFormat = "{0} {1}({2})";
        public const string PacketEndMarker = "\r";
        public const string PacketFormat = "{0}" + PacketEndMarker;
        public const string ParameterDelimiter = ",";

        private static readonly NumberFormatInfo numberFormat = new NumberFormatInfo();
        private static readonly StringBuilder param = new StringBuilder(1024);


        public static string FormatToString(this CommandDescriptor cmd)
        {
            lock (typeof (CommandDescriptor))
            {
                param.Length = 0;
                foreach (IConvertible item in cmd.Parameters)
                {
                    param.Append(ParameterDelimiter);
                    switch (item.GetTypeCode())
                    {
                        case TypeCode.SByte:
                        case TypeCode.Byte:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            param.Append(item.ToString(numberFormat));
                            break;
                        case TypeCode.String:
                            param.Append('"' + item.ToString().Replace("\"", "\"\"") + '"');
                            break;
                        default:
                            throw new ApplicationException("Не поддерживаемый тип в параметре команды");
                    }
                }
                if (param.Length > 0) param.Remove(0, ParameterDelimiter.Length);
                return String.Format(CommandFormat, cmd.EquipmentId, cmd.CommandName, param);
            }
        }
    }
}
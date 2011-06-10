using System;
using System.Collections.Generic;

namespace TechnicalServices.Communication.EquipmentController
{
    public enum ChannelError
    {
        ConnectionBroken,
        ErrorEndMarker,
        WrongUID
    }

    public class ChannelException : ApplicationException
    {
        private static readonly Dictionary<ChannelError, string> list =
            new Dictionary<ChannelError, string>(Enum.GetValues(typeof (ChannelError)).Length)
                {
                    {ChannelError.ConnectionBroken, "Ёп тить!"},
                    {ChannelError.ErrorEndMarker, "Не правильный признак конца команды!"},
                    {ChannelError.WrongUID, "Не правильный признак UID оборудования"}
                };

        public ChannelException(ChannelError errCode)
            : base(list[errCode])
        {
        }

        public ChannelException(Exception ex, ChannelError errCode)
            : base(list[errCode], ex)
        {
        }
    }
}
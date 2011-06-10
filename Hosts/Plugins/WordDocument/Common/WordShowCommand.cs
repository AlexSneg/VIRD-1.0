using System;

namespace Hosts.Plugins.WordDocument.Common
{
    [Serializable]
    public enum WordShowCommand
    {
        Next,
        Prev,
        Left,
        Right,
        NextPage,
        PrevPage,
        LastPage,
        FirstPage,
        GoToPage,
        Zoom,
        Status,
    }
}
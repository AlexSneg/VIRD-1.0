using System;

namespace Hosts.Plugins.PowerPointPresentation.Common
{
    [Serializable]
    public enum PowerPointShowCommand
    {
        NextSlide,
        PrevSlide,
        GoToSlide,
        Status,
    }
}
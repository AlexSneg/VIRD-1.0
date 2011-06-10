using System.Collections.Generic;

namespace TechnicalServices.Util
{
    public static class ListExtension
    {
        public static void AddNotNull<T>(this List<T> list, T arg) where T : class
        {
            if (arg != null) list.Add(arg);
        }
    }
}
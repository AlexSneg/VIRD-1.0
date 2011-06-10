using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Common.Classes
{
    public class NotifierEventArg<T> : EventArgs
    {
        public T Data
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TechnicalServices.Common.Utils
{
    public class Identity
    {
        private int _identity;
        public Identity(int currentIdentity)
        {
            _identity = currentIdentity;
        }

        public int NextID
        {
            get
            {
                lock (this)
                {
                    return ++_identity;
                }
            }
        }

        public int CurrentID
        {
            get
            {
                lock (this)
                {
                    return _identity;
                }
            }
        }
    }
}

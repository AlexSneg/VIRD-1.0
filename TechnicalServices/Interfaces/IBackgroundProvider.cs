using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace TechnicalServices.Interfaces
{
    public interface IBackgroundProvider : IDisposable
    {
        void SetBackgroundImage(string imageFileName, Control control, int width, int height);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.Visualizator
{
    public interface IDoCommand
    {
        string DoCommand(string command);
    }
}

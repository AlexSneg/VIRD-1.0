using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using TechnicalServices.Configuration.Server.Properties;
using TechnicalServices.Entity;

namespace TechnicalServices.Configuration.Server
{
    [Serializable]
    [XmlRoot(Namespace = "urn:userstorage-schema", ElementName = "UserStorage")]
    public class UserStorage : List<UserInfo>
    {
        internal UserStorage()
            : base(4)
        {
        }
    }
}
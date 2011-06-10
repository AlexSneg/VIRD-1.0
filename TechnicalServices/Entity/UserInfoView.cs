using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Entity
{
    /// <summary>
    /// представление данных о пользователе для административной консоль
    /// </summary>
    public class UserInfoView
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string FullName { get; set; }
        
        public string Role { get; set; }
        
        public string Comment { get; set; }
        
    }
}

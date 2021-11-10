﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppDesktopUI.Library.Models
{
    public class LoggedInUserModel : ILoggedInUserModel
    {
        public string Token { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAdress { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

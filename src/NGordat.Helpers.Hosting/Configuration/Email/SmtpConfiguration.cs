﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGordat.Helpers.Hosting.Configuration.Email
{
    public class SmtpConfiguration
    {
        public string From { get; set; }
        public string Host { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Port { get; set; } = 587; // default smtp port
        public bool UseSSL { get; set; } = true;
    }
}

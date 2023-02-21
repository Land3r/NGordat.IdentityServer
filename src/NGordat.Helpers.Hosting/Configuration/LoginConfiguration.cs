using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGordat.Helpers.Hosting.Configuration
{
    public class LoginConfiguration
    {
        public LoginResolutionPolicy ResolutionPolicy { get; set; }
    }

    public enum LoginResolutionPolicy
    {
        Username,
        Email
    }
}

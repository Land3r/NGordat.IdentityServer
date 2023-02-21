using Microsoft.Extensions.Configuration;
using NGordat.Helpers.Hosting.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGordat.Helpers.Hosting.Extensions
{
    public static class ConfigurationExtensions
    {

        #region Configuration

        /// <summary>
        /// Get configuration for login
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static LoginConfiguration GetLoginConfiguration(this IConfiguration configuration)
        {
            var loginConfiguration = configuration.GetSection(nameof(LoginConfiguration)).Get<LoginConfiguration>();

            // Cannot load configuration - use default configuration values
            if (loginConfiguration == null)
            {
                return new LoginConfiguration();
            }

            return loginConfiguration;
        }

        /// <summary>
        /// Get configuration for registration
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static RegisterConfiguration GetRegistrationConfiguration(this IConfiguration configuration)
        {
            var registerConfiguration = configuration.GetSection(nameof(RegisterConfiguration)).Get<RegisterConfiguration>();

            // Cannot load configuration - use default configuration values
            if (registerConfiguration == null)
            {
                return new RegisterConfiguration();
            }

            return registerConfiguration;
        }

        #endregion Configuration
    }
}

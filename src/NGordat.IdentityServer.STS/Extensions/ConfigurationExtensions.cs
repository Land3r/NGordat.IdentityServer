using NGordat.IdentityServer.STS.Configuration.Interfaces;
using NGordat.IdentityServer.STS.Configuration;
using NGordat.IdentityServer.STS.Constants;
using Microsoft.Extensions.Configuration;

namespace NGordat.IdentityServer.STS.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IRootConfiguration CreateRootConfiguration(IConfiguration configuration)
        {
            var rootConfiguration = new RootConfiguration();
            configuration.GetSection(nameof(AdminConfiguration)).Bind(rootConfiguration.AdminConfiguration);
            configuration.GetSection(nameof(RegisterConfiguration)).Bind(rootConfiguration.RegisterConfiguration);
            return rootConfiguration;
        }
    }
}

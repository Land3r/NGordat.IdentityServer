using NGordat.Helpers.Hosting.Configuration;

namespace NGordat.IdentityServer.STS.Configuration.Interfaces
{
    public interface IRootConfiguration
    {
        AdminConfiguration AdminConfiguration { get; }

        RegisterConfiguration RegisterConfiguration { get; }

        SecurityConfiguration SecurityConfiguration { get; }
    }
}

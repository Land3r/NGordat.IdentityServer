﻿using NGordat.IdentityServer.STS.Configuration.Interfaces;

namespace NGordat.IdentityServer.STS.Configuration
{
    public class RootConfiguration : IRootConfiguration
    {
        public AdminConfiguration AdminConfiguration { get; } = new AdminConfiguration();
        public RegisterConfiguration RegisterConfiguration { get; } = new RegisterConfiguration();
    }
}

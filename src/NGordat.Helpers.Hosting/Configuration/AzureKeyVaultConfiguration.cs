using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGordat.Helpers.Hosting.Configuration
{
    public class AzureKeyVaultConfiguration
    {
        public string AzureKeyVaultEndpoint { get; set; }

        public string TenantId { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public bool UseClientCredentials { get; set; }

        public string IdentityServerCertificateName { get; set; }

        public string DataProtectionKeyIdentifier { get; set; }

        public bool ReadConfigurationFromKeyVault { get; set; }
    }
}

using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using NGordat.Helpers.Hosting.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NGordat.Helpers.Hosting.Helpers
{
    public class AzureKeyVaultService
    {
        private readonly AzureKeyVaultConfiguration _azureKeyVaultConfiguration;

        public AzureKeyVaultService(AzureKeyVaultConfiguration azureKeyVaultConfiguration)
        {
            if (azureKeyVaultConfiguration == null)
            {
                throw new ArgumentException("missing azureKeyVaultConfiguration");
            }

            if (string.IsNullOrEmpty(azureKeyVaultConfiguration.AzureKeyVaultEndpoint))
            {
                throw new ArgumentException("missing keyVaultEndpoint");
            }

            _azureKeyVaultConfiguration = azureKeyVaultConfiguration;
        }

        private async Task<List<CertificateItem>> GetAllEnabledCertificateVersionsAsync(IKeyVaultClient keyVaultClient)
        {
            // Get all the certificate versions (this will also get the current active version)
            var certificateVersions = await keyVaultClient.GetCertificateVersionsAsync(_azureKeyVaultConfiguration.AzureKeyVaultEndpoint, _azureKeyVaultConfiguration.IdentityServerCertificateName);

            // Find all enabled versions of the certificate and sort them by creation date in descending order 
            return certificateVersions
              .Where(certVersion => certVersion.Attributes.Enabled.HasValue && certVersion.Attributes.Enabled.Value)
              .OrderByDescending(certVersion => certVersion.Attributes.Created)
              .ToList();
        }

        private async Task<X509Certificate2> GetCertificateAsync(string identifier, IKeyVaultClient keyVaultClient)
        {
            var certificateVersionBundle = await keyVaultClient.GetCertificateAsync(identifier);
            var certificatePrivateKeySecretBundle = await keyVaultClient.GetSecretAsync(certificateVersionBundle.SecretIdentifier.Identifier);
            var privateKeyBytes = Convert.FromBase64String(certificatePrivateKeySecretBundle.Value);
            var certificateWithPrivateKey = new X509Certificate2(privateKeyBytes, (string)null, X509KeyStorageFlags.MachineKeySet);

            return certificateWithPrivateKey;
        }
    }
}

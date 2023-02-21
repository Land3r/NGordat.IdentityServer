using Azure.Identity;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NGordat.Helpers.Hosting.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGordat.Helpers.Hosting.Extensions
{
    public static class ServiceCollectionExtensions
    {
        #region DataProtection

        public static void AddDataProtection<TDbContext>(this IServiceCollection services, IConfiguration configuration)
                    where TDbContext : DbContext, IDataProtectionKeyContext
        {
            AddDataProtection<TDbContext>(
                services,
                configuration.GetSection(nameof(DataProtectionConfiguration)).Get<DataProtectionConfiguration>(),
                configuration.GetSection(nameof(AzureKeyVaultConfiguration)).Get<AzureKeyVaultConfiguration>());
        }

        public static void AddDataProtection<TDbContext>(this IServiceCollection services, DataProtectionConfiguration dataProtectionConfiguration, AzureKeyVaultConfiguration azureKeyVaultConfiguration)
            where TDbContext : DbContext, IDataProtectionKeyContext
        {
            var dataProtectionBuilder = services.AddDataProtection()
                .SetApplicationName("Skoruba.Duende.IdentityServer")
                .PersistKeysToDbContext<TDbContext>();

            if (dataProtectionConfiguration.ProtectKeysWithAzureKeyVault)
            {
                if (azureKeyVaultConfiguration.UseClientCredentials)
                {
                    dataProtectionBuilder.ProtectKeysWithAzureKeyVault(
                        new Uri(azureKeyVaultConfiguration.DataProtectionKeyIdentifier),
                        new ClientSecretCredential(azureKeyVaultConfiguration.TenantId,
                            azureKeyVaultConfiguration.ClientId, azureKeyVaultConfiguration.ClientSecret));
                }
                else
                {
                    dataProtectionBuilder.ProtectKeysWithAzureKeyVault(new Uri(azureKeyVaultConfiguration.DataProtectionKeyIdentifier), new DefaultAzureCredential());
                }
            }
        }

        public static void AddAzureKeyVaultConfiguration(this IConfiguration configuration, IConfigurationBuilder configurationBuilder)
        {
            if (configuration.GetSection(nameof(AzureKeyVaultConfiguration)).Exists())
            {
                var azureKeyVaultConfiguration = configuration.GetSection(nameof(AzureKeyVaultConfiguration)).Get<AzureKeyVaultConfiguration>();

                if (azureKeyVaultConfiguration.ReadConfigurationFromKeyVault)
                {
                    //if (azureKeyVaultConfiguration.UseClientCredentials)
                    //{
                    //    configurationBuilder.AddAzureKeyVault(azureKeyVaultConfiguration.AzureKeyVaultEndpoint,
                    //        azureKeyVaultConfiguration.ClientId, azureKeyVaultConfiguration.ClientSecret);
                    //}
                    //else
                    //{
                    //    var keyVaultClient = new KeyVaultClient(
                    //        new KeyVaultClient.AuthenticationCallback(new AzureServiceTokenProvider()
                    //            .KeyVaultTokenCallback));

                    //    configurationBuilder.AddAzureKeyVault(azureKeyVaultConfiguration.AzureKeyVaultEndpoint,
                    //        keyVaultClient, new DefaultKeyVaultSecretManager());
                    //}
                }
            }
        }

        #endregion DataProtection
    }
}

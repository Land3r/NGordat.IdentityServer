using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NGordat.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NGordat.Helpers.Hosting.Extensions;
using NGordat.Helpers.Hosting.Authentication;
using Microsoft.AspNetCore.Authentication;

namespace NGordat.IdentityServer.Dal.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterDbContexts<TKey>(this IServiceCollection services, IConfiguration configuration)
            where TKey : IEquatable<TKey>
        {
            // Following line is specific to postgres ef provider.
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddDbContext<IdentityServerConfigurationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString(typeof(IdentityServerConfigurationDbContext).GetFriendlyName())));
            services.AddDbContext<IdentityServerDataProtectionDbContext>(options => options.UseNpgsql(configuration.GetConnectionString(typeof(IdentityServerDataProtectionDbContext).GetFriendlyName())));
            services.AddDbContext<IdentityServerPersistedGrantDbContext>(options => options.UseNpgsql(configuration.GetConnectionString(typeof(IdentityServerPersistedGrantDbContext).GetFriendlyName())));
            services.AddDbContext<IdentityServerIdentityDbContext<TKey>>(options => options.UseNpgsql(configuration.GetConnectionString(typeof(IdentityServerIdentityDbContext<TKey>).GetFriendlyName())));
        }
    }
}

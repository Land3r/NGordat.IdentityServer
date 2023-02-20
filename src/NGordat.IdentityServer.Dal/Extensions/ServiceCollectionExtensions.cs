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

namespace NGordat.IdentityServer.Dal.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterDbContexts<TKey>(this IServiceCollection services, IConfiguration configuration)
            where TKey : IEquatable<TKey>
        {
            // Following line is specific to postgres ef provider.
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddDbContext<IdentityServerConfigurationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString(nameof(IdentityServerConfigurationDbContext))));
            services.AddDbContext<IdentityServerDataProtectionDbContext>(options => options.UseNpgsql(configuration.GetConnectionString(nameof(IdentityServerDataProtectionDbContext))));
            services.AddDbContext<IdentityServerPersistedGrantDbContext>(options => options.UseNpgsql(configuration.GetConnectionString(nameof(IdentityServerPersistedGrantDbContext))));
            services.AddDbContext<IdentityServerIdentityDbContext<TKey>>(options => options.UseNpgsql(configuration.GetConnectionString(nameof(IdentityServerPersistedGrantDbContext))));
        }

        public static void RegisterAuthentication<TKey>(this IServiceCollection services, IConfiguration configuration)
            where TKey : IEquatable<TKey>
        {
            services.AddAuthenticationServices<IdentityServerIdentityDbContext<TKey>, UserIdentity<TKey>, UserIdentityRole<TKey>>(configuration);
            services.AddIdentityServer<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, UserIdentity<TKey>>(services.AddAuthenticationServices<IdentityServerIdentityDbContext<TKey>, UserIdentity<TKey>, UserIdentityRole<TKey>>(configuration));
        }

        private void AddAuthenticationServices<TIdentityDbContext, TUserIdentity, TUserIdentityRole>(this IServiceCollection services, IConfiguration configuration) where TIdentityDbContext : DbContext
            where TUserIdentity : class
            where TUserIdentityRole : class
        {
            var loginConfiguration = GetLoginConfiguration(configuration);
            var registrationConfiguration = GetRegistrationConfiguration(configuration);
            var identityOptions = configuration.GetSection(nameof(IdentityOptions)).Get<IdentityOptions>();

            services
                .AddSingleton(registrationConfiguration)
                .AddSingleton(loginConfiguration)
                .AddSingleton(identityOptions)
                .AddScoped<ApplicationSignInManager<TUserIdentity>>()
                .AddScoped<UserResolver<TUserIdentity>>()
                .AddIdentity<TUserIdentity, TUserIdentityRole>(options => configuration.GetSection(nameof(IdentityOptions)).Bind(options))
                .AddEntityFrameworkStores<TIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.Secure = CookieSecurePolicy.SameAsRequest;
                options.OnAppendCookie = cookieContext =>
                    AuthenticationHelpers.CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext =>
                    AuthenticationHelpers.CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            });


            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            var authenticationBuilder = services.AddAuthentication();

            AddExternalProviders(authenticationBuilder, configuration);
        }
    }
}

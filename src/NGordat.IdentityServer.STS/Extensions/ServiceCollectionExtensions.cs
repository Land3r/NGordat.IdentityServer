using Duende.IdentityServer.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Identity.Web;
using NGordat.Helpers.Hosting.Authentication;
using NGordat.Helpers.Hosting.Extensions;
using NGordat.Identity.Domain.Entities;
using NGordat.IdentityServer.Dal;
using NGordat.IdentityServer.Dal.Helpers;
using NGordat.IdentityServer.Dal.Interfaces;
using NGordat.IdentityServer.STS.Configuration;
using NGordat.IdentityServer.STS.Configuration.Interfaces;
using NGordat.IdentityServer.STS.Constants;
using NGordat.IdentityServer.STS.Services;
using NGordat.IdentityServer.STS.Services.Localization;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NGordat.IdentityServer.STS.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAuthentication<TKey>(this IServiceCollection services, IConfiguration configuration)
            where TKey : IEquatable<TKey>
        {
            services.AddAuthenticationServices<IdentityServerIdentityDbContext, UserIdentity, UserIdentityRole>(configuration);
            services.AddIdentityServer<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, UserIdentity>(configuration);
        }

        private static void AddAuthenticationServices<TIdentityDbContext, TUserIdentity, TUserIdentityRole>(this IServiceCollection services, IConfiguration configuration) where TIdentityDbContext : DbContext
            where TUserIdentity : class
            where TUserIdentityRole : class
        {
            var loginConfiguration = configuration.GetLoginConfiguration();
            var registrationConfiguration = configuration.GetRegistrationConfiguration();
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

        /// <summary>
        /// Add external providers
        /// </summary>
        /// <param name="authenticationBuilder"></param>
        /// <param name="configuration"></param>
        private static void AddExternalProviders(AuthenticationBuilder authenticationBuilder,
            IConfiguration configuration)
        {
            var externalProviderConfiguration = configuration.GetSection(nameof(ExternalProvidersConfiguration)).Get<ExternalProvidersConfiguration>();

            if (externalProviderConfiguration.UseGitHubProvider)
            {
                authenticationBuilder.AddGitHub(options =>
                {
                    options.ClientId = externalProviderConfiguration.GitHubClientId;
                    options.ClientSecret = externalProviderConfiguration.GitHubClientSecret;
                    options.CallbackPath = externalProviderConfiguration.GitHubCallbackPath;
                    options.Scope.Add("user:email");
                });
            }

            if (externalProviderConfiguration.UseAzureAdProvider)
            {
                authenticationBuilder.AddMicrosoftIdentityWebApp(options =>
                {
                    options.ClientSecret = externalProviderConfiguration.AzureAdSecret;
                    options.ClientId = externalProviderConfiguration.AzureAdClientId;
                    options.TenantId = externalProviderConfiguration.AzureAdTenantId;
                    options.Instance = externalProviderConfiguration.AzureInstance;
                    options.Domain = externalProviderConfiguration.AzureDomain;
                    options.CallbackPath = externalProviderConfiguration.AzureAdCallbackPath;
                }, cookieScheme: null);
            }

            if (externalProviderConfiguration.UseGoogleProvider)
            {
                authenticationBuilder.AddGoogle(options =>
                {
                    //options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to https://localhost:5001/signin-google
                    options.ClientId = externalProviderConfiguration.GoogleClientId;
                    options.ClientSecret = externalProviderConfiguration.GoogleClientSecret;
                });
            }
            if (externalProviderConfiguration.UseSteamProvider)
            {
                authenticationBuilder.AddSteam("Steam", options =>
                {
                    //options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    options.ApplicationKey = externalProviderConfiguration.SteamClientId;
                    // Note that Steam OpenId implementation doesn't support requesting additionnal claims.
                    // In order to add several claims from the Steam OpenId response, we need to add manually the requested claims
                    options.Events.OnAuthenticated = context =>
                    {
                        // Get user informations
                        //var steamOpenIdResponse = context.UserPayload.Deserialize<SteamOpenIdResponse>();


                        //string picture = steamOpenIdResponse?.Response?.Players?.FirstOrDefault()?.AvatarFull;
                        //string profile = steamOpenIdResponse?.Response?.Players?.FirstOrDefault()?.ProfileUrl;
                        //if (!string.IsNullOrEmpty(picture))
                        //{
                        //    context.Identity.AddClaim(new Claim(MidoneUserClaimConstants.Picture, picture));
                        //}
                        //if (!string.IsNullOrEmpty(profile))
                        //{
                        //    context.Identity.AddClaim(new Claim(MidoneUserClaimConstants.SteamProfile, profile));
                        //}

                        return Task.FromResult(0);
                    };
                });
            }
            if (externalProviderConfiguration.UseFacebookProvider)
            {
                authenticationBuilder.AddFacebook(options =>
                {
                    //options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.AppId = externalProviderConfiguration.FacebookClientId;
                    options.AppSecret = externalProviderConfiguration.FacebookClientSecret;
                    options.Scope.Add("public_profile");
                });
            }
        }

        /// <summary>
        /// Add configuration for Duende IdentityServer
        /// </summary>
        /// <typeparam name="TUserIdentity"></typeparam>
        /// <typeparam name="TConfigurationDbContext"></typeparam>
        /// <typeparam name="TPersistedGrantDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static IIdentityServerBuilder AddIdentityServer<TConfigurationDbContext, TPersistedGrantDbContext, TUserIdentity>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where TConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            where TUserIdentity : class
        {
            var configurationSection = configuration.GetSection(nameof(IdentityServerOptions));

            var identityServerOptions = configurationSection.Get<IdentityServerOptions>();

            var builder = services.AddIdentityServer(options =>
            {
                configurationSection.Bind(options);

                options.DynamicProviders.SignInScheme = IdentityConstants.ExternalScheme;
                options.DynamicProviders.SignOutScheme = IdentityConstants.ApplicationScheme;
            })
            .AddConfigurationStore<TConfigurationDbContext>()
            .AddOperationalStore<TPersistedGrantDbContext>()
            .AddAspNetIdentity<TUserIdentity>();

            services.ConfigureOptions<OpenIdClaimsMappingConfig>();

            if (!identityServerOptions.KeyManagement.Enabled)
            {
                builder.AddCustomSigningCredential(configuration);
                builder.AddCustomValidationKey(configuration);
            }

            builder.AddExtensionGrantValidator<DelegationGrantValidator>();

            return builder;
        }

        public static void RegisterAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorizationPolicies(configuration);
        }

        /// <summary>
        /// Add authorization policies
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddAuthorizationPolicies(this IServiceCollection services,
                IConfiguration configuration)
        {
            services.AddAuthorization(options =>
            {
                // TODO: Vérifier la syntaxe
                string? administrationRole = configuration.GetValue("AdminConfiguration__AdministrationRole", string.Empty);


                options.AddPolicy(AuthorizationConsts.AdministrationPolicy,
                    policy => policy.RequireRole(administrationRole));
            });
        }


        /// <summary>
        /// Register services for MVC and localization including available languages
        /// </summary>
        /// <param name="services"></param>
        public static IMvcBuilder AddRazorWithLocalization<TUser, TKey>(this IServiceCollection services, IConfiguration configuration)
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
        {
            services.AddLocalization(opts => { opts.ResourcesPath = ConfigurationConsts.ResourcesPath; });

            services.TryAddTransient(typeof(IGenericControllerLocalizer<>), typeof(GenericControllerLocalizer<>));

            var pageBuilder = services.AddRazorPages(o =>
            {
            }).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, opts =>
            {
                opts.ResourcesPath = ConfigurationConsts.ResourcesPath;
            })
            .AddDataAnnotationsLocalization()
            .ConfigureApplicationPartManager(m =>
            {
                m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider<TUser, TKey>());
            });

            var cultureConfiguration = configuration.GetSection(nameof(CultureConfiguration)).Get<CultureConfiguration>();
            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    // If cultures are specified in the configuration, use them (making sure they are among the available cultures),
                    // otherwise use all the available cultures
                    var supportedCultureCodes = (cultureConfiguration?.Cultures?.Count > 0 ?
                        cultureConfiguration.Cultures.Intersect(CultureConfiguration.AvailableCultures) :
                        CultureConfiguration.AvailableCultures).ToArray();

                    if (!supportedCultureCodes.Any()) supportedCultureCodes = CultureConfiguration.AvailableCultures;
                    var supportedCultures = supportedCultureCodes.Select(c => new CultureInfo(c)).ToList();

                    // If the default culture is specified use it, otherwise use CultureConfiguration.DefaultRequestCulture ("en")
                    var defaultCultureCode = string.IsNullOrEmpty(cultureConfiguration?.DefaultCulture) ?
                        CultureConfiguration.DefaultRequestCulture : cultureConfiguration?.DefaultCulture;

                    // If the default culture is not among the supported cultures, use the first supported culture as default
                    if (!supportedCultureCodes.Contains(defaultCultureCode)) defaultCultureCode = supportedCultureCodes.FirstOrDefault();

                    opts.DefaultRequestCulture = new RequestCulture(defaultCultureCode);
                    opts.SupportedCultures = supportedCultures;
                    opts.SupportedUICultures = supportedCultures;
                });

            return pageBuilder;
        }

        public static void RegisterHstsOptions(this IServiceCollection services)
        {
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });
        }

        public static void AddIdentityHealthChecks<TConfigurationDbContext, TPersistedGrantDbContext, TIdentityDbContext, TDataProtectionDbContext>(this IServiceCollection services, IConfiguration configuration)
            where TConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where TIdentityDbContext : DbContext
            where TDataProtectionDbContext : DbContext, IDataProtectionKeyContext
        {
            var serviceProvider = services.BuildServiceProvider();
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var configurationDbConnectionString = configuration.GetConnectionString(typeof(TConfigurationDbContext).GetFriendlyName());
                var persistedGrantsDbConnectionString = configuration.GetConnectionString(typeof(TPersistedGrantDbContext).GetFriendlyName());
                var identityDbConnectionString = configuration.GetConnectionString(typeof(TIdentityDbContext).GetFriendlyName());
                var dataProtectionDbConnectionString = configuration.GetConnectionString(typeof(TDataProtectionDbContext).GetFriendlyName());

                var healthChecksBuilder = services.AddHealthChecks()
                    .AddDbContextCheck<TConfigurationDbContext>("ConfigurationDbContext")
                    .AddDbContextCheck<TPersistedGrantDbContext>("PersistedGrantsDbContext")
                    .AddDbContextCheck<TIdentityDbContext>("IdentityDbContext")
                    .AddDbContextCheck<TDataProtectionDbContext>("DataProtectionDbContext");

                var configurationTableName = DbContextHelpers.GetEntityTable<TConfigurationDbContext>(scope.ServiceProvider);
                var persistedGrantTableName = DbContextHelpers.GetEntityTable<TPersistedGrantDbContext>(scope.ServiceProvider);
                var identityTableName = DbContextHelpers.GetEntityTable<TIdentityDbContext>(scope.ServiceProvider);
                var dataProtectionTableName = DbContextHelpers.GetEntityTable<TDataProtectionDbContext>(scope.ServiceProvider);

                healthChecksBuilder
                    .AddNpgSql(configurationDbConnectionString, name: "ConfigurationDb",
                        healthQuery: $"SELECT * FROM \"{configurationTableName}\" LIMIT 1")
                    .AddNpgSql(persistedGrantsDbConnectionString, name: "PersistentGrantsDb",
                        healthQuery: $"SELECT * FROM \"{persistedGrantTableName}\" LIMIT 1")
                    .AddNpgSql(identityDbConnectionString, name: "IdentityDb",
                        healthQuery: $"SELECT * FROM \"{identityTableName}\" LIMIT 1")
                    .AddNpgSql(dataProtectionDbConnectionString, name: "DataProtectionDb",
                        healthQuery: $"SELECT * FROM \"{dataProtectionTableName}\"  LIMIT 1");
            }
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NGordat.Helpers.Hosting.Authentication;
using NGordat.Helpers.Hosting.Extensions;
using NGordat.Identity.Domain.Entities;
using NGordat.IdentityServer.Dal;
using NGordat.IdentityServer.STS.Services;

namespace NGordat.IdentityServer.STS.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAuthentication<TKey>(this IServiceCollection services, IConfiguration configuration)
            where TKey : IEquatable<TKey>
{
            services.AddAuthenticationServices<IdentityServerIdentityDbContext<TKey>, UserIdentity<TKey>, UserIdentityRole<TKey>>(configuration);
            //services.AddIdentityServer<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, UserIdentity<TKey>>(services.AddAuthenticationServices<IdentityServerIdentityDbContext<TKey>, UserIdentity<TKey>, UserIdentityRole<TKey>>(configuration));
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
            //var externalProviderConfiguration = configuration.GetSection(nameof(ExternalProvidersConfiguration)).Get<ExternalProvidersConfiguration>();

            //if (externalProviderConfiguration.UseGitHubProvider)
            //{
            //    authenticationBuilder.AddGitHub(options =>
            //    {
            //        options.ClientId = externalProviderConfiguration.GitHubClientId;
            //        options.ClientSecret = externalProviderConfiguration.GitHubClientSecret;
            //        options.CallbackPath = externalProviderConfiguration.GitHubCallbackPath;
            //        options.Scope.Add("user:email");
            //    });
            //}

            //if (externalProviderConfiguration.UseAzureAdProvider)
            //{
            //    authenticationBuilder.AddMicrosoftIdentityWebApp(options =>
            //    {
            //        options.ClientSecret = externalProviderConfiguration.AzureAdSecret;
            //        options.ClientId = externalProviderConfiguration.AzureAdClientId;
            //        options.TenantId = externalProviderConfiguration.AzureAdTenantId;
            //        options.Instance = externalProviderConfiguration.AzureInstance;
            //        options.Domain = externalProviderConfiguration.AzureDomain;
            //        options.CallbackPath = externalProviderConfiguration.AzureAdCallbackPath;
            //    }, cookieScheme: null);
            //}

            //if (externalProviderConfiguration.UseGoogleProvider)
            //{
            //    authenticationBuilder.AddGoogle(options =>
            //    {
            //        //options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

            //        // register your IdentityServer with Google at https://console.developers.google.com
            //        // enable the Google+ API
            //        // set the redirect URI to https://localhost:5001/signin-google
            //        options.ClientId = externalProviderConfiguration.GoogleClientId;
            //        options.ClientSecret = externalProviderConfiguration.GoogleClientSecret;
            //    });
            //}
            //if (externalProviderConfiguration.UseSteamProvider)
            //{
            //    authenticationBuilder.AddSteam("Steam", options =>
            //    {
            //        //options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

            //        options.ApplicationKey = externalProviderConfiguration.SteamClientId;
            //        // Note that Steam OpenId implementation doesn't support requesting additionnal claims.
            //        // In order to add several claims from the Steam OpenId response, we need to add manually the requested claims
            //        options.Events.OnAuthenticated = context =>
            //        {
            //            // Get user informations
            //            var steamOpenIdResponse = context.UserPayload.Deserialize<SteamOpenIdResponse>();


            //            string picture = steamOpenIdResponse?.Response?.Players?.FirstOrDefault()?.AvatarFull;
            //            string profile = steamOpenIdResponse?.Response?.Players?.FirstOrDefault()?.ProfileUrl;
            //            if (!string.IsNullOrEmpty(picture))
            //            {
            //                context.Identity.AddClaim(new Claim(MidoneUserClaimConstants.Picture, picture));
            //            }
            //            if (!string.IsNullOrEmpty(profile))
            //            {
            //                context.Identity.AddClaim(new Claim(MidoneUserClaimConstants.SteamProfile, profile));
            //            }

            //            return Task.FromResult(0);
            //        };
            //    });
            //}
            //if (externalProviderConfiguration.UseFacebookProvider)
            //{
            //    authenticationBuilder.AddFacebook(options =>
            //    {
            //        //options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //        options.AppId = externalProviderConfiguration.FacebookClientId;
            //        options.AppSecret = externalProviderConfiguration.FacebookClientSecret;
            //        options.Scope.Add("public_profile");
            //    });
            //}
        }
    }
}

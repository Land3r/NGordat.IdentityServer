using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using NGordat.IdentityServer.STS.Constants;

namespace NGordat.IdentityServer.STS.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void UseAuthentication(this WebApplication app)
        {
            app.UseIdentityServer();
        }

        /// <summary>
        /// Using of Forwarded Headers and Referrer Policy
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        public static void UseSecurityHeaders(this WebApplication app, IConfiguration configuration)
        {
            var forwardingOptions = new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.All
            };

            forwardingOptions.KnownNetworks.Clear();
            forwardingOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(forwardingOptions);

            app.UseReferrerPolicy(options => options.NoReferrer());

            // CSP Configuration to be able to use external resources
            var cspTrustedDomains = new List<string>();
            configuration.GetSection(ConfigurationConsts.CspTrustedDomainsKey).Bind(cspTrustedDomains);
            if (cspTrustedDomains.Any())
            {
                app.UseCsp(csp =>
                {
                    var imagesSources = new List<string> { "data:" };
                    imagesSources.AddRange(cspTrustedDomains);

                    csp.ImageSources(options =>
                    {
                        options.SelfSrc = true;
                        options.CustomSources = imagesSources;
                        options.Enabled = true;
                    });
                    csp.FontSources(options =>
                    {
                        options.SelfSrc = true;
                        options.CustomSources = cspTrustedDomains;
                        options.Enabled = true;
                    });
                    csp.ScriptSources(options =>
                    {
                        options.SelfSrc = true;
                        options.CustomSources = cspTrustedDomains;
                        options.Enabled = true;
                        options.UnsafeInlineSrc = true;
                    });
                    csp.StyleSources(options =>
                    {
                        options.SelfSrc = true;
                        options.CustomSources = cspTrustedDomains;
                        options.Enabled = true;
                        options.UnsafeInlineSrc = true;
                    });
                    csp.Sandbox(options =>
                    {
                        options.AllowForms()
                            .AllowSameOrigin()
                            .AllowScripts();
                    });
                    csp.FrameAncestors(option =>
                    {
                        option.NoneSrc = true;
                        option.Enabled = true;
                    });

                    csp.BaseUris(options =>
                    {
                        options.SelfSrc = true;
                        options.Enabled = true;
                    });

                    csp.ObjectSources(options =>
                    {
                        options.NoneSrc = true;
                        options.Enabled = true;
                    });

                    csp.DefaultSources(options =>
                    {
                        options.Enabled = true;
                        options.SelfSrc = true;
                        options.CustomSources = cspTrustedDomains;
                    });
                });
            }

        }

        /// <summary>
        /// Register middleware for localization
        /// </summary>
        /// <param name="app"></param>
        public static void UseLocalizationServices(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NGordat.Helpers.Hosting.Filters;
using NGordat.Identity.Domain.Entities;
using NGordat.IdentityServer.STS.Pages.Shared;
using NGordat.IdentityServer.STS.Services;
using System;
using System.Threading.Tasks;

namespace NGordat.IdentityServer.STS.Pages.Account
{

    [SecurityHeaders]
    public class ExternalLoginModel : ALoggedPageModel<ExternalLoginModel>
    {
        private readonly ApplicationSignInManager<UserIdentity<Guid>> _signInManager;

        public ExternalLoginModel(ILogger<ExternalLoginModel> logger, ApplicationSignInManager<UserIdentity<Guid>> signInManager)
            : base(logger)
        {
            _signInManager = signInManager;
        }

        public IActionResult OnGet(string provider, string? returnUrl)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        public IActionResult OnPost(string provider, string? returnUrl)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }
    }
}

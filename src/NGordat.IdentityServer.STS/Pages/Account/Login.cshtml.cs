using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using NGordat.Helpers.Hosting.Configuration;
using NGordat.Helpers.Hosting.Filters;
using NGordat.IdentityServer.STS.Pages.Shared;
using NGordat.IdentityServer.STS.Services;
using NGordat.IdentityServer.STS.ViewModels.Account;
using NGordat.Razor.Helpers.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace NGordat.IdentityServer.STS.Pages.Account
{
    [SecurityHeaders]
    public class LoginModel : ALoggedPageModel<LoginModel>
    {
        #region LoginInputModel

        public LoginViewModel LoginViewModel { get; set; }

        #endregion LoginInputModel

        private readonly AccountService _accountService;

        public LoginModel(ILogger<LoginModel> logger,
            AccountService accountService
            ) : base(logger)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> OnGetAsync(string? returnUrl)
        {
            LoginViewModel = await _accountService.BuildLoginViewModelAsync(returnUrl);

            // If there is only remote providers and only 1 external identity provider, redirect to this provider login workflow.
            if (LoginViewModel.EnableLocalLogin == false && LoginViewModel.ExternalProviders.Count() == 1)
            {
                // only one option for logging in
                return RedirectToPage("ExternalLogin", new { provider = LoginViewModel.ExternalProviders.First().AuthenticationScheme, returnUrl = returnUrl });
            }

            return Page();
        }
    }
}

using Duende.IdentityServer;
using Duende.IdentityServer.Models;
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
using NGordat.Helpers.Hosting.Extensions;
using NGordat.Identity.Domain.Entities;
using Duende.IdentityServer.Events;
using NGordat.IdentityServer.STS.Configuration;
using NGordat.IdentityServer.STS.Extensions;

namespace NGordat.IdentityServer.STS.Pages.Account
{
    public class LoginModel : ALoggedPageModel<LoginModel>
    {
        #region LoginInputModel

        [BindProperty]
        public LoginViewModel LoginViewModel { get; set; }

        #endregion LoginInputModel

        private readonly AccountService _accountService;
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly UserResolver<UserIdentity> _userResolver;
        private readonly ApplicationSignInManager<UserIdentity> _signInManager;
        private readonly IEventService _eventService;

        public LoginModel(ILogger<LoginModel> logger,
            AccountService accountService,
            IIdentityServerInteractionService interactionService,
            UserResolver<UserIdentity> userResolver,
            ApplicationSignInManager<UserIdentity> signInManager,
            IEventService eventService
            ) : base(logger)
        {
            _accountService = accountService;
            _interactionService = interactionService;
            _userResolver = userResolver;
            _signInManager = signInManager;
            _eventService = eventService;
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

        public async Task<IActionResult> OnPostLoginAsync()
        {
            // check if we are in the context of an authorization request
            var context = await _interactionService.GetAuthorizationContextAsync(LoginViewModel.ReturnUrl);

            if (ModelState.IsValid)
            {
                var user = await _userResolver.GetUserAsync(LoginViewModel.Username);
                if (user != default(UserIdentity))
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, LoginViewModel.Password, LoginViewModel.RememberLogin, lockoutOnFailure: true);
                    if (result.Succeeded)
                    {
                        await _eventService.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName));

                        if (context != null)
                        {
                            if (context.IsNativeClient())
                            {
                                // The client is native, so this change in how to
                                // return the response is for better UX for the end user.
                                return this.LoadingPage(viewName: "Redirect", redirectUri: LoginViewModel.ReturnUrl);
                            }

                            // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                            return Redirect(LoginViewModel.ReturnUrl);
                        }

                        // request for a local page
                        if (Url.IsLocalUrl(LoginViewModel.ReturnUrl))
                        {
                            return Redirect(LoginViewModel.ReturnUrl);
                        }

                        if (string.IsNullOrEmpty(LoginViewModel.ReturnUrl))
                        {
                            return Redirect("~/");
                        }

                        // user might have clicked on a malicious link - should be logged
                        throw new Exception("invalid return URL");
                    }

                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("LoginWith2fa", new { LoginViewModel.ReturnUrl, RememberMe = LoginViewModel.RememberLogin });
                    }

                    if (result.IsLockedOut)
                    {
                        //return View("Lockout");
                        return Partial("Lockout");
                    }
                }
                await _eventService.RaiseAsync(new UserLoginFailureEvent(LoginViewModel.Username, "invalid credentials", clientId: context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }

            // something went wrong, show form with error
            LoginViewModel = await _accountService.BuildLoginViewModelAsync(LoginViewModel.ReturnUrl);
            return Page();
        }

        public async Task<IActionResult> OnPostCancelAsync()
        {
            // check if we are in the context of an authorization request
            var context = await _interactionService.GetAuthorizationContextAsync(LoginViewModel.ReturnUrl);
            if (context != null)
            {
                // if the user cancels, send a result back into IdentityServer as if they 
                // denied the consent (even if this client does not require consent).
                // this will send back an access denied OIDC error response to the client.
                await _interactionService.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                if (context.IsNativeClient())
                {
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage(viewName: "Redirect", redirectUri: LoginViewModel.ReturnUrl);
                }

                return Redirect(LoginViewModel.ReturnUrl);
            }

            // since we don't have a valid context, then we just go back to the home page
            return Redirect("~/");
        }
    }
}

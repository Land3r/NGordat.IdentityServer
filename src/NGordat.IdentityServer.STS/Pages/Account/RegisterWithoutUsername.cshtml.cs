using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using NGordat.IdentityServer.STS.Pages.Shared;
using NGordat.IdentityServer.STS.ViewModels.Account;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using NGordat.Identity.Domain.Entities;
using Microsoft.Extensions.Localization;
using NGordat.Helpers.Hosting.Configuration;
using NGordat.IdentityServer.STS.Services;
using Microsoft.Extensions.Options;

namespace NGordat.IdentityServer.STS.Pages.Account
{
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public class RegisterWithoutUsernameModel : ALoggedPageModel<RegisterWithoutUsernameModel>
    {
        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        private readonly RegisterConfiguration _registerConfiguration;
        private readonly LoginConfiguration _loginConfiguration;
        private readonly IdentityOptions _identityOptions;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IStringLocalizer<RegisterModel> _localizer;
        private readonly ApplicationSignInManager<UserIdentity> _signInManager;

        public RegisterWithoutUsernameModel(ILogger<RegisterWithoutUsernameModel> logger,
            IOptions<RegisterConfiguration> registerConfiguration,
            IOptions<LoginConfiguration> loginConfiguration,
            UserManager<UserIdentity> userManager,
            IEmailSender emailSender,
            IStringLocalizer<RegisterModel> localizer,
            IOptions<IdentityOptions> identityOptions,
            ApplicationSignInManager<UserIdentity> signInManager) : base(logger)
        {
            _registerConfiguration = registerConfiguration.Value;
            _loginConfiguration = loginConfiguration.Value;
            _userManager = userManager;
            _emailSender = emailSender;
            _localizer = localizer;
            _identityOptions = identityOptions.Value;
            _signInManager = signInManager;
        }

        public IActionResult OnGet(string returnUrl = null)
        {
            if (!_registerConfiguration.Enabled) return RedirectToPage("RegisterFailure");
            ViewData["ReturnUrl"] = returnUrl;

            return _loginConfiguration.ResolutionPolicy switch
            {
                LoginResolutionPolicy.Username => RedirectToPage("Register"),
                LoginResolutionPolicy.Email => Page(),
                _ => RedirectToPage("RegisterFailure")
            };
        }

        public async Task<IActionResult> OnPostRegisterAsync(string returnUrl = null)
        {
            if (!_registerConfiguration.Enabled) return RedirectToPage("RegisterFailure");

            returnUrl ??= Url.Content("~/");

            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid) return Page();

            var user = new UserIdentity
            {
                UserName = Email,
                Email = Email
            };

            var result = await _userManager.CreateAsync(user, Password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code }, HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(Email, _localizer["ConfirmEmailTitle"], _localizer["ConfirmEmailBody", HtmlEncoder.Default.Encode(callbackUrl)]);

                if (_identityOptions.SignIn.RequireConfirmedAccount)
                {
                    return RedirectToPage("RegisterConfirmation");
                }
                else
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
            }

            AddErrors(result);

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}

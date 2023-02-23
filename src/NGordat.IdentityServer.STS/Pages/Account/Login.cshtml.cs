using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NGordat.Helpers.Hosting.Configuration;
using NGordat.IdentityServer.STS.Pages.Shared;
using NGordat.IdentityServer.STS.ViewModels.Account;
using NGordat.Razor.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace NGordat.IdentityServer.STS.Pages.Account
{
    public class LoginModel : ALoggedPageModel<LoginModel>
    {
        #region LoginInputModel

        public bool AllowRememberLogin { get; set; } = true;
        public bool EnableLocalLogin { get; set; } = true;
        public LoginResolutionPolicy LoginResolutionPolicy { get; set; } = LoginResolutionPolicy.Username;

        public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();
        public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));

        public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
        public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;

        #endregion LoginInputModel

        [Required]
        [Presentation(DataType = "string", Icon = "fa fa-user", IconPosition = IconPosition.Start, Autofocus = true)]
        public string Username { get; set; }

        [Required]
        [Presentation(DataType = "password", Icon = "fa fa-key", IconPosition = IconPosition.Start)]
        public string Password { get; set; }

        public bool RememberLogin { get; set; }

        public string ReturnUrl { get; set; }

        public LoginModel(ILogger<LoginModel> logger) : base(logger)
        {
        }

        public void OnGet()
        {
        }
    }
}

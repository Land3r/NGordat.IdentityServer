using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
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

        public LoginViewModel LoginViewModel { get; set; }

        #endregion LoginInputModel

        [Required]
        [Presentation(DataType = "string", Icon = "fa fa-user", IconPosition = IconPosition.Start, Autofocus = true)]
        public string Username { get; set; }

        [Required]
        [Presentation(DataType = "password", Icon = "fa fa-key", IconPosition = IconPosition.Start)]
        public string Password { get; set; }

        public bool RememberLogin { get; set; }

        public string ReturnUrl { get; set; }


        private readonly IIdentityServerInteractionService _interaction;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IClientStore _clientStore;

        public LoginModel(ILogger<LoginModel> logger,
            IIdentityServerInteractionService interaction,
            IAuthenticationSchemeProvider schemeProvider,
            IClientStore clientStore) : base(logger)
        {
            _interaction = interaction;
            _schemeProvider = schemeProvider;
            _clientStore = clientStore;
        }

        public void OnGet()
        {
        }
    }
}

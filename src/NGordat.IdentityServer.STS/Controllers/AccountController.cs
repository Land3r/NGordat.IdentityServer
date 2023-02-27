using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using NGordat.IdentityServer.STS.Configuration;
using NGordat.IdentityServer.STS.Pages.Account;
using NGordat.IdentityServer.STS.ViewModels.Account;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NGordat.IdentityServer.STS.Controllers
{
    public class AccountController<TUser, TKey> : BaseController<AccountController<TUser, TKey>>
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IClientStore _clientStore;

        public AccountController(ILogger<AccountController<TUser, TKey>> logger,
            IStringLocalizer<AccountController<TUser, TKey>> localizer,
            IIdentityServerInteractionService interaction,
            IAuthenticationSchemeProvider schemeProvider,
            IClientStore clientStore
            ) : base(logger, localizer)
        {
            _interaction = interaction;
            _schemeProvider = schemeProvider;
            _clientStore = clientStore;
        }


    }
}

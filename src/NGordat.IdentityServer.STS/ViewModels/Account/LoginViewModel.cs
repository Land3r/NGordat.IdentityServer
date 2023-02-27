// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Original file: https://github.com/DuendeSoftware/IdentityServer.Quickstart.UI
// Modified by Jan Škoruba

using NGordat.Helpers.Hosting.Configuration;
using NGordat.Razor.Helpers.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;

namespace NGordat.IdentityServer.STS.ViewModels.Account
{
    public class LoginViewModel
    {
        public bool AllowRememberLogin { get; set; } = true;
        public bool EnableLocalLogin { get; set; } = true;

        public LoginResolutionPolicy LoginResolutionPolicy { get; set; } = LoginResolutionPolicy.Username;
        public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();
        public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));
        public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
        public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;

        public string ReturnUrl { get; set; }

        [Required]
        [Presentation(DataType = "string", Icon = "fa fa-user", IconPosition = IconPosition.Start, Autofocus = true)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Presentation(DataType = "password", Icon = "fa fa-key", IconPosition = IconPosition.Start)]
        public string Password { get; set; }

        [Presentation(DataType = "checkbox")]
        public bool RememberLogin { get; set; }
    }
}








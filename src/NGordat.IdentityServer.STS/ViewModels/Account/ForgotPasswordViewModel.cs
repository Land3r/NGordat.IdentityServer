using NGordat.Helpers.Hosting.Configuration;
using System.ComponentModel.DataAnnotations;

namespace NGordat.IdentityServer.STS.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        public LoginResolutionPolicy? Policy { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Username { get; set; }
    }
}









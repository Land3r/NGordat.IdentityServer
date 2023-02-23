using System.ComponentModel.DataAnnotations;

namespace NGordat.IdentityServer.STS.ViewModels.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_@\-\.\+]+$")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string ProviderPicture { get; set; }

        public string ExternalLoginType { get; set; }

        public string ProviderProfile { get; set; }
    }
}









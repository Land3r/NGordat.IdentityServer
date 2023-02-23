using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NGordat.IdentityServer.STS.Pages.Shared
{
    public abstract class ALoggedPageModel<TPageModel> : PageModel
    {
        public readonly ILogger<TPageModel> _logger;

        public ALoggedPageModel(ILogger<TPageModel> logger)
        {
            _logger = logger;
        }
    }
}

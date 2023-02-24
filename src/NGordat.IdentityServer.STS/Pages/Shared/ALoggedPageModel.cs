using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

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

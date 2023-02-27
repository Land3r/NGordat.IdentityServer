using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NGordat.Helpers.Hosting.Filters;

namespace NGordat.IdentityServer.STS.Pages
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        public IndexModel()
        {
        }

        public void OnGet()
        {

        }
    }
}
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using NGordat.IdentityServer.STS.ViewModels.Account;

namespace NGordat.IdentityServer.STS.Extensions
{
    public static class PageExtensions
    {
        public static IActionResult LoadingPage(this PageModel page, string viewName, string redirectUri)
        {
            page.HttpContext.Response.StatusCode = 200;
            page.HttpContext.Response.Headers["Location"] = "";

            return page.Partial(viewName, new RedirectViewModel { RedirectUrl = redirectUri });
        }
    }
}

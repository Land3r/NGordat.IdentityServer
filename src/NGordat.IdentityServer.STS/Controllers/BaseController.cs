using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace NGordat.IdentityServer.STS.Controllers
{
    public abstract class BaseController<TController>
    {
        public ILogger<TController> _logger;
        public IStringLocalizer<TController> _localizer;

        public BaseController(ILogger<TController> logger, IStringLocalizer<TController> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }
    }
}

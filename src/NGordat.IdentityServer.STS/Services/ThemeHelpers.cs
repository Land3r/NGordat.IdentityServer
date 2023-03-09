using System.Collections.Generic;
using System.Linq;

namespace NGordat.IdentityServer.STS.Services
{
    public static class ThemeHelpers
    {
        public const string CookieThemeKey = "Application.Theme";

        public const string DefaultTheme = "no-theme";
        private static readonly IList<string> AvailableThemes = new List<string>()
        {   "darkly", "cosmo", "cerulean", "cyborg", "flatly", "journal", "litera", "lumen", "lux",
            "materia", "minty", "pulse", "sandstone", "simplex", "sketchy", "slate", "solar", "spacelab", "superhero",
            "united", "yeti"
        };

        public const string NoThemeKey = "no-theme";
        public const bool NoThemeAvailable = true;

        public static ICollection<string> GetThemes()
        {
            if (NoThemeAvailable)
            {
                return AvailableThemes.Prepend(NoThemeKey).ToList();
            }
            return AvailableThemes;
        }
    }
}

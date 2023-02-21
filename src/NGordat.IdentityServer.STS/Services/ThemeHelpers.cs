namespace NGordat.IdentityServer.STS.Services
{
    public static class ThemeHelpers
    {
        public const string CookieThemeKey = "Application.Theme";

        public const string DefaultTheme = "default";

        public static ICollection<string> GetThemes()
        {
            return new List<string>
            {
                "default", "darkly", "cosmo", "cerulean", "cyborg", "flatly", "journal", "litera", "lumen", "lux",
                "materia", "minty", "pulse", "sandstone", "simplex", "sketchy", "slate", "solar", "spacelab", "superhero",
                "united", "yeti"
            };
        }
    }
}

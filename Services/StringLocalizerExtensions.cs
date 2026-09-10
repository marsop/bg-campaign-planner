using Microsoft.Extensions.Localization;

namespace bg_campaign_planner.Services;

public static class StringLocalizerExtensions
{
    /// <summary>
    /// Formats a localized string using string.Format and the current UI culture.
    /// </summary>
    public static string Get(this IStringLocalizer localizer, string key, params object[] args)
    {
        return localizer[key, args].Value;
    }
}

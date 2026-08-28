using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace bg_campaign_planner.Services;

public interface ILocalizationService
{
    string CurrentLanguage { get; }
    CultureInfo CurrentCulture { get; }
    IReadOnlyList<LanguageInfo> SupportedLanguages { get; }
    event Action? OnLanguageChanged;

    string this[string key] { get; }
    string Get(string key, params object[] args);
    Task SetLanguageAsync(string langCode);
    Task InitializeAsync();
}

public record LanguageInfo(string Code, string Name, string Flag, string CultureName);

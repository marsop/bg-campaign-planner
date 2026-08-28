using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using bg_campaign_planner.Services.Translations;

namespace bg_campaign_planner.Services;

public class LocalizationService : ILocalizationService
{
    private readonly IJSRuntime _js;
    private string _currentLanguage = "en";
    private CultureInfo _currentCulture = new("en-US");

    public event Action? OnLanguageChanged;

    public IReadOnlyList<LanguageInfo> SupportedLanguages { get; } = new List<LanguageInfo>
    {
        new("en", "English", "🇬🇧", "en-US"),
        new("es", "Español", "🇪🇸", "es-ES"),
        new("de", "Deutsch", "🇩🇪", "de-DE"),
        new("fr", "Français", "🇫🇷", "fr-FR"),
        new("it", "Italiano", "🇮🇹", "it-IT")
    };

    public LocalizationService(IJSRuntime js)
    {
        _js = js;
        ApplyCulture("en-US");
    }

    public string CurrentLanguage => _currentLanguage;
    public CultureInfo CurrentCulture => _currentCulture;

    public string this[string key]
    {
        get
        {
            if (TranslationDictionary.Translations.TryGetValue(_currentLanguage, out var dict) &&
                dict.TryGetValue(key, out var val))
            {
                return val;
            }

            // Fallback to English
            if (TranslationDictionary.Translations.TryGetValue("en", out var enDict) &&
                enDict.TryGetValue(key, out var enVal))
            {
                return enVal;
            }

            return key;
        }
    }

    public string Get(string key, params object[] args)
    {
        var template = this[key];
        try
        {
            return string.Format(_currentCulture, template, args);
        }
        catch
        {
            return template;
        }
    }

    public async Task InitializeAsync()
    {
        try
        {
            var savedLang = await _js.InvokeAsync<string?>("campaignPlanner.getSavedLanguage");
            if (!string.IsNullOrEmpty(savedLang) && SupportedLanguages.Any(l => l.Code.Equals(savedLang, StringComparison.OrdinalIgnoreCase)))
            {
                await SetLanguageInternalAsync(savedLang.ToLowerInvariant(), savePreference: false);
                return;
            }

            var browserLang = await _js.InvokeAsync<string?>("campaignPlanner.getBrowserLanguage");
            if (!string.IsNullOrEmpty(browserLang))
            {
                var matched = SupportedLanguages.FirstOrDefault(l =>
                    browserLang.StartsWith(l.Code, StringComparison.OrdinalIgnoreCase));
                if (matched != null)
                {
                    await SetLanguageInternalAsync(matched.Code, savePreference: false);
                    return;
                }
            }
        }
        catch
        {
            // Fallback to English if JS is not available at pre-render or during tests
        }

        await SetLanguageInternalAsync("en", savePreference: false);
    }

    public async Task SetLanguageAsync(string langCode)
    {
        await SetLanguageInternalAsync(langCode, savePreference: true);
    }

    private async Task SetLanguageInternalAsync(string langCode, bool savePreference)
    {
        var langInfo = SupportedLanguages.FirstOrDefault(l => l.Code.Equals(langCode, StringComparison.OrdinalIgnoreCase))
                       ?? SupportedLanguages.First();

        _currentLanguage = langInfo.Code;
        ApplyCulture(langInfo.CultureName);

        if (savePreference)
        {
            try
            {
                await _js.InvokeVoidAsync("campaignPlanner.setSavedLanguage", _currentLanguage);
            }
            catch
            {
                // Silently handle if JS unavailable
            }
        }

        OnLanguageChanged?.Invoke();
    }

    private void ApplyCulture(string cultureName)
    {
        try
        {
            _currentCulture = new CultureInfo(cultureName);
            CultureInfo.DefaultThreadCurrentCulture = _currentCulture;
            CultureInfo.DefaultThreadCurrentUICulture = _currentCulture;
            CultureInfo.CurrentCulture = _currentCulture;
            CultureInfo.CurrentUICulture = _currentCulture;
        }
        catch
        {
            _currentCulture = new CultureInfo("en-US");
        }
    }
}

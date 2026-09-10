using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using bg_campaign_planner.Resources;

namespace bg_campaign_planner.Services;

public class LocalizationService : ILocalizationService
{
    private readonly IStringLocalizer<AppResources> _localizer;
    private readonly IJSRuntime _js;
    private readonly NavigationManager _navigation;

    public event Action? OnLanguageChanged;

    public IReadOnlyList<LanguageInfo> SupportedLanguages { get; } = new List<LanguageInfo>
    {
        new("en", "English", "🇬🇧", "en-US"),
        new("es", "Español", "🇪🇸", "es-ES"),
        new("de", "Deutsch", "🇩🇪", "de-DE"),
        new("fr", "Français", "🇫🇷", "fr-FR"),
        new("it", "Italiano", "🇮🇹", "it-IT")
    };

    public LocalizationService(IStringLocalizer<AppResources> localizer, IJSRuntime js, NavigationManager navigation)
    {
        _localizer = localizer;
        _js = js;
        _navigation = navigation;
    }

    public string CurrentLanguage => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
    public CultureInfo CurrentCulture => CultureInfo.CurrentCulture;

    public string this[string key] => _localizer[key].Value;

    public string Get(string key, params object[] args) => _localizer[key, args].Value;

    public Task InitializeAsync()
    {
        // Thread culture is initialized on startup in Program.cs
        return Task.CompletedTask;
    }

    public async Task SetLanguageAsync(string langCode)
    {
        var langInfo = SupportedLanguages.FirstOrDefault(l => l.Code.Equals(langCode, StringComparison.OrdinalIgnoreCase))
                       ?? SupportedLanguages.First();

        try
        {
            await _js.InvokeVoidAsync("campaignPlanner.setSavedLanguage", langInfo.Code);
        }
        catch
        {
            // Silently handle if JS is not available
        }

        var culture = new CultureInfo(langInfo.CultureName);
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        OnLanguageChanged?.Invoke();

        // Reload so that Blazor WebAssembly runtime loads the corresponding satellite assembly
        _navigation.NavigateTo(_navigation.Uri, forceLoad: true);
    }
}

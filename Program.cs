using System.Globalization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using bg_campaign_planner;
using bg_campaign_planner.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddLocalization();
builder.Services.AddSingleton<CampaignDataService>();
builder.Services.AddSingleton<PlannerCalculatorService>();
builder.Services.AddScoped<ILocalizationService, LocalizationService>();
builder.Services.AddScoped<ThemeService>();

var host = builder.Build();

// Establish initial culture before running the Blazor WebAssembly app
var js = host.Services.GetRequiredService<IJSRuntime>();
string? langCode = null;
try
{
    langCode = await js.InvokeAsync<string?>("campaignPlanner.getSavedLanguage");
    if (string.IsNullOrEmpty(langCode))
    {
        var browserLang = await js.InvokeAsync<string?>("campaignPlanner.getBrowserLanguage");
        if (!string.IsNullOrEmpty(browserLang))
        {
            var supported = new[] { "en", "es", "de", "fr", "it" };
            langCode = supported.FirstOrDefault(s => browserLang.StartsWith(s, StringComparison.OrdinalIgnoreCase));
        }
    }
}
catch
{
    // Fallback if JS interop is unavailable
}

var cultureMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
{
    ["en"] = "en-US",
    ["es"] = "es-ES",
    ["de"] = "de-DE",
    ["fr"] = "fr-FR",
    ["it"] = "it-IT"
};

var resolvedCultureName = (langCode != null && cultureMap.TryGetValue(langCode, out var cultureName))
    ? cultureName
    : "en-US";

var culture = new CultureInfo(resolvedCultureName);
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;
CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

await host.RunAsync();

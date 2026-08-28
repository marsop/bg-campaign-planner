using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace bg_campaign_planner.Services;

public enum ThemeMode
{
    Light,
    Dark,
    System
}

public class ThemeService
{
    private readonly IJSRuntime _js;
    private ThemeMode _currentTheme = ThemeMode.System;
    private string _resolvedTheme = "dark";
    private bool _initialized;

    public event Action? OnThemeChanged;

    public ThemeMode CurrentTheme => _currentTheme;
    public string ResolvedTheme => _resolvedTheme;
    public bool IsDark => _resolvedTheme == "dark";

    public ThemeService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task InitializeAsync()
    {
        if (_initialized) return;

        try
        {
            var stored = await _js.InvokeAsync<string?>("campaignPlannerTheme.getStoredTheme");
            if (Enum.TryParse<ThemeMode>(stored, true, out var parsedMode))
            {
                _currentTheme = parsedMode;
            }
            else
            {
                _currentTheme = ThemeMode.System;
            }

            _resolvedTheme = await _js.InvokeAsync<string>("campaignPlannerTheme.applyTheme", _currentTheme.ToString().ToLowerInvariant());
            
            // Register JS listener for system theme changes
            var dotNetRef = DotNetObjectReference.Create(this);
            await _js.InvokeVoidAsync("campaignPlannerTheme.listenToSystemThemeChanges", dotNetRef);
            
            _initialized = true;
        }
        catch
        {
            _resolvedTheme = "dark";
            _initialized = true;
        }
    }

    public async Task SetThemeAsync(ThemeMode mode)
    {
        _currentTheme = mode;
        try
        {
            _resolvedTheme = await _js.InvokeAsync<string>("campaignPlannerTheme.applyTheme", mode.ToString().ToLowerInvariant());
        }
        catch
        {
            _resolvedTheme = mode == ThemeMode.Light ? "light" : "dark";
        }

        OnThemeChanged?.Invoke();
    }

    [JSInvokable]
    public void OnSystemThemeChanged(string newSystemTheme)
    {
        if (_currentTheme == ThemeMode.System)
        {
            _resolvedTheme = newSystemTheme;
            OnThemeChanged?.Invoke();
        }
    }
}

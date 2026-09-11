using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using bg_campaign_planner.Models;

namespace bg_campaign_planner.Services;

public class CampaignDataService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly List<string> _orderedGameIds = new();
    private readonly Dictionary<string, GameJsonDto> _baseGameDtos = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, Dictionary<string, TranslationJsonDto>> _translations = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, Dictionary<string, BoardGame>> _localizedCache = new(StringComparer.OrdinalIgnoreCase);

    public CampaignDataService()
    {
        LoadAllFromResources();
    }

    public IReadOnlyList<BoardGame> GetAllGames(string lang = "en")
    {
        var dict = GetDictionaryForLang(lang);
        return _orderedGameIds
            .Where(dict.ContainsKey)
            .Select(id => dict[id])
            .ToList();
    }

    public BoardGame GetGame(string id, string lang = "en")
    {
        var dict = GetDictionaryForLang(lang);
        if (dict.TryGetValue(id, out var game))
        {
            return game;
        }

        // Fallback to English
        var enDict = GetDictionaryForLang("en");
        if (enDict.TryGetValue(id, out var enGame))
        {
            return enGame;
        }

        return GetAllGames(lang).FirstOrDefault() ?? GetAllGames("en").First();
    }

    private Dictionary<string, BoardGame> GetDictionaryForLang(string lang)
    {
        if (string.IsNullOrWhiteSpace(lang)) lang = "en";
        lang = lang.ToLowerInvariant();

        if (_localizedCache.TryGetValue(lang, out var cached))
        {
            return cached;
        }

        var prefix = lang.Split('-')[0];
        if (_localizedCache.TryGetValue(prefix, out var prefixCached))
        {
            return prefixCached;
        }

        // Build for this language
        var built = BuildCatalogForLanguage(prefix);
        _localizedCache[prefix] = built;
        return built;
    }

    private Dictionary<string, BoardGame> BuildCatalogForLanguage(string lang)
    {
        var result = new Dictionary<string, BoardGame>(StringComparer.OrdinalIgnoreCase);

        foreach (var gameId in _orderedGameIds)
        {
            if (!_baseGameDtos.TryGetValue(gameId, out var baseDto))
                continue;

            // Retrieve translation for requested lang, fallback to English
            TranslationJsonDto? trans = null;
            if (_translations.TryGetValue(lang, out var langMap) && langMap.TryGetValue(gameId, out var foundTrans))
            {
                trans = foundTrans;
            }
            else if (_translations.TryGetValue("en", out var enMap) && enMap.TryGetValue(gameId, out var enTrans))
            {
                trans = enTrans;
            }

            var game = new BoardGame
            {
                Id = baseDto.Id,
                Title = !string.IsNullOrEmpty(trans?.Title) ? trans.Title : baseDto.Title,
                Subtitle = trans?.Subtitle ?? string.Empty,
                ReleaseYear = baseDto.ReleaseYear,
                Designers = baseDto.Designers,
                PlayersMin = baseDto.PlayersMin,
                PlayersMax = baseDto.PlayersMax,
                BggRating = baseDto.BggRating,
                BggWeight = baseDto.BggWeight,
                BggUrl = baseDto.BggUrl,
                BaseScenarioMinutes = baseDto.BaseScenarioMinutes,
                SetupTeardownMinutes = baseDto.SetupTeardownMinutes,
                UnitType = baseDto.UnitType,
                ImageUrl = $"games/{baseDto.Id}/cover.webp",
                BackgroundImageUrl = !string.IsNullOrEmpty(baseDto.BackgroundImage) ? $"games/{baseDto.Id}/{baseDto.BackgroundImage}" : null,
                PublicSourceReferences = new List<string>(baseDto.PublicSourceReferences),
                KeyFeatures = trans?.KeyFeatures != null ? new List<string>(trans.KeyFeatures) : new List<string>(),
                Theme = new GameTheming
                {
                    PrimaryColor = baseDto.Theme.PrimaryColor,
                    SecondaryColor = baseDto.Theme.SecondaryColor,
                    AccentColor = baseDto.Theme.AccentColor,
                    BackgroundGradient = baseDto.Theme.BackgroundGradient,
                    CardBackground = baseDto.Theme.CardBackground,
                    BorderColor = baseDto.Theme.BorderColor,
                    GlowColor = baseDto.Theme.GlowColor,
                    HeaderFont = baseDto.Theme.HeaderFont,
                    BodyFont = baseDto.Theme.BodyFont,
                    LoreIcon = baseDto.Theme.LoreIcon,
                    Emblem = baseDto.Theme.Emblem,
                    ThemeClass = baseDto.Theme.ThemeClass,
                    ActiveThemeClass = baseDto.Theme.ActiveThemeClass,
                    RoadmapClass = baseDto.Theme.RoadmapClass,
                    HeroClass = baseDto.Theme.HeroClass,
                    Tagline = trans?.Tagline ?? baseDto.Theme.Tagline,
                    LoreSummary = trans?.LoreSummary ?? baseDto.Theme.LoreSummary,
                    BadgeCategory = trans?.BadgeCategory ?? baseDto.Theme.BadgeCategory,
                    RoadmapTitle = trans?.RoadmapTitle ?? baseDto.Theme.RoadmapTitle,
                    RoadmapBadge = trans?.RoadmapBadge ?? baseDto.Theme.RoadmapBadge
                }
            };

            foreach (var scope in baseDto.Scopes)
            {
                var scopeOpt = new CampaignScopeOption
                {
                    Id = scope.Id,
                    BaseScenarioCount = scope.BaseScenarioCount,
                    EstimatedFailRatePercent = scope.EstimatedFailRatePercent,
                    IsRecommended = scope.IsRecommended
                };

                if (trans?.Scopes != null && trans.Scopes.TryGetValue(scope.Id, out var scopeTrans))
                {
                    scopeOpt.Name = scopeTrans.Name;
                    scopeOpt.Description = scopeTrans.Description;
                }
                else
                {
                    scopeOpt.Name = scope.Id;
                    scopeOpt.Description = string.Empty;
                }

                game.Scopes.Add(scopeOpt);
            }

            result[gameId] = game;
        }

        return result;
    }

    private void LoadAllFromResources()
    {
        var assembly = typeof(CampaignDataService).Assembly;
        var resourceNames = assembly.GetManifestResourceNames();

        // 1. Discover all game.json resources
        // Pattern: ...games.{gameId}.game.json
        foreach (var resName in resourceNames)
        {
            if (resName.EndsWith(".game.json", StringComparison.OrdinalIgnoreCase))
            {
                using var stream = assembly.GetManifestResourceStream(resName);
                if (stream == null) continue;
                using var reader = new StreamReader(stream);
                var json = reader.ReadToEnd();
                var dto = JsonSerializer.Deserialize<GameJsonDto>(json, JsonOptions);
                if (dto != null && !string.IsNullOrEmpty(dto.Id))
                {
                    _baseGameDtos[dto.Id] = dto;
                }
            }
        }

        // Establish stable catalog order: Gloomhaven, Frosthaven, Pandemic series, Sleeping Gods, Oathsworn, Tainted Grail
        var defaultOrder = new[]
        {
            KnownGameIds.Gloomhaven,
            KnownGameIds.Frosthaven,
            KnownGameIds.PandemicSeason0,
            KnownGameIds.PandemicSeason1,
            KnownGameIds.PandemicSeason2,
            KnownGameIds.SleepingGods,
            KnownGameIds.Oathsworn,
            KnownGameIds.TaintedGrail
        };

        foreach (var id in defaultOrder)
        {
            if (_baseGameDtos.ContainsKey(id) && !_orderedGameIds.Contains(id))
            {
                _orderedGameIds.Add(id);
            }
        }

        foreach (var id in _baseGameDtos.Keys)
        {
            if (!_orderedGameIds.Contains(id))
            {
                _orderedGameIds.Add(id);
            }
        }

        // 2. Discover all translation resources
        // Pattern: ...games.{gameId}.translations.{lang}.json
        foreach (var resName in resourceNames)
        {
            if (resName.Contains(".translations.", StringComparison.OrdinalIgnoreCase) &&
                resName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                // Extract gameId and lang
                // Example resource name: bg-campaign-planner.games.gloomhaven.translations.es.json
                var parts = resName.Split('.');
                // Find index of 'games' and 'translations'
                var gamesIdx = Array.FindIndex(parts, p => string.Equals(p, "games", StringComparison.OrdinalIgnoreCase));
                var transIdx = Array.FindIndex(parts, p => string.Equals(p, "translations", StringComparison.OrdinalIgnoreCase));

                if (gamesIdx >= 0 && transIdx > gamesIdx + 1 && transIdx + 2 < parts.Length)
                {
                    var rawGameId = string.Join(".", parts.Skip(gamesIdx + 1).Take(transIdx - gamesIdx - 1));
                    var gameId = _baseGameDtos.Keys.FirstOrDefault(k => string.Equals(k, rawGameId, StringComparison.OrdinalIgnoreCase) || string.Equals(k.Replace('-', '_'), rawGameId, StringComparison.OrdinalIgnoreCase)) ?? rawGameId.Replace('_', '-');
                    var lang = parts[transIdx + 1].ToLowerInvariant();

                    using var stream = assembly.GetManifestResourceStream(resName);
                    if (stream == null) continue;
                    using var reader = new StreamReader(stream);
                    var json = reader.ReadToEnd();
                    var transDto = JsonSerializer.Deserialize<TranslationJsonDto>(json, JsonOptions);

                    if (transDto != null)
                    {
                        if (!_translations.TryGetValue(lang, out var langDict))
                        {
                            langDict = new Dictionary<string, TranslationJsonDto>(StringComparer.OrdinalIgnoreCase);
                            _translations[lang] = langDict;
                        }

                        langDict[gameId] = transDto;
                    }
                }
            }
        }

        // Pre-build default languages
        foreach (var l in new[] { "en", "es", "de", "fr", "it" })
        {
            _localizedCache[l] = BuildCatalogForLanguage(l);
        }
    }

    private class GameJsonDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public string Designers { get; set; } = string.Empty;
        public int PlayersMin { get; set; } = 1;
        public int PlayersMax { get; set; } = 4;
        public int MinPlayers { set => PlayersMin = value; }
        public int MaxPlayers { set => PlayersMax = value; }
        public double BggRating { get; set; }
        public double BggWeight { get; set; }
        public string BggUrl { get; set; } = string.Empty;
        public int BaseScenarioMinutes { get; set; } = 100;
        public int SetupTeardownMinutes { get; set; } = 25;
        public string UnitType { get; set; } = "scenarios";
        public string? BackgroundImage { get; set; } = "background.webp";
        public GameTheming Theme { get; set; } = new();
        public List<string> PublicSourceReferences { get; set; } = new();
        public List<ScopeDto> Scopes { get; set; } = new();
    }

    private class ScopeDto
    {
        public string Id { get; set; } = string.Empty;
        public int BaseScenarioCount { get; set; }
        public double EstimatedFailRatePercent { get; set; } = 15.0;
        public bool IsRecommended { get; set; }
    }

    private class TranslationJsonDto
    {
        public string? Title { get; set; }
        public string Subtitle { get; set; } = string.Empty;
        public string Tagline { get; set; } = string.Empty;
        public string LoreSummary { get; set; } = string.Empty;
        public string BadgeCategory { get; set; } = string.Empty;
        public string RoadmapTitle { get; set; } = string.Empty;
        public string RoadmapBadge { get; set; } = string.Empty;
        public List<string> KeyFeatures { get; set; } = new();
        public Dictionary<string, ScopeTranslationDto> Scopes { get; set; } = new();
    }

    private class ScopeTranslationDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}


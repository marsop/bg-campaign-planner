using System;
using System.Collections.Generic;

namespace bg_campaign_planner.Models;

public enum GameId
{
    Gloomhaven,
    PandemicSeason0,
    Frosthaven
    SleepingGods
}

public class CampaignScopeOption
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int BaseScenarioCount { get; set; }
    public double EstimatedFailRatePercent { get; set; } = 15.0;
    public bool IsRecommended { get; set; }
}

public class GameMilestone
{
    public int Order { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Phase { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int AtScenarioOrGameIndex { get; set; }
    public string BadgeText { get; set; } = string.Empty;
    public string IconEmoji { get; set; } = string.Empty;
}

public class GameTheming
{
    public string PrimaryColor { get; set; } = "#1b6ec2";
    public string SecondaryColor { get; set; } = "#2d3748";
    public string AccentColor { get; set; } = "#e2b714";
    public string BackgroundGradient { get; set; } = "linear-gradient(135deg, #1a202c, #2d3748)";
    public string CardBackground { get; set; } = "rgba(26, 32, 44, 0.85)";
    public string BorderColor { get; set; } = "rgba(226, 183, 20, 0.3)";
    public string GlowColor { get; set; } = "rgba(226, 183, 20, 0.25)";
    public string HeaderFont { get; set; } = "'Cinzel', serif";
    public string BodyFont { get; set; } = "'Inter', sans-serif";
    public string Tagline { get; set; } = string.Empty;
    public string LoreSummary { get; set; } = string.Empty;
    public string BadgeCategory { get; set; } = string.Empty;
}

public class BoardGame
{
    public GameId Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public string Designers { get; set; } = string.Empty;
    public int PlayersMin { get; set; } = 1;
    public int PlayersMax { get; set; } = 4;
    public double BggRating { get; set; }
    public double BggWeight { get; set; }
    public string BggUrl { get; set; } = string.Empty;
    public int BaseScenarioMinutes { get; set; } = 105;
    public int SetupTeardownMinutes { get; set; } = 25;
    public string TotalBoxContentSummary { get; set; } = string.Empty;
    public List<CampaignScopeOption> Scopes { get; set; } = new();
    public List<GameMilestone> Milestones { get; set; } = new();
    public GameTheming Theme { get; set; } = new();
    public List<string> KeyFeatures { get; set; } = new();
    public List<string> PublicSourceReferences { get; set; } = new();
    public string ImageUrl { get; set; } = string.Empty;
}

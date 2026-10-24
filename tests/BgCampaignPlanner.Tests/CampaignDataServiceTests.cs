using System;
using System.Linq;
using System.Text.Json;
using bg_campaign_planner.Models;
using bg_campaign_planner.Services;
using Xunit;

namespace BgCampaignPlanner.Tests;

public class CampaignDataServiceTests
{
    [Fact]
    public void CampaignDataService_InitializesWithoutException()
    {
        var service = new CampaignDataService();
        Assert.NotNull(service);
    }

    [Theory]
    [InlineData("en")]
    [InlineData("es")]
    [InlineData("de")]
    [InlineData("fr")]
    [InlineData("it")]
    public void GetAllGames_Returns100GamesAcrossAllLanguages(string lang)
    {
        var service = new CampaignDataService();
        var games = service.GetAllGames(lang);

        Assert.Equal(100, games.Count);
        foreach (var game in games)
        {
            Assert.False(string.IsNullOrWhiteSpace(game.Id), "Game Id must not be empty");
            Assert.False(string.IsNullOrWhiteSpace(game.Title), $"Game '{game.Id}' Title must not be empty in {lang}");
            Assert.False(string.IsNullOrWhiteSpace(game.Designers), $"Game '{game.Id}' Designers must not be empty");
            Assert.True(game.PlayersMin > 0, $"Game '{game.Id}' PlayersMin must be > 0");
            Assert.True(game.PlayersMax >= game.PlayersMin, $"Game '{game.Id}' PlayersMax must be >= PlayersMin");
            Assert.NotEmpty(game.Scopes);
            Assert.NotNull(game.Theme);
            Assert.False(string.IsNullOrWhiteSpace(game.Theme.PrimaryColor), $"Game '{game.Id}' Theme.PrimaryColor must not be empty");
            Assert.True(game.AddedAt.HasValue, $"Game '{game.Id}' AddedAt must be set");
            Assert.Contains(game.Quality, new[] { "high", "mid", "low" });
            Assert.StartsWith("http", game.BggUrl);
            Assert.False(string.IsNullOrWhiteSpace(game.Subtitle), $"Game '{game.Id}' Subtitle missing in {lang}");
            Assert.False(string.IsNullOrWhiteSpace(game.Theme.Tagline), $"Game '{game.Id}' Theme.Tagline missing in {lang}");
            Assert.False(string.IsNullOrWhiteSpace(game.Theme.LoreSummary), $"Game '{game.Id}' Theme.LoreSummary missing in {lang}");
            Assert.False(string.IsNullOrWhiteSpace(game.Theme.BadgeCategory), $"Game '{game.Id}' Theme.BadgeCategory missing in {lang}");

            foreach (var scope in game.Scopes)
            {
                Assert.False(string.IsNullOrWhiteSpace(scope.Id), $"Game '{game.Id}' scope Id empty");
                Assert.True(scope.BaseScenarioCount > 0, $"Game '{game.Id}' scope '{scope.Id}' BaseScenarioCount <= 0");
                Assert.False(string.IsNullOrWhiteSpace(scope.Name), $"Game '{game.Id}' scope '{scope.Id}' Name empty in {lang}");
                Assert.NotEqual(scope.Id, scope.Name); // ensure translated name was found
                Assert.False(string.IsNullOrWhiteSpace(scope.Description), $"Game '{game.Id}' scope '{scope.Id}' Description empty in {lang}");
            }

            foreach (var milestone in game.Milestones)
            {
                Assert.True(milestone.Order > 0, $"Game '{game.Id}' milestone order <= 0");
                Assert.False(string.IsNullOrWhiteSpace(milestone.Title), $"Game '{game.Id}' milestone order {milestone.Order} Title empty in {lang}");
                Assert.False(string.IsNullOrWhiteSpace(milestone.Description), $"Game '{game.Id}' milestone order {milestone.Order} Description empty in {lang}");
                Assert.False(string.IsNullOrWhiteSpace(milestone.IconEmoji), $"Game '{game.Id}' milestone order {milestone.Order} IconEmoji empty");
            }
        }
    }

    [Fact]
    public void Freelancers_LoadsProperlyWithCorrectDesignersAndMilestones()
    {
        var service = new CampaignDataService();
        var game = service.GetGame("freelancers", "en");

        Assert.NotNull(game);
        Assert.Equal("freelancers", game.Id);
        Assert.Equal("Donald Shults", game.Designers);
        Assert.Equal(1, game.PlayersMin);
        Assert.Equal(7, game.PlayersMax);
        Assert.Equal(3, game.Scopes.Count);
        Assert.Equal(5, game.Milestones.Count);
        Assert.Equal("🪙", game.Milestones[0].IconEmoji);
        Assert.Equal("Gig I", game.Milestones[0].BadgeText);
    }

    [Fact]
    public void LegacyOfDragonholt_LoadsProperlyWithCorrectDesignersAndMilestones()
    {
        var service = new CampaignDataService();
        var game = service.GetGame("legacy-of-dragonholt", "en");

        Assert.NotNull(game);
        Assert.Equal("legacy-of-dragonholt", game.Id);
        Assert.Contains("Nikki Valens", game.Designers);
        Assert.Equal(1, game.PlayersMin);
        Assert.Equal(6, game.PlayersMax);
        Assert.Equal(3, game.Scopes.Count);
        Assert.Equal(5, game.Milestones.Count);
        Assert.Equal("🛤️", game.Milestones[0].IconEmoji);
        Assert.Equal("Prologue", game.Milestones[0].BadgeText);
    }

    [Theory]
    [InlineData("""{"id":"test","designers":["Alice","Bob"]}""", "Alice, Bob")]
    [InlineData("""{"id":"test","designers":"Alice, Bob"}""", "Alice, Bob")]
    [InlineData("""{"id":"test","designers":["Solo"]}""", "Solo")]
    [InlineData("""{"id":"test","designers":[]}""", "")]
    [InlineData("""{"id":"test","designers":["First","","  ","Second"]}""", "First, Second")]
    [InlineData("""{"id":"test","designers":null}""", "")]
    [InlineData("""{"id":"test","designers":[123]}""", "123")]
    [InlineData("""{"id":"test","designers":[1.5]}""", "1.5")]
    [InlineData("""{"id":"test","designers":[{"name":"Donald"}]}""", "Donald")]
    public void SingleOrArrayStringConverter_EdgeCases(string json, string expected)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var result = JsonSerializer.Deserialize<TestGameDto>(json, options);
        Assert.NotNull(result);
        Assert.Equal(expected, result.Designers);
    }

    [Fact]
    public void FlexibleMilestoneListConverter_HandlesArrayAndDictionary()
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        // 1. Array format
        var arrayJson = """
        {
            "milestones": [
                { "order": 1, "title": "First", "phase": "Act 1" },
                { "order": 2, "title": "Second", "phase": "Act 2" }
            ]
        }
        """;
        var arrayResult = JsonSerializer.Deserialize<CampaignDataService.TranslationJsonDto>(arrayJson, options);
        Assert.NotNull(arrayResult);
        Assert.Equal(2, arrayResult.Milestones.Count);
        Assert.Equal(1, arrayResult.Milestones[0].Order);
        Assert.Equal("First", arrayResult.Milestones[0].Title);

        // 2. Dictionary format
        var dictJson = """
        {
            "milestones": {
                "m1": { "title": "First", "phase": "Act 1" },
                "m2": { "title": "Second", "phase": "Act 2" }
            }
        }
        """;
        var dictResult = JsonSerializer.Deserialize<CampaignDataService.TranslationJsonDto>(dictJson, options);
        Assert.NotNull(dictResult);
        Assert.Equal(2, dictResult.Milestones.Count);
        Assert.Equal(1, dictResult.Milestones[0].Order);
        Assert.Equal(2, dictResult.Milestones[1].Order);
        Assert.Equal("Second", dictResult.Milestones[1].Title);

        // 3. Null or empty
        var emptyResult = JsonSerializer.Deserialize<CampaignDataService.TranslationJsonDto>("""{"milestones": []}""", options);
        Assert.NotNull(emptyResult);
        Assert.Empty(emptyResult.Milestones);

        var nullResult = JsonSerializer.Deserialize<CampaignDataService.TranslationJsonDto>("""{"milestones": null}""", options);
        Assert.NotNull(nullResult);
        Assert.Empty(nullResult.Milestones);
    }

    private class TestGameDto
    {
        public string Id { get; set; } = string.Empty;
        [System.Text.Json.Serialization.JsonConverter(typeof(CampaignDataService.SingleOrArrayStringConverter))]
        public string Designers { get; set; } = string.Empty;
    }
}


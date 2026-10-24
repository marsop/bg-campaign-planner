using System.Collections.Generic;
using bg_campaign_planner.Models;
using bg_campaign_planner.Services;
using Xunit;

namespace BgCampaignPlanner.Tests;

public class BggServiceTests
{
    [Fact]
    public void ParseCollectionCsv_EmptyOrWhitespace_ReturnsEmptyList()
    {
        Assert.Empty(BggService.ParseCollectionCsv(""));
        Assert.Empty(BggService.ParseCollectionCsv("   "));
        Assert.Empty(BggService.ParseCollectionCsv(null!));
    }

    [Fact]
    public void ParseCollectionCsv_HeaderOnly_ReturnsEmptyList()
    {
        var csv = "objectid,objectname,own,rating,numplays\n";
        Assert.Empty(BggService.ParseCollectionCsv(csv));
    }

    [Fact]
    public void ParseCollectionCsv_StandardBggCsv_ParsesOwnedGamesCorrectly()
    {
        var csv = """
            objectid,objectname,own,rating,numplays
            174430,"Gloomhaven",1,9.5,12
            312484,"Frosthaven",1,,0
            240196,"Betrayal Legacy",0,8.0,5
            """;

        var items = BggService.ParseCollectionCsv(csv);

        Assert.Equal(2, items.Count);

        var gloom = items[0];
        Assert.Equal(174430, gloom.BggId);
        Assert.Equal("Gloomhaven", gloom.Title);
        Assert.True(gloom.IsOwned);
        Assert.Equal(9.5, gloom.UserRating);
        Assert.Equal(12, gloom.NumPlays);

        var frost = items[1];
        Assert.Equal(312484, frost.BggId);
        Assert.Equal("Frosthaven", frost.Title);
        Assert.True(frost.IsOwned);
        Assert.Null(frost.UserRating);
        Assert.Equal(0, frost.NumPlays);
    }

    [Fact]
    public void ParseCollectionCsv_HandlesCommasAndEscapedQuotesInTitle()
    {
        var csv = """
            objectid,objectname,own,rating,numplays
            297920,"Aliens: Another Glorious Day in the Corps, Co-Op",1,8.0,3
            364867,"Freelancers: A ""Crossroads"" Game",1,7.5,1
            """;

        var items = BggService.ParseCollectionCsv(csv);

        Assert.Equal(2, items.Count);
        Assert.Equal("Aliens: Another Glorious Day in the Corps, Co-Op", items[0].Title);
        Assert.Equal("Freelancers: A \"Crossroads\" Game", items[1].Title);
    }

    [Fact]
    public void ParseCollectionCsv_HandlesAlternativeHeaderNames()
    {
        var csv = """
            id,title,owned,userrating,plays
            174430,Gloomhaven,true,10,25
            """;

        var items = BggService.ParseCollectionCsv(csv);

        Assert.Single(items);
        Assert.Equal(174430, items[0].BggId);
        Assert.Equal("Gloomhaven", items[0].Title);
        Assert.True(items[0].IsOwned);
        Assert.Equal(10.0, items[0].UserRating);
        Assert.Equal(25, items[0].NumPlays);
    }

    [Fact]
    public void BggUnauthorizedException_SetsIsTokenMissingCorrectly()
    {
        var exMissing = new BggUnauthorizedException("Token missing", isTokenMissing: true);
        Assert.True(exMissing.IsTokenMissing);
        Assert.Equal("Token missing", exMissing.Message);

        var exInvalid = new BggUnauthorizedException("Token invalid", isTokenMissing: false);
        Assert.False(exInvalid.IsTokenMissing);
        Assert.Equal("Token invalid", exInvalid.Message);
    }
}

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace bg_campaign_planner.Models;

/// <summary>
/// Represents a single item from a BGG user collection (owned game with stats).
/// </summary>
public class BggCollectionItem
{
    /// <summary>BGG numeric object ID (e.g. 174430 for Gloomhaven).</summary>
    public int BggId { get; set; }

    /// <summary>Primary game title as returned by BGG.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>True when the user marks the item as owned in their collection.</summary>
    public bool IsOwned { get; set; }

    /// <summary>Total number of logged plays on BGG.</summary>
    public int NumPlays { get; set; }

    /// <summary>User's personal rating (1–10), null if not rated.</summary>
    public double? UserRating { get; set; }
}

/// <summary>
/// State of a BGG collection sync operation.
/// </summary>
public enum BggSyncState
{
    Idle,
    Loading,
    Success,
    Error
}

/// <summary>
/// Holds the result of a BGG collection fetch, including the loaded items and any error message.
/// </summary>
public class BggCollectionResult
{
    public List<BggCollectionItem> Items { get; set; } = new();
    public BggSyncState State { get; set; } = BggSyncState.Idle;
    public string? ErrorMessage { get; set; }
    public string? Username { get; set; }
    public DateTime? FetchedAt { get; set; }
}

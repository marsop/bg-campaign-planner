using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using bg_campaign_planner.Models;
using Microsoft.Extensions.Logging;

namespace bg_campaign_planner.Services;

/// <summary>
/// Fetches and parses a user's owned BoardGameGeek collection via the BGG XML API2.
/// Handles BGG's HTTP 202 "queued" responses with automatic retry logic,
/// and falls back to a public CORS proxy when a direct browser request is blocked.
/// </summary>
public class BggService
{
    private const string BggApiBase = "https://boardgamegeek.com/xmlapi2";
    private const string CorsProxy1 = "https://api.allorigins.win/raw?url=";
    private const string CorsProxy2 = "https://corsproxy.io/?url=";

    private const int MaxRetryAttempts = 4;
    private const int RetryDelayMs = 2500;

    private readonly HttpClient _http;
    private readonly ILogger<BggService> _logger;

    public BggService(HttpClient http, ILogger<BggService> logger)
    {
        _http = http;
        _logger = logger;
    }

    /// <summary>
    /// Fetches the owned collection for the given BGG username.
    /// Returns a list of <see cref="BggCollectionItem"/> (owned=true games with play counts).
    /// Throws <see cref="BggServiceException"/> on unrecoverable errors.
    /// </summary>
    public async Task<List<BggCollectionItem>> FetchUserCollectionAsync(
        string username,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new BggServiceException("BGG username cannot be empty.");

        var encodedUser = Uri.EscapeDataString(username.Trim());
        var path = $"/collection?username={encodedUser}&own=1&stats=1";

        // 1. Try direct request (works when BGG allows CORS for that browser).
        try
        {
            var result = await FetchWithRetryAsync(BggApiBase + path, ct);
            return result;
        }
        catch (BggServiceException)
        {
            throw; // Re-throw domain errors (invalid user, rate limit, etc.)
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Direct BGG request failed; falling back to CORS proxy.");
        }

        // 2. Fallback to first CORS proxy.
        try
        {
            var proxyUrl = CorsProxy1 + Uri.EscapeDataString(BggApiBase + path);
            var result = await FetchWithRetryAsync(proxyUrl, ct);
            return result;
        }
        catch (BggServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "First CORS proxy failed; trying second proxy.");
        }

        // 3. Fallback to second CORS proxy.
        try
        {
            var proxyUrl = CorsProxy2 + Uri.EscapeDataString(BggApiBase + path);
            var result = await FetchWithRetryAsync(proxyUrl, ct);
            return result;
        }
        catch (BggServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "All BGG fetch attempts failed.");
            throw new BggServiceException("Unable to reach BoardGameGeek. Please check your connection and try again.", ex);
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Private helpers
    // ─────────────────────────────────────────────────────────────────────────

    private async Task<List<BggCollectionItem>> FetchWithRetryAsync(string url, CancellationToken ct)
    {
        for (int attempt = 1; attempt <= MaxRetryAttempts; attempt++)
        {
            ct.ThrowIfCancellationRequested();

            HttpResponseMessage response;
            try
            {
                response = await _http.GetAsync(url, ct);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                if (attempt >= MaxRetryAttempts) throw;
                await Task.Delay(RetryDelayMs, ct);
                continue;
            }

            // BGG returns 429 when rate-limited.
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
                throw new BggServiceException("BGG rate limit reached. Please wait a moment and try again.");

            // BGG returns 404 for unknown usernames (sometimes 200 with an error element).
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new BggServiceException("BGG user not found. Please check the username and try again.");

            // BGG returns 202 when the collection is being queued server-side; wait and retry.
            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                _logger.LogInformation("BGG returned 202 on attempt {Attempt}; retrying after delay.", attempt);
                if (attempt < MaxRetryAttempts)
                    await Task.Delay(RetryDelayMs, ct);
                continue;
            }

            if (!response.IsSuccessStatusCode)
                throw new BggServiceException($"BGG returned HTTP {(int)response.StatusCode}. Please try again later.");

            var xml = await response.Content.ReadAsStringAsync(ct);

            // Some proxies wrap 202 in a 200 envelope with a "please try again" XML message.
            if (xml.Contains("Your request for this collection has been accepted", StringComparison.OrdinalIgnoreCase) ||
                xml.Contains("Please try again", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("BGG queued response on attempt {Attempt}; retrying after delay.", attempt);
                if (attempt < MaxRetryAttempts)
                    await Task.Delay(RetryDelayMs, ct);
                continue;
            }

            return ParseCollectionXml(xml);
        }

        throw new BggServiceException("BGG is still processing your collection. Please try again in a few seconds.");
    }

    private static List<BggCollectionItem> ParseCollectionXml(string xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
            return new List<BggCollectionItem>();

        XDocument doc;
        try
        {
            doc = XDocument.Parse(xml);
        }
        catch (Exception ex)
        {
            throw new BggServiceException("Received an unexpected response from BGG. Please try again.", ex);
        }

        // Check for <error> root element (e.g. invalid username returns <errors><error>…</error></errors>)
        var errorEl = doc.Root?.Element("error");
        if (errorEl != null)
        {
            var msg = errorEl.Value?.Trim();
            if (!string.IsNullOrEmpty(msg) && msg.Contains("Invalid username", StringComparison.OrdinalIgnoreCase))
                throw new BggServiceException("BGG user not found. Please check the username and try again.");
            throw new BggServiceException(string.IsNullOrWhiteSpace(msg) ? "BGG returned an error." : msg);
        }

        var items = new List<BggCollectionItem>();

        foreach (var itemEl in doc.Descendants("item"))
        {
            // Only process boardgames (not expansions) — subtype="boardgame"
            var subtype = itemEl.Attribute("subtype")?.Value;
            if (!string.Equals(subtype, "boardgame", StringComparison.OrdinalIgnoreCase))
                continue;

            if (!int.TryParse(itemEl.Attribute("objectid")?.Value, out var bggId))
                continue;

            var statusEl = itemEl.Element("status");
            var isOwned = statusEl?.Attribute("own")?.Value == "1";
            if (!isOwned) continue;

            var title = itemEl.Element("name")?.Value?.Trim() ?? string.Empty;

            int.TryParse(itemEl.Element("numplays")?.Value, out var numPlays);

            double? userRating = null;
            var ratingEl = itemEl
                .Element("stats")?
                .Element("rating");
            if (ratingEl != null)
            {
                var ratingVal = ratingEl.Attribute("value")?.Value;
                if (double.TryParse(ratingVal,
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out var rating))
                {
                    userRating = rating;
                }
            }

            items.Add(new BggCollectionItem
            {
                BggId = bggId,
                Title = title,
                IsOwned = true,
                NumPlays = numPlays,
                UserRating = userRating
            });
        }

        return items;
    }
}

/// <summary>
/// Domain exception raised when the BGG service encounters a known, user-facing error condition.
/// </summary>
public class BggServiceException : Exception
{
    public BggServiceException(string message) : base(message) { }
    public BggServiceException(string message, Exception inner) : base(message, inner) { }
}

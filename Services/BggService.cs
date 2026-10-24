using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using bg_campaign_planner.Models;
using Microsoft.Extensions.Logging;

namespace bg_campaign_planner.Services;

/// <summary>
/// Fetches and parses a user's owned BoardGameGeek collection via the BGG XML API2,
/// or parses a user-exported BGG collection CSV file.
/// Handles BGG's HTTP 202 "queued" responses with automatic retry logic,
/// supports Bearer application token authentication required by BGG,
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
    /// Supports an optional Bearer application token required by BoardGameGeek API.
    /// Returns a list of <see cref="BggCollectionItem"/> (owned=true games with play counts).
    /// Throws <see cref="BggServiceException"/> or <see cref="BggUnauthorizedException"/> on errors.
    /// </summary>
    public async Task<List<BggCollectionItem>> FetchUserCollectionAsync(
        string username,
        string? apiToken = null,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new BggServiceException("BGG username cannot be empty.");

        var encodedUser = Uri.EscapeDataString(username.Trim());
        var path = $"/collection?username={encodedUser}&own=1&stats=1";

        // 1. Try direct request (works when BGG allows CORS for that browser).
        try
        {
            var result = await FetchWithRetryAsync(BggApiBase + path, apiToken, ct);
            return result;
        }
        catch (BggUnauthorizedException)
        {
            // Do not retry via CORS proxies on 401: proxies cannot bypass BGG's authentication requirement.
            throw;
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
            var result = await FetchWithRetryAsync(proxyUrl, apiToken, ct);
            return result;
        }
        catch (BggUnauthorizedException)
        {
            throw;
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
            var result = await FetchWithRetryAsync(proxyUrl, apiToken, ct);
            return result;
        }
        catch (BggUnauthorizedException)
        {
            throw;
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
    // CSV Collection Parser
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Parses a BoardGameGeek collection CSV export into a list of owned <see cref="BggCollectionItem"/>.
    /// Handles quoted commas, escaped quotes, and CRLF / LF line endings.
    /// </summary>
    public static List<BggCollectionItem> ParseCollectionCsv(string csv)
    {
        if (string.IsNullOrWhiteSpace(csv))
            return new List<BggCollectionItem>();

        var rows = SplitCsvRows(csv);
        if (rows.Count < 2)
            return new List<BggCollectionItem>();

        var header = rows[0];
        int idIdx = -1, titleIdx = -1, ownIdx = -1, playsIdx = -1, ratingIdx = -1;

        for (int i = 0; i < header.Count; i++)
        {
            var col = header[i].Trim().ToLowerInvariant();
            if (col is "objectid" or "id" or "bggid" or "gameid") idIdx = i;
            else if (col is "objectname" or "name" or "title" or "gamename") titleIdx = i;
            else if (col is "own" or "owned") ownIdx = i;
            else if (col is "numplays" or "plays") playsIdx = i;
            else if (col is "rating" or "userrating" or "myrating") ratingIdx = i;
        }

        if (idIdx == -1 && titleIdx == -1)
            return new List<BggCollectionItem>();

        var items = new List<BggCollectionItem>();

        for (int r = 1; r < rows.Count; r++)
        {
            var row = rows[r];
            if (row.Count == 0 || (row.Count == 1 && string.IsNullOrWhiteSpace(row[0])))
                continue;

            int bggId = 0;
            if (idIdx >= 0 && idIdx < row.Count)
                int.TryParse(row[idIdx].Trim(), out bggId);

            string title = string.Empty;
            if (titleIdx >= 0 && titleIdx < row.Count)
                title = row[titleIdx].Trim();

            if (bggId <= 0 && string.IsNullOrWhiteSpace(title))
                continue;

            bool isOwned = true;
            if (ownIdx >= 0 && ownIdx < row.Count)
            {
                var ownVal = row[ownIdx].Trim();
                isOwned = ownVal == "1" ||
                          ownVal.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                          ownVal.Equals("yes", StringComparison.OrdinalIgnoreCase);
            }

            if (!isOwned) continue;

            int numPlays = 0;
            if (playsIdx >= 0 && playsIdx < row.Count)
                int.TryParse(row[playsIdx].Trim(), out numPlays);

            double? userRating = null;
            if (ratingIdx >= 0 && ratingIdx < row.Count)
            {
                var rVal = row[ratingIdx].Trim();
                if (double.TryParse(rVal, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var parsedRating))
                    userRating = parsedRating;
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

    private static List<List<string>> SplitCsvRows(string csv)
    {
        var result = new List<List<string>>();
        var currentRow = new List<string>();
        var currentField = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < csv.Length; i++)
        {
            char c = csv[i];

            if (inQuotes)
            {
                if (c == '"')
                {
                    if (i + 1 < csv.Length && csv[i + 1] == '"')
                    {
                        currentField.Append('"');
                        i++; // Skip escaped quote
                    }
                    else
                    {
                        inQuotes = false;
                    }
                }
                else
                {
                    currentField.Append(c);
                }
            }
            else
            {
                if (c == '"')
                {
                    inQuotes = true;
                }
                else if (c == ',')
                {
                    currentRow.Add(currentField.ToString());
                    currentField.Clear();
                }
                else if (c == '\r')
                {
                    if (i + 1 < csv.Length && csv[i + 1] == '\n')
                        i++;
                    currentRow.Add(currentField.ToString());
                    currentField.Clear();
                    result.Add(currentRow);
                    currentRow = new List<string>();
                }
                else if (c == '\n')
                {
                    currentRow.Add(currentField.ToString());
                    currentField.Clear();
                    result.Add(currentRow);
                    currentRow = new List<string>();
                }
                else
                {
                    currentField.Append(c);
                }
            }
        }

        if (currentField.Length > 0 || currentRow.Count > 0)
        {
            currentRow.Add(currentField.ToString());
            result.Add(currentRow);
        }

        return result;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Private helpers
    // ─────────────────────────────────────────────────────────────────────────

    private async Task<List<BggCollectionItem>> FetchWithRetryAsync(string url, string? apiToken, CancellationToken ct)
    {
        for (int attempt = 1; attempt <= MaxRetryAttempts; attempt++)
        {
            ct.ThrowIfCancellationRequested();

            HttpResponseMessage response;
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                if (!string.IsNullOrWhiteSpace(apiToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiToken.Trim());
                }
                response = await _http.SendAsync(request, ct);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                if (attempt >= MaxRetryAttempts) throw;
                await Task.Delay(RetryDelayMs, ct);
                continue;
            }

            // BGG returns 401 when application token is required, missing, or invalid.
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (string.IsNullOrWhiteSpace(apiToken))
                {
                    throw new BggUnauthorizedException(
                        "BoardGameGeek now requires an API Application Token (HTTP 401 Unauthorized). Please provide a BGG API token from boardgamegeek.com/applications or import your collection CSV.",
                        isTokenMissing: true);
                }
                else
                {
                    throw new BggUnauthorizedException(
                        "BGG rejected the API Application Token (HTTP 401 Unauthorized). Please check that your token is valid and active at boardgamegeek.com/applications.",
                        isTokenMissing: false);
                }
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

/// <summary>
/// Domain exception raised when BGG returns HTTP 401 Unauthorized (missing or invalid Bearer application token).
/// </summary>
public class BggUnauthorizedException : BggServiceException
{
    public bool IsTokenMissing { get; }

    public BggUnauthorizedException(string message, bool isTokenMissing) : base(message)
    {
        IsTokenMissing = isTokenMissing;
    }
}

# game.json — Annotated Field Reference

This file lives at `games/{game-id}/game.json`.

---

## Full Schema with Annotations

```json
{
  // Required. Must exactly match the parent folder name.
  // Lowercase, alphanumeric, hyphens only.
  // Examples: "gloomhaven", "pandemic-season-1", "kingdom-death-monster"
  "id": "your-game-id",

  // Required. The official English title of the game exactly as printed on the box.
  "title": "Your Game Title",

  // Required. 4-digit year the game was first published (not Kickstarter year).
  "releaseYear": 2023,

  // Required. Comma-separated designer names, exactly as listed on BGG.
  "designers": "First Designer, Second Designer",

  // Required. Minimum player count supported by the campaign mode.
  "playersMin": 1,

  // Required. Maximum player count supported by the campaign mode.
  "playersMax": 4,

  // Required. BGG weighted average rating, rounded to 1 decimal. Verify on BGG.
  "bggRating": 8.5,

  // Required. BGG "weight" (complexity) average, rounded to 2 decimals.
  "bggWeight": 3.75,

  // Required. Full URL to the BGG game page.
  "bggUrl": "https://boardgamegeek.com/boardgame/XXXXXX/your-game-id",

  // Required. Median session play time in MINUTES (not counting setup/teardown).
  // Use community survey data or BGG play time medians. This is per scenario/session.
  "baseScenarioMinutes": 90,

  // Required. Average setup + teardown time in MINUTES for one session.
  "setupTeardownMinutes": 20,

  // Required. How the game labels its individual units of play.
  // Drives the UI label ("X scenarios remaining", "X months remaining", etc.)
  // Allowed values (extend if needed, keep snake_case):
  //   "scenarios"     — Gloomhaven, Frosthaven, Oathsworn
  //   "games_months"  — Pandemic Legacy (monthly structure)
  //   "chapters"      — narrative chapter-based games
  //   "expeditions"   — exploration-driven games
  //   "quests"        — quest-based open-world games
  //   "missions"      — mission-based games
  //   "lantern_years" — Kingdom Death: Monster
  "unitType": "scenarios",

  // Optional. Set to null to disable the background image entirely.
  // Omit this field to use the default "background.webp".
  // "backgroundImage": null,

  // Required. Theme visual identity for this game.
  "theme": {
    // Primary brand color (e.g., dominant color from the box art).
    "primaryColor": "#e5a93b",

    // Secondary color (usually a very dark shade for backgrounds).
    "secondaryColor": "#1e1b18",

    // Accent color (used for highlights, CTAs, milestones).
    "accentColor": "#d9534f",

    // CSS radial-gradient for the main app background.
    // Use dark, atmospheric tones consistent with the game's mood.
    "backgroundGradient": "radial-gradient(ellipse at top, #2b1f1d 0%, #151111 60%, #0c0a0a 100%)",

    // Translucent dark background for UI cards.
    "cardBackground": "rgba(28, 22, 20, 0.90)",

    // Translucent border color (usually primaryColor at low opacity).
    "borderColor": "rgba(229, 169, 59, 0.35)",

    // Translucent glow color for shadows and halos.
    "glowColor": "rgba(229, 169, 59, 0.25)",

    // Google Font for headings. Must already be loaded in wwwroot/index.html,
    // or add a new @import. Format: "'Font Name', fallback-stack".
    // Common choices: Cinzel, Cinzel Decorative, Cormorant SC, Special Elite,
    //                 MedievalSharp, Crimson Text.
    "headerFont": "'Cinzel', 'Cinzel Decorative', Georgia, serif",

    // Body font. Always use "'Inter', system-ui, sans-serif" unless special.
    "bodyFont": "'Inter', system-ui, sans-serif",

    // Single emoji used as a decorative icon in lore/story sections.
    "loreIcon": "📜",

    // 2 emojis representing the game's theme, shown in the game selector card.
    "emblem": "⚔️🛡️",

    // Unique CSS class names for this game (no spaces, no special chars).
    // Must NOT match any existing game's class names.
    // Convention: "theme-{gameid}" (no hyphens in the game id part is fine).
    "themeClass": "theme-yourgame",
    "activeThemeClass": "theme-yourgame-active",
    "roadmapClass": "roadmap-yourgame",
    "heroClass": "hero-yourgame"
  },

  // Required. 2–4 strings citing where your data comes from.
  // Must be public domain sources (BGG, official rulebooks, community surveys).
  "publicSourceReferences": [
    "BoardGameGeek (BGG #XXX Overall)",
    "Publisher Official Rulebook",
    "Community Campaign Survey Statistics"
  ],

  // Required. Array of 2–4 scope objects representing different campaign lengths.
  // Exactly ONE must have "isRecommended": true.
  "scopes": [
    {
      // Unique within this game. Referenced identically in all translation files.
      // Use snake_case. Prefix with an abbreviated game id to avoid collisions.
      "id": "yourgame_standard",

      // Expected number of sessions for this scope BEFORE applying fail rate.
      // Source from rulebook scenario count, BGG logs, or community data.
      "baseScenarioCount": 30,

      // Percentage of sessions likely to be replayed due to failure/loss.
      // 0.0 = no retries expected, 50.0 = half of sessions played twice on avg.
      "estimatedFailRatePercent": 15.0,

      "isRecommended": true
    },
    {
      "id": "yourgame_quick",
      "baseScenarioCount": 20,
      "estimatedFailRatePercent": 8.0,
      "isRecommended": false
    },
    {
      "id": "yourgame_completionist",
      "baseScenarioCount": 45,
      "estimatedFailRatePercent": 20.0,
      "isRecommended": false
    }
  ],

  // Optional. Narrative campaign milestones representing key acts, chapters, or major turning points.
  // If no clear campaign milestones exist for the game, omit this property entirely.
  "milestones": [
    {
      // Sequential index (1-based)
      "order": 1,
      // The scenario / game index where this milestone occurs
      "atScenarioOrGameIndex": 10,
      // Icon emoji displayed on the milestone roadmap card
      "iconEmoji": "🏰",
      // Short badge text (e.g. "Act I", "Ch. 3", "Month 4")
      "badgeText": "Act I",
      // Descriptive milestone title (masked when Show Spoilers is OFF)
      "title": "Fall of the Citadel",
      // Narrative phase header (masked when Show Spoilers is OFF)
      "phase": "Campaign Phase I - The Outskirts",
      // Flavour summary of the milestone (masked when Show Spoilers is OFF)
      "description": "Securing the perimeter and penetrating the inner gate."
    }
  ]
}
```

---

## Existing Theme Classes (Do Not Reuse)

| Game | themeClass | activeThemeClass |
|---|---|---|
| Gloomhaven | theme-gloomhaven | theme-gloomhaven-active |
| Frosthaven | theme-frosthaven | theme-frosthaven-active |
| Oathsworn | theme-oathsworn | theme-oathsworn-active |
| Pandemic Legacy: Season 0 | theme-pandemic | theme-pandemic-active |
| Sleeping Gods | theme-sleepinggods | theme-sleepinggods-active |
| Tainted Grail | theme-taintedgrail | theme-taintedgrail-active |

---

## Existing unitType Values

| Value | Used by |
|---|---|
| `scenarios` | Gloomhaven, Frosthaven, Oathsworn, Sleeping Gods, Tainted Grail |
| `games_months` | Pandemic Legacy: Season 0 |

Add new values freely — just keep them snake_case and ensure the UI strings in
the resource files (`AppResources.resx` etc.) handle them if custom labels
are needed.

---
name: add-game
description: >-
  Use this skill when the user asks to add a new board game to the Board Game
  Campaign Planner application. Guides the agent through every required file,
  field, image asset, and translation needed to fully integrate a new game,
  then verifies the build compiles cleanly. Maintains a living backlog of
  candidate games in game-backlog.md — removes each game from the list once it
  is successfully added.
---

# Add a New Game to the Campaign Planner

This skill walks through every step required to add a new board game to the
application. The app auto-discovers games from the `games/` directory — **zero
C# code changes are needed**. All you must do is create the correct folder
structure with the correct content.

> **Campaign Milestones & Spoiler Policy**:
> - Research and define campaign-specific milestones in `game.json` and localize them in all translation files (`en`, `es`, `de`, `fr`, `it`).
> - Milestones should represent meaningful narrative acts, chapters, or pivotal campaign turning points (not just arbitrary 25%, 50%, 75%, 100% percentages).
> - The application includes a "Show Spoilers" toggle which is **deactivated by default** (hiding titles, phase names, and descriptions under spoiler masks while retaining schedule dates and meetup metrics). When enabled by the user, full narrative details are displayed.
> - **If no clear campaign milestones can be found for a game, omit the `milestones` array entirely.** When omitted, the app will gracefully skip milestone tracking and hide the roadmap and timeline milestone filters for that game.

---

## Step 0 — Select Which Game to Add

Read the backlog file first: [game-backlog.md](./game-backlog.md)

**Decision rules:**

1. **A specific game was requested by the user** → use that game. If it is
   present in the backlog, proceed and remove it at the end (Step 8). If it
   is not in the backlog, proceed anyway — it may be a net-new suggestion.

2. **No specific game was requested** → pick the **first game listed** in the
   High Priority section of the backlog. Inform the user which game was
   selected and why (top of the list = highest priority not yet added).

Confirm the selected game with a brief one-line acknowledgement before
proceeding to Step 1 (e.g. *"Adding **Pandemic Legacy: Season 1** — top of
the backlog."*).

---

## Step 1 — Research the Game

Before writing any files, gather accurate public-domain data from BGG and the
official rulebook. You need:

| Data point | Where to find it |
|---|---|
| Exact title, release year, designers | BGG game page |
| Player count (min / max) | BGG / rulebook |
| BGG Rating & Weight | BGG game page (weighted average) |
| BGG URL | BGG game page |
| Average scenario / session duration (minutes) | BGG forums, community survey data |
| Average setup & teardown time (minutes) | BGG forums |
| Unit type (scenarios, games_months, chapters, expeditions, etc.) | How the game itself labels individual sessions |
| Number of scenarios per scope (see Step 3) | Official rulebook, BGG wiki |
| Estimated fail/retry rate per scope | Community statistics, BGG forums |
| Campaign-specific milestones (optional) | Rulebook narrative acts, chapter breaks, or campaign arcs |

Cross-reference at least two sources. All data must be verifiable and from
public domain (BGG pages, official rulebooks, community surveys).

---

## Step 2 — Create the Folder

```
games/{game-id}/
├── game.json
├── cover.webp
├── background.webp
└── translations/
    ├── en.json
    ├── es.json
    ├── de.json
    ├── fr.json
    └── it.json
```

- `{game-id}` must be **lowercase, alphanumeric, hyphens only**
  (e.g. `pandemic-season-1`, `kingdom-death-monster`).
- The id must be unique across all existing games in `games/`.

---

## Step 3 — Write game.json

See the full annotated reference: [game-json-reference.md](./references/game-json-reference.md)

**Quick rules:**
- `id` must exactly match the folder name.
- `unitType` must accurately describe how the game calls one session of play.
  Common values: `"scenarios"`, `"games_months"`, `"chapters"`,
  `"expeditions"`, `"quests"`, `"missions"`.
- Define **2–4 scopes** covering realistic play ranges (e.g. story-only,
  standard, completionist). Always mark exactly **one** scope as
  `"isRecommended": true`.
- `baseScenarioCount` = expected sessions for that scope *before* retry
  padding. The calculator applies `estimatedFailRatePercent` on top.
- Scope `id` strings must be unique within the game and referenced
  identically in all translation files.
- **Milestones (Optional)**: Include a `milestones` array representing campaign narrative checkpoints (e.g., Act or Chapter completions) with `order`, `atScenarioOrGameIndex`, `iconEmoji`, `badgeText`, `title`, `phase`, and `description`. If the game doesn't have clear narrative milestones or none can be identified, omit the `milestones` property completely.
- Theme colors should reflect the game's visual identity. Use CSS hex colors.
  `themeClass` / `activeThemeClass` / `roadmapClass` / `heroClass`
  must all be unique strings not used by any other game.
- `backgroundImage` can be omitted (defaults to `background.webp`) or
  set to `null` to disable the background entirely.

---

## Step 4 — Prepare Images

### cover.webp
- **Content**: Front box art, tightly cropped.
- **Aspect ratio**: **1:1 square**.
- **Format**: WebP.
- **Recommended size**: 400x400 px to 600x600 px.
- **Source**: Publisher press kit, BGG image gallery (check licence), or
  generate a representative placeholder if real art is not freely available.

### background.webp
- **Content**: Atmospheric artwork or illustration — NOT the box cover.
  Ideally a landscape/scene from the game's universe.
- **Aspect ratio**: **~16:9 widescreen**.
- **Format**: WebP.
- **Recommended size**: 1920×1080 px (at least 1280×720 px).
- **Usage**: Rendered as a fixed, subtle atmospheric backdrop behind the app
  when this game is selected. CSS applies theme-aware opacity — the image
  does NOT need to be pre-darkened.
- Every game **must** have a `background.webp`. Do not omit it.

#### Default fallback background

A pre-converted generic campaign background is stored in this skill folder:
[default-background.webp](./default-background.webp)

**Use it when:**
- No game-specific atmospheric artwork can be found from public-domain or
  press-kit sources.
- The available art is not suitable (wrong aspect ratio, too busy, low
  resolution, unclear licence).

**How to use it:**

```powershell
Copy-Item ".agents\skills\add-game\default-background.webp" `
          "games\{game-id}\background.webp"
```

Note this in a comment when creating the game, so it is easy to replace
later with game-specific artwork if it becomes available.

Place both files directly inside `games/{game-id}/`.

---

## Step 5 — Write Translation Files

All five locales are required: en, es, de, fr, it.

See the annotated reference: [translations-reference.md](./references/translations-reference.md)

**Key rules:**
- `title` is usually the original game title (not translated).
- `subtitle`, `tagline`, `loreSummary`, `badgeCategory`,
  `roadmapTitle`, `roadmapBadge`, and `keyFeatures` must all be
  genuinely localized into the respective languages (Spanish, German, French, Italian) — not machine-translated word-for-word without review, and NOT left in English.
- Each scope key in `scopes` must exactly match the `id` values in
  `game.json`.
- `keyFeatures` is an array of 3–5 short strings highlighting what makes
  this game's campaign special.
- `loreSummary` is 1–2 sentences of evocative, spoiler-free flavour text
  describing the premise. Write in second person ("You are…") where natural.
- If `milestones` are defined in `game.json`, translate each milestone's `badgeText`, `title`, `phase`, and `description` in all 5 locale files (`en.json`, `es.json`, `de.json`, `fr.json`, `it.json`).

---

## Step 6 — Verify the Build

Run the following commands from the repository root and confirm they succeed:

```bash
# 1. Compilation check (fast)
dotnet build --no-restore

# 2. Release publish (full integration check)
dotnet publish -c Release -o ./publish-test
```

Both must exit with code 0 and show no errors. Warnings are acceptable but
should be investigated.

---

## Step 7 — Checklist Before Finishing

- [ ] games/{game-id}/game.json — valid JSON, all required fields present
- [ ] games/{game-id}/cover.webp — 1:1 square WebP image exists
- [ ] games/{game-id}/background.webp — 16:9 widescreen WebP image exists
- [ ] games/{game-id}/translations/en.json — complete
- [ ] games/{game-id}/translations/es.json — translated into Spanish
- [ ] games/{game-id}/translations/de.json — translated into German
- [ ] games/{game-id}/translations/fr.json — translated into French
- [ ] games/{game-id}/translations/it.json — translated into Italian
- [ ] Scope IDs in game.json match keys in all translation files
- [ ] If milestones are included, milestone definitions in game.json match milestones arrays in all 5 translation files (or milestones are completely omitted if no campaign checkpoints exist)
- [ ] themeClass, activeThemeClass, roadmapClass, heroClass are unique across all games
- [ ] dotnet build --no-restore passes with exit code 0
- [ ] dotnet publish -c Release -o ./publish-test passes with exit code 0
- [ ] Game appears in the game selector when running dotnet watch
- [ ] Increment minor version in version.json (e.g. 0.2 -> 0.3) as per AGENTS.md versioning rules
- [ ] Update CHANGELOG.md to record the newly added game under the new version matching version.json
- [ ] Backlog updated — game removed from game-backlog.md (Step 8)

---

## Step 8 — Update the Backlog

Once the build passes and the checklist above is complete, edit
[game-backlog.md](./game-backlog.md) and **remove the line** corresponding to
the game that was just added.

Also update the **"Currently in app"** line at the top of `game-backlog.md`
to include the newly added game title.

**Do this even if the game was not originally in the backlog** — simply skip
the removal step in that case, but still update the "Currently in app" line.

After editing the backlog, briefly summarise for the user:
- Which game was added
- How many games remain in the backlog (count the bullet points)
- Which game is next in line (the new first entry in High Priority)

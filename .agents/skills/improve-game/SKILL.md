---
name: improve-game
description: >-
  Use this skill when the user asks to improve games in the Campaign Planner whose
  quality is "low" or "mid" up to "high". Scans the repository for games with
  incomplete metadata, placeholder or missing image assets, or missing translations,
  remediates all deficiencies to bring the game to high quality, sets quality to
  "high", and verifies the build. If all games already have quality "high", it
  reports this and does nothing.
---

# Improve Campaign Game Quality to "High"

This skill audits existing board game entries in `games/` to detect any game whose
`quality` is `"low"` or `"mid"`, systematically identifies and rectifies all
deficiencies (placeholder/missing images, missing or unlocalized translations,
incomplete scopes, missing narrative milestones, or inaccurate metadata), updates
the game's `quality` to `"high"`, and verifies that the application builds and
publishes cleanly.

If **all** games already have `"quality": "high"`, this skill **does nothing**
and exits cleanly.

---

## Quality Definitions Reference

According to `AGENTS.md`, game entries are categorized into three quality tiers:

| Quality | Criteria |
|---|---|
| `"high"` | Everything is correct, complete, and authentic: game-specific 1:1 flat box cover art, custom ~16:9 atmospheric widescreen background illustration (not the generic fallback), all 5 localized translations (`en`, `es`, `de`, `fr`, `it`) fully translated with no English placeholders, complete scopes (2–4 scopes, exactly one recommended), campaign milestones properly defined and localized (or cleanly omitted if not applicable), and validated theme colors/fonts. |
| `"mid"` | Basic functionality works, but placeholder or default assets are used (e.g. using `default-background.webp`), narrative milestones are missing when the game has clear narrative acts, or scope timings are rough estimates. |
| `"low"` | Critical content is missing or malformed: translations are missing or completely untranslated, cover/background images are missing or broken, BGG metadata is incomplete, or scope keys mismatch translation files. |

---

## Step 0 — Scan & Identify Candidate Games

Run the following command to scan all games in `games/` and list any game whose
`quality` is not `"high"`:

```powershell
Get-ChildItem -Path games -Directory | ForEach-Object {
    $gameJsonPath = Join-Path $_.FullName "game.json"
    if (Test-Path $gameJsonPath) {
        $json = Get-Content $gameJsonPath -Raw | ConvertFrom-Json
        if ($json.quality -ne "high") {
            $bg = Join-Path $_.FullName "background.webp"
            $cover = Join-Path $_.FullName "cover.webp"
            $bgLen = if (Test-Path $bg) { (Get-Item $bg).Length } else { 0 }
            $coverLen = if (Test-Path $cover) { (Get-Item $cover).Length } else { 0 }
            $translations = (Get-ChildItem (Join-Path $_.FullName "translations") -Filter "*.json" -ErrorAction SilentlyContinue | Select-Object -ExpandProperty BaseName) -join ", "
            $isDefaultBg = ($bgLen -eq 141598)

            [PSCustomObject]@{
                Id = $json.id
                Title = $json.title
                Quality = $json.quality
                HasDefaultBg = $isDefaultBg
                CoverBytes = $coverLen
                BgBytes = $bgLen
                Locales = $translations
            }
        }
    }
} | Format-Table -AutoSize
```

### Decision Rules:

1. **No candidate games found** (all games have `"quality": "high"`):
   - **Do nothing.**
   - Output a clear message to the user:
     > "All board games in the catalog currently have **high** quality. No improvements are needed at this time."
   - Halt execution immediately.

2. **A specific game was requested by the user**:
   - If the user explicitly requested a specific game (e.g. *"Improve Aeon's End: Legacy"*), work on that game.
   - If the requested game already has `"quality": "high"`, verify whether there is any specific aspect the user wanted improved; if none, inform the user it is already rated high.

3. **No specific game requested, but candidates exist**:
   - Prioritize games by urgency:
     1. **`"quality": "low"`** games first (most critical deficiencies).
     2. **`"quality": "mid"`** games next.
   - Pick the first candidate from the prioritized list.
   - Inform the user which game is being upgraded and summarize why:
     > *"Selected **[Game Title]** (`[game-id]`) for quality improvement (current quality: `[low/mid]`)."*

---

## Step 1 — Audit the Chosen Game for Deficiencies

Inspect the target game folder: `games/{game-id}/` across four key areas:

### 1. Cover Art (`cover.webp`)
- **Check presence and size**: Does `cover.webp` exist? Is the file non-empty?
- **Aspect Ratio & Framing**: Must be a **1:1 square** image.
- **Content Authenticity**: Must be flat 2D graphic/illustration of the front box art cropped cleanly.
- **Defects to fix**:
  - 3D angled box shots, photos of physical boxes sitting on tables, camera perspective distortion, or watermarks.
  - Corrupted or tiny placeholder images (< 10 KB).

### 2. Atmospheric Background (`background.webp`)
- **Check placeholder status**: Does `background.webp` have a size of **141,598 bytes**?
  - 141,598 bytes is the exact size of `default-background.webp` (the generic fallback).
  - If it matches or is missing, the game **does not have high quality** and needs a bespoke atmospheric background.
- **Aspect Ratio & Resolution**: Must be widescreen **~16:9** (recommended 1920×1080 or at least 1280×720).
- **Content**: Widescreen evocative world illustration, landscape, battle scene, or concept art from the game's setting. NOT a duplicate of the box cover.

### 3. Translations (`translations/`)
- Check that all **five** locale files exist:
  - `en.json` (English)
  - `es.json` (Spanish)
  - `de.json` (German)
  - `fr.json` (French)
  - `it.json` (Italian)
- Check each translation file for:
  - **Completeness**: Are all required keys present (`title`, `subtitle`, `tagline`, `loreSummary`, `badgeCategory`, `roadmapTitle`, `roadmapBadge`, `keyFeatures`, `scopes`)?
  - **Scope keys**: Do the scope IDs in `scopes` match the scope IDs in `game.json` exactly?
  - **Milestone keys**: If `milestones` exist in `game.json`, does each locale file have a corresponding `milestones` array with `badgeText`, `title`, `phase`, and `description`?
  - **Localization quality**: Are `es`, `de`, `fr`, and `it` genuinely translated, or were they left in English or filled with placeholder text?

### 4. Game Configuration & Metadata (`game.json`)
- Verify all required fields:
  - Accurate `bggRating`, `bggWeight`, `bggUrl`, `releaseYear`, and `designers`.
  - Realistic `baseScenarioMinutes` and `setupTeardownMinutes`.
  - Appropriate `unitType` (`scenarios`, `chapters`, `missions`, `games_months`, `expeditions`, `quests`).
  - Cohesive `theme`: valid hex colors (`primaryColor`, `secondaryColor`, `accentColor`), gradient, card background, font choices, unique theme classes (`theme-{id}`, etc.).
  - Realistic `scopes` (2–4 scopes, exactly one with `"isRecommended": true`).
  - `milestones`: Narrative campaign checkpoints (Acts, Chapters, Major Milestones) if the game has narrative progression. If the game does not have narrative campaign milestones, ensure the `milestones` property is cleanly omitted.

---

## Step 2 — Remediate Deficiencies

Perform the necessary fixes based on your audit findings:

### A. Rectify Images
1. **Background**:
   - Find or source authentic, high-resolution widescreen (~16:9) artwork from the game universe (publisher media kits, promotional wallpapers, rulebook splash art, or BGG image gallery).
   - Crop and format as WebP at `games/{game-id}/background.webp`.
2. **Cover**:
   - Acquire a clean, flat 2D box art image.
   - Crop to a 1:1 square centered on the game title and art.
   - Save as WebP at `games/{game-id}/cover.webp`.

### B. Rectify Translations
1. Ensure all five files (`en.json`, `es.json`, `de.json`, `fr.json`, `it.json`) exist in `games/{game-id}/translations/`.
2. Ensure every non-English file is fluently and idiomatically localized:
   - `subtitle`: Evocative, 8–14 words.
   - `tagline`: Punchy, 6–12 words.
   - `loreSummary`: 1–2 sentences of immersive setting summary.
   - `badgeCategory`: Natural gaming genre terminology in the target language.
   - `roadmapTitle` & `roadmapBadge`: Localized roadmap headers.
   - `keyFeatures`: 3–5 bullet points translated into fluent target language.
   - `scopes`: Every scope name and description translated.
   - `milestones`: If milestones exist, localize every `badgeText`, `title`, `phase`, and `description`.

### C. Rectify game.json
1. Correct any outdated or missing fields in `games/{game-id}/game.json`.
2. Align scopes and milestones between `game.json` and all translation files.
3. Verify public source references cite real, verifiable sources (BGG, official rulebook, community logs).

---

## Step 3 — Upgrade Quality to "High"

Once all deficiencies have been completely resolved and all criteria for `"high"` quality are satisfied:

1. Edit `games/{game-id}/game.json`:
   ```json
   "quality": "high"
   ```

---

## Step 4 — Verification & Testing

Run the standard build and publish commands to verify code and asset integrity:

```bash
# 1. Compilation check
dotnet build --no-restore

# 2. Release publish check
dotnet publish -c Release -o ./publish-test
```

Both commands must exit with code 0 without errors.

---

## Step 5 — Report Improvements to the User

Present a clear summary of what was audited, upgraded, and verified:

1. **Game Upgraded**: Title and ID.
2. **Deficiencies Resolved**:
   - Background replaced (from generic placeholder `default-background.webp` to bespoke artwork).
   - Cover updated (clean flat 1:1 2D box art).
   - Translations updated (languages completed or improved).
   - Milestones or scopes added or corrected.
3. **Quality Status**: Now `"quality": "high"`.
4. **Remaining Candidates**:
   - List any remaining games with `"quality": "low"` or `"quality": "mid"`, if any.
   - If no remaining candidates exist, congratulate the user that all catalog games are now high quality!

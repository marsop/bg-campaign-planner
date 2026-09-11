# Translation Files — Annotated Reference

Each game requires **five** translation files in
`games/{game-id}/translations/`:

| File | Locale |
|---|---|
| `en.json` | English (default, always required first) |
| `es.json` | Spanish |
| `de.json` | German |
| `fr.json` | French |
| `it.json` | Italian |

---

## Full Schema with Annotations (using en.json as the template)

```json
{
  // The game title. Usually identical across all locales unless an official
  // localized title exists (e.g. a publisher-released Spanish edition).
  "title": "Your Game Title",

  // A short evocative subtitle describing the game experience.
  // Appears below the title in the game selector card.
  // Aim for 8–14 words. Should capture the *feel* of playing the campaign.
  "subtitle": "A brutal, narrative-driven campaign of survival and discovery",

  // A punchy one-liner / flavour tagline. 6–12 words.
  // Think of it as a movie poster tagline.
  "tagline": "Every choice leaves a mark on the world",

  // 1–2 sentences of evocative, spoiler-free lore describing the premise.
  // Write in second person ("You are…") where it feels natural.
  // Do NOT reference specific scenario names, boss names, or plot twists.
  "loreSummary": "You are a band of adventurers on the edge of civilization, where every decision permanently shapes the world around you.",

  // Short genre/category badge shown in the game card.
  // 2–5 words, e.g. "Dark Fantasy Tactical RPG", "Legacy Cooperative"
  "badgeCategory": "Dark Fantasy Tactical RPG",

  // Title for the thematic roadmap section shown during campaign planning.
  // Include a relevant emoji at the start. ~5–8 words.
  "roadmapTitle": "🗺️ Campaign Chronicle & Story Progression",

  // Short badge label shown in the roadmap header. ALL CAPS, 2–4 words.
  "roadmapBadge": "FANTASY ROADMAP",

  // Array of 3–5 short strings describing the game's unique campaign features.
  // Each string should be a complete thought, 5–12 words.
  // Highlight mechanics that make THIS campaign interesting/different.
  // Do NOT mention specific scenario names or story beats.
  "keyFeatures": [
    "Card-driven deterministic combat — no dice, pure strategy",
    "Character retirement unlocks new classes and story arcs",
    "Persistent world stickers alter locations permanently",
    "Branching narrative driven by town and road events"
  ],

  // Scope translations. Keys must EXACTLY match the "id" values in game.json.
  "scopes": {
    "yourgame_standard": {
      // Human-readable scope name shown in the scope selector dropdown.
      // 3–6 words.
      "name": "Standard Campaign",
      // One or two sentences describing what this scope covers.
      // Be specific about what content is/isn't included.
      "description": "The main story arc plus key side quests and 2–3 character retirement chains."
    },
    "yourgame_quick": {
      "name": "Quick Story Run",
      "description": "Direct path through the primary questlines to the finale with minimal optional diversions."
    },
    "yourgame_completionist": {
      "name": "Full Completionist",
      "description": "Every unlockable scenario, all character retirements, maximum prosperity, and all side dungeons."
    }
  }
}
```

---

## Localization Quality Guidelines

- Translate meaning, not words. A fluent native-speaker tone is preferred over
  a literal translation.
- Ensure all non-English translation files (es.json, de.json, fr.json, it.json) are correctly translated into their respective languages and not simply copied in English.
- Keep `keyFeatures` concise — each bullet should be skimmable.
- `loreSummary` sets the emotional mood — make it feel immersive, not like a
  rulebook description.
- `badgeCategory` should use gaming genre vocabulary natural in that language.
- Scope `name` and `description` should help the player choose their
  commitment level, not describe rules mechanics.

---

## Example: Gloomhaven en.json (Reference)

```json
{
  "title": "Gloomhaven",
  "subtitle": "Euro-inspired tactical combat in a persistent, evolving world of high fantasy",
  "tagline": "Venture into the unforgiving shadows of the Sleeping Lion",
  "loreSummary": "You are wandering mercenaries with your own special set of skills. In the harsh wilderness on the edge of civilization, every choice permanently alters the world around you.",
  "badgeCategory": "Dark Fantasy Tactical RPG",
  "roadmapTitle": "🗺️ Dungeon Chronicle & Story Progression",
  "roadmapBadge": "FANTASY ROADMAP",
  "keyFeatures": [
    "Card-driven deterministic combat (No dice, pure tactical strategy)",
    "Retirement & Class Unlocking (Personal Quests drive new character growth)",
    "Branching Narrative (Town & Road events, world stickers, prosperity tracker)",
    "High difficulty scaling across 1-4 players"
  ],
  "scopes": {
    "core_campaign": {
      "name": "Standard Core Campaign",
      "description": "The main storyline arc to the final boss plus key side-quests and 2-3 character retirement chains."
    },
    "story_speedrun": {
      "name": "Main Story Focus (Speedrun)",
      "description": "Direct path through the primary questlines to the final boss with minimal optional side dungeon diversions."
    },
    "extended_campaign": {
      "name": "Extended Campaign Experience",
      "description": "Main story, numerous character unlocks, town records progression, and side quest chains."
    },
    "completionist": {
      "name": "Full Completionist (All Accessible)",
      "description": "Attempting every unlockable scenario, high prosperity, all class retirements, and high-level side dungeons."
    }
  }
}
```

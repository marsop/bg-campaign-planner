---
name: find-game-candidates
description: >-
  Use this skill when the user wants to discover and evaluate new board games
  to add to the game-backlog.md used by the add-game skill. Researches BGG and
  community sources to find popular, mainstream campaign board games (prioritizing
  widespread community reach over obscure or boutique epic-length games), checks
  existing games and backlog to avoid duplicates, scores candidates by popularity
  and quality, and appends approved entries to game-backlog.md.
---

# Find New Game Candidates for the Campaign Planner

This skill discovers suitable campaign board games and adds them to
the [game-backlog.md](../add-game/game-backlog.md) used by the `add-game`
skill. The backlog is the source of truth for what gets added to the app next.

---

## Step 0 — Understand the Current State

Before searching for anything, read the current state of the repository:

### 0a — Games already in the application

List all subdirectory names under `games/` in the repository root. Each folder
name is a game that is **already in the app** and must never be added to the
backlog.

```powershell
Get-ChildItem -Path games -Directory | Select-Object -ExpandProperty Name
```

Cross-reference these IDs with the **"Currently in app"** line at the top of
[game-backlog.md](../add-game/game-backlog.md) to confirm they match.

### 0b — Games already in the backlog

Read [game-backlog.md](../add-game/game-backlog.md) in full and extract the
name of every game already listed (all three priority tiers). These must not
be suggested again — duplicates add noise and confuse priority ordering.

Build two exclusion sets before proceeding:

- **Already in app** — folder names from `games/`
- **Already in backlog** — titles from `game-backlog.md`

---

## Step 1 — Define Eligibility Criteria

A game is a **good candidate** for the Campaign Planner if it meets ALL of the
following criteria:

### Must-Have
| Criterion | Threshold |
|---|---|
| **Campaign structure** | Has a fixed sequence of scenarios, chapters, acts, episodes, cases, or legacy games where progress carries over between sessions |
| **Total play time / scope** | Spans multiple sessions (typically >= 10-15 hours or >= 8 discrete sessions/scenarios) — does not need to be an epic 50+ hour monster; mainstream accessible campaigns are welcome as long as they are campaign-based |
| **Popularity & Mainstream Presence** | Widely played and recognized (prefer >= 2,000+ BGG ratings, ideally >= 5,000+; avoid obscure boutique crowdfunding titles known by only a tiny niche) |
| **BGG rating** | >= 7.0 weighted average |
| **Published & Accessible** | Released at retail and widely available (not an obscure, out-of-print, or unreleased prototype) |
| **Player count** | Supports 1-5 players (solo, cooperative, or group play) |
| **Campaign-based game** | Cooperative, semi-cooperative, or campaign/legacy game with persistent progression |

### Strong Signals (boost priority)
- **High Popularity & Wide Ownership** (>= 5,000+ BGG ratings, prominent BGG ranking, household name in board gaming)
- **Mainstream Retail Presence** — games that casual-to-dedicated hobbyists frequently encounter, purchase, and play
- **Accessible Campaign Structure** — games groups can realistically schedule and complete (10-35 hours or 8-24 sessions are ideal; doesn't have to be a 100-hour behemoth)
- **Clear Milestone / Progression Checkpoints** (named scenarios, legacy envelopes/boxes, chapters, acts, calendar months)
- **Legacy or Campaign Evolution** (deck building across games, map stickers, unlocked character abilities, evolving story)

### Disqualifying or Penalizing Signals (lower or exclude)
- **Obscure or Boutique Games**: Low ownership or low rating count (< 1,000-2,000 BGG ratings), even if heavily hyped by a small backer community or boasting 100+ hours of "epic" content
- Pure one-shot games without persistent campaign, scenario progression, or legacy progression
- Completely procedurally generated games with no discrete scenarios, acts, or structured endpoints
- Out of print with no reprint in sight (difficult for regular players to find or buy)
- BGG rating < 7.0

---

## Step 2 — Research Candidates

Search BGG and community sources for games that meet the eligibility criteria
and are **not already in the app or the backlog**.

### Recommended search angles

1. **BGG Most Rated / Popular Campaign & Legacy Games** — Search BGG by number of ratings (voters) filtered by "Campaign / Battle Card Driven", "Legacy Game", "Scenario / Mission / Campaign Game". Focus on the most-voted and most-played titles first.
2. **Mainstream BGG Top Rankings** — Browse BGG's overall top games, thematic, strategy, and family rankings to identify well-known campaign and legacy titles (e.g., *Ticket to Ride: Legends of the West*, *Harry Potter: Hogwarts Battle*, *The King's Dilemma*, *Charterstone*, *Marvel Champions* campaign expansions, *Clank! Legacy*, *Pandemic Legacy* titles, etc.).
3. **Widely Played Mainstream Favorites** — Look for popular retail hits with campaign or episodic progression (adventure book games, accessible dungeon crawlers, narrative mystery campaigns).
4. **Avoid the "Obscure Crowdfunding Trap"** — Do not prioritize massive 100-hour, $300 Kickstarter/Gamefound behemoths that only a few hundred or thousand backers received unless they have truly crossed over into mainstream popularity. Popularity and actual table presence trump "epicness".
5. **Community recommendations** — Check discussions on r/boardgames ("most popular campaign games", "best legacy games for groups") and mainstream board game media channels.

### For each candidate found

Collect the following data points (same fields needed later by `add-game`):

| Field | Source |
|---|---|
| Exact title | BGG |
| Release year | BGG |
| Designers | BGG |
| Player count (min/max) | BGG |
| BGG Rating | BGG (weighted average) |
| BGG Number of Ratings / Voters | BGG (indicator of popularity) |
| BGG Weight | BGG |
| Campaign length (hours, range, session count) | BGG forums / reviews |
| Unit type | Rulebook / BGG description |
| Brief description (1 sentence) | BGG / publisher |

---

## Step 3 — Score and Prioritize Candidates

Assign each candidate a **priority tier** emphasizing **popularity and mainstream reach over sheer complexity or epic length**:

### High Priority — Mainstream Hits & Widely Played Campaigns:
- **High popularity**: >= 5,000 BGG ratings (or high rank in BGG Top 500 / mainstream recognition)
- BGG rating >= 7.5
- Clear multi-session campaign or legacy structure (even if not "epic" length — 10-30 hours is great)
- Widely available at retail

### Medium Priority — Popular & Solidly Established:
- **Moderate popularity**: 2,000 - 5,000 BGG ratings
- BGG rating >= 7.2
- Distinct campaign progression or legacy milestones
- Well-known in its category with steady retail presence

### Lower Priority — Niche or Borderline Popularity:
- Moderate-to-low popularity: 1,000 - 2,000 BGG ratings
- BGG rating 7.0 - 7.4
- More niche, heavier, or boutique appeal that has less mainstream traction
- Games with unusual campaign structures that require bespoke planner adaptation

> **Important**: Do NOT promote a game to High or Medium Priority merely because it is "epic" in scope (e.g. 80-150 hours) if it is obscure or has few ratings (< 2,000 ratings). High player count and mainstream popularity come first.

Do not add a game to the backlog if it does not meet the Must-Have criteria,
regardless of hype or novelty.

---

## Step 4 — Check for Conflicts and Deduplication

Before writing anything, perform a final check:

1. Re-confirm that every candidate is absent from `games/` folder names.
2. Re-confirm that every candidate title is absent from `game-backlog.md`
    (case-insensitive, partial match is enough to flag).
3. Check that no candidate is a direct variant/edition of a game already in
    the app or backlog (e.g. do not add *Gloomhaven: Jaws of the Lion* if
    *Gloomhaven* is already tracked, unless the variant has substantially
    different campaign scope).

Discard any candidate that fails any of these checks.

---

## Step 5 — Present Candidates for Review

Before writing to the backlog, **present the proposed additions to the user**
as a summary table:

```
| Title | BGG Rating | BGG Voters / Ratings | Campaign Length | Priority Tier | Reason |
|---|---|---|---|---|---|
| Game A | 8.2 | ~15,000 | 15-25 h | High | Mainstream hit, high player base, clear legacy progression |
| Game B | 7.6 | ~4,200 | 20-30 h | Medium | Popular retail title, solid campaign structure |
```

Also include:
- How many games are currently in the backlog (per tier)
- How many net-new games you are proposing to add

Ask the user to confirm before writing to the backlog. Prompt: *"Shall I add
these candidates to the backlog? You can also tell me to adjust priorities or
drop any entry."*

If the user approves (or approves with modifications), proceed to Step 6.

---

## Step 6 — Update game-backlog.md

Edit [game-backlog.md](../add-game/game-backlog.md) to append the approved
candidates under the correct priority section (High / Medium / Lower).

### Format for each new entry

```markdown
- **{Title}** — {One-sentence description}. {Session count or hour range}. ~{BGG rating} BGG. {min}-{max} players. {Key mechanics / genre}.
```

Examples (follow this style exactly):

```markdown
- **Ticket to Ride: Legends of the West** — Competitive legacy train route expansion game across 12 historical rounds. 12 sessions (20-30 hours). ~8.4 BGG. 2-5 players. Route building, legacy box unlocks, evolving map.
- **The King's Dilemma** — Interactive narrative legacy dilemma experience resolving generational crises for the realm. 15-20 sessions (30-45 hours). ~7.8 BGG. 3-5 players. Secret agendas, branching storyline, legacy stickers.
```

### Placement rules

- Append new entries **at the end of the appropriate priority section**.
- Do **not** reorder existing entries — the `add-game` skill picks from the
  top of each section, so stability matters.
- Do **not** remove or modify existing entries.

### Update the Notes section

If any new game has unusual structural properties (e.g. app dependency,
unique player count dynamics, legacy destruction), add a brief note in the
`## Notes` section.

---

## Step 7 — Confirm and Report

After updating the backlog, report to the user:

- How many new candidates were added (and to which tiers)
- The updated backlog counts per tier
- Which game is now **first in the High Priority queue** (the next game the
  `add-game` skill will pick by default)
- A reminder that the `add-game` skill can be triggered to start adding the
  top-priority game

Example summary:

> Added 5 new candidates to the backlog (2 High, 2 Medium, 1 Lower).
>
> Backlog totals: High 6, Medium 9, Lower 8
>
> Next up for `add-game`: Ticket to Ride: Legends of the West
>
> Run the `add-game` skill to start adding games from the top of the queue.

---

## Reference — Eligibility Quick-Check

Use this checklist when evaluating any single game:

- [ ] Has a multi-session campaign or legacy structure (scenarios / chapters / months / episodes)
- [ ] Popular and mainstream (solid BGG voter volume, strongly prefer >= 2,000+ ratings; avoid obscure titles)
- [ ] Accessible campaign length (does not have to be an epic 50+ hour monster; 10+ hours or 8+ sessions is great)
- [ ] BGG weighted average rating >= 7.0
- [ ] Widely available at retail / not an out-of-print obscure oddity
- [ ] Cooperative, semi-cooperative, or campaign/legacy game
- [ ] Not already in `games/` folder
- [ ] Not already in `game-backlog.md`
- [ ] Not a trivial variant of an already-tracked game

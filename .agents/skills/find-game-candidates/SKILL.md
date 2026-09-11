---
name: find-game-candidates
description: >-
  Use this skill when the user wants to discover and evaluate new board games
  to add to the game-backlog.md used by the add-game skill. Researches BGG and
  community sources to find long-form campaign board games that are a good fit
  for the Campaign Planner, checks which games are already in the repository or
  in the backlog to avoid duplicates, scores candidates, and appends new entries
  to game-backlog.md.
---

# Find New Game Candidates for the Campaign Planner

This skill discovers suitable long-form campaign board games and adds them to
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
| **Campaign structure** | Has a fixed, numbered sequence of scenarios, chapters, acts, or months — i.e. progress is measurable in discrete units |
| **Total play time** | >= 20 hours for a full campaign (one play-through) |
| **BGG rating** | >= 7.0 weighted average |
| **Published** | Already released (not a crowdfunding campaign or upcoming title with no release date) |
| **Player count** | Supports 1-5 players (solo or small group play) |
| **Cooperative or semi-cooperative** | Fully competitive games with no shared narrative are excluded |

### Strong Signals (boost priority)
- BGG Weight >= 3.0 (complex enough to justify planning tools)
- Very long campaigns (50+ hours) — these benefit most from scheduling
- Active community (many ratings, active forums on BGG)
- Legacy or campaign-legacy mechanics that make replaying impractical
- Game has named scenarios or chapters making milestone tracking natural

### Disqualifying Signals (lower or exclude)
- Primarily competitive with no cooperative mode
- Campaign is entirely procedurally generated with no fixed scenario count
- Out of print with no reprint announced (hard for users to obtain)
- BGG rating < 7.0

---

## Step 2 — Research Candidates

Search BGG and community sources for games that meet the eligibility criteria
and are **not already in the app or the backlog**.

### Recommended search angles

1. **BGG "Campaign Games" lists** — Search BGG for "campaign game", "legacy
   game", "dungeon crawler campaign". Filter by rating >= 7.0.

2. **BGG Top-200 filtered** — Browse BGG's top-rated games and filter for
   campaign / cooperative genres.

3. **Recent releases** — Look for highly-rated campaign games released in the
   last 2-3 years that may not yet be on the backlog.

4. **Crowdfunding graduates** — Games that were Kickstarter/Gamefound successes
   and have since been released at retail.

5. **Community recommendations** — Check BGG forums, Reddit (r/boardgames,
   r/soloboardgaming), and board game review sites for "best campaign games"
   lists.

### For each candidate found

Collect the following data points (same fields needed later by `add-game`):

| Field | Source |
|---|---|
| Exact title | BGG |
| Release year | BGG |
| Designers | BGG |
| Player count (min/max) | BGG |
| BGG Rating | BGG (weighted average) |
| BGG Weight | BGG |
| Campaign length (hours, range) | BGG forums / reviews |
| Unit type | Rulebook / BGG description |
| Brief description (1 sentence) | BGG / publisher |

---

## Step 3 — Score and Prioritize Candidates

Assign each candidate a **priority tier** using this rubric:

### High Priority — ALL of:
- BGG rating >= 8.0
- Campaign >= 30 hours
- Published, widely available at retail
- Strong community presence (>= 1,000 BGG ratings)

### Medium Priority — ANY of:
- BGG rating 7.5-7.9
- Campaign 20-30 hours with high community regard
- Niche but critically acclaimed (e.g. unique mechanic, cult following)
- Strong candidate that narrowly misses High Priority on one criterion

### Lower Priority — remaining eligible candidates:
- BGG rating 7.0-7.4
- Recently released with limited rating volume but positive early reception
- Games that are niche but may appeal to specific audiences
- Games with unusual structures that may need extra design work to support

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
| Title | BGG Rating | Campaign Hours | Priority Tier | Reason |
|---|---|---|---|---|
| Game A | 8.5 | 80-120 h | High | Rating + length + legacy mechanics |
| Game B | 7.8 | 30-40 h | Medium | Acclaimed but shorter campaign |
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
- **Gloom of Kilforth** — Solo/cooperative dark fantasy quest game. 10-30 sessions. ~7.8 BGG. 1-4 players. Open-world, deck-building, narrative.
- **Cloudspire** — Solo/cooperative tower-defense campaign. 20-40+ hours. ~8.1 BGG. 1-4 players. Strategic, modular campaign.
```

### Placement rules

- Append new entries **at the end of the appropriate priority section**.
- Do **not** reorder existing entries — the `add-game` skill picks from the
  top of each section, so stability matters.
- Do **not** remove or modify existing entries.

### Update the Notes section

If any new game has unusual structural properties (e.g. enormous session count,
non-standard units, app dependency), add a brief note in the `## Notes`
section.

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
> Next up for `add-game`: Pandemic Legacy: Season 1
>
> Run the `add-game` skill to start adding games from the top of the queue.

---

## Reference — Eligibility Quick-Check

Use this checklist when evaluating any single game:

- [ ] Has a fixed, countable campaign structure (scenarios / chapters / months)
- [ ] Total play time >= 20 hours
- [ ] BGG weighted average rating >= 7.0
- [ ] Already released at retail
- [ ] Supports cooperative play with 1-5 players
- [ ] Not already in `games/` folder
- [ ] Not already in `game-backlog.md`
- [ ] Not a trivial variant of an already-tracked game

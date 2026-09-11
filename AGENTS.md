# 🤖 AGENTS.md - Developer & Agent Guide

This document provides operational context, architectural principles, codebase structure, and development workflows for AI agents and human contributors working on **Board Game Campaign Planner**.

---

## 🧭 Project Overview

- **Type**: Single Page Application (SPA)
- **Framework**: Blazor WebAssembly (.NET 10)
- **Runtime**: Client-side execution in the browser via WebAssembly (No server backend)
- **Deployment**: Static hosting on GitHub Pages via GitHub Actions workflow
- **Purpose**: Calculate and visualize meetups, table hours, calendar finish dates, and spoiler-free progress checkpoints for long-form campaign board games (**Gloomhaven**, **Frosthaven**, **Pandemic Legacy: Season 0**, **Sleeping Gods**, **Oathsworn**, **Tainted Grail**, and future games).

---

## 📂 Codebase Layout

```
bg-campaign-planner/
├── .github/
│   └── workflows/
│       └── deploy.yml              # GitHub Actions CI/CD to build & deploy to GitHub Pages
├── Components/
│   ├── CalculationSummaryCards.razor # 4 KPI cards + velocity progress bar
│   ├── GameSelector.razor          # Themed game selection cards with ratings & metadata
│   ├── PlannerForm.razor           # Frequency, recurrence, player count, and modifier form
│   ├── SessionsTimeline.razor      # Detailed calendar table, milestone filter, .ics download
│   └── ThematicRoadmap.razor       # Milestone progression tree tailored to the active game
├── Layout/
│   └── MainLayout.razor            # Application root layout container
├── Models/
│   ├── GameModels.cs               # BoardGame, CampaignScopeOption, GameMilestone, GameTheming
│   └── PlannerInput.cs             # PlannerInputModel, ScheduledSession, MilestoneProjection, CalculationResult
├── Pages/
│   └── Home.razor                  # Main orchestrator component integrating all features
├── Resources/                      # Standard .resx resource files & AppResources marker class
│   ├── AppResources.cs
│   ├── AppResources.resx           # Default (English)
│   ├── AppResources.es.resx        # Spanish
│   ├── AppResources.de.resx        # German
│   ├── AppResources.fr.resx        # French
│   └── AppResources.it.resx        # Italian
├── Services/
│   ├── CampaignDataService.cs      # Curated public domain campaign data & metadata
│   └── PlannerCalculatorService.cs # Calculation math, date generator, milestone mapping, ICS generator
├── wwwroot/
│   ├── css/
│   │   └── app.css                 # Themed CSS variables, dark-mode styling, responsive rules
│   ├── .nojekyll                   # Bypasses Jekyll processing on GitHub Pages
│   ├── 404.html                    # Single Page Application routing fallback for GitHub Pages
│   └── index.html                  # HTML entry point, Google Fonts, and JS interop helpers
├── _Imports.razor                  # Global namespace imports for Razor components
├── App.razor                       # Blazor router and error boundary configuration
├── Program.cs                      # WebAssembly host initialization and service DI registration
├── bg-campaign-planner.csproj      # .NET 10 project file
├── version.json                    # Nerdbank.GitVersioning configuration (major.minor version)
├── CHANGELOG.md                    # Changelog adhering to Keep a Changelog standard
├── README.md                       # User-facing documentation and quick start
└── AGENTS.md                       # This guide for AI coding assistants
```

---

## ⚙️ Architecture & Data Flow

1. **Static Data Provider (`CampaignDataService`)**:
   - In-memory curated catalog of board games, scopes, timings, and milestones.
   - All data is verifiable from public domain sources (BGG, official rulebooks, community surveys).
2. **State & Input Model (`PlannerInputModel`)**:
   - Holds user preferences: `FrequencyMode` (Weekly, Monthly, Custom), preferred days of week, start date, session count, player count, experience level, setup toggle, and failure buffer toggle.
3. **Calculation Engine (`PlannerCalculatorService`)**:
   - `Calculate(input, game)` produces an immutable `CalculationResult`.
   - Accounts for player count time scaling ($0.85\times$ to $1.20\times$), experience time and fail rate offsets, vacation buffer weeks, and meetup scheduling steps.
   - Generates RFC 5545 compliant `.ics` calendar content via `GenerateIcsFile(result)`.
4. **Reactive UI (`Home.razor`)**:
   - Coordinates inputs and triggers `Recalculate()` on any parameter change.
   - Switches the root CSS class (`theme-gloomhaven-active` / `theme-pandemic-active`) to dynamically change color palettes and typography.

---

## 🛠️ How to Extend the Codebase

### Adding a New Board Game
To add support for a new board game (e.g. *Frosthaven*, *Oathsworn*, or *Pandemic Legacy Season 1*):

1. Create a dedicated folder under `games/{game-id}/` where `{game-id}` is the unique identifier of the game (lowercase, alphanumeric, with hyphens).
2. Inside `games/{game-id}/`:
   - `game.json`: Invariant configuration (id, title, releaseYear, designers, player counts, BGG stats, scenario timings, unitType, theme colors/fonts/emblem, public sources, and scope options). Note: omit `"backgroundImage"` or let it default to `"background.webp"` (unless intentionally disabled via `"backgroundImage": null`).
   - `cover.webp`: Photo of the front box cover (cropped 1:1 square WebP format).
   - `background.webp`: Themed background illustration/artwork for the application (widescreen ~16:9 WebP format). Every game has this background image. It renders as a fixed, subtle atmospheric backdrop behind the app when the game is selected, styled with theme-aware opacity to ensure content remains visible and readable.
   - `translations/`: Directory with localized strings (`en.json`, `es.json`, `de.json`, `fr.json`, `it.json`). Each translation file provides localized `subtitle`, `tagline`, `loreSummary`, `badgeCategory`, `roadmapTitle`, `roadmapBadge`, `keyFeatures`, and scope names/descriptions.
3. That's it! The application automatically discovers all games and translations at build time via embedded resources and serves images through `games/{game-id}/cover.webp` and `games/{game-id}/background.webp`. Zero C# code changes or switch statements required.
   - Note on Milestones & Spoilers: Campaign milestones can be defined in `game.json` and localized in `translations/*.json` as meaningful story checkpoints (Acts, Chapters, or Months). A user-facing "Show Spoilers" toggle is deactivated by default to mask milestone titles, phases, and descriptions while keeping schedule dates visible. If no milestones are found for a game, the `milestones` array can be omitted entirely, and the roadmap will not be displayed.

### Modifying Calculation Logic
- All math and scheduling rules reside in `Services/PlannerCalculatorService.cs`.
- When modifying recurrence rules, ensure `GetNextSessionDate(...)` respects selected days of the week and handles month boundary transitions cleanly.

---

## 🧪 Build and Verification Workflows

When working in this repository, always verify changes using standard .NET commands:

```bash
# 1. Check compilation and linting
dotnet build --no-restore

# 2. Run local development server with hot-reload
dotnet watch

# 3. Test static release publish output
dotnet publish -c Release -o ./publish-test
```

### GitHub Pages Deployment Rules
- Keep all assets relative or compatible with subpath deployment (`/bg-campaign-planner/`).
- Do **not** introduce server-side ASP.NET Core controllers or database dependencies; all logic must remain purely client-side WebAssembly.
- Any new web assets or scripts should be added to `wwwroot/` and referenced in `wwwroot/index.html`.

---

## 🔢 Versioning Strategy (Nerdbank.GitVersioning)

This repository uses [Nerdbank.GitVersioning](https://github.com/dotnet/Nerdbank.GitVersioning) for semantic project and assembly versioning, driven by `version.json` at the repository root (starting at version `0.2`).

### Versioning Rules:
- **Minor Version (the second number, e.g. `0.2` → `0.3`)**:
  - **Every major change in functionality must increase the minor version** in `version.json`.
  - Examples of changes that require bumping the minor version:
    - Adding support for a new campaign board game.
    - Introducing major new features or architectural capabilities.
    - Significant updates or redesigns to scheduling math, algorithms, or campaign planning workflows.
- **Major Version (the first number, e.g. `0.x` → `1.0`)**:
  - The major version **will ONLY be updated specifically when asked by the user**. Do not increment the major version autonomously.
- **Patch and Revision Numbers (the third and fourth numbers)**:
  - Automatically calculated and managed by Nerdbank.GitVersioning based on git commit height and git commit IDs. Never manually configure git commit counts or patch numbers in `version.json`.


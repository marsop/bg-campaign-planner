# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.3.0] - 2026-08-28

### Added
- **Multi-Language (i18n) Support**:
  - Full localization for **5 languages**:
    - 🇬🇧 **English** (`en-US`)
    - 🇪🇸 **Español** (`es-ES`)
    - 🇩🇪 **Deutsch** (`de-DE`)
    - 🇫🇷 **Français** (`fr-FR`)
    - 🇮🇹 **Italiano** (`it-IT`)
  - Reactive `ILocalizationService` with instant in-memory language switching without requiring a page reload.
  - Automatic `CultureInfo` switching for localized date formatting (month and day names in calendar views, cards, and schedules).
  - Language preference persisted in `localStorage` with browser locale auto-detection on first visit.
  - Accessible `LanguageSelector` dropdown component in the application header.
  - Complete translation of all UI controls, buttons, tooltips, presets, summary notifications, and .ics export strings.
  - Localized game data catalog in `CampaignDataService` covering campaign scopes, descriptions, milestone acts, thematic taglines, lore summaries, and key features for both **Gloomhaven** and **Pandemic Legacy: Season 0**.

## [0.2.0] - 2026-08-28

### Added
- **Light and Dark Mode Support**:
  - Full support for **Light Mode**, **Dark Mode**, and **Auto / System** mode tracking OS preference (`prefers-color-scheme`).
  - Persistent user preference stored in `localStorage` with instant head pre-loading to prevent any theme flicker.
  - Interactive, accessible `ThemeToggle` pill button component in the application header.
  - Tailored light-mode color palettes preserving the distinctive identity of both campaign games:
    - *Gloomhaven*: Warm parchment with deep amber, rich gold, and bronze accents.
    - *Pandemic Legacy: Season 0*: Clean blueprint/slate with cold-war cobalt and neon cyan accents.
  - Fluid CSS transitions across all cards, form controls, tables, badges, and background layers.
  - High-contrast, WCAG-friendly table rows, form inputs, roadmap cards, and KPI metrics in both themes.

## [0.1.0] - 2026-08-28

### Added
- **Blazor WebAssembly (.NET 10) Architecture**:
  - Standalone client-side Single Page Application (SPA) requiring no backend server.
  - Responsive dark-mode board game dashboard with custom theming and animations.
- **Game Theming & Public Campaign Data**:
  - **Gloomhaven**:
    - High-fantasy aesthetic with gold/crimson styling, dungeon crawler crests, and `Cinzel` typography.
    - 95-scenario box metadata with support for multiple campaign scopes (Standard Core, Story Speedrun, Extended, Completionist).
    - Act-based story milestones: Black Barrow, First Class Retirement, Branching Factions, The Gloom Rift, and Final Boss.
  - **Pandemic Legacy: Season 0**:
    - Cold War 1962 spy thriller aesthetic, CIA top-secret briefing styling, and `Special Elite` typography.
    - 12-month calendar and prologue campaign metadata with early/late month win-loss attempts.
    - Milestone tracking: CIA Induction, Project MEDUSA (Q1), Double Agent Infiltration (Q2), Biological Arms Race (Q3), and December Climax.
- **Interactive Campaign Calculator (`PlannerCalculatorService`)**:
  - Flexible frequency patterns: Weekly ($N\times/\text{week}$), Bi-weekly (every 2 weeks), Monthly ($N\times/\text{month}$), and Custom interval (every $N$ days).
  - Multi-select day-of-week preferences (e.g. Friday and Sunday game nights).
  - Configurable scenarios/games per session (1, 2, or 3 marathon scenarios).
  - Modifiers for player count (1–4 players) and group experience level (Beginner, Regular, Veteran).
  - Failure/retry padding and vacation/holiday buffer week injection.
  - Projections for total meetups, table time (gameplay vs. setup hours), and exact projected calendar completion dates.
- **Visualizations & Roadmap**:
  - KPI stat cards (Total Meetups, Completion Date, Table Time, Average Session Length).
  - Campaign milestone velocity tracker.
  - Thematic narrative roadmap displaying estimated meetup numbers and dates for key story achievements.
- **Calendar Export & Sharing**:
  - Standard RFC 5545 `.ics` iCalendar export for importing scheduled campaign nights directly into Google Calendar, Apple Calendar, or Outlook.
  - One-click "Copy Summary" for sharing campaign schedules on Discord or WhatsApp.
- **GitHub Pages CI/CD & Hosting Setup**:
  - Automated GitHub Actions workflow (`.github/workflows/deploy.yml`) for building and publishing .NET 10 WASM to GitHub Pages.
  - `404.html` SPA routing redirect handler for single-page application refreshes on GitHub Pages.
  - `.nojekyll` file to bypass Jekyll processing.
  - Comprehensive `.gitignore` and `README.md` documentation.

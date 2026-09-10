# 🎲 Board Game Campaign Planner

A client-side Single Page Application (SPA) built with **Blazor WebAssembly (.NET 10)** for planning and calculating campaign meetups, table time, and projected completion dates for epic legacy and campaign board games.

Hosted and deployed serverless on **GitHub Pages**.

---

## ✨ Features

- **🎮 Game Selector & Theming**:
  - Wide curated catalog of epic campaign games: **Gloomhaven**, **Frosthaven**, **Pandemic Legacy: Season 0**, **Sleeping Gods**, **Oathsworn: Into the Deepwood**, and **Tainted Grail: The Fall of Avalon**.
  - Dynamic game-specific theming, color palettes, custom typography, and atmospheric lore for each title.
- **⏱️ Campaign Meetup Calculator**:
  - Choose between **Weekly** (1x, 2x, 3x, 4x per week), **Bi-weekly** (every 2 weeks), **Monthly** (1x to 4x per month), or **Custom intervals** (every N days).
  - Select preferred days of the week (e.g., Fridays & Sundays).
  - Configurable scenarios/games per meetup (1 scenario = ~2 hrs, 2 scenarios = ~4 hrs, marathon = ~6 hrs).
  - Group experience modifiers (Beginner, Regular, Veteran) and player count scaling (1 to 4 players).
  - Buffer periods for vacations, holidays, and scenario failure/retries.
- **📊 Interactive Projections**:
  - Total meetups required.
  - Total table hours (gameplay time vs. setup/teardown breakdown).
  - Projected finish date and total calendar duration in months/weeks.
  - Interactive thematic milestone progression roadmap with estimated completion dates per Act / Month.
- **🌍 Multi-Language Support (i18n)**:
  - Supports **5 languages**: 🇬🇧 English, 🇪🇸 Español, 🇩🇪 Deutsch, 🇫🇷 Français, and 🇮🇹 Italiano.
  - Seamless in-memory reactive language switching with culture-aware date formatting.
  - Browser language auto-detection with `localStorage` persistence.
  - Fully translated UI controls, narrative acts, milestones, lore, and calendar summaries.
- **📅 Calendar Export & Sharing**:
  - Export full schedule as standard **`.ics` iCalendar** file to import directly into Google Calendar, Apple Calendar, or Outlook.
  - Quick "Copy Summary" button for group chat sharing (Discord / WhatsApp).
- **🚀 Serverless & GitHub Pages Ready**:
  - Pure client-side Blazor WebAssembly (.NET 10).
  - Pre-configured GitHub Actions workflow (`.github/workflows/deploy.yml`) for automated builds and deployment to GitHub Pages.
  - Includes `.nojekyll` and `404.html` SPA routing redirect.

---

## 📚 Public Data Sources & Game Reference

All campaign data and completion statistics are curated from public domain sources:

1. **Gloomhaven (Cephalofair Games - Designer: Isaac Childres)**:
   - 95 total scenarios in box.
   - Core Campaign: ~52 scenarios (Standard storyline + side branches).
   - Main Story Speedrun: ~42 scenarios.
   - Extended Campaign: ~68 scenarios.
   - Completionist: ~88 scenarios.
   - Average scenario duration: ~100 mins (+25 mins setup/teardown).
   - Replay/Failure rate modeled from community surveys (~15-20%).

2. **Pandemic Legacy: Season 0 (Z-Man Games - Designers: Matt Leacock & Rob Daviau)**:
   - 12 Campaign months (January – December) + Prologue training.
   - 2 attempts max per month (Early + Late).
   - Expected campaign duration: ~16 games (1 prologue + 12 months + ~3 retries).
   - Best case: 13 games (0 retries). Maximum possible: 26 games (all retries + 2 prologues).
   - Average game duration: ~65 mins (+18 mins setup/debriefing).

---

## 🛠️ Local Development & Running

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Run locally:
```bash
# Clone the repository
git clone https://github.com/marsop/bg-campaign-planner.git
cd bg-campaign-planner

# Run development server
dotnet watch
```
Open your browser at `https://localhost:5001` or `http://localhost:5000`.

### Build & Publish:
```bash
dotnet publish -c Release -o ./publish
```

---

## 🚀 GitHub Pages Deployment

The repository includes an automated GitHub Actions workflow (`.github/workflows/deploy.yml`).

1. Push your code to the `main` or `master` branch.
2. In your GitHub repository settings, go to **Settings ➔ Pages**.
3. Under **Build and deployment ➔ Source**, select **GitHub Actions**.
4. The deployment will automatically publish your app to `https://<username>.github.io/bg-campaign-planner/`.

---

## 📄 License
MIT License.

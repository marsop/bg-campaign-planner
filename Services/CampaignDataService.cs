using System.Collections.Generic;
using System.Linq;
using bg_campaign_planner.Models;

namespace bg_campaign_planner.Services;

public class CampaignDataService
{
    private readonly Dictionary<string, Dictionary<GameId, BoardGame>> _localizedGames = new();

    public CampaignDataService()
    {
        InitializeData();
    }

    public IReadOnlyList<BoardGame> GetAllGames(string lang = "en")
    {
        var dict = GetDictionaryForLang(lang);
        return dict.Values.ToList();
    }

    public BoardGame GetGame(GameId id, string lang = "en")
    {
        var dict = GetDictionaryForLang(lang);
        if (dict.TryGetValue(id, out var game))
        {
            return game;
        }

        // Fallback to English
        return _localizedGames["en"][id];
    }

    private Dictionary<GameId, BoardGame> GetDictionaryForLang(string lang)
    {
        if (string.IsNullOrEmpty(lang)) lang = "en";
        lang = lang.ToLowerInvariant();
        if (_localizedGames.TryGetValue(lang, out var dict))
        {
            return dict;
        }
        return _localizedGames["en"];
    }

    private void InitializeData()
    {
        _localizedGames["en"] = BuildEnglishCatalog();
        _localizedGames["es"] = BuildSpanishCatalog();
        _localizedGames["de"] = BuildGermanCatalog();
        _localizedGames["fr"] = BuildFrenchCatalog();
        _localizedGames["it"] = BuildItalianCatalog();
    }

    private Dictionary<GameId, BoardGame> BuildEnglishCatalog()
    {
        var dict = new Dictionary<GameId, BoardGame>();

        dict[GameId.Gloomhaven] = new BoardGame
        {
            Id = GameId.Gloomhaven,
            Title = "Gloomhaven",
            Subtitle = "Euro-inspired tactical combat in a persistent, evolving world of high fantasy",
            ReleaseYear = 2017,
            Designers = "Isaac Childres",
            PlayersMin = 1,
            PlayersMax = 4,
            BggRating = 8.6,
            BggWeight = 3.90,
            BggUrl = "https://boardgamegeek.com/boardgame/174430/gloomhaven",
            BaseScenarioMinutes = 100,
            SetupTeardownMinutes = 25,
            TotalBoxContentSummary = "95 Scenario campaign book, 17 playable mercenary classes, 47 monster types, 300+ items, evolving city stickers and town records.",
            Theme = new GameTheming
            {
                PrimaryColor = "#e5a93b",
                SecondaryColor = "#1e1b18",
                AccentColor = "#d9534f",
                BackgroundGradient = "radial-gradient(ellipse at top, #2b1f1d 0%, #151111 60%, #0c0a0a 100%)",
                CardBackground = "rgba(28, 22, 20, 0.90)",
                BorderColor = "rgba(229, 169, 59, 0.35)",
                GlowColor = "rgba(229, 169, 59, 0.25)",
                HeaderFont = "'Cinzel', 'Cinzel Decorative', Georgia, serif",
                BodyFont = "'Inter', system-ui, sans-serif",
                Tagline = "Venture into the unforgiving shadows of the Sleeping Lion",
                LoreSummary = "You are wandering mercenaries with your own special set of skills. In the harsh wilderness on the edge of civilization, every choice permanently alters the world around you.",
                BadgeCategory = "Dark Fantasy Tactical RPG"
            },
            KeyFeatures = new List<string>
            {
                "Card-driven deterministic combat (No dice, pure tactical strategy)",
                "Retirement & Class Unlocking (Personal Quests drive new character growth)",
                "Branching Narrative (Town & Road events, world stickers, prosperity tracker)",
                "High difficulty scaling across 1-4 players"
            },
            PublicSourceReferences = new List<string>
            {
                "BoardGameGeek (BGG #3 Overall / #1 Thematic)",
                "Cephalofair Games Official Scenario Book & Rulebook",
                "Community Playthrough Survey Statistics (r/Gloomhaven & BGG Campaign Logs)"
            },
            Scopes = new List<CampaignScopeOption>
            {
                new()
                {
                    Id = "core_campaign",
                    Name = "Standard Core Campaign",
                    Description = "The main storyline arc to the final boss plus key side-quests and 2-3 character retirement chains.",
                    BaseScenarioCount = 52,
                    EstimatedFailRatePercent = 15.0,
                    IsRecommended = true
                },
                new()
                {
                    Id = "story_speedrun",
                    Name = "Main Story Focus (Speedrun)",
                    Description = "Direct path through the primary questlines to the final boss with minimal optional side dungeon diversions.",
                    BaseScenarioCount = 42,
                    EstimatedFailRatePercent = 10.0,
                    IsRecommended = false
                },
                new()
                {
                    Id = "extended_campaign",
                    Name = "Extended Campaign Experience",
                    Description = "Main story, numerous character unlocks, town records progression, and side quest chains.",
                    BaseScenarioCount = 68,
                    EstimatedFailRatePercent = 18.0,
                    IsRecommended = false
                },
                new()
                {
                    Id = "completionist",
                    Name = "Full Completionist (All Accessible)",
                    Description = "Attempting every unlockable scenario, high prosperity, all class retirements, and high-level side dungeons.",
                    BaseScenarioCount = 88,
                    EstimatedFailRatePercent = 20.0,
                    IsRecommended = false
                }
            },
            Milestones = new List<GameMilestone>
            {
                new()
                {
                    Order = 1,
                    Title = "The Black Barrow & Bandit Lair",
                    Phase = "Act I - The Mercenary Contracts",
                    Description = "First dungeon dive, encountering the Bandit Commander and unraveling the Necromancer's plot.",
                    AtScenarioOrGameIndex = 4,
                    BadgeText = "Intro Quest",
                    IconEmoji = "⚔️"
                },
                new()
                {
                    Order = 2,
                    Title = "First Mercenary Retirement",
                    Phase = "Mid Act I - Character Rebirth",
                    Description = "A starting mercenary completes their Personal Goal and retires, opening new class boxes and town prosperity.",
                    AtScenarioOrGameIndex = 14,
                    BadgeText = "Class Unlock",
                    IconEmoji = "📜"
                },
                new()
                {
                    Order = 3,
                    Title = "The Merchant's Secret & Demon Invasions",
                    Phase = "Act II - Branching Factions",
                    Description = "Diving into the elemental planes, dealing with the Rift and choosing alliances with the Voice or Drake.",
                    AtScenarioOrGameIndex = 28,
                    BadgeText = "Major Story Branch",
                    IconEmoji = "🔥"
                },
                new()
                {
                    Order = 4,
                    Title = "The Gloom Rift & Crypts of the Ancient",
                    Phase = "Act III - Endgame Ascent",
                    Description = "Gathering artifacts, reaching Sanctuary of the Great Oak, and pushing into the heart of darkness.",
                    AtScenarioOrGameIndex = 42,
                    BadgeText = "Endgame Prep",
                    IconEmoji = "🔮"
                },
                new()
                {
                    Order = 5,
                    Title = "The Final Boss & Campaign Climax",
                    Phase = "Finale - The Sleeping Lion Legend",
                    Description = "The ultimate confrontation deciding the fate of Gloomhaven and legacy completion.",
                    AtScenarioOrGameIndex = 52,
                    BadgeText = "Grand Finale",
                    IconEmoji = "👑"
                }
            }
        };

        dict[GameId.SleepingGods] = new BoardGame
        {
            Id = GameId.SleepingGods,
            Title = "Sleeping Gods",
            Subtitle = "You are Captain Sofi Odessa and her crew lost at sea in a strange world. Aboard the steamship Manticore, you must work together to survive by exploring mysterious islands, battling creatures, and meeting the world's inhabitants. Along the way, seek out the totems of the gods.",
            Designers = "Ryan Laukat",
            BggUrl = "https://boardgamegeek.com/boardgame/255984/sleeping-gods",
            ImageUrl = "https://cf.geekdo-images.com/nS3N3B75f4h_7vXWzVvTww__imagepage/img/Lh9D_Jk2xS4eG5UqQ2sD3w5n_wM=/fit-in/900x600/filters:no_upscale():strip_icc()/pic4223169.jpg",
            BaseScenarioMinutes = 60,
            SetupTeardownMinutes = 15,
            Theme = new GameTheming(),
            KeyFeatures = new List<string>(),
            PublicSourceReferences = new List<string>(),
            Scopes = new List<CampaignScopeOption>
            {
                new()
                {
                    Id = "sg_standard",
                    Name = "Standard Campaign",
                    Description = "A single full campaign through the Wandering Seas.",
                    BaseScenarioCount = 15, // Approx 15 hours / 1 hour chunks
                    EstimatedFailRatePercent = 0.0,
                    IsRecommended = true
                }
            },
            Milestones = new List<GameMilestone>()
        };


        dict[GameId.Frosthaven] = new BoardGame
        {
            Id = GameId.Frosthaven,
            Title = "Frosthaven",
            Subtitle = "A cooperative, campaign-driven tactical dungeon crawler in a frozen world",
            ReleaseYear = 2022,
            Designers = "Isaac Childres",
            PlayersMin = 1,
            PlayersMax = 4,
            BggRating = 8.9,
            BggWeight = 4.41,
            BggUrl = "https://boardgamegeek.com/boardgame/295770/frosthaven",
            BaseScenarioMinutes = 120,
            SetupTeardownMinutes = 30,
            TotalBoxContentSummary = "Massive campaign book with 138 scenarios, 18 playable classes, dozens of monsters, town building components, and over 1000 cards.",
            ImageUrl = "https://cf.geekdo-images.com/nS3N3B75f4h_7vXWzVvTww__imagepage/img/Lh9D_Jk2xS4eG5UqQ2sD3w5n_wM=/fit-in/900x600/filters:no_upscale():strip_icc()/pic4223169.jpg",
            Theme = new GameTheming
            {
                PrimaryColor = "#4a90e2",
                SecondaryColor = "#1a2a3a",
                AccentColor = "#a0aec0",
                BackgroundGradient = "linear-gradient(135deg, #1a2a3a, #2a4365)",
                CardBackground = "rgba(26, 42, 58, 0.85)",
                BorderColor = "rgba(160, 174, 192, 0.3)",
                GlowColor = "rgba(160, 174, 192, 0.25)",
                HeaderFont = "'Cinzel', serif",
                BodyFont = "'Inter', sans-serif",
                Tagline = "Build and defend your frozen outpost",
                LoreSummary = "You are mercenaries traveling to the frigid North to the small outpost of Frosthaven. You will deal with harsh weather, dangerous enemies, and build up your town over time.",
                BadgeCategory = "Dark Fantasy Tactical RPG"
            },
            KeyFeatures = new List<string>
            {
                "Tactical card-based combat with zero dice (pure strategy)",
                "Town building and resource management mechanics",
                "Retiring and unlocking new character classes",
                "Dynamic scaling difficulty for 1-4 players"
            },
            Scopes = new List<CampaignScopeOption>
            {
                new CampaignScopeOption
                {
                    Id = "core",
                    Name = "Standard Core Campaign",
                    Description = "The main narrative arc through the final boss with key side quests.",
                    BaseScenarioCount = 65,
                    IsRecommended = true
                },
                new CampaignScopeOption
                {
                    Id = "completionist",
                    Name = "Full Completionist",
                    Description = "Completing every unlockable scenario, max prosperity, and all retirements.",
                    BaseScenarioCount = 138,
                    IsRecommended = false
                }
            },
            Milestones = new List<GameMilestone>
            {
                new GameMilestone
                {
                    Order = 1,
                    Title = "Arrival at Frosthaven",
                    Phase = "Act I - The Outpost",
                    Description = "First exploration and establishing the basics of your town.",
                    AtScenarioOrGameIndex = 5,
                    BadgeText = "Initial Quest",
                    IconEmoji = "❄️"
                },
                new GameMilestone
                {
                    Order = 2,
                    Title = "First Mercenary Retirement",
                    Phase = "Mid Act I - Rebirth",
                    Description = "An initial mercenary completes their Personal Quest and retires, unlocking new classes.",
                    AtScenarioOrGameIndex = 15,
                    BadgeText = "Unlock",
                    IconEmoji = "🔓"
                },
                new GameMilestone
                {
                    Order = 3,
                    Title = "The Northern Threat",
                    Phase = "Act II - Factions",
                    Description = "Interacting with the Algox, Lurkers, and Unfettered.",
                    AtScenarioOrGameIndex = 35,
                    BadgeText = "Narrative Branch",
                    IconEmoji = "⚔️"
                },
                new GameMilestone
                {
                    Order = 4,
                    Title = "The Final Confrontation",
                    Phase = "Finale - The Fate of the North",
                    Description = "The ultimate clash that will decide the fate of Frosthaven.",
                    AtScenarioOrGameIndex = 65,
                    BadgeText = "Grand Finale",
                    IconEmoji = "👑"
                }
            }
        };

        dict[GameId.PandemicSeason0] = new BoardGame
        {
            Id = GameId.PandemicSeason0,
            Title = "Pandemic Legacy: Season 0",
            Subtitle = "Cold War 1962 espionage thriller and covert CIA operations against Soviet bioweapons",
            ReleaseYear = 2020,
            Designers = "Matt Leacock, Rob Daviau",
            PlayersMin = 2,
            PlayersMax = 4,
            BggRating = 8.3,
            BggWeight = 3.18,
            BggUrl = "https://boardgamegeek.com/boardgame/314040/pandemic-legacy-season-0",
            BaseScenarioMinutes = 65,
            SetupTeardownMinutes = 18,
            TotalBoxContentSummary = "12 Campaign Month dossiers, customizable spy passports with fake aliases, scratch-off cards, surveillance tokens, locked mystery boxes.",
            Theme = new GameTheming
            {
                PrimaryColor = "#38bdf8",
                SecondaryColor = "#0f172a",
                AccentColor = "#f43f5e",
                BackgroundGradient = "radial-gradient(ellipse at top, #1e293b 0%, #0f172a 60%, #020617 100%)",
                CardBackground = "rgba(15, 23, 42, 0.90)",
                BorderColor = "rgba(56, 189, 248, 0.35)",
                GlowColor = "rgba(56, 189, 248, 0.25)",
                HeaderFont = "'Special Elite', 'Courier New', monospace",
                BodyFont = "'Inter', system-ui, sans-serif",
                Tagline = "TOP SECRET // CLASSIFIED CIA BRIEFING // 1962",
                LoreSummary = "The year is 1962. The Cold War is at its peak. You and your fellow CIA operatives have been recruited to investigate Project MEDUSA, a dangerous Soviet bioweapons program.",
                BadgeCategory = "Espionage Cooperative Campaign"
            },
            KeyFeatures = new List<string>
            {
                "Operative Passports with customizable aliases, disguises, and forged documents",
                "Two chances per month: Win and advance, or fail and attempt 'Late Month'",
                "Scratch-off surveillance maps, covert CIA safehouses, and KGB tracker tokens",
                "High-tension story unfolding across a 12-month calendar (plus Prologues)"
            },
            PublicSourceReferences = new List<string>
            {
                "BoardGameGeek (BGG #107 Overall / #22 Thematic)",
                "Z-Man Games Official Briefing & Debriefing Dossiers",
                "Community Completion Metrics & Win/Loss Averages across 12-month runs"
            },
            Scopes = new List<CampaignScopeOption>
            {
                new()
                {
                    Id = "pandemic_standard",
                    Name = "Standard Expected Campaign (1-2 Prologues + ~4 Retries)",
                    Description = "1 training prologue plus 12 months with typical failure rate (~3-4 months requiring a 2nd attempt). Total 16-17 game plays.",
                    BaseScenarioCount = 16,
                    EstimatedFailRatePercent = 25.0,
                    IsRecommended = true
                },
                new()
                {
                    Id = "pandemic_optimistic",
                    Name = "Clean Operative Run (1 Prologue + 1 Retry)",
                    Description = "High win rate run: 1 prologue and almost all months won on the first attempt (14 total games).",
                    BaseScenarioCount = 14,
                    EstimatedFailRatePercent = 8.0,
                    IsRecommended = false
                },
                new()
                {
                    Id = "pandemic_grind",
                    Name = "Hardcore Surveillance (2 Prologues + 7 Retries)",
                    Description = "Struggling against heavy Soviet interference; several months go to 'Late [Month]' debriefings (21 total games).",
                    BaseScenarioCount = 21,
                    EstimatedFailRatePercent = 40.0,
                    IsRecommended = false
                },
                new()
                {
                    Id = "pandemic_max",
                    Name = "Maximum Legacy Box Capacity (24 Games + 2 Prologues)",
                    Description = "Every single calendar month played twice (Early + Late) plus both prologue missions (26 total games).",
                    BaseScenarioCount = 26,
                    EstimatedFailRatePercent = 50.0,
                    IsRecommended = false
                }
            },
            Milestones = new List<GameMilestone>
            {
                new()
                {
                    Order = 1,
                    Title = "Prologue & Training Debrief",
                    Phase = "Phase 0 - CIA Langley Induction",
                    Description = "Creating operative passports, forging aliases, and mastering undercover surveillance mechanisms.",
                    AtScenarioOrGameIndex = 1,
                    BadgeText = "Induction",
                    IconEmoji = "🕵️"
                },
                new()
                {
                    Order = 2,
                    Title = "Q1: Project MEDUSA Emergence (Jan - Mar)",
                    Phase = "Phase I - The Soviet Threat",
                    Description = "First contact with Soviet saboteurs, establishing allied safehouses across Europe and Asia.",
                    AtScenarioOrGameIndex = 4,
                    BadgeText = "Dossier Unlocked",
                    IconEmoji = "🗂️"
                },
                new()
                {
                    Order = 3,
                    Title = "Q2: Infiltration & The Double Agent (Apr - Jun)",
                    Phase = "Phase II - Covert Infiltration",
                    Description = "High-stakes spy operations, acquiring Soviet lab blueprints, managing compromised aliases.",
                    AtScenarioOrGameIndex = 8,
                    BadgeText = "Midpoint Climax",
                    IconEmoji = "🌐"
                },
                new()
                {
                    Order = 4,
                    Title = "Q3: The Biological Arms Race (Jul - Sep)",
                    Phase = "Phase III - Escalation",
                    Description = "Tracking weaponized pathogen vectors, neutralising KGB teams, and dismantling bio-laboratories.",
                    AtScenarioOrGameIndex = 12,
                    BadgeText = "Crisis Point",
                    IconEmoji = "🧪"
                },
                new()
                {
                    Order = 5,
                    Title = "Q4 & December Climax (Oct - Dec)",
                    Phase = "Finale - Operation Endgame",
                    Description = "The final covert operation deciding whether the Cold War descends into catastrophic biological disaster.",
                    AtScenarioOrGameIndex = 16,
                    BadgeText = "Final Debrief",
                    IconEmoji = "🏆"
                }
            }
        };


        dict[GameId.Oathsworn] = new BoardGame
        {
            Id = GameId.Oathsworn,
            Title = "Oathsworn: Into the Deepwood",
            Subtitle = "Dark fantasy cooperative boss battler and mystery campaign",
            ReleaseYear = 2022,
            Designers = "Jamie Jolly",
            PlayersMin = 1,
            PlayersMax = 4,
            BggRating = 8.7,
            BggWeight = 3.79,
            BggUrl = "https://boardgamegeek.com/boardgame/251661/oathsworn-into-the-deepwood",
            ImageUrl = "https://cf.geekdo-images.com/gK1303HhYx1_E-9Fw0r4Mw__imagepage/img/Khyf_j5Xh9Z1bX-Jp0g5X4w7gQc=/fit-in/900x600/filters:no_upscale():strip_icc()/pic4654922.jpg",
            BaseScenarioMinutes = 120,
            SetupTeardownMinutes = 30,
            TotalBoxContentSummary = "21 Chapters, 15+ Boss encounters, modular game boards, interactive app.",
            Theme = new GameTheming
            {
                PrimaryColor = "#4a5568",
                SecondaryColor = "#1a202c",
                AccentColor = "#a0aec0",
                BackgroundGradient = "radial-gradient(ellipse at top, #2d3748 0%, #1a202c 60%, #000000 100%)",
                CardBackground = "rgba(26, 32, 44, 0.90)",
                BorderColor = "rgba(160, 174, 192, 0.35)",
                GlowColor = "rgba(160, 174, 192, 0.25)",
                HeaderFont = "'Cinzel', 'Cinzel Decorative', Georgia, serif",
                BodyFont = "'Inter', system-ui, sans-serif",
                Tagline = "Venture into the Deepwood and face the horrors within",
                LoreSummary = "In a world consumed by the Deepwood, the last bastion of humanity relies on the Oathsworn, a company of hardened mercenaries, to fight back the encroaching darkness and monstrous entities.",
                BadgeCategory = "Dark Fantasy Boss Battler"
            },
            KeyFeatures = new List<string>
            {
                "Unique combat system with cards or dice",
                "Two-phase gameplay: Story and Encounter",
                "Branching narrative with meaningful choices",
                "Deep character customization and progression"
            },
            PublicSourceReferences = new List<string>
            {
                "BoardGameGeek",
                "Shadowborne Games Official Rulebook"
            },
            Scopes = new List<CampaignScopeOption>
            {
                new()
                {
                    Id = "os_core",
                    Name = "Standard Core Campaign",
                    Description = "The main 21-chapter storyline.",
                    BaseScenarioCount = 21,
                    EstimatedFailRatePercent = 10.0,
                    IsRecommended = true
                },
                new()
                {
                    Id = "os_extended",
                    Name = "Extended Campaign (All Encounters)",
                    Description = "Main story and all optional boss encounters.",
                    BaseScenarioCount = 25,
                    EstimatedFailRatePercent = 15.0,
                    IsRecommended = false
                }
            },
            Milestones = new List<GameMilestone>
            {
                new()
                {
                    Order = 1,
                    Title = "The First Contract",
                    Phase = "Act I - The Deepwood Beckons",
                    Description = "First foray into the Deepwood and initial boss encounter.",
                    AtScenarioOrGameIndex = 1,
                    BadgeText = "Initiation",
                    IconEmoji = "🌲"
                },
                new()
                {
                    Order = 2,
                    Title = "The Broodmother",
                    Phase = "Act I - Infestation",
                    Description = "Confronting the source of the recent attacks.",
                    AtScenarioOrGameIndex = 5,
                    BadgeText = "Swarm Slayer",
                    IconEmoji = "🕷️"
                },
                new()
                {
                    Order = 3,
                    Title = "The Warden's Secret",
                    Phase = "Act II - Deeper Mysteries",
                    Description = "Uncovering the truth about the Deepwood's origins.",
                    AtScenarioOrGameIndex = 11,
                    BadgeText = "Revelation",
                    IconEmoji = "👁️"
                },
                new()
                {
                    Order = 4,
                    Title = "The Final Oath",
                    Phase = "Finale - Heart of Darkness",
                    Description = "The ultimate battle against the primeval horror.",
                    AtScenarioOrGameIndex = 21,
                    BadgeText = "Grand Finale",
                    IconEmoji = "⚔️"
                }
            }
        };

        dict[GameId.Frosthaven] = new BoardGame
        {
            Id = GameId.Frosthaven,
            Title = "Frosthaven",
            Subtitle = "Epic cooperative adventure and settlement building in the frozen north",
            ReleaseYear = 2022,
            Designers = "Isaac Childres",
            PlayersMin = 1,
            PlayersMax = 4,
            BggRating = 8.8,
            BggWeight = 4.40,
            BggUrl = "https://boardgamegeek.com/boardgame/295770/frosthaven",
            BaseScenarioMinutes = 120,
            SetupTeardownMinutes = 30,
            TotalBoxContentSummary = "138 Scenario campaign book, 18 playable classes, extensive crafting and settlement building systems, evolving world.",
            Theme = new GameTheming
            {
                PrimaryColor = "#4fd1c5",
                SecondaryColor = "#1a202c",
                AccentColor = "#90cdf4",
                BackgroundGradient = "radial-gradient(ellipse at top, #2c3e50 0%, #1a252f 60%, #0f171e 100%)",
                CardBackground = "rgba(26, 32, 44, 0.90)",
                BorderColor = "rgba(79, 209, 197, 0.35)",
                GlowColor = "rgba(79, 209, 197, 0.25)",
                HeaderFont = "'Cinzel', 'Cinzel Decorative', Georgia, serif",
                BodyFont = "'Inter', system-ui, sans-serif",
                Tagline = "Brave the freezing wastes and rebuild the northern outpost",
                LoreSummary = "You are a group of mercenaries at the edge of the world, struggling to protect a small outpost from the harsh elements and monstrous threats of the frozen north.",
                BadgeCategory = "Fantasy RPG & City Building"
            },
            KeyFeatures = new List<string>
            {
                "Complex card-driven combat and class progression",
                "Deep settlement building and defense phases",
                "Extensive crafting system and resource management",
                "Sprawling branching narrative with seasonal events"
            },
            PublicSourceReferences = new List<string>
            {
                "BoardGameGeek (Top 100 Overall)",
                "Cephalofair Games Official Frosthaven Rulebook"
            },
            Scopes = new List<CampaignScopeOption>
            {
                new()
                {
                    Id = "fh_core",
                    Name = "Standard Core Campaign",
                    Description = "The main storyline arcs and essential settlement progression.",
                    BaseScenarioCount = 70,
                    EstimatedFailRatePercent = 15.0,
                    IsRecommended = true
                },
                new()
                {
                    Id = "fh_extended",
                    Name = "Extended Campaign Experience",
                    Description = "Main story, significant side quests, and most character unlocks.",
                    BaseScenarioCount = 100,
                    EstimatedFailRatePercent = 18.0,
                    IsRecommended = false
                },
                new()
                {
                    Id = "fh_completionist",
                    Name = "Full Completionist",
                    Description = "Attempting every accessible scenario and maxing out the settlement.",
                    BaseScenarioCount = 138,
                    EstimatedFailRatePercent = 20.0,
                    IsRecommended = false
                }
            },
            Milestones = new List<GameMilestone>
            {
                new()
                {
                    Order = 1,
                    Title = "Arrival at Frosthaven",
                    Phase = "Act I - The Frozen Outpost",
                    Description = "First steps into the northern wastes and initial settlement defense.",
                    AtScenarioOrGameIndex = 5,
                    BadgeText = "Arrival",
                    IconEmoji = "❄️"
                },
                new()
                {
                    Order = 2,
                    Title = "First Winter",
                    Phase = "Act I - Seasonal Shift",
                    Description = "Surviving the first harsh winter and unlocking advanced buildings.",
                    AtScenarioOrGameIndex = 20,
                    BadgeText = "Winter Is Here",
                    IconEmoji = "🌨️"
                },
                new()
                {
                    Order = 3,
                    Title = "The Algox Threat",
                    Phase = "Act II - Escalation",
                    Description = "Delving into the mountains to deal with the Algox tribes.",
                    AtScenarioOrGameIndex = 40,
                    BadgeText = "Major Faction",
                    IconEmoji = "🏔️"
                },
                new()
                {
                    Order = 4,
                    Title = "Secrets of the Lurkers",
                    Phase = "Act III - Deep Dive",
                    Description = "Exploring the depths and confronting ancient mysteries.",
                    AtScenarioOrGameIndex = 60,
                    BadgeText = "Endgame Prep",
                    IconEmoji = "🌊"
                },
                new()
                {
                    Order = 5,
                    Title = "The Final Confrontation",
                    Phase = "Finale - Fate of the North",
                    Description = "The ultimate battle that will decide the future of Frosthaven.",
                    AtScenarioOrGameIndex = 70,
                    BadgeText = "Grand Finale",
                    IconEmoji = "👑"
                }
            }
        };

        dict[GameId.TaintedGrailFallOfAvalon] = new BoardGame
        {
            Id = GameId.TaintedGrailFallOfAvalon,
            Title = "Tainted Grail: The Fall of Avalon",
            Subtitle = "A dark, cooperative survival adventure set in a dying Arthurian world",
            ReleaseYear = 2019,
            Designers = "Krzysztof Piskorski, Marcin Świerkot",
            PlayersMin = 1,
            PlayersMax = 4,
            BggRating = 8.0,
            BggWeight = 3.28,
            BggUrl = "https://boardgamegeek.com/boardgame/264220/tainted-grail-the-fall-of-avalon",
            BaseScenarioMinutes = 120,
            SetupTeardownMinutes = 20,
            TotalBoxContentSummary = "A branching 15-chapter campaign, oversized location cards, detailed miniatures, and a deep narrative exploration journal.",
            Theme = new GameTheming
            {
                PrimaryColor = "#593c7a",
                SecondaryColor = "#1a1a1a",
                AccentColor = "#b2904b",
                BackgroundGradient = "radial-gradient(ellipse at top, #2b1d3d 0%, #151111 60%, #0c0a0a 100%)",
                CardBackground = "rgba(26, 26, 26, 0.90)",
                BorderColor = "rgba(178, 144, 75, 0.35)",
                GlowColor = "rgba(178, 144, 75, 0.25)",
                HeaderFont = "'Cinzel', 'Cinzel Decorative', Georgia, serif",
                BodyFont = "'Inter', system-ui, sans-serif",
                Tagline = "Light the Menhirs and survive the Wyrdness",
                LoreSummary = "The legendary King Arthur is dead, the Menhirs that protect Avalon are fading, and the Wyrdness is consuming the land. You are not the chosen heroes, but you are all that is left.",
                BadgeCategory = "Dark Fantasy Survival RPG"
            },
            KeyFeatures = new List<string>
            {
                "Rich, branching narrative exploration with the Exploration Journal",
                "Resource management and survival elements (Food, Magic, Health, Terror)",
                "Card-driven combat and diplomacy encounters with deck-building",
                "Evolving open-world map using oversized location cards"
            },
            PublicSourceReferences = new List<string>
            {
                "BoardGameGeek (Top 100 Thematic)",
                "Awaken Realms Official Rulebook and Campaign Log"
            },
            ImageUrl = "https://cf.geekdo-images.com/S-K-P0Z81Dk4F_d_s3k80Q__imagepage/img/KjZgYhC0k-rY4aZ5y4H9b-N7L_A=/fit-in/900x600/filters:no_upscale():strip_icc()/pic4473767.jpg",
            Scopes = new List<CampaignScopeOption>
            {
                new()
                {
                    Id = "tg_core",
                    Name = "The Fall of Avalon (Core Campaign)",
                    Description = "The complete main story arc through Avalon to discover the fate of the original heroes.",
                    BaseScenarioCount = 15,
                    EstimatedFailRatePercent = 20.0,
                    IsRecommended = true
                }
            },
            Milestones = new List<GameMilestone>
            {
                new()
                {
                    Order = 1,
                    Title = "Leaving Cuanacht",
                    Phase = "Chapter 1 - The Journey Begins",
                    Description = "Gathering supplies and leaving your dying hometown as the Menhir fades.",
                    AtScenarioOrGameIndex = 1,
                    BadgeText = "First Steps",
                    IconEmoji = "🕯️"
                },
                new()
                {
                    Order = 2,
                    Title = "The Secret of the Menhirs",
                    Phase = "Chapter 4 - Deep in the Wyrdness",
                    Description = "Discovering the rituals required to keep the protective Menhirs lit across Avalon.",
                    AtScenarioOrGameIndex = 4,
                    BadgeText = "Survival",
                    IconEmoji = "🗿"
                },
                new()
                {
                    Order = 3,
                    Title = "The Knights of the Round Table",
                    Phase = "Chapter 8 - Echoes of the Past",
                    Description = "Uncovering the truth about Arthur's knights and their fateful expedition.",
                    AtScenarioOrGameIndex = 8,
                    BadgeText = "Revelation",
                    IconEmoji = "🛡️"
                },
                new()
                {
                    Order = 4,
                    Title = "The Heart of Avalon",
                    Phase = "Chapter 12 - Point of No Return",
                    Description = "Navigating the most dangerous regions of the Wyrdness to reach the core of the mystery.",
                    AtScenarioOrGameIndex = 12,
                    BadgeText = "Endgame Prep",
                    IconEmoji = "⚔️"
                },
                new()
                {
                    Order = 5,
                    Title = "The Fate of the Isle",
                    Phase = "Chapter 15 - The Final Choice",
                    Description = "The ultimate confrontation that determines the future of Avalon and its people.",
                    AtScenarioOrGameIndex = 15,
                    BadgeText = "Grand Finale",
                    IconEmoji = "👑"
                }
            }
        };

        return dict;
    }

    private Dictionary<GameId, BoardGame> BuildSpanishCatalog()
    {
        var dict = BuildEnglishCatalog();

        // Gloomhaven ES
        var gh = dict[GameId.Gloomhaven];
        gh.Subtitle = "Combate táctico de estilo europeo en un mundo persistente y evolutivo de alta fantasía";
        gh.TotalBoxContentSummary = "Libro de campaña con 95 escenarios, 17 clases de mercenarios, 47 tipos de monstruos, más de 300 objetos, pegatinas y registros de la ciudad.";
        gh.Theme.Tagline = "Adéntrate en las implacables sombras del León Durmiente";
        gh.Theme.LoreSummary = "Sois mercenarios errantes con habilidades únicas. En la inhóspita frontera de la civilización, cada decisión altera permanentemente el mundo que os rodea.";
        gh.Theme.BadgeCategory = "RPG Táctico de Fantasía Oscura";
        gh.KeyFeatures = new List<string>
        {
            "Combate táctico con cartas sin dados (estrategia pura)",
            "Retiro y desbloqueo de nuevas clases de personajes",
            "Narrativa ramificada con eventos de ciudad y camino",
            "Dificultad escalable de 1 a 4 jugadores"
        };
        gh.Scopes[0].Name = "Campaña Principal Estándar";
        gh.Scopes[0].Description = "Arco narrativo principal hasta el jefe final más misiones secundarias clave y 2-3 retiros de personajes.";
        gh.Scopes[1].Name = "Enfoque en Historia Principal (Rápida)";
        gh.Scopes[1].Description = "Ruta directa a través de las misiones primarias hasta el jefe final minimizando desvíos secundarios.";
        gh.Scopes[2].Name = "Experiencia de Campaña Extendida";
        gh.Scopes[2].Description = "Historia principal, numerosos desbloqueos de personajes, archivos de la ciudad y cadenas secundarias.";
        gh.Scopes[3].Name = "Completista Total (Todo Accesible)";
        gh.Scopes[3].Description = "Jugar todos los escenarios desbloqueables, máxima prosperidad, todos los retiros y mazmorras avanzadas.";
        gh.Milestones[0].Title = "El Túmulo Negro y Guarida de Bandidos";
        gh.Milestones[0].Phase = "Acto I - Contratos de Mercenarios";
        gh.Milestones[0].Description = "Primera incursión en la mazmorra, enfrentando al Comandante Bandido y descubriendo la trama del Nigromante.";
        gh.Milestones[0].BadgeText = "Misión Inicial";
        gh.Milestones[1].Title = "Primer Retiro de Mercenario";
        gh.Milestones[1].Phase = "Mitad Acto I - Renacimiento";
        gh.Milestones[1].Description = "Un mercenario inicial cumple su Objetivo Personal y se retira, desbloqueando nuevas clases y prosperidad.";
        gh.Milestones[1].BadgeText = "Desbloqueo";
        gh.Milestones[2].Title = "El Secreto del Mercader e Invasión Demoníaca";
        gh.Milestones[2].Phase = "Acto II - Facciones";
        gh.Milestones[2].Description = "Exploración de planos elementales, enfrentando la Grieta y eligiendo alianzas cruciales.";
        gh.Milestones[2].BadgeText = "Ramificación";
        gh.Milestones[3].Title = "La Grieta del Gloom y Criptas Ancestrales";
        gh.Milestones[3].Phase = "Acto III - Ascenso Final";
        gh.Milestones[3].Description = "Reunir artefactos, alcanzar el Santuario del Gran Roble y adentrarse en las sombras.";
        gh.Milestones[3].BadgeText = "Preparación Final";
        gh.Milestones[4].Title = "Jefe Final y Clímax de la Campaña";
        gh.Milestones[4].Phase = "Final - Leyenda del León Durmiente";
        gh.Milestones[4].Description = "El enfrentamiento definitivo que sella el destino de Gloomhaven y completa el legado.";
        gh.Milestones[4].BadgeText = "Gran Final";


        // Frosthaven ES
        var fh = dict[GameId.Frosthaven];
        fh.Subtitle = "Un juego cooperativo de exploración táctica en un mundo helado";
        fh.TotalBoxContentSummary = "Libro de campaña masivo con 138 escenarios, 18 clases jugables, docenas de monstruos, componentes de construcción de la ciudad y más de 1000 cartas.";
        fh.Theme.Tagline = "Construye y defiende tu puesto de avanzada helado";
        fh.Theme.LoreSummary = "Sois mercenarios que viajan al gélido norte hasta el pequeño puesto de avanzada de Frosthaven. Tendréis que lidiar con un clima duro, enemigos peligrosos y desarrollar vuestra ciudad con el tiempo.";
        fh.Theme.BadgeCategory = "RPG Táctico de Fantasía Oscura";
        fh.KeyFeatures = new List<string>
        {
            "Combate táctico con cartas sin dados (pura estrategia)",
            "Mecánicas de construcción de ciudad y gestión de recursos",
            "Retirada y desbloqueo de nuevas clases de personajes",
            "Dificultad adaptable dinámicamente para 1-4 jugadores"
        };
        fh.Scopes[0].Name = "Campaña Base Estándar";
        fh.Scopes[0].Description = "El arco narrativo principal hasta el jefe final con misiones secundarias clave.";
        fh.Scopes[1].Name = "Completista Total";
        fh.Scopes[1].Description = "Completar todos los escenarios desbloqueables, prosperidad máxima y todas las retiradas.";
        fh.Milestones[0].Title = "Llegada a Frosthaven";
        fh.Milestones[0].Phase = "Acto I - El Puesto Avanzado";
        fh.Milestones[0].Description = "Primera exploración y establecimiento de las bases de tu ciudad.";
        fh.Milestones[0].BadgeText = "Misión Inicial";
        fh.Milestones[1].Title = "Primera Retirada de Mercenario";
        fh.Milestones[1].Phase = "Mitad del Acto I - Renacimiento";
        fh.Milestones[1].Description = "Un mercenario inicial completa su Misión Personal y se retira, desbloqueando nuevas clases.";
        fh.Milestones[1].BadgeText = "Desbloqueo";
        fh.Milestones[2].Title = "La Amenaza del Norte";
        fh.Milestones[2].Phase = "Acto II - Facciones";
        fh.Milestones[2].Description = "Interactuando con los Algox, Lurkers y Unfettered.";
        fh.Milestones[2].BadgeText = "Rama Narrativa";
        fh.Milestones[3].Title = "La Confrontación Final";
        fh.Milestones[3].Phase = "Final - El Destino del Norte";
        fh.Milestones[3].Description = "El enfrentamiento definitivo que decidirá el destino de Frosthaven.";
        fh.Milestones[3].BadgeText = "Gran Final";

        // Pandemic Season 0 ES
        var pan = dict[GameId.PandemicSeason0];
        pan.Subtitle = "Thriller de espionaje en plena Guerra Fría (1962) y operaciones encubiertas de la CIA";
        pan.TotalBoxContentSummary = "12 dossiers mensuales de campaña, pasaportes de espías personalizables con identidades falsas, mapas 'rasca', fichas de vigilancia.";
        pan.Theme.Tagline = "ALTO SECRETO // INFORME CLASIFICADO DE LA CIA // 1962";
        pan.Theme.LoreSummary = "Es el año 1962. La Guerra Fría está en su punto álgido. Tú y tu equipo de la CIA debéis investigar el Proyecto MEDUSA, un programa soviético de armas biológicas.";
        pan.Theme.BadgeCategory = "Campaña Cooperativa de Espionaje";
        pan.KeyFeatures = new List<string>
        {
            "Pasaportes de operativos con identidades falsas y disfraces",
            "Dos intentos por mes: victoria o intento tardío ('Late Month')",
            "Mapas de vigilancia para rascar y casas francas de la CIA",
            "Tensión narrativa a lo largo de un calendario de 12 meses"
        };
        pan.Scopes[0].Name = "Campaña Estándar Esperada (1-2 Prólogos + ~4 Reintentos)";
        pan.Scopes[0].Description = "1 prólogo de entrenamiento más 12 meses con tasa típica de reintentos. Total 16-17 partidas.";
        pan.Scopes[1].Name = "Operación Limpia (1 Prólogo + 1 Reintento)";
        pan.Scopes[1].Description = "Alta tasa de victorias: 1 prólogo y casi todos los meses ganados a la primera (14 partidas).";
        pan.Scopes[2].Name = "Vigilancia Extrema (2 Prólogos + 7 Reintentos)";
        pan.Scopes[2].Description = "Gran interferencia soviética; varios meses requieren segundas tentativas (21 partidas).";
        pan.Scopes[3].Name = "Capacidad Máxima de la Caja (24 Partidas + 2 Prólogos)";
        pan.Scopes[3].Description = "Cada mes jugado dos veces (Temprano + Tardío) más ambos prólogos (26 partidas).";
        pan.Milestones[0].Title = "Prólogo y Sesión de Entrenamiento";
        pan.Milestones[0].Phase = "Fase 0 - Inducción CIA";
        pan.Milestones[0].Description = "Creación de pasaportes operativos, alias encubiertos y dominio de mecanismos de vigilancia.";
        pan.Milestones[0].BadgeText = "Inducción";
        pan.Milestones[1].Title = "T1: Aparición del Proyecto MEDUSA (Ene - Mar)";
        pan.Milestones[1].Phase = "Fase I - La Amenaza Soviética";
        pan.Milestones[1].Description = "Primer contacto con saboteadores soviéticos y establecimiento de casas francas en Europa y Asia.";
        pan.Milestones[1].BadgeText = "Dossier";
        pan.Milestones[2].Title = "T2: Infiltración y el Agente Doble (Abr - Jun)";
        pan.Milestones[2].Phase = "Fase II - Infiltración Encubierta";
        pan.Milestones[2].Description = "Operaciones de alto riesgo, planos de laboratorios soviéticos y gestión de identidades comprometidas.";
        pan.Milestones[2].BadgeText = "Clímax Medio";
        pan.Milestones[3].Title = "T3: Carrera Armamentística Biológica (Jul - Sep)";
        pan.Milestones[3].Phase = "Fase III - Escalada";
        pan.Milestones[3].Description = "Rastreo de patógenos, neutralización de equipos del KGB y desmantelamiento de laboratorios.";
        pan.Milestones[3].BadgeText = "Crisis";
        pan.Milestones[4].Title = "T4 y Clímax de Diciembre (Oct - Dic)";
        pan.Milestones[4].Phase = "Final - Operación Endgame";
        pan.Milestones[4].Description = "La operación encubierta final que decidirá el desenlace de la Guerra Fría.";
        pan.Milestones[4].BadgeText = "Informe Final";


        // Oathsworn ES
        var os = dict[GameId.Oathsworn];
        os.Subtitle = "Campaña de misterio y combate cooperativo contra jefes de fantasía oscura";
        os.TotalBoxContentSummary = "21 Capítulos, más de 15 encuentros con jefes, tableros modulares, aplicación interactiva.";
        os.Theme.Tagline = "Adéntrate en Deepwood y enfréntate a los horrores que alberga";
        os.Theme.LoreSummary = "En un mundo consumido por Deepwood, el último bastión de la humanidad confía en los Oathsworn, una compañía de mercenarios curtidos, para luchar contra la oscuridad invasora y las entidades monstruosas.";
        os.Theme.BadgeCategory = "Combate contra Jefes de Fantasía Oscura";
        os.KeyFeatures = new List<string>
        {
            "Sistema de combate único con cartas o dados",
            "Juego en dos fases: Historia y Encuentro",
            "Narrativa ramificada con decisiones significativas",
            "Profunda personalización y progresión de personajes"
        };
        os.Scopes[0].Name = "Campaña Principal Estándar";
        os.Scopes[0].Description = "La historia principal de 21 capítulos.";
        os.Scopes[1].Name = "Campaña Extendida (Todos los Encuentros)";
        os.Scopes[1].Description = "Historia principal y todos los encuentros opcionales con jefes.";
        os.Milestones[0].Title = "El Primer Contrato";
        os.Milestones[0].Phase = "Acto I - Deepwood Llama";
        os.Milestones[0].Description = "Primera incursión en Deepwood y encuentro inicial con un jefe.";
        os.Milestones[0].BadgeText = "Iniciación";
        os.Milestones[1].Title = "La Madre de la Progenie";
        os.Milestones[1].Phase = "Acto I - Infestación";
        os.Milestones[1].Description = "Enfrentando el origen de los recientes ataques.";
        os.Milestones[1].BadgeText = "Matador de Enjambres";
        os.Milestones[2].Title = "El Secreto del Guardián";
        os.Milestones[2].Phase = "Acto II - Misterios Más Profundos";
        os.Milestones[2].Description = "Descubriendo la verdad sobre los orígenes de Deepwood.";
        os.Milestones[2].BadgeText = "Revelación";
        os.Milestones[3].Title = "El Juramento Final";
        os.Milestones[3].Phase = "Final - El Corazón de las Tinieblas";
        os.Milestones[3].Description = "La batalla definitiva contra el horror primigenio.";
        os.Milestones[3].BadgeText = "Gran Final";

        // Frosthaven ES
        var fh = dict[GameId.Frosthaven];
        fh.Subtitle = "Aventura cooperativa épica y construcción de asentamientos en el gélido norte";
        fh.TotalBoxContentSummary = "Libro de campaña con 138 escenarios, 18 clases jugables, extensos sistemas de artesanía y construcción, mundo en evolución.";
        fh.Theme.Tagline = "Desafía los páramos helados y reconstruye el puesto fronterizo";
        fh.Theme.LoreSummary = "Sois un grupo de mercenarios en el fin del mundo, luchando para proteger un pequeño asentamiento de los elementos y las bestias del norte helado.";
        fh.Theme.BadgeCategory = "RPG de Fantasía y Construcción de Ciudades";
        fh.KeyFeatures = new List<string>
        {
            "Combate complejo con cartas y progresión de clases",
            "Profunda construcción y defensa del asentamiento",
            "Extenso sistema de artesanía y gestión de recursos",
            "Narrativa ramificada con eventos estacionales"
        };
        fh.Scopes[0].Name = "Campaña Principal Estándar";
        fh.Scopes[0].Description = "Arcos argumentales principales y progresión esencial del asentamiento.";
        fh.Scopes[1].Name = "Experiencia de Campaña Extendida";
        fh.Scopes[1].Description = "Historia principal, misiones secundarias importantes y mayoría de desbloqueos.";
        fh.Scopes[2].Name = "Completista Total";
        fh.Scopes[2].Description = "Jugar todos los escenarios accesibles y mejorar al máximo el asentamiento.";
        fh.Milestones[0].Title = "Llegada a Frosthaven";
        fh.Milestones[0].Phase = "Acto I - El Puesto Helado";
        fh.Milestones[0].Description = "Primeros pasos en los páramos y defensa inicial del asentamiento.";
        fh.Milestones[0].BadgeText = "Llegada";
        fh.Milestones[1].Title = "Primer Invierno";
        fh.Milestones[1].Phase = "Acto I - Cambio Estacional";
        fh.Milestones[1].Description = "Sobrevivir al primer invierno duro y desbloquear edificios avanzados.";
        fh.Milestones[1].BadgeText = "Llega el Invierno";
        fh.Milestones[2].Title = "La Amenaza Algox";
        fh.Milestones[2].Phase = "Acto II - Escalada";
        fh.Milestones[2].Description = "Adentrarse en las montañas para lidiar con las tribus Algox.";
        fh.Milestones[2].BadgeText = "Facción Principal";
        fh.Milestones[3].Title = "Secretos de los Acechadores";
        fh.Milestones[3].Phase = "Acto III - Inmersión Profunda";
        fh.Milestones[3].Description = "Explorar las profundidades y confrontar misterios antiguos.";
        fh.Milestones[3].BadgeText = "Prep. Final";
        fh.Milestones[4].Title = "La Confrontación Final";
        fh.Milestones[4].Phase = "Final - El Destino del Norte";
        fh.Milestones[4].Description = "La batalla definitiva que decidirá el futuro de Frosthaven.";
        fh.Milestones[4].BadgeText = "Gran Final";


        // Tainted Grail ES
        var tg = dict[GameId.TaintedGrailFallOfAvalon];
        tg.Subtitle = "Una oscura aventura cooperativa de supervivencia en un mundo artúrico agonizante";
        tg.TotalBoxContentSummary = "Campaña ramificada de 15 capítulos, cartas de localización gigantes, miniaturas detalladas y un profundo diario de exploración.";
        tg.Theme.Tagline = "Enciende los Menhires y sobrevive a la Rareza";
        tg.Theme.LoreSummary = "El legendario Rey Arturo ha muerto, los Menhires que protegen Avalon se desvanecen y la Rareza está consumiendo la tierra. No sois los héroes elegidos, pero sois todo lo que queda.";
        tg.Theme.BadgeCategory = "RPG de Supervivencia y Fantasía Oscura";
        tg.KeyFeatures = new List<string>
        {
            "Rica exploración narrativa ramificada con el Diario de Exploración",
            "Gestión de recursos y elementos de supervivencia (Comida, Magia, Salud, Terror)",
            "Combate con cartas y encuentros de diplomacia con construcción de mazos",
            "Mapa de mundo abierto en evolución con cartas de localización gigantes"
        };
        tg.Scopes[0].Name = "La Caída de Avalon (Campaña Principal)";
        tg.Scopes[0].Description = "El arco argumental completo a través de Avalon para descubrir el destino de los héroes originales.";
        tg.Milestones[0].Title = "Saliendo de Cuanacht";
        tg.Milestones[0].Phase = "Capítulo 1 - El Viaje Comienza";
        tg.Milestones[0].Description = "Reuniendo suministros y dejando atrás tu pueblo agonizante mientras el Menhir se apaga.";
        tg.Milestones[0].BadgeText = "Primeros Pasos";
        tg.Milestones[1].Title = "El Secreto de los Menhires";
        tg.Milestones[1].Phase = "Capítulo 4 - En lo Profundo de la Rareza";
        tg.Milestones[1].Description = "Descubriendo los rituales necesarios para mantener encendidos los Menhires protectores por todo Avalon.";
        tg.Milestones[1].BadgeText = "Supervivencia";
        tg.Milestones[2].Title = "Los Caballeros de la Mesa Redonda";
        tg.Milestones[2].Phase = "Capítulo 8 - Ecos del Pasado";
        tg.Milestones[2].Description = "Descubriendo la verdad sobre los caballeros de Arturo y su fatídica expedición.";
        tg.Milestones[2].BadgeText = "Revelación";
        tg.Milestones[3].Title = "El Corazón de Avalon";
        tg.Milestones[3].Phase = "Capítulo 12 - Punto de No Retorno";
        tg.Milestones[3].Description = "Navegando por las regiones más peligrosas de la Rareza para alcanzar el núcleo del misterio.";
        tg.Milestones[3].BadgeText = "Prep. Final";
        tg.Milestones[4].Title = "El Destino de la Isla";
        tg.Milestones[4].Phase = "Capítulo 15 - La Elección Final";
        tg.Milestones[4].Description = "El enfrentamiento definitivo que determina el futuro de Avalon y su gente.";
        tg.Milestones[4].BadgeText = "Gran Final";

        return dict;
    }

    private Dictionary<GameId, BoardGame> BuildGermanCatalog()
    {
        var dict = BuildEnglishCatalog();

        // Gloomhaven DE
        var gh = dict[GameId.Gloomhaven];
        gh.Subtitle = "Euro-inspiriertes taktisches Kampfsystem in einer persistenten, sich entwickelnden Fantasy-Welt";
        gh.TotalBoxContentSummary = "Kampagnenbuch mit 95 Szenarien, 17 spielbare Söldnerklassen, 47 Monstertypen, 300+ Gegenstände, Stadt-Sticker und Stadtchronik.";
        gh.Theme.Tagline = "Wage dich in die unbarmherzigen Schatten des Schlafenden Löwen";
        gh.Theme.LoreSummary = "Ihr seid wandernde Söldner mit besonderen Talenten. In der rauen Wildnis am Rande der Zivilisation verändert jede Entscheidung die Welt dauerhaft.";
        gh.Theme.BadgeCategory = "Dark-Fantasy Taktik-RPG";
        gh.KeyFeatures = new List<string>
        {
            "Kartengesteuertes deterministisches Kampfsystem (keine Würfel)",
            "Ruhestand & Klassenfreischaltungen durch persönliche Ziele",
            "Verzweigte Handlung mit Stadt- und Wegereignissen",
            "Hohe Schwierigkeitsskalierung für 1-4 Spieler"
        };
        gh.Scopes[0].Name = "Standard-Hauptkampagne";
        gh.Scopes[0].Description = "Der Haupthandlungsbogen bis zum Endboss plus wichtige Nebenquests und 2-3 Charakter-Ruhestandsreihen.";
        gh.Scopes[1].Name = "Hauptstory-Fokus (Speedrun)";
        gh.Scopes[1].Description = "Direkter Weg durch die Hauptquests zum Endboss mit minimalen optionalen Abstechern.";
        gh.Scopes[2].Name = "Erweiterte Kampagnen-Erfahrung";
        gh.Scopes[2].Description = "Hauptgeschichte, zahlreiche Charakter-Freischaltungen, Fortschritt der Stadtchronik und Questreihen.";
        gh.Scopes[3].Name = "Vollständiger Komplettierer (Alles)";
        gh.Scopes[3].Description = "Absolvieren aller erreichbaren Szenarien, höchste Wohlstandsstufe, alle Ruhestände und High-Level-Dungeons.";
        gh.Milestones[0].Title = "Das Schwarze Hünengrab & Räuberversteck";
        gh.Milestones[0].Phase = "Akt I - Die Söldneraufträge";
        gh.Milestones[0].Description = "Erster Dungeon-Vorstoß, Konfrontation mit dem Räuberhauptmann und Aufdeckung des Nekromanten-Komplotts.";
        gh.Milestones[0].BadgeText = "Einführungsquest";
        gh.Milestones[1].Title = "Erster Söldner-Ruhestand";
        gh.Milestones[1].Phase = "Mitte Akt I - Charakterwandel";
        gh.Milestones[1].Description = "Ein Startsöldner erfüllt sein persönliches Ziel und geht in den Ruhestand; schaltet neue Klassen frei.";
        gh.Milestones[1].BadgeText = "Klassenfreischaltung";
        gh.Milestones[2].Title = "Das Geheimnis des Händlers & Dämoneninvasion";
        gh.Milestones[2].Phase = "Akt II - Fraktionsspaltung";
        gh.Milestones[2].Description = "Vorstoß in die Elementarebenen, Konfrontation mit dem Riss und Wahl der Allianzen.";
        gh.Milestones[2].BadgeText = "Story-Verzweigung";
        gh.Milestones[3].Title = "Der Schattenriss & Gruften der Uralten";
        gh.Milestones[3].Phase = "Akt III - Vorbereitung Finale";
        gh.Milestones[3].Description = "Artefakte sammeln, Zuflucht der Großen Eiche erreichen und ins Herz der Finsternis vorstoßen.";
        gh.Milestones[3].BadgeText = "Endspiel-Vorbereitung";
        gh.Milestones[4].Title = "Der Endboss & Kampagnen-Höhepunkt";
        gh.Milestones[4].Phase = "Finale - Legende des Schlafenden Löwen";
        gh.Milestones[4].Description = "Die ultimative Konfrontation, die über das Schicksal von Gloomhaven entscheidet.";
        gh.Milestones[4].BadgeText = "Großes Finale";


        // Frosthaven DE
        var fh = dict[GameId.Frosthaven];
        fh.Subtitle = "Ein kooperatives, kampagnengesteuertes taktisches Dungeon-Crawler-Spiel in einer eisigen Welt";
        fh.TotalBoxContentSummary = "Massives Kampagnenbuch mit 138 Szenarien, 18 spielbaren Klassen, dutzenden Monstern, Stadtbau-Komponenten und über 1000 Karten.";
        fh.Theme.Tagline = "Baue und verteidige deinen eisigen Außenposten";
        fh.Theme.LoreSummary = "Ihr seid Söldner, die in den eisigen Norden zum kleinen Außenposten Frosthaven reisen. Ihr müsst mit rauem Wetter und gefährlichen Feinden umgehen und eure Stadt mit der Zeit aufbauen.";
        fh.Theme.BadgeCategory = "Dark Fantasy Taktik-RPG";
        fh.KeyFeatures = new List<string>
        {
            "Taktischer kartenbasierter Kampf ohne Würfel (reine Strategie)",
            "Stadtbau- und Ressourcenmanagement-Mechaniken",
            "Ruhestand und Freischalten neuer Charakterklassen",
            "Dynamisch skalierbare Schwierigkeit für 1-4 Spieler"
        };
        fh.Scopes[0].Name = "Standard-Kernkampagne";
        fh.Scopes[0].Description = "Der Hauptgeschichtsbogen bis zum Endboss mit wichtigen Nebenquests.";
        fh.Scopes[1].Name = "Vollständiger Komplettist";
        fh.Scopes[1].Description = "Abschluss jedes freischaltbaren Szenarios, maximale Wohlstand und alle Ruhestand.";
        fh.Milestones[0].Title = "Ankunft in Frosthaven";
        fh.Milestones[0].Phase = "Akt I - Der Außenposten";
        fh.Milestones[0].Description = "Erste Erkundung und Aufbau der Grundlagen deiner Stadt.";
        fh.Milestones[0].BadgeText = "Startquest";
        fh.Milestones[1].Title = "Erster Söldner-Ruhestand";
        fh.Milestones[1].Phase = "Mitte Akt I - Wiedergeburt";
        fh.Milestones[1].Description = "Ein Anfangssöldner schließt seine persönliche Quest ab und geht in den Ruhestand, wodurch neue Klassen freigeschaltet werden.";
        fh.Milestones[1].BadgeText = "Freischaltung";
        fh.Milestones[2].Title = "Die nördliche Bedrohung";
        fh.Milestones[2].Phase = "Akt II - Fraktionen";
        fh.Milestones[2].Description = "Interaktion mit den Algox, Lurkers und Unfettered.";
        fh.Milestones[2].BadgeText = "Narrativer Zweig";
        fh.Milestones[3].Title = "Die finale Konfrontation";
        fh.Milestones[3].Phase = "Finale - Das Schicksal des Nordens";
        fh.Milestones[3].Description = "Der ultimative Zusammenstoß, der das Schicksal von Frosthaven entscheiden wird.";
        fh.Milestones[3].BadgeText = "Großes Finale";

        // Pandemic Season 0 DE
        var pan = dict[GameId.PandemicSeason0];
        pan.Subtitle = "Spionage-Thriller zur Zeit des Kalten Krieges (1962) und geheime CIA-Operationen";
        pan.TotalBoxContentSummary = "12 Kampagnen-Monatsdossiers, anpassbare Spionagepässe mit Tarnidentitäten, Rubbelkarten, Überwachungsmarker.";
        pan.Theme.Tagline = "STRENG GEHEIM // CIA-BRIEFING // 1962";
        pan.Theme.LoreSummary = "Wir schreiben das Jahr 1962. Der Kalte Krieg erreicht seinen Höhepunkt. Ihr untersucht Projekt MEDUSA, ein sowjetisches Biowaffenprogramm.";
        pan.Theme.BadgeCategory = "Kooperative Spionage-Kampagne";
        pan.KeyFeatures = new List<string>
        {
            "Agentenpässe mit Tarnungen und gefälschten Dokumenten",
            "Zwei Chancen pro Monat: Gewinnen oder Spätmonat versuchen",
            "Rubbel-Überwachungskarten und geheime CIA-Unterschlüpfe",
            "Spannungsgeladene Story über einen 12-monatigen Kalender"
        };
        pan.Scopes[0].Name = "Standard-Kampagne (1-2 Prologe + ~4 Wiederholungen)";
        pan.Scopes[0].Description = "1 Trainingsprolog plus 12 Monate mit typischer Wiederholungsrate. Insgesamt 16-17 Partien.";
        pan.Scopes[1].Name = "Optimale Agenten-Mission (1 Prolog + 1 Wiederholung)";
        pan.Scopes[1].Description = "Hohe Erfolgsquote: 1 Prolog und fast alle Monate im ersten Anlauf gewonnen (14 Partien).";
        pan.Scopes[2].Name = "Intensive Überwachung (2 Prologe + 7 Wiederholungen)";
        pan.Scopes[2].Description = "Starke sowjetische Einmischung; mehrere Monate erfordern Nachbesprechungen (21 Partien).";
        pan.Scopes[3].Name = "Maximale Schachtelkapazität (24 Partien + 2 Prologe)";
        pan.Scopes[3].Description = "Jeder Kalendermonat doppelt gespielt (Früh + Spät) plus beide Prologe (26 Partien).";
        pan.Milestones[0].Title = "Prolog & Trainings-Debriefing";
        pan.Milestones[0].Phase = "Phase 0 - CIA-Einführung";
        pan.Milestones[0].Description = "Erstellung von Agentenpässen, Tarnidentitäten und Beherrschung der Überwachung.";
        pan.Milestones[0].BadgeText = "Einführung";
        pan.Milestones[1].Title = "Q1: Projekt MEDUSA taucht auf (Jan - Mär)";
        pan.Milestones[1].Phase = "Phase I - Die sowjetische Bedrohung";
        pan.Milestones[1].Description = "Erster Kontakt mit Saboteuren, Errichtung von Unterschlüpfen in Europa und Asien.";
        pan.Milestones[1].BadgeText = "Dossier";
        pan.Milestones[2].Title = "Q2: Infiltration & der Doppelagent (Apr - Jun)";
        pan.Milestones[2].Phase = "Phase II - Verdeckte Operation";
        pan.Milestones[2].Description = "Gefährliche Spionageakte, sowjetische Laborpläne und kompromittierte Identitäten.";
        pan.Milestones[2].BadgeText = "Halbzeit";
        pan.Milestones[3].Title = "Q3: Das biologische Wettrüsten (Jul - Sep)";
        pan.Milestones[3].Phase = "Phase III - Eskalation";
        pan.Milestones[3].Description = "Verfolgung von Erregern, Neutralisierung von KGB-Teams und Zerschlagung von Laboren.";
        pan.Milestones[3].BadgeText = "Krisenpunkt";
        pan.Milestones[4].Title = "Q4 & Dezember-Höhepunkt (Okt - Dez)";
        pan.Milestones[4].Phase = "Finale - Operation Endspiel";
        pan.Milestones[4].Description = "Die finale Geheimoperation, die über das Schicksal des Kalten Krieges entscheidet.";
        pan.Milestones[4].BadgeText = "Abschlussbericht";


        // Oathsworn DE
        var os = dict[GameId.Oathsworn];
        os.Subtitle = "Dark Fantasy kooperativer Boss-Battler und Mystery-Kampagne";
        os.TotalBoxContentSummary = "21 Kapitel, über 15 Boss-Begegnungen, modulare Spielpläne, interaktive App.";
        os.Theme.Tagline = "Wage dich in den Deepwood und stelle dich den Schrecken darin";
        os.Theme.LoreSummary = "In einer vom Deepwood verschlungenen Welt verlässt sich die letzte Bastion der Menschheit auf die Oathsworn, eine Kompanie abgehärteter Söldner, um die vordringende Dunkelheit und monströsen Wesen zu bekämpfen.";
        os.Theme.BadgeCategory = "Dark Fantasy Boss-Battler";
        os.KeyFeatures = new List<string>
        {
            "Einzigartiges Kampfsystem mit Karten oder Würfeln",
            "Zweiphasiges Gameplay: Geschichte und Begegnung",
            "Verzweigte Handlung mit bedeutungsvollen Entscheidungen",
            "Tiefe Charakteranpassung und Progression"
        };
        os.Scopes[0].Name = "Standard-Hauptkampagne";
        os.Scopes[0].Description = "Die 21 Kapitel umfassende Hauptgeschichte.";
        os.Scopes[1].Name = "Erweiterte Kampagne (Alle Begegnungen)";
        os.Scopes[1].Description = "Hauptgeschichte und alle optionalen Boss-Begegnungen.";
        os.Milestones[0].Title = "Der Erste Vertrag";
        os.Milestones[0].Phase = "Akt I - Der Deepwood ruft";
        os.Milestones[0].Description = "Erster Vorstoß in den Deepwood und anfängliche Boss-Begegnung.";
        os.Milestones[0].BadgeText = "Initiation";
        os.Milestones[1].Title = "Die Brutmutter";
        os.Milestones[1].Phase = "Akt I - Befall";
        os.Milestones[1].Description = "Konfrontation mit dem Ursprung der jüngsten Angriffe.";
        os.Milestones[1].BadgeText = "Schwarmtöter";
        os.Milestones[2].Title = "Das Geheimnis des Wächters";
        os.Milestones[2].Phase = "Akt II - Tiefere Mysterien";
        os.Milestones[2].Description = "Aufdeckung der Wahrheit über die Ursprünge des Deepwood.";
        os.Milestones[2].BadgeText = "Offenbarung";
        os.Milestones[3].Title = "Der Letzte Eid";
        os.Milestones[3].Phase = "Finale - Herz der Finsternis";
        os.Milestones[3].Description = "Die ultimative Schlacht gegen den urzeitlichen Schrecken.";
        os.Milestones[3].BadgeText = "Großes Finale";

        // Frosthaven DE
        var fh = dict[GameId.Frosthaven];
        fh.Subtitle = "Episches kooperatives Abenteuer und Siedlungsbau im eisigen Norden";
        fh.TotalBoxContentSummary = "Kampagnenbuch mit 138 Szenarien, 18 spielbare Klassen, umfangreiches Crafting- und Siedlungssystem, sich entwickelnde Welt.";
        fh.Theme.Tagline = "Trotze der eisigen Ödnis und baue den nördlichen Außenposten wieder auf";
        fh.Theme.LoreSummary = "Ihr seid Söldner am Ende der Welt, die kämpfen, um einen kleinen Außenposten vor den harten Elementen und monströsen Bedrohungen zu schützen.";
        fh.Theme.BadgeCategory = "Fantasy-RPG & Städtebau";
        fh.KeyFeatures = new List<string>
        {
            "Komplexes kartengesteuertes Kampfsystem und Klassenprogression",
            "Tiefgehender Siedlungsbau und Verteidigungsphasen",
            "Umfangreiches Crafting-System und Ressourcenmanagement",
            "Verzweigte Handlung mit saisonalen Ereignissen"
        };
        fh.Scopes[0].Name = "Standard-Hauptkampagne";
        fh.Scopes[0].Description = "Die Hauptstory-Bögen und grundlegender Siedlungsfortschritt.";
        fh.Scopes[1].Name = "Erweiterte Kampagnen-Erfahrung";
        fh.Scopes[1].Description = "Hauptgeschichte, wichtige Nebenquests und meiste Freischaltungen.";
        fh.Scopes[2].Name = "Vollständiger Komplettierer";
        fh.Scopes[2].Description = "Absolvieren aller erreichbaren Szenarien und maximaler Ausbau der Siedlung.";
        fh.Milestones[0].Title = "Ankunft in Frosthaven";
        fh.Milestones[0].Phase = "Akt I - Der eisige Außenposten";
        fh.Milestones[0].Description = "Erste Schritte in der Ödnis und anfängliche Siedlungsverteidigung.";
        fh.Milestones[0].BadgeText = "Ankunft";
        fh.Milestones[1].Title = "Erster Winter";
        fh.Milestones[1].Phase = "Akt I - Saisonwechsel";
        fh.Milestones[1].Description = "Überleben des ersten harten Winters und Freischaltung erweiterter Gebäude.";
        fh.Milestones[1].BadgeText = "Der Winter naht";
        fh.Milestones[2].Title = "Die Algox-Bedrohung";
        fh.Milestones[2].Phase = "Akt II - Eskalation";
        fh.Milestones[2].Description = "Vorstoß in die Berge, um mit den Algox-Stämmen fertig zu werden.";
        fh.Milestones[2].BadgeText = "Hauptfraktion";
        fh.Milestones[3].Title = "Geheimnisse der Lauerer";
        fh.Milestones[3].Phase = "Akt III - Tiefer Vorstoß";
        fh.Milestones[3].Description = "Erkundung der Tiefen und Konfrontation mit alten Geheimnissen.";
        fh.Milestones[3].BadgeText = "Endspiel-Vorbereitung";
        fh.Milestones[4].Title = "Die finale Konfrontation";
        fh.Milestones[4].Phase = "Finale - Schicksal des Nordens";
        fh.Milestones[4].Description = "Der letzte Kampf, der über die Zukunft von Frosthaven entscheidet.";
        fh.Milestones[4].BadgeText = "Großes Finale";


        // Tainted Grail DE
        var tg = dict[GameId.TaintedGrailFallOfAvalon];
        tg.Subtitle = "Ein düsteres, kooperatives Survival-Abenteuer in einer sterbenden Artus-Welt";
        tg.TotalBoxContentSummary = "Eine verzweigte Kampagne mit 15 Kapiteln, übergroße Ortskarten, detaillierte Miniaturen und ein tiefgründiges Erkundungstagebuch.";
        tg.Theme.Tagline = "Entzünde die Menhire und überlebe die Wyrdnis";
        tg.Theme.LoreSummary = "Der legendäre König Artus ist tot, die Menhire, die Avalon schützen, verblassen und die Wyrdnis verschlingt das Land. Ihr seid nicht die auserwählten Helden, aber ihr seid alles, was noch übrig ist.";
        tg.Theme.BadgeCategory = "Dark Fantasy Survival RPG";
        tg.KeyFeatures = new List<string>
        {
            "Reichhaltige, verzweigte Erkundung der Geschichte mit dem Erkundungstagebuch",
            "Ressourcenmanagement und Überlebenselemente (Nahrung, Magie, Gesundheit, Terror)",
            "Kartengesteuerte Kampf- und Diplomatiebegegnungen mit Deckbau",
            "Sich entwickelnde offene Weltkarte mit übergroßen Ortskarten"
        };
        tg.Scopes[0].Name = "Der Niedergang Avalons (Hauptkampagne)";
        tg.Scopes[0].Description = "Der gesamte Hauptstory-Bogen durch Avalon, um das Schicksal der ursprünglichen Helden zu entdecken.";
        tg.Milestones[0].Title = "Abschied von Cuanacht";
        tg.Milestones[0].Phase = "Kapitel 1 - Die Reise beginnt";
        tg.Milestones[0].Description = "Vorräte sammeln und das sterbende Heimatdorf verlassen, während der Menhir verblasst.";
        tg.Milestones[0].BadgeText = "Erste Schritte";
        tg.Milestones[1].Title = "Das Geheimnis der Menhire";
        tg.Milestones[1].Phase = "Kapitel 4 - Tief in der Wyrdnis";
        tg.Milestones[1].Description = "Entdeckung der Rituale, die notwendig sind, um die schützenden Menhire in ganz Avalon am Leuchten zu halten.";
        tg.Milestones[1].BadgeText = "Überleben";
        tg.Milestones[2].Title = "Die Ritter der Tafelrunde";
        tg.Milestones[2].Phase = "Kapitel 8 - Echos der Vergangenheit";
        tg.Milestones[2].Description = "Die Wahrheit über Artus' Ritter und ihre schicksalhafte Expedition ans Licht bringen.";
        tg.Milestones[2].BadgeText = "Offenbarung";
        tg.Milestones[3].Title = "Das Herz von Avalon";
        tg.Milestones[3].Phase = "Kapitel 12 - Punkt ohne Wiederkehr";
        tg.Milestones[3].Description = "Navigation durch die gefährlichsten Regionen der Wyrdnis, um den Kern des Mysteriums zu erreichen.";
        tg.Milestones[3].BadgeText = "Endspiel-Vorb.";
        tg.Milestones[4].Title = "Das Schicksal der Insel";
        tg.Milestones[4].Phase = "Kapitel 15 - Die finale Entscheidung";
        tg.Milestones[4].Description = "Die ultimative Konfrontation, die die Zukunft von Avalon und seiner Bewohner bestimmt.";
        tg.Milestones[4].BadgeText = "Großes Finale";

        return dict;
    }

    private Dictionary<GameId, BoardGame> BuildFrenchCatalog()
    {
        var dict = BuildEnglishCatalog();

        // Gloomhaven FR
        var gh = dict[GameId.Gloomhaven];
        gh.Subtitle = "Combat tactique d'inspiration européenne dans un monde persistant et évolutif de dark fantasy";
        gh.TotalBoxContentSummary = "Livre de campagne de 95 scénarios, 17 classes de mercenaires, 47 types de monstres, plus de 300 objets, autocollants et archives de la ville.";
        gh.Theme.Tagline = "Aventurez-vous dans les ombres impitoyables du Lion Assoupi";
        gh.Theme.LoreSummary = "Vous êtes des mercenaires itinérants aux compétences uniques. Dans les terres sauvages aux confins de la civilisation, chaque choix transforme le monde.";
        gh.Theme.BadgeCategory = "RPG Tactique Dark Fantasy";
        gh.KeyFeatures = new List<string>
        {
            "Combat tactique basé sur les cartes sans dés (stratégie pure)",
            "Départs à la retraite et déblocage de nouvelles classes",
            "Scénario à embranchements avec événements de ville et de route",
            "Équilibrage de difficulté dynamique de 1 à 4 joueurs"
        };
        gh.Scopes[0].Name = "Campagne Principale Standard";
        gh.Scopes[0].Description = "L'arc narratif principal jusqu'au boss final avec les quêtes secondaires clés et 2-3 départs à la retraite.";
        gh.Scopes[1].Name = "Focus Histoire Principale (Speedrun)";
        gh.Scopes[1].Description = "Chemin direct à travers les quêtes majeures jusqu'au boss final avec un minimum de détours.";
        gh.Scopes[2].Name = "Expérience de Campagne Étendue";
        gh.Scopes[2].Description = "Histoire principale, nombreux déblocages de classes, archives de la cité et séries de quêtes.";
        gh.Scopes[3].Name = "Complétiste Intégral (Tout Accessible)";
        gh.Scopes[3].Description = "Tous les scénarios déblocables, prospérité maximale, retraites de toutes les classes et donjons ultimes.";
        gh.Milestones[0].Title = "Le Tertre Noir & Repaire des Bandits";
        gh.Milestones[0].Phase = "Acte I - Les Contrats";
        gh.Milestones[0].Description = "Première incursion en donjon, affrontement du Chef des Bandits et découverte du complot.";
        gh.Milestones[0].BadgeText = "Quête Initiale";
        gh.Milestones[1].Title = "Première Retraite de Mercenaire";
        gh.Milestones[1].Phase = "Mi-Acte I - Renaissance";
        gh.Milestones[1].Description = "Un mercenaire de départ accomplit sa quête personnelle et prend sa retraite, débloquant une nouvelle classe.";
        gh.Milestones[1].BadgeText = "Déblocage";
        gh.Milestones[2].Title = "Le Secret du Marchand & Invasions Démoniaques";
        gh.Milestones[2].Phase = "Acte II - Factions";
        gh.Milestones[2].Description = "Plongée dans les plans élémentaires, gestion de la Faille et alliances cruciales.";
        gh.Milestones[2].BadgeText = "Embranchement";
        gh.Milestones[3].Title = "La Faille du Gloom & Cryptes des Anciens";
        gh.Milestones[3].Phase = "Acte III - Ascension Finale";
        gh.Milestones[3].Description = "Collecte d'artefacts, sanctuaire du Grand Chêne et progression au cœur des ténèbres.";
        gh.Milestones[3].BadgeText = "Préparation Finale";
        gh.Milestones[4].Title = "Le Boss Final & Climax de la Campagne";
        gh.Milestones[4].Phase = "Final - La Légende du Lion Assoupi";
        gh.Milestones[4].Description = "La confrontation ultime qui scelle le destin de Havrenuit et parachève la campagne.";
        gh.Milestones[4].BadgeText = "Grand Final";


        // Frosthaven FR
        var fh = dict[GameId.Frosthaven];
        fh.Subtitle = "Un jeu d'exploration tactique coopératif basé sur une campagne dans un monde glacial";
        fh.TotalBoxContentSummary = "Livre de campagne massif avec 138 scénarios, 18 classes jouables, des dizaines de monstres, des composants de construction de ville et plus de 1000 cartes.";
        fh.Theme.Tagline = "Construisez et défendez votre avant-poste glacial";
        fh.Theme.LoreSummary = "Vous êtes des mercenaires voyageant vers le nord glacial jusqu'au petit avant-poste de Frosthaven. Vous devrez faire face à des conditions météorologiques difficiles, à des ennemis dangereux et développer votre ville au fil du temps.";
        fh.Theme.BadgeCategory = "RPG Tactique Dark Fantasy";
        fh.KeyFeatures = new List<string>
        {
            "Combat tactique par cartes sans dés (pure stratégie)",
            "Mécaniques de construction de ville et de gestion des ressources",
            "Retraite et déblocage de nouvelles classes de personnages",
            "Difficulté évolutive dynamiquement pour 1-4 joueurs"
        };
        fh.Scopes[0].Name = "Campagne de base standard";
        fh.Scopes[0].Description = "L'arc narratif principal jusqu'au boss final avec des quêtes secondaires clés.";
        fh.Scopes[1].Name = "Complétionniste total";
        fh.Scopes[1].Description = "Terminer tous les scénarios déblocables, prospérité maximale et toutes les retraites.";
        fh.Milestones[0].Title = "Arrivée à Frosthaven";
        fh.Milestones[0].Phase = "Acte I - L'avant-poste";
        fh.Milestones[0].Description = "Première exploration et établissement des bases de votre ville.";
        fh.Milestones[0].BadgeText = "Quête initiale";
        fh.Milestones[1].Title = "Première retraite de mercenaire";
        fh.Milestones[1].Phase = "Milieu de l'Acte I - Renaissance";
        fh.Milestones[1].Description = "Un mercenaire de départ termine sa quête personnelle et prend sa retraite, débloquant de nouvelles classes.";
        fh.Milestones[1].BadgeText = "Déblocage";
        fh.Milestones[2].Title = "La menace du Nord";
        fh.Milestones[2].Phase = "Acte II - Factions";
        fh.Milestones[2].Description = "Interaction avec les Algox, les Lurkers et les Unfettered.";
        fh.Milestones[2].BadgeText = "Branche narrative";
        fh.Milestones[3].Title = "La confrontation finale";
        fh.Milestones[3].Phase = "Final - Le destin du Nord";
        fh.Milestones[3].Description = "L'affrontement ultime qui décidera du sort de Frosthaven.";
        fh.Milestones[3].BadgeText = "Grand Final";

        // Pandemic Season 0 FR
        var pan = dict[GameId.PandemicSeason0];
        pan.Subtitle = "Thriller d'espionnage pendant la Guerre Froide (1962) et opérations secrètes de la CIA";
        pan.TotalBoxContentSummary = "12 dossiers de mois de campagne, passeports d'espions avec faux alias, cartes à gratter, jetons de surveillance.";
        pan.Theme.Tagline = "TOP SECRET // BRIEFING CLASSIFIÉ CIA // 1962";
        pan.Theme.LoreSummary = "En 1962, au paroxysme de la Guerre Froide, vous enquêtez sur le Projet MÉDUSE, une redoutable menace biologique soviétique.";
        pan.Theme.BadgeCategory = "Campagne Coopérative d'Espionnage";
        pan.KeyFeatures = new List<string>
        {
            "Passeports d'agents avec fausses identités et déguisements",
            "Deux chances par mois : victoire directe ou tentative 'Fin de Mois'",
            "Cartes de surveillance à gratter et planques de la CIA",
            "Intrigue à haute tension répartie sur 12 mois de calendrier"
        };
        pan.Scopes[0].Name = "Campagne Standard Attendue (1-2 Prologues + ~4 Rejeux)";
        pan.Scopes[0].Description = "1 prologue d'entraînement plus 12 mois avec taux d'échec typique. Total 16-17 parties.";
        pan.Scopes[1].Name = "Opération Efficace (1 Prologue + 1 Rejeu)";
        pan.Scopes[1].Description = "Taux de victoire élevé : 1 prologue et quasi tous les mois remportés au 1er essai (14 parties).";
        pan.Scopes[2].Name = "Surveillance Lourde (2 Prologues + 7 Rejeux)";
        pan.Scopes[2].Description = "Forte résistance soviétique ; plusieurs mois nécessitent un débriefing tardif (21 parties).";
        pan.Scopes[3].Name = "Capacité Maximale de la Boîte (24 Parties + 2 Prologues)";
        pan.Scopes[3].Description = "Chaque mois joué deux fois (Début + Fin) plus les deux prologues (26 parties).";
        pan.Milestones[0].Title = "Prologue & Entraînement";
        pan.Milestones[0].Phase = "Phase 0 - Intégration CIA";
        pan.Milestones[0].Description = "Création de passeports, alias d'espions et maîtrise des mécanismes de surveillance.";
        pan.Milestones[0].BadgeText = "Intégration";
        pan.Milestones[1].Title = "T1 : Émergence du Projet MÉDUSE (Jan - Mar)";
        pan.Milestones[1].Phase = "Phase I - La Menace Soviétique";
        pan.Milestones[1].Description = "Premier contact avec des saboteurs, établissement de planques en Europe et Asie.";
        pan.Milestones[1].BadgeText = "Dossier";
        pan.Milestones[2].Title = "T2 : Infiltration & l'Agent Double (Avr - Juin)";
        pan.Milestones[2].Phase = "Phase II - Infiltration Secrète";
        pan.Milestones[2].Description = "Opérations d'espionnage à haut risque, plans de labos soviétiques et fausses identités.";
        pan.Milestones[2].BadgeText = "Climax Médian";
        pan.Milestones[3].Title = "T3 : Course aux Armes Biologiques (Juil - Sep)";
        pan.Milestones[3].Phase = "Phase III - Escalade";
        pan.Milestones[3].Description = "Traque de pathogènes, neutralisation d'équipes du KGB et démantèlement de labos.";
        pan.Milestones[3].BadgeText = "Point Critique";
        pan.Milestones[4].Title = "T4 & Climax de Décembre (Oct - Déc)";
        pan.Milestones[4].Phase = "Final - Opération Endgame";
        pan.Milestones[4].Description = "L'opération finale qui décidera de l'issue de la Guerre Froide.";
        pan.Milestones[4].BadgeText = "Rapport Final";


        // Oathsworn FR
        var os = dict[GameId.Oathsworn];
        os.Subtitle = "Campagne de mystère et de combat coopératif contre des boss de dark fantasy";
        os.TotalBoxContentSummary = "21 Chapitres, plus de 15 affrontements de boss, plateaux modulaires, application interactive.";
        os.Theme.Tagline = "Aventurez-vous dans le Deepwood et affrontez les horreurs qu'il renferme";
        os.Theme.LoreSummary = "Dans un monde consumé par le Deepwood, le dernier bastion de l'humanité s'en remet aux Oathsworn, une compagnie de mercenaires endurcis, pour repousser les ténèbres envahissantes et les entités monstrueuses.";
        os.Theme.BadgeCategory = "Combat de Boss Dark Fantasy";
        os.KeyFeatures = new List<string>
        {
            "Système de combat unique avec cartes ou dés",
            "Gameplay en deux phases : Histoire et Rencontre",
            "Narrative à embranchements avec des choix significatifs",
            "Personnalisation et progression approfondies des personnages"
        };
        os.Scopes[0].Name = "Campagne Principale Standard";
        os.Scopes[0].Description = "L'histoire principale de 21 chapitres.";
        os.Scopes[1].Name = "Campagne Étendue (Toutes les Rencontres)";
        os.Scopes[1].Description = "L'histoire principale et tous les affrontements de boss optionnels.";
        os.Milestones[0].Title = "Le Premier Contrat";
        os.Milestones[0].Phase = "Acte I - L'Appel du Deepwood";
        os.Milestones[0].Description = "Première incursion dans le Deepwood et premier affrontement de boss.";
        os.Milestones[0].BadgeText = "Initiation";
        os.Milestones[1].Title = "La Mère de la Couvée";
        os.Milestones[1].Phase = "Acte I - Infestation";
        os.Milestones[1].Description = "Affrontement contre l'origine des récentes attaques.";
        os.Milestones[1].BadgeText = "Tueur d'Essaim";
        os.Milestones[2].Title = "Le Secret du Gardien";
        os.Milestones[2].Phase = "Acte II - Mystères Profonds";
        os.Milestones[2].Description = "Découverte de la vérité sur les origines du Deepwood.";
        os.Milestones[2].BadgeText = "Révélation";
        os.Milestones[3].Title = "Le Serment Final";
        os.Milestones[3].Phase = "Final - Le Cœur des Ténèbres";
        os.Milestones[3].Description = "L'ultime bataille contre l'horreur primitive.";
        os.Milestones[3].BadgeText = "Grand Final";

        // Frosthaven FR
        var fh = dict[GameId.Frosthaven];
        fh.Subtitle = "Aventure coopérative épique et construction de colonie dans le nord glacé";
        fh.TotalBoxContentSummary = "Livre de campagne de 138 scénarios, 18 classes jouables, systèmes complets d'artisanat et de colonie, monde évolutif.";
        fh.Theme.Tagline = "Brave les terres gelées et reconstruis l'avant-poste du nord";
        fh.Theme.LoreSummary = "Vous êtes un groupe de mercenaires au bout du monde, luttant pour protéger un avant-poste des éléments rudes et des monstres du nord.";
        fh.Theme.BadgeCategory = "RPG Fantasy & Construction de Ville";
        fh.KeyFeatures = new List<string>
        {
            "Combat complexe basé sur des cartes et progression des classes",
            "Construction et défense approfondies de la colonie",
            "Système d'artisanat complet et gestion des ressources",
            "Scénario à embranchements avec événements saisonniers"
        };
        fh.Scopes[0].Name = "Campagne Principale Standard";
        fh.Scopes[0].Description = "Arcs narratifs principaux et progression essentielle de la colonie.";
        fh.Scopes[1].Name = "Expérience de Campagne Étendue";
        fh.Scopes[1].Description = "Histoire principale, quêtes secondaires importantes et la plupart des déblocages.";
        fh.Scopes[2].Name = "Complétiste Intégral";
        fh.Scopes[2].Description = "Jouer tous les scénarios accessibles et maximiser la colonie.";
        fh.Milestones[0].Title = "Arrivée à Frosthaven";
        fh.Milestones[0].Phase = "Acte I - L'Avant-poste Gelé";
        fh.Milestones[0].Description = "Premiers pas dans les terres désolées et défense initiale.";
        fh.Milestones[0].BadgeText = "Arrivée";
        fh.Milestones[1].Title = "Premier Hiver";
        fh.Milestones[1].Phase = "Acte I - Changement de Saison";
        fh.Milestones[1].Description = "Survivre au premier hiver rude et débloquer des bâtiments avancés.";
        fh.Milestones[1].BadgeText = "L'Hiver est Là";
        fh.Milestones[2].Title = "La Menace Algox";
        fh.Milestones[2].Phase = "Acte II - Escalade";
        fh.Milestones[2].Description = "Plongée dans les montagnes pour affronter les tribus Algox.";
        fh.Milestones[2].BadgeText = "Faction Majeure";
        fh.Milestones[3].Title = "Secrets des Rôdeurs";
        fh.Milestones[3].Phase = "Acte III - Plongée Profonde";
        fh.Milestones[3].Description = "Explorer les profondeurs et confronter d'anciens mystères.";
        fh.Milestones[3].BadgeText = "Préparation Finale";
        fh.Milestones[4].Title = "L'Affrontement Final";
        fh.Milestones[4].Phase = "Final - Le Destin du Nord";
        fh.Milestones[4].Description = "La bataille ultime qui décidera de l'avenir de Frosthaven.";
        fh.Milestones[4].BadgeText = "Grand Final";


        // Tainted Grail FR
        var tg = dict[GameId.TaintedGrailFallOfAvalon];
        tg.Subtitle = "Une sombre aventure de survie coopérative dans un monde arthurien mourant";
        tg.TotalBoxContentSummary = "Une campagne à embranchements de 15 chapitres, des cartes de lieux surdimensionnées, des figurines détaillées et un journal d'exploration profond.";
        tg.Theme.Tagline = "Allumez les Menhirs et survivez à la Wyrd";
        tg.Theme.LoreSummary = "Le légendaire roi Arthur est mort, les Menhirs qui protègent Avalon s'éteignent, et la Wyrd consume la terre. Vous n'êtes pas les héros élus, mais vous êtes tout ce qu'il reste.";
        tg.Theme.BadgeCategory = "RPG de Survie Dark Fantasy";
        tg.KeyFeatures = new List<string>
        {
            "Exploration narrative riche et à embranchements avec le Journal d'Exploration",
            "Gestion des ressources et éléments de survie (Nourriture, Magie, Santé, Terreur)",
            "Combats et rencontres diplomatiques par cartes avec deck-building",
            "Carte du monde ouvert évolutive utilisant des cartes de lieux géantes"
        };
        tg.Scopes[0].Name = "La Chute d'Avalon (Campagne Principale)";
        tg.Scopes[0].Description = "L'arc narratif principal à travers Avalon pour découvrir le destin des héros originaux.";
        tg.Milestones[0].Title = "Quitter Cuanacht";
        tg.Milestones[0].Phase = "Chapitre 1 - Le Voyage Commence";
        tg.Milestones[0].Description = "Rassemblement de fournitures et départ de votre village natal mourant alors que le Menhir s'éteint.";
        tg.Milestones[0].BadgeText = "Premiers Pas";
        tg.Milestones[1].Title = "Le Secret des Menhirs";
        tg.Milestones[1].Phase = "Chapitre 4 - Profondément dans la Wyrd";
        tg.Milestones[1].Description = "Découverte des rituels nécessaires pour maintenir les Menhirs protecteurs allumés à travers Avalon.";
        tg.Milestones[1].BadgeText = "Survie";
        tg.Milestones[2].Title = "Les Chevaliers de la Table Ronde";
        tg.Milestones[2].Phase = "Chapitre 8 - Échos du Passé";
        tg.Milestones[2].Description = "Découverte de la vérité sur les chevaliers d'Arthur et leur expédition fatidique.";
        tg.Milestones[2].BadgeText = "Révélation";
        tg.Milestones[3].Title = "Le Cœur d'Avalon";
        tg.Milestones[3].Phase = "Chapitre 12 - Point de Non-Retour";
        tg.Milestones[3].Description = "Navigation dans les régions les plus dangereuses de la Wyrd pour atteindre le cœur du mystère.";
        tg.Milestones[3].BadgeText = "Préparation Finale";
        tg.Milestones[4].Title = "Le Destin de l'Île";
        tg.Milestones[4].Phase = "Chapitre 15 - Le Choix Final";
        tg.Milestones[4].Description = "L'affrontement ultime qui détermine l'avenir d'Avalon et de son peuple.";
        tg.Milestones[4].BadgeText = "Grand Final";

        return dict;
    }

    private Dictionary<GameId, BoardGame> BuildItalianCatalog()
    {
        var dict = BuildEnglishCatalog();

        // Gloomhaven IT
        var gh = dict[GameId.Gloomhaven];
        gh.Subtitle = "Combattimento tattico di stampo euro in un mondo persistente ed evolutivo di dark fantasy";
        gh.TotalBoxContentSummary = "Libro di campagna con 95 scenari, 17 classi di mercenari, 47 tipi di mostri, oltre 300 oggetti, adesivi ed archivio cittadino.";
        gh.Theme.Tagline = "Avventurati nelle ombre implacabili del Leone Addormentato";
        gh.Theme.LoreSummary = "Siete mercenari erranti dotati di abilità speciali. Nelle aspre terre selvagge ai margini della civiltà, ogni scelta altera permanentemente il mondo circostante.";
        gh.Theme.BadgeCategory = "GDR Tattico Dark Fantasy";
        gh.KeyFeatures = new List<string>
        {
            "Combattimento tattico con carte senza dadi (pura strategia)",
            "Pensionamento e sblocco di nuove classi di personaggi",
            "Narrativa a bivi con eventi di città e di strada",
            "Difficoltà scalabile dinamicamente per 1-4 giocatori"
        };
        gh.Scopes[0].Name = "Campagna Base Standard";
        gh.Scopes[0].Description = "L'arco narrativo principale fino al boss finale con missioni secondarie chiave e 2-3 pensionamenti.";
        gh.Scopes[1].Name = "Focus Storia Principale (Speedrun)";
        gh.Scopes[1].Description = "Percorso diretto attraverso le missioni primarie fino al boss finale con minime deviazioni.";
        gh.Scopes[2].Name = "Esperienza di Campagna Estesa";
        gh.Scopes[2].Description = "Storia principale, numerosi sblocchi di personaggi, progressione dell'archivio e catene secondarie.";
        gh.Scopes[3].Name = "Completista Totale (Tutto Accessibile)";
        gh.Scopes[3].Description = "Completamento di ogni scenario sbloccabile, massima prosperità, tutti i pensionamenti e dungeon avanzati.";
        gh.Milestones[0].Title = "Il Tumulo Nero e il Covo dei Banditi";
        gh.Milestones[0].Phase = "Atto I - I Contratti";
        gh.Milestones[0].Description = "Prima esplorazione, scontro con il Comandante dei Banditi e scoperta della trama del Negromante.";
        gh.Milestones[0].BadgeText = "Missione Iniziale";
        gh.Milestones[1].Title = "Primo Pensionamento del Mercenario";
        gh.Milestones[1].Phase = "Metà Atto I - Rinascita";
        gh.Milestones[1].Description = "Un mercenario iniziale completa il suo Obiettivo Personale e si ritira, sbloccando nuove classi.";
        gh.Milestones[1].BadgeText = "Sblocco";
        gh.Milestones[2].Title = "Il Segreto del Mercante e Invasione dei Demoni";
        gh.Milestones[2].Phase = "Atto II - Fazioni";
        gh.Milestones[2].Description = "Esplorazione dei piani elementali, gestione della Fenditura e alleanze strategiche.";
        gh.Milestones[2].BadgeText = "Bivio Narrativo";
        gh.Milestones[3].Title = "La Fenditura del Gloom e Cripte degli Antichi";
        gh.Milestones[3].Phase = "Atto III - Ascesa Finale";
        gh.Milestones[3].Description = "Raccolta di manufatti, raggiungimento del Santuario della Grande Quercia e discesa nell'oscurità.";
        gh.Milestones[3].BadgeText = "Preparazione Finale";
        gh.Milestones[4].Title = "Boss Finale e Climax della Campagna";
        gh.Milestones[4].Phase = "Finale - La Leggenda del Leone Addormentato";
        gh.Milestones[4].Description = "Lo scontro definitivo che deciderà il destino di Gloomhaven.";
        gh.Milestones[4].BadgeText = "Gran Finale";


        // Frosthaven IT
        var fh = dict[GameId.Frosthaven];
        fh.Subtitle = "Un gioco di esplorazione tattica cooperativo a campagna in un mondo di ghiaccio";
        fh.TotalBoxContentSummary = "Libro della campagna enorme con 138 scenari, 18 classi giocabili, dozzine di mostri, componenti per la costruzione della città e oltre 1000 carte.";
        fh.Theme.Tagline = "Costruisci e difendi il tuo avamposto di ghiaccio";
        fh.Theme.LoreSummary = "Siete mercenari in viaggio verso il gelido Nord, nel piccolo avamposto di Frosthaven. Dovrete affrontare un clima ostile, nemici pericolosi e sviluppare la vostra città nel tempo.";
        fh.Theme.BadgeCategory = "GDR Tattico Dark Fantasy";
        fh.KeyFeatures = new List<string>
        {
            "Combattimento tattico con carte senza dadi (pura strategia)",
            "Meccaniche di costruzione della città e gestione delle risorse",
            "Pensionamento e sblocco di nuove classi di personaggi",
            "Difficoltà scalabile dinamicamente per 1-4 giocatori"
        };
        fh.Scopes[0].Name = "Campagna Base Standard";
        fh.Scopes[0].Description = "L'arco narrativo principale fino al boss finale con missioni secondarie chiave.";
        fh.Scopes[1].Name = "Completista Totale";
        fh.Scopes[1].Description = "Completamento di ogni scenario sbloccabile, massima prosperità e tutti i pensionamenti.";
        fh.Milestones[0].Title = "Arrivo a Frosthaven";
        fh.Milestones[0].Phase = "Atto I - L'Avamposto";
        fh.Milestones[0].Description = "Prima esplorazione e fondazione delle basi della tua città.";
        fh.Milestones[0].BadgeText = "Missione Iniziale";
        fh.Milestones[1].Title = "Primo Pensionamento del Mercenario";
        fh.Milestones[1].Phase = "Metà Atto I - Rinascita";
        fh.Milestones[1].Description = "Un mercenario iniziale completa il suo Obiettivo Personale e si ritira, sbloccando nuove classi.";
        fh.Milestones[1].BadgeText = "Sblocco";
        fh.Milestones[2].Title = "La Minaccia del Nord";
        fh.Milestones[2].Phase = "Atto II - Fazioni";
        fh.Milestones[2].Description = "Interazione con Algox, Lurkers e Unfettered.";
        fh.Milestones[2].BadgeText = "Ramo Narrativo";
        fh.Milestones[3].Title = "Il Confronto Finale";
        fh.Milestones[3].Phase = "Finale - Il Destino del Nord";
        fh.Milestones[3].Description = "Lo scontro definitivo che deciderà il destino di Frosthaven.";
        fh.Milestones[3].BadgeText = "Gran Finale";

        // Pandemic Season 0 IT
        var pan = dict[GameId.PandemicSeason0];
        pan.Subtitle = "Thriller di spionaggio durante la Guerra Fredda (1962) e operazioni segrete della CIA";
        pan.TotalBoxContentSummary = "12 dossier mensili di campagna, passaporti di spie personalizzabili con false identità, mappe gratta-e-scopri, segnalini sorveglianza.";
        pan.Theme.Tagline = "TOP SECRET // BRIEFING CLASSIFICATO CIA // 1962";
        pan.Theme.LoreSummary = "Nel 1962, al culmine della Guerra Fredda, tu e la tua squadra della CIA dovete indagare sul Progetto MEDUSA, una pericolosa arma biologica sovietica.";
        pan.Theme.BadgeCategory = "Campagna Cooperativa di Spionaggio";
        pan.KeyFeatures = new List<string>
        {
            "Passaporti operativi con identità contraffatte e travestimenti",
            "Due tentativi al mese: vittoria o tentativo di fine mese ('Late Month')",
            "Mappe di sorveglianza da grattare e rifugi segreti della CIA",
            "Tensione narrativa che si sviluppa lungo un calendario di 12 mesi"
        };
        pan.Scopes[0].Name = "Campagna Standard Prevista (1-2 Prologhi + ~4 Ripetizioni)";
        pan.Scopes[0].Description = "1 prologo di addestramento più 12 mesi con normale tasso di sconfitte. Totale 16-17 partite.";
        pan.Scopes[1].Name = "Operazione Pulita (1 Prologo + 1 Ripetizione)";
        pan.Scopes[1].Description = "Alta percentuale di vittorie: 1 prologo e quasi tutti i mesi vinti al 1° tentativo (14 partite).";
        pan.Scopes[2].Name = "Sorveglianza Intensa (2 Prologhi + 7 Ripetizioni)";
        pan.Scopes[2].Description = "Forte contrasto sovietico; diversi mesi richiedono debriefing di fine mese (21 partite).";
        pan.Scopes[3].Name = "Capienza Massima della Scatola (24 Partite + 2 Prologhi)";
        pan.Scopes[3].Description = "Ogni mese giocato due volte (Inizio + Fine) più entrambi i prologhi (26 partite).";
        pan.Milestones[0].Title = "Prologo e Addestramento";
        pan.Milestones[0].Phase = "Fase 0 - Reclutamento CIA";
        pan.Milestones[0].Description = "Creazione di passaporti, alias di copertura e padronanza della sorveglianza.";
        pan.Milestones[0].BadgeText = "Reclutamento";
        pan.Milestones[1].Title = "Q1: Emersione del Progetto MEDUSA (Gen - Mar)";
        pan.Milestones[1].Phase = "Fase I - La Minaccia Sovietica";
        pan.Milestones[1].Description = "Primo contatto con i sabotatori sovietici e allestimento di rifugi in Europa e Asia.";
        pan.Milestones[1].BadgeText = "Dossier";
        pan.Milestones[2].Title = "Q2: Infiltrazione e il Doppio Gioco (Apr - Giu)";
        pan.Milestones[2].Phase = "Fase II - Infiltrazione Segreta";
        pan.Milestones[2].Description = "Operazioni di spionaggio ad alto rischio, piani di laboratori sovietici e identità compromesse.";
        pan.Milestones[2].BadgeText = "Climax Mediano";
        pan.Milestones[3].Title = "T3: Corsa agli Armamenti Biologici (Lug - Set)";
        pan.Milestones[3].Phase = "Fase III - Escalation";
        pan.Milestones[3].Description = "Tracciamento di agenti patogeni, neutralizzazione di squadre KGB e laboratori.";
        pan.Milestones[3].BadgeText = "Punto Critico";
        pan.Milestones[4].Title = "Q4 e Climax di Dicembre (Ott - Dic)";
        pan.Milestones[4].Phase = "Finale - Operazione Endgame";
        pan.Milestones[4].Description = "L'operazione conclusiva che deciderà le sorti della Guerra Fredda.";
        pan.Milestones[4].BadgeText = "Rapporto Finale";


        // Oathsworn IT
        var os = dict[GameId.Oathsworn];
        os.Subtitle = "Campagna di mistero e combattimento cooperativo contro boss di dark fantasy";
        os.TotalBoxContentSummary = "21 Capitoli, oltre 15 scontri con boss, tabelloni modulari, app interattiva.";
        os.Theme.Tagline = "Avventurati nel Deepwood e affronta gli orrori che vi si celano";
        os.Theme.LoreSummary = "In un mondo consumato dal Deepwood, l'ultimo baluardo dell'umanità si affida agli Oathsworn, una compagnia di mercenari temprati, per combattere l'oscurità invadente e le entità mostruose.";
        os.Theme.BadgeCategory = "Combattimento contro Boss Dark Fantasy";
        os.KeyFeatures = new List<string>
        {
            "Sistema di combattimento unico con carte o dadi",
            "Gameplay in due fasi: Storia e Incontro",
            "Narrativa a bivi con scelte significative",
            "Profonda personalizzazione e progressione dei personaggi"
        };
        os.Scopes[0].Name = "Campagna Principale Standard";
        os.Scopes[0].Description = "La trama principale di 21 capitoli.";
        os.Scopes[1].Name = "Campagna Estesa (Tutti gli Incontri)";
        os.Scopes[1].Description = "Storia principale e tutti gli scontri opzionali con i boss.";
        os.Milestones[0].Title = "Il Primo Contratto";
        os.Milestones[0].Phase = "Atto I - Il Deepwood Chiama";
        os.Milestones[0].Description = "Prima incursione nel Deepwood e scontro iniziale con un boss.";
        os.Milestones[0].BadgeText = "Iniziazione";
        os.Milestones[1].Title = "La Madre della Covata";
        os.Milestones[1].Phase = "Atto I - Infestazione";
        os.Milestones[1].Description = "Affrontare la fonte dei recenti attacchi.";
        os.Milestones[1].BadgeText = "Sterminatore di Sciami";
        os.Milestones[2].Title = "Il Segreto del Guardiano";
        os.Milestones[2].Phase = "Atto II - Misteri Più Profondi";
        os.Milestones[2].Description = "Scoprire la verità sulle origini del Deepwood.";
        os.Milestones[2].BadgeText = "Rivelazione";
        os.Milestones[3].Title = "Il Giuramento Finale";
        os.Milestones[3].Phase = "Finale - Il Cuore di Tenebra";
        os.Milestones[3].Description = "La battaglia finale contro l'orrore primigenio.";
        os.Milestones[3].BadgeText = "Gran Finale";

        // Frosthaven IT
        var fh = dict[GameId.Frosthaven];
        fh.Subtitle = "Epica avventura cooperativa e costruzione di un insediamento nel nord gelido";
        fh.TotalBoxContentSummary = "Libro della campagna con 138 scenari, 18 classi giocabili, ampio sistema di creazione e costruzione, mondo in evoluzione.";
        fh.Theme.Tagline = "Affronta le lande gelate e ricostruisci l'avamposto del nord";
        fh.Theme.LoreSummary = "Siete un gruppo di mercenari ai confini del mondo, che lottano per proteggere un avamposto dalle intemperie e dalle mostruosità del nord.";
        fh.Theme.BadgeCategory = "GDR Fantasy & Costruzione Città";
        fh.KeyFeatures = new List<string>
        {
            "Complesso combattimento con carte e progressione di classe",
            "Fasi profonde di costruzione e difesa dell'insediamento",
            "Ampio sistema di creazione e gestione delle risorse",
            "Narrativa a bivi con eventi stagionali"
        };
        fh.Scopes[0].Name = "Campagna Base Standard";
        fh.Scopes[0].Description = "Gli archi narrativi principali e la progressione essenziale dell'insediamento.";
        fh.Scopes[1].Name = "Esperienza di Campagna Estesa";
        fh.Scopes[1].Description = "Storia principale, missioni secondarie importanti e gran parte degli sblocchi.";
        fh.Scopes[2].Name = "Completista Totale";
        fh.Scopes[2].Description = "Affrontare tutti gli scenari accessibili e massimizzare l'insediamento.";
        fh.Milestones[0].Title = "Arrivo a Frosthaven";
        fh.Milestones[0].Phase = "Atto I - L'Avamposto Gelato";
        fh.Milestones[0].Description = "Primi passi nelle lande e difesa iniziale dell'insediamento.";
        fh.Milestones[0].BadgeText = "Arrivo";
        fh.Milestones[1].Title = "Primo Inverno";
        fh.Milestones[1].Phase = "Atto I - Cambio Stagionale";
        fh.Milestones[1].Description = "Sopravvivere al primo duro inverno e sbloccare edifici avanzati.";
        fh.Milestones[1].BadgeText = "L'Inverno è Qui";
        fh.Milestones[2].Title = "La Minaccia Algox";
        fh.Milestones[2].Phase = "Atto II - Escalation";
        fh.Milestones[2].Description = "Spingersi nelle montagne per affrontare le tribù Algox.";
        fh.Milestones[2].BadgeText = "Fazione Principale";
        fh.Milestones[3].Title = "Segreti degli Inseguitori";
        fh.Milestones[3].Phase = "Atto III - Immersione Profonda";
        fh.Milestones[3].Description = "Esplorare le profondità e confrontarsi con antichi misteri.";
        fh.Milestones[3].BadgeText = "Preparazione Finale";
        fh.Milestones[4].Title = "Lo Scontro Finale";
        fh.Milestones[4].Phase = "Finale - Il Destino del Nord";
        fh.Milestones[4].Description = "La battaglia definitiva che deciderà il futuro di Frosthaven.";
        fh.Milestones[4].BadgeText = "Gran Finale";


        // Tainted Grail IT
        var tg = dict[GameId.TaintedGrailFallOfAvalon];
        tg.Subtitle = "Un'oscura avventura cooperativa di sopravvivenza in un mondo arturiano morente";
        tg.TotalBoxContentSummary = "Una campagna a bivi di 15 capitoli, carte luogo giganti, miniature dettagliate e un profondo diario di esplorazione.";
        tg.Theme.Tagline = "Accendi i Menhir e sopravvivi all'Anomalia";
        tg.Theme.LoreSummary = "Il leggendario Re Artù è morto, i Menhir che proteggono Avalon si stanno spegnendo e l'Anomalia sta consumando la terra. Non siete gli eroi prescelti, ma siete tutto ciò che resta.";
        tg.Theme.BadgeCategory = "GDR di Sopravvivenza Dark Fantasy";
        tg.KeyFeatures = new List<string>
        {
            "Ricca esplorazione narrativa a bivi con il Diario di Esplorazione",
            "Gestione delle risorse ed elementi di sopravvivenza (Cibo, Magia, Salute, Terrore)",
            "Combattimenti con carte e incontri diplomatici con deck-building",
            "Mappa del mondo aperto in evoluzione utilizzando carte luogo giganti"
        };
        tg.Scopes[0].Name = "La Caduta di Avalon (Campagna Base)";
        tg.Scopes[0].Description = "L'arco narrativo principale attraverso Avalon per scoprire il destino degli eroi originali.";
        tg.Milestones[0].Title = "Lasciare Cuanacht";
        tg.Milestones[0].Phase = "Capitolo 1 - Il Viaggio Inizia";
        tg.Milestones[0].Description = "Raccogliere provviste e lasciare il villaggio natale morente mentre il Menhir si spegne.";
        tg.Milestones[0].BadgeText = "Primi Passi";
        tg.Milestones[1].Title = "Il Segreto dei Menhir";
        tg.Milestones[1].Phase = "Capitolo 4 - Nel Profondo dell'Anomalia";
        tg.Milestones[1].Description = "Scoprire i rituali necessari per mantenere accesi i Menhir protettivi in tutta Avalon.";
        tg.Milestones[1].BadgeText = "Sopravvivenza";
        tg.Milestones[2].Title = "I Cavalieri della Tavola Rotonda";
        tg.Milestones[2].Phase = "Capitolo 8 - Echi del Passato";
        tg.Milestones[2].Description = "Scoprire la verità sui cavalieri di Artù e sulla loro fatidica spedizione.";
        tg.Milestones[2].BadgeText = "Rivelazione";
        tg.Milestones[3].Title = "Il Cuore di Avalon";
        tg.Milestones[3].Phase = "Capitolo 12 - Punto di Non Ritorno";
        tg.Milestones[3].Description = "Navigare attraverso le regioni più pericolose dell'Anomalia per raggiungere il cuore del mistero.";
        tg.Milestones[3].BadgeText = "Prep. Finale";
        tg.Milestones[4].Title = "Il Destino dell'Isola";
        tg.Milestones[4].Phase = "Capitolo 15 - La Scelta Finale";
        tg.Milestones[4].Description = "Lo scontro definitivo che determina il futuro di Avalon e del suo popolo.";
        tg.Milestones[4].BadgeText = "Gran Finale";

        return dict;
    }
}

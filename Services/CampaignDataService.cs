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
        if (_localizedGames.TryGetValue("en", out var enDict) && enDict.TryGetValue(id, out var enGame))
        {
            return enGame;
        }

        return _localizedGames["en"].Values.First();
    }

    private Dictionary<GameId, BoardGame> GetDictionaryForLang(string lang)
    {
        if (string.IsNullOrEmpty(lang)) lang = "en";
        lang = lang.ToLowerInvariant();
        if (_localizedGames.TryGetValue(lang, out var dict))
        {
            return dict;
        }

        var prefix = lang.Split('-')[0];
        if (_localizedGames.TryGetValue(prefix, out var prefixDict))
        {
            return prefixDict;
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
            },        };

        dict[GameId.SleepingGods] = new BoardGame
        {
            Id = GameId.SleepingGods,
            Title = "Sleeping Gods",
            Subtitle = "Cooperative nautical exploration and storybook survival across the Wandering Sea",
            ReleaseYear = 2021,
            Designers = "Ryan Laukat",
            PlayersMin = 1,
            PlayersMax = 4,
            BggRating = 8.4,
            BggWeight = 3.28,
            BggUrl = "https://boardgamegeek.com/boardgame/255984/sleeping-gods",
            ImageUrl = "https://cf.geekdo-images.com/nS3N3B75f4h_7vXWzVvTww__imagepage/img/Lh9D_Jk2xS4eG5UqQ2sD3w5n_wM=/fit-in/900x600/filters:no_upscale():strip_icc()/pic4223169.jpg",
            BaseScenarioMinutes = 60,
            SetupTeardownMinutes = 15,
            TotalBoxContentSummary = "Spiral-bound atlas of the Wandering Sea, 172-page storybook, 8 playable crew members, event deck, quest cards, and secret adventure envelopes.",
            Theme = new GameTheming
            {
                PrimaryColor = "#0284c7",
                SecondaryColor = "#0f172a",
                AccentColor = "#f59e0b",
                BackgroundGradient = "radial-gradient(ellipse at top, #1e3a5f 0%, #0f172a 60%, #020617 100%)",
                CardBackground = "rgba(15, 23, 42, 0.90)",
                BorderColor = "rgba(2, 132, 199, 0.35)",
                GlowColor = "rgba(2, 132, 199, 0.25)",
                HeaderFont = "'Cormorant SC', 'Cinzel', Georgia, serif",
                BodyFont = "'Inter', system-ui, sans-serif",
                Tagline = "Explore the Wandering Sea and awaken the slumbering gods",
                LoreSummary = "The year is 1929. The steamship Manticore is lost in a strange, uncharted world. You and your crew must explore mysterious islands, survive perilous encounters, and discover the totems of the gods to find your way home.",
                BadgeCategory = "Cooperative Narrative Adventure"
            },
            KeyFeatures = new List<string>
            {
                "Rich storybook-driven exploration with hundreds of branching quests",
                "Tactical combat synergy system with weapon skills and condition damage",
                "Expansive atlas navigation aboard the steamship Manticore",
                "Persistent campaign unlocks with totems, recipes, and quest cards"
            },
            PublicSourceReferences = new List<string>
            {
                "BoardGameGeek (Top 100 Overall / Top 20 Thematic)",
                "Red Raven Games Official Rulebook & Storybook",
                "Community Campaign Logs and Session Averages"
            },
            Scopes = new List<CampaignScopeOption>
            {
                new()
                {
                    Id = "sg_standard",
                    Name = "Standard Campaign (Full Journey)",
                    Description = "A complete voyage through the Wandering Sea completing major quests and collecting divine totems.",
                    BaseScenarioCount = 16,
                    EstimatedFailRatePercent = 5.0,
                    IsRecommended = true
                },
                new()
                {
                    Id = "sg_quick",
                    Name = "Short Exploratory Expedition",
                    Description = "A streamlined nautical expedition exploring nearby archipelagos and solving introductory quests.",
                    BaseScenarioCount = 10,
                    EstimatedFailRatePercent = 5.0,
                    IsRecommended = false
                }
            },        };

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
            },        };


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
                HeaderFont = "'MedievalSharp', 'Cinzel', cursive, serif",
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
            },        };

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
                HeaderFont = "'Cinzel Decorative', 'Cinzel', Georgia, serif",
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
            },        };

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
                HeaderFont = "'Uncial Antiqua', 'Cinzel', cursive, serif",
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
            },        };

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

        // Sleeping Gods ES
        var sg = dict[GameId.SleepingGods];
        sg.Subtitle = "Exploración náutica cooperativa y supervivencia con libro de historias a través del Mar Errante";
        sg.TotalBoxContentSummary = "Atlas encuadernado en espiral del Mar Errante, libro de historias de 172 páginas, 8 miembros de tripulación jugables, mazo de eventos, cartas de misión y sobres secretos.";
        sg.Theme.Tagline = "Explora el Mar Errante y despierta a los dioses dormidos";
        sg.Theme.LoreSummary = "Es el año 1929. El barco de vapor Manticore se pierde en un mundo extraño y desconocido. Tú y tu tripulación debéis explorar islas misteriosas, sobrevivir a encuentros peligrosos y descubrir los tótems de los dioses para encontrar el camino de regreso a casa.";
        sg.Theme.BadgeCategory = "Aventura Narrativa Cooperativa";
        sg.KeyFeatures = new List<string>
        {
            "Rica exploración guiada por libro de historias con cientos de misiones ramificadas",
            "Sistema de combate táctico con habilidades de armas y daño por condiciones",
            "Navegación mediante atlas a bordo del barco de vapor Manticore",
            "Desbloqueos persistentes de campaña con tótems, recetas y cartas de misión"
        };
        sg.Scopes[0].Name = "Campaña Estándar (Viaje Completo)";
        sg.Scopes[0].Description = "Un viaje completo por el Mar Errante completando misiones principales y reuniendo tótems divinos.";
        sg.Scopes[1].Name = "Expedición Exploratoria Breve";
        sg.Scopes[1].Description = "Una expedición náutica ágil explorando archipiélagos cercanos y resolviendo misiones introductorias.";

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

        // Sleeping Gods DE
        var sg = dict[GameId.SleepingGods];
        sg.Subtitle = "Kooperative nautische Erkundung und Storybuch-Überleben auf der Wandernden See";
        sg.TotalBoxContentSummary = "Spiralgebundener Atlas der Wandernden See, 172-seitiges Storybuch, 8 spielbare Besatzungsmitglieder, Ereignisdeck, Questkarten und geheime Abenteuerumschläge.";
        sg.Theme.Tagline = "Erkunde die Wandernde See und erwecke die schlummernden Götter";
        sg.Theme.LoreSummary = "Wir schreiben das Jahr 1929. Das Dampfschiff Manticore ist in einer seltsamen, unbekannten Welt verschollen. Du und deine Crew müsst geheimnisvolle Inseln erkunden, gefährliche Begegnungen überstehen und die Totems der Götter finden, um den Weg nach Hause zu entdecken.";
        sg.Theme.BadgeCategory = "Kooperatives Erzählabenteuer";
        sg.KeyFeatures = new List<string>
        {
            "Reichhaltige, buchgestützte Erkundung mit Hunderten von verzweigten Quests",
            "Taktisches Kampfsystem mit Waffenfertigkeiten und Zustandsschaden",
            "Umfassende Atlas-Navigation an Bord des Dampfschiffs Manticore",
            "Permanente Kampagnen-Freischaltungen mit Totems, Rezepten und Questkarten"
        };
        sg.Scopes[0].Name = "Standard-Kampagne (Vollständige Reise)";
        sg.Scopes[0].Description = "Eine komplette Reise über die Wandernde See mit allen Hauptquests und dem Sammeln göttlicher Totems.";
        sg.Scopes[1].Name = "Kurze Erkundungsexpedition";
        sg.Scopes[1].Description = "Eine gestraffte nautische Expedition zur Erkundung nahegelegener Inseln und Einführung in die Quests.";

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

        // Sleeping Gods FR
        var sg = dict[GameId.SleepingGods];
        sg.Subtitle = "Exploration nautique coopérative et survie narrative à travers la Mer Errange";
        sg.TotalBoxContentSummary = "Atlas à spirales de la Mer Errange, livre d'aventures de 172 pages, 8 membres d'équipage jouables, deck d'événements, cartes de quête et enveloppes secrètes.";
        sg.Theme.Tagline = "Explorez la Mer Errange et réveillez les dieux endormis";
        sg.Theme.LoreSummary = "Nous sommes en 1929. Le bateau à vapeur Manticore est perdu dans un monde étrange et inexploré. Vous et votre équipage devez explorer des îles mystérieuses, survivre à de périlleuses rencontres et trouver les totems des dieux pour rentrer chez vous.";
        sg.Theme.BadgeCategory = "Aventure Narrative Coopérative";
        sg.KeyFeatures = new List<string>
        {
            "Exploration riche guidée par un livre d'histoires aux centaines de quêtes ramifiées",
            "Système de combat tactique avec compétences d'armes et dégâts d'états",
            "Navigation sur atlas à bord du navire à vapeur Manticore",
            "Déblocages permanents avec totems, recettes et cartes de quêtes"
        };
        sg.Scopes[0].Name = "Campagne Standard (Voyage Complet)";
        sg.Scopes[0].Description = "Un périple complet à travers la Mer Errange pour accomplir les quêtes majeures et récupérer les totems divins.";
        sg.Scopes[1].Name = "Courte Expédition d'Exploration";
        sg.Scopes[1].Description = "Une expédition maritime rapide pour explorer les archipels voisins et s'initier aux premières quêtes.";

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

        // Sleeping Gods IT
        var sg = dict[GameId.SleepingGods];
        sg.Subtitle = "Esplorazione nautica cooperativa e sopravvivenza narrativa attraverso il Mare Errante";
        sg.TotalBoxContentSummary = "Atlante a spirale del Mare Errante, libro di storie di 172 pagine, 8 membri dell'equipaggio giocabili, mazzo eventi, carte missione e buste segrete.";
        sg.Theme.Tagline = "Esplora il Mare Errante e risveglia gli dei assopiti";
        sg.Theme.LoreSummary = "È l'anno 1929. Il piroscafo Manticore si è perso in un mondo strano e inesplorato. Tu e il tuo equipaggio dovete esplorare isole misteriose, sopravvivere a pericoli insidiosi e scoprire i totem degli dei per ritrovare la rotta verso casa.";
        sg.Theme.BadgeCategory = "Avventura Narrativa Cooperativa";
        sg.KeyFeatures = new List<string>
        {
            "Ricca esplorazione guidata da un libro narrativo con centinaia di missioni ramificate",
            "Combattimento tattico con abilità d'arma e danni da condizioni",
            "Navigazione su atlante a bordo del piroscafo Manticore",
            "Sblocchi persistenti della campagna con totem, ricette e carte missione"
        };
        sg.Scopes[0].Name = "Campagna Standard (Viaggio Completo)";
        sg.Scopes[0].Description = "Un viaggio completo attraverso il Mare Errante completando le missioni principali e raccogliendo i totem divini.";
        sg.Scopes[1].Name = "Breve Spedizione Esplorativa";
        sg.Scopes[1].Description = "Una spedizione marittima agile per esplorare gli arcipelaghi vicini e affrontare le prime missioni.";

        return dict;
    }
}

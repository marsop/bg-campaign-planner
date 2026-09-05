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

        return dict;
    }
}

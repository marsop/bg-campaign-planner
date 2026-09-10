using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bg_campaign_planner.Models;

namespace bg_campaign_planner.Services;

public class PlannerCalculatorService
{
    public CalculationResult Calculate(
        PlannerInputModel input,
        BoardGame game,
        Microsoft.Extensions.Localization.IStringLocalizer<bg_campaign_planner.Resources.AppResources>? loc = null)
    {
        var scope = game.Scopes.FirstOrDefault(s => s.Id == input.SelectedScopeId)
                    ?? game.Scopes.FirstOrDefault(s => s.IsRecommended)
                    ?? game.Scopes.First();

        // 1. Modifiers
        double playerCountTimeMultiplier = input.PlayerCount switch
        {
            1 => 0.85,
            2 => 0.95,
            3 => 1.05,
            4 => 1.20,
            _ => 1.00
        };

        (double expTimeMultiplier, double expFailDelta) = input.Experience switch
        {
            ExperienceLevel.Beginner => (1.20, 10.0),
            ExperienceLevel.Regular => (1.00, 0.0),
            ExperienceLevel.Veteran => (0.85, -5.0),
            _ => (1.00, 0.0)
        };

        // 2. Play counts and retries
        int baseScenarios = scope.BaseScenarioCount;
        int failureBuffer = 0;

        if (input.EnableFailureBuffer)
        {
            double effectiveFailRate = Math.Max(0, (scope.EstimatedFailRatePercent + expFailDelta) / 100.0);
            failureBuffer = (int)Math.Round(baseScenarios * effectiveFailRate);
        }

        int totalEstimatedPlays = baseScenarios + failureBuffer;
        int scenariosPerMeetup = Math.Max(1, input.ScenariosPerMeetup);
        int totalMeetups = (int)Math.Ceiling((double)totalEstimatedPlays / scenariosPerMeetup);

        // 3. Durations
        double scenarioMinutes = game.BaseScenarioMinutes * playerCountTimeMultiplier * expTimeMultiplier;
        double setupMinutes = input.IncludeSetupTeardown ? game.SetupTeardownMinutes : 0;
        
        double totalGameplayMinutes = scenarioMinutes * totalEstimatedPlays;
        double totalSetupMinutes = setupMinutes * totalMeetups;
        double totalTableMinutes = totalGameplayMinutes + totalSetupMinutes;

        double totalGameplayHours = Math.Round(totalGameplayMinutes / 60.0, 1);
        double totalSetupHours = Math.Round(totalSetupMinutes / 60.0, 1);
        double totalTableHours = Math.Round(totalTableMinutes / 60.0, 1);
        double averageSessionHours = Math.Round((scenarioMinutes * scenariosPerMeetup + setupMinutes) / 60.0, 1);

        // 4. Session schedule generation with dynamic progress checkpoints
        var progressCheckpoints = CreateProgressCheckpoints(totalEstimatedPlays, loc);
        var sessions = GenerateSessions(input, totalEstimatedPlays, scenariosPerMeetup, averageSessionHours, progressCheckpoints);

        DateTime startDate = input.StartDate;
        DateTime projectedFinishDate = sessions.Count > 0 ? sessions[^1].Date : startDate;
        
        if (input.VacationWeeksBuffer > 0 && sessions.Count > 0)
        {
            projectedFinishDate = projectedFinishDate.AddDays(input.VacationWeeksBuffer * 7);
        }

        int totalCalendarDays = Math.Max(1, (int)(projectedFinishDate - startDate).TotalDays);
        double totalCalendarWeeks = Math.Round(totalCalendarDays / 7.0, 1);
        double totalCalendarMonths = Math.Round(totalCalendarDays / 30.4375, 1);

        // 5. Milestone projections (calculated cleanly from progress checkpoints)
        var milestoneProjections = new List<MilestoneProjection>();

        foreach (var milestone in progressCheckpoints.OrderBy(m => m.Order))
        {
            int targetPlayIndex = milestone.AtScenarioOrGameIndex;
            var matchingSession = sessions.FirstOrDefault(s => s.EndScenarioIndex >= targetPlayIndex) ?? sessions.LastOrDefault();
            int meetupNum = matchingSession?.SessionNumber ?? totalMeetups;
            DateTime milestoneDate = matchingSession?.Date ?? projectedFinishDate;

            double hoursSpent = Math.Round(meetupNum * averageSessionHours, 1);

            milestoneProjections.Add(new MilestoneProjection
            {
                Milestone = milestone,
                TargetScenarioIndex = targetPlayIndex,
                EstimatedMeetupNumber = meetupNum,
                EstimatedDate = milestoneDate,
                HoursSpentUntilMilestone = hoursSpent
            });
        }

        return new CalculationResult
        {
            Game = game,
            Scope = scope,
            BaseScenariosOrGames = baseScenarios,
            BufferPlaysForFailures = failureBuffer,
            TotalEstimatedPlays = totalEstimatedPlays,
            TotalMeetupsRequired = totalMeetups,
            AverageSessionHours = averageSessionHours,
            TotalTableHours = totalTableHours,
            TotalGameplayHours = totalGameplayHours,
            TotalSetupHours = totalSetupHours,
            StartDate = startDate,
            ProjectedFinishDate = projectedFinishDate,
            TotalCalendarDays = totalCalendarDays,
            TotalCalendarWeeks = totalCalendarWeeks,
            TotalCalendarMonths = totalCalendarMonths,
            Sessions = sessions,
            MilestoneProjections = milestoneProjections
        };
    }

    public List<GameMilestone> CreateProgressCheckpoints(
        int totalPlays,
        Microsoft.Extensions.Localization.IStringLocalizer<bg_campaign_planner.Resources.AppResources>? loc = null)
    {
        int safeTotal = Math.Max(1, totalPlays);

        int q1 = Math.Max(1, (int)Math.Round(safeTotal * 0.25));
        int q2 = Math.Max(q1, (int)Math.Round(safeTotal * 0.50));
        int q3 = Math.Max(q2, (int)Math.Round(safeTotal * 0.75));
        int q4 = safeTotal;

        if (safeTotal >= 4)
        {
            if (q2 <= q1) q2 = q1 + 1;
            if (q3 <= q2) q3 = q2 + 1;
            if (q4 <= q3) q4 = q3 + 1;
            if (q4 > safeTotal)
            {
                q4 = safeTotal;
                if (q3 >= q4) q3 = Math.Max(1, q4 - 1);
                if (q2 >= q3) q2 = Math.Max(1, q3 - 1);
                if (q1 >= q2) q1 = Math.Max(1, q2 - 1);
            }
        }

        return new List<GameMilestone>
        {
            new()
            {
                Order = 1,
                Title = loc != null ? loc["Checkpoint.QuarterTitle"] : "Campaign Quarter Mark (25%)",
                Phase = loc != null ? loc["Checkpoint.QuarterPhase"] : "Phase I - Opening Stretch",
                Description = loc != null ? loc["Checkpoint.QuarterDesc", q1] : $"Estimated completion of the first 25% of campaign scenarios ({q1} scenarios played).",
                AtScenarioOrGameIndex = q1,
                BadgeText = loc != null ? loc["Checkpoint.QuarterBadge"] : "25%",
                IconEmoji = "🚩"
            },
            new()
            {
                Order = 2,
                Title = loc != null ? loc["Checkpoint.MidpointTitle"] : "Campaign Midpoint (50%)",
                Phase = loc != null ? loc["Checkpoint.MidpointPhase"] : "Phase II - Campaign Midpoint",
                Description = loc != null ? loc["Checkpoint.MidpointDesc", q2] : $"The halfway milestone of the campaign ({q2} scenarios played).",
                AtScenarioOrGameIndex = q2,
                BadgeText = loc != null ? loc["Checkpoint.MidpointBadge"] : "50%",
                IconEmoji = "⚡"
            },
            new()
            {
                Order = 3,
                Title = loc != null ? loc["Checkpoint.PenultimateTitle"] : "Three-Quarter Stretch (75%)",
                Phase = loc != null ? loc["Checkpoint.PenultimatePhase"] : "Phase III - Final Approach",
                Description = loc != null ? loc["Checkpoint.PenultimateDesc", q3] : $"Approaching the final stretch of the campaign ({q3} scenarios played).",
                AtScenarioOrGameIndex = q3,
                BadgeText = loc != null ? loc["Checkpoint.PenultimateBadge"] : "75%",
                IconEmoji = "🧭"
            },
            new()
            {
                Order = 4,
                Title = loc != null ? loc["Checkpoint.FinaleTitle"] : "Campaign Finale (100%)",
                Phase = loc != null ? loc["Checkpoint.FinalePhase"] : "Phase IV - Campaign Completion",
                Description = loc != null ? loc["Checkpoint.FinaleDesc", q4] : $"Final meetup to complete all {q4} campaign scenarios.",
                AtScenarioOrGameIndex = q4,
                BadgeText = loc != null ? loc["Checkpoint.FinaleBadge"] : "100%",
                IconEmoji = "🏁"
            }
        };
    }

    private List<ScheduledSession> GenerateSessions(
        PlannerInputModel input,
        int totalPlays,
        int scenariosPerMeetup,
        double averageSessionHours,
        List<GameMilestone> milestones)
    {
        var sessions = new List<ScheduledSession>();
        var preferredDays = (input.PreferredDaysOfWeek != null && input.PreferredDaysOfWeek.Any())
            ? input.PreferredDaysOfWeek.Distinct().OrderBy(d => (int)d).ToList()
            : new List<DayOfWeek> { DayOfWeek.Friday };

        DateTime currentDate = input.StartDate;
        int currentScenario = 1;
        int sessionNum = 1;

        // Align start date to first preferred day if needed
        if (input.Frequency == FrequencyMode.TimesPerWeek || input.Frequency == FrequencyMode.BiWeekly)
        {
            while (!preferredDays.Contains(currentDate.DayOfWeek))
            {
                currentDate = currentDate.AddDays(1);
            }
        }

        while (currentScenario <= totalPlays)
        {
            int sessionPlayCount = Math.Min(scenariosPerMeetup, totalPlays - currentScenario + 1);
            int startIdx = currentScenario;
            int endIdx = currentScenario + sessionPlayCount - 1;

            // Find if any milestone is reached in this session (prefer highest milestone reached in this session)
            var milestoneHit = milestones
                .OrderByDescending(m => m.Order)
                .FirstOrDefault(m => m.AtScenarioOrGameIndex >= startIdx && m.AtScenarioOrGameIndex <= endIdx);

            sessions.Add(new ScheduledSession
            {
                SessionNumber = sessionNum,
                Date = currentDate,
                StartTime = input.StartTime,
                StartScenarioIndex = startIdx,
                EndScenarioIndex = endIdx,
                ScenariosPlayedInSession = sessionPlayCount,
                EstimatedSessionHours = averageSessionHours,
                MilestoneNote = milestoneHit?.Title,
                MilestonePhase = milestoneHit?.Phase
            });

            currentScenario += sessionPlayCount;
            sessionNum++;

            if (currentScenario > totalPlays) break;

            // Step to next date based on frequency mode
            currentDate = GetNextSessionDate(currentDate, input, preferredDays);
        }

        return sessions;
    }

    private DateTime GetNextSessionDate(DateTime currentDate, PlannerInputModel input, List<DayOfWeek> preferredDays)
    {
        switch (input.Frequency)
        {
            case FrequencyMode.TimesPerWeek:
                if (preferredDays.Count > 1)
                {
                    // Find next day in preferred list
                    DateTime candidate = currentDate.AddDays(1);
                    while (!preferredDays.Contains(candidate.DayOfWeek))
                    {
                        candidate = candidate.AddDays(1);
                    }
                    return candidate;
                }
                else
                {
                    int times = Math.Max(1, input.TimesPerWeekCount);
                    int step = Math.Max(1, 7 / times);
                    return currentDate.AddDays(step);
                }

            case FrequencyMode.BiWeekly:
                // Jump 2 weeks, ensuring same preferred day
                return currentDate.AddDays(14);

            case FrequencyMode.TimesPerMonth:
                int timesPerMonth = Math.Max(1, input.TimesPerMonthCount);
                int daysInterval = (int)Math.Round(30.4375 / timesPerMonth);
                var nextMonthDate = currentDate.AddDays(daysInterval);
                if (preferredDays.Any())
                {
                    while (!preferredDays.Contains(nextMonthDate.DayOfWeek))
                    {
                        nextMonthDate = nextMonthDate.AddDays(1);
                    }
                }
                return nextMonthDate;

            case FrequencyMode.CustomDaysInterval:
                int interval = Math.Max(1, input.CustomDaysInterval);
                return currentDate.AddDays(interval);

            default:
                return currentDate.AddDays(7);
        }
    }

    public string GenerateIcsFile(CalculationResult result, Microsoft.Extensions.Localization.IStringLocalizer<bg_campaign_planner.Resources.AppResources>? loc = null)
    {
        var sb = new StringBuilder();
        sb.AppendLine("BEGIN:VCALENDAR");
        sb.AppendLine("VERSION:2.0");
        sb.AppendLine("PRODID:-//BoardGameCampaignPlanner//EN");
        string calName = loc != null ? loc["Ics.CalName", result.Game.Title] : $"{result.Game.Title} Campaign Schedule";
        sb.AppendLine($"X-WR-CALNAME:{EscapeIcs(calName)}");
        sb.AppendLine("CALSCALE:GREGORIAN");
        sb.AppendLine("METHOD:PUBLISH");

        string location = loc != null ? loc["Ics.Location"] : "Game Table";
        string scenarioUnit = (result.Game.Id != GameId.PandemicSeason0)
            ? (loc != null ? loc["Timeline.ColScenarios"] : "Scenarios")
            : (loc != null ? loc["Timeline.ColGames"] : "Games");

        foreach (var session in result.Sessions)
        {
            DateTime start = session.StartDateTime;
            DateTime end = session.EndDateTime;

            string uid = $"bg-session-{result.Game.Id}-{session.SessionNumber}-{session.Date:yyyyMMdd}@campaignplanner";
            string summary = loc != null
                ? loc["Ics.EventSummary", result.Game.Title, session.SessionNumber, scenarioUnit, session.StartScenarioIndex, session.EndScenarioIndex]
                : $"{result.Game.Title} - Meetup #{session.SessionNumber} ({scenarioUnit} {session.StartScenarioIndex}-{session.EndScenarioIndex})";

            string description = loc != null
                ? loc["Ics.EventDescription", session.SessionNumber, result.Game.Title, result.Scope.Name, session.EstimatedSessionHours.ToString("F1")]
                : $"Campaign session #{session.SessionNumber} for {result.Game.Title} ({result.Scope.Name}). Estimated play time: {session.EstimatedSessionHours:F1} hrs.";

            if (!string.IsNullOrEmpty(session.MilestoneNote))
            {
                description += loc != null
                    ? loc["Ics.EventMilestone", session.MilestoneNote, session.MilestonePhase ?? string.Empty]
                    : $"\\n🎯 CHECKPOINT: {session.MilestoneNote} ({session.MilestonePhase})";
            }

            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine($"UID:{uid}");
            sb.AppendLine($"DTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmssZ}");
            sb.AppendLine($"DTSTART:{start:yyyyMMddTHHmmss}");
            sb.AppendLine($"DTEND:{end:yyyyMMddTHHmmss}");
            sb.AppendLine($"SUMMARY:{EscapeIcs(summary)}");
            sb.AppendLine($"DESCRIPTION:{EscapeIcs(description)}");
            sb.AppendLine($"LOCATION:{EscapeIcs(location)}");
            sb.AppendLine("STATUS:CONFIRMED");
            sb.AppendLine("END:VEVENT");
        }

        sb.AppendLine("END:VCALENDAR");
        return sb.ToString();
    }

    private static string EscapeIcs(string text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        return text.Replace("\\", "\\\\")
                   .Replace(";", "\\;")
                   .Replace(",", "\\,");
    }
}

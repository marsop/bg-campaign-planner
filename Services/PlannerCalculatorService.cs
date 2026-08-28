using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bg_campaign_planner.Models;

namespace bg_campaign_planner.Services;

public class PlannerCalculatorService
{
    public CalculationResult Calculate(PlannerInputModel input, BoardGame game)
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

        // 4. Session schedule generation
        var sessions = GenerateSessions(input, totalEstimatedPlays, scenariosPerMeetup, averageSessionHours, game.Milestones);

        DateTime startDate = input.StartDate;
        DateTime projectedFinishDate = sessions.Count > 0 ? sessions[^1].Date : startDate;
        
        if (input.VacationWeeksBuffer > 0 && sessions.Count > 0)
        {
            projectedFinishDate = projectedFinishDate.AddDays(input.VacationWeeksBuffer * 7);
        }

        int totalCalendarDays = Math.Max(1, (int)(projectedFinishDate - startDate).TotalDays);
        double totalCalendarWeeks = Math.Round(totalCalendarDays / 7.0, 1);
        double totalCalendarMonths = Math.Round(totalCalendarDays / 30.4375, 1);

        // 5. Milestone projections
        var milestoneProjections = new List<MilestoneProjection>();
        double scaledMilestoneRatio = (double)totalEstimatedPlays / Math.Max(1, scope.BaseScenarioCount);

        foreach (var milestone in game.Milestones.OrderBy(m => m.Order))
        {
            int targetPlayIndex = (int)Math.Round(milestone.AtScenarioOrGameIndex * scaledMilestoneRatio);
            targetPlayIndex = Math.Min(targetPlayIndex, totalEstimatedPlays);

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

            // Find if any milestone is reached in this session
            var milestoneHit = milestones
                .OrderBy(m => m.Order)
                .FirstOrDefault(m => m.AtScenarioOrGameIndex >= startIdx && m.AtScenarioOrGameIndex <= endIdx);

            sessions.Add(new ScheduledSession
            {
                SessionNumber = sessionNum,
                Date = currentDate,
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

    public string GenerateIcsFile(CalculationResult result)
    {
        var sb = new StringBuilder();
        sb.AppendLine("BEGIN:VCALENDAR");
        sb.AppendLine("VERSION:2.0");
        sb.AppendLine("PRODID:-//BoardGameCampaignPlanner//EN");
        sb.AppendLine($"X-WR-CALNAME:{result.Game.Title} Campaign Schedule");
        sb.AppendLine("CALSCALE:GREGORIAN");
        sb.AppendLine("METHOD:PUBLISH");

        foreach (var session in result.Sessions)
        {
            DateTime start = session.Date.Date.AddHours(19); // 7:00 PM default start
            DateTime end = start.AddHours(session.EstimatedSessionHours);

            string uid = $"bg-session-{result.Game.Id}-{session.SessionNumber}-{session.Date:yyyyMMdd}@campaignplanner";
            string summary = $"{result.Game.Title} - Meetup #{session.SessionNumber} (Scenarios {session.StartScenarioIndex}-{session.EndScenarioIndex})";
            
            string description = $"Campaign session #{session.SessionNumber} for {result.Game.Title} ({result.Scope.Name}). Estimated play time: {session.EstimatedSessionHours:F1} hrs.";
            if (!string.IsNullOrEmpty(session.MilestoneNote))
            {
                description += $"\\n🎯 MILESTONE: {session.MilestoneNote} ({session.MilestonePhase})";
            }

            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine($"UID:{uid}");
            sb.AppendLine($"DTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmssZ}");
            sb.AppendLine($"DTSTART:{start:yyyyMMddTHHmmss}");
            sb.AppendLine($"DTEND:{end:yyyyMMddTHHmmss}");
            sb.AppendLine($"SUMMARY:{EscapeIcs(summary)}");
            sb.AppendLine($"DESCRIPTION:{EscapeIcs(description)}");
            sb.AppendLine($"LOCATION:Game Table");
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

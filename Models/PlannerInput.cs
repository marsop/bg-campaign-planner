using System;
using System.Collections.Generic;

namespace bg_campaign_planner.Models;

public enum FrequencyMode
{
    TimesPerWeek,
    BiWeekly,
    TimesPerMonth,
    CustomDaysInterval
}

public enum ExperienceLevel
{
    Beginner,
    Regular,
    Veteran
}

public class PlannerInputModel
{
    public GameId SelectedGameId { get; set; } = GameId.Gloomhaven;
    public string SelectedScopeId { get; set; } = string.Empty;
    
    // Frequency
    public FrequencyMode Frequency { get; set; } = FrequencyMode.TimesPerWeek;
    public int TimesPerWeekCount { get; set; } = 1;
    public int TimesPerMonthCount { get; set; } = 2;
    public int CustomDaysInterval { get; set; } = 7;
    public List<DayOfWeek> PreferredDaysOfWeek { get; set; } = new() { DayOfWeek.Friday };

    // Calendar
    public DateTime StartDate { get; set; } = DateTime.Today;
    public TimeOnly StartTime { get; set; } = new TimeOnly(17, 0);
    public int VacationWeeksBuffer { get; set; } = 0;
    
    // Session setup
    public int ScenariosPerMeetup { get; set; } = 1;
    public int PlayerCount { get; set; } = 3;
    public ExperienceLevel Experience { get; set; } = ExperienceLevel.Regular;
    public bool IncludeSetupTeardown { get; set; } = true;
    public bool EnableFailureBuffer { get; set; } = true;
}

public class ScheduledSession
{
    public int SessionNumber { get; set; }
    public DateTime Date { get; set; }
    public DayOfWeek DayOfWeek => Date.DayOfWeek;
    public TimeOnly StartTime { get; set; } = new TimeOnly(17, 0);
    public DateTime StartDateTime => Date.Date.Add(StartTime.ToTimeSpan());
    public DateTime EndDateTime => StartDateTime.AddHours(EstimatedSessionHours);
    public int StartScenarioIndex { get; set; }
    public int EndScenarioIndex { get; set; }
    public int ScenariosPlayedInSession { get; set; }
    public double EstimatedSessionHours { get; set; }
    public string? MilestoneNote { get; set; }
    public string? MilestonePhase { get; set; }
    public bool IsMilestoneSession => !string.IsNullOrEmpty(MilestoneNote);
}

public class MilestoneProjection
{
    public GameMilestone Milestone { get; set; } = new();
    public int TargetScenarioIndex { get; set; }
    public int EstimatedMeetupNumber { get; set; }
    public DateTime EstimatedDate { get; set; }
    public double HoursSpentUntilMilestone { get; set; }
}

public class CalculationResult
{
    public BoardGame Game { get; set; } = new();
    public CampaignScopeOption Scope { get; set; } = new();
    public int BaseScenariosOrGames { get; set; }
    public int BufferPlaysForFailures { get; set; }
    public int TotalEstimatedPlays { get; set; }
    public int TotalMeetupsRequired { get; set; }
    public double AverageSessionHours { get; set; }
    public double TotalTableHours { get; set; }
    public double TotalGameplayHours { get; set; }
    public double TotalSetupHours { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ProjectedFinishDate { get; set; }
    public int TotalCalendarDays { get; set; }
    public double TotalCalendarWeeks { get; set; }
    public double TotalCalendarMonths { get; set; }
    public List<ScheduledSession> Sessions { get; set; } = new();
    public List<MilestoneProjection> MilestoneProjections { get; set; } = new();
}

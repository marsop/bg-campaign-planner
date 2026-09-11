namespace bg_campaign_planner.Services;

/// <summary>
/// Provides application version metadata generated at build-time by Nerdbank.GitVersioning.
/// </summary>
public static class AppVersion
{
    public static string DisplayVersion { get; }
    public static string CommitShortHash { get; }
    public static string GitCommitId { get; }
    public static string InformationalVersion { get; }
    public static DateTime? CommitDate { get; }
    public static string CommitUrl { get; }
    public static string ReleasesUrl => "https://github.com/marsop/bg-campaign-planner/releases";
    public static string RepositoryUrl => "https://github.com/marsop/bg-campaign-planner";

    static AppVersion()
    {
        string infoVersion;
        string commitId;
        string fileVersion;
        DateTime? commitDate = null;

        try
        {
            infoVersion = global::ThisAssembly.AssemblyInformationalVersion;
            commitId = global::ThisAssembly.GitCommitId;
            fileVersion = global::ThisAssembly.AssemblyFileVersion;
            commitDate = global::ThisAssembly.GitCommitDate;
        }
        catch
        {
            var asm = typeof(AppVersion).Assembly;
            infoVersion = asm.GetName().Version?.ToString() ?? "0.3.0";
            commitId = string.Empty;
            fileVersion = infoVersion;
        }

        InformationalVersion = string.IsNullOrWhiteSpace(infoVersion) ? "0.3.0" : infoVersion;
        GitCommitId = commitId ?? string.Empty;
        CommitDate = commitDate;

        if (!string.IsNullOrEmpty(GitCommitId) && GitCommitId.Length >= 7)
        {
            CommitShortHash = GitCommitId[..7];
        }
        else
        {
            CommitShortHash = string.Empty;
        }

        // Clean display version (e.g., "0.3.1" from "0.3.1+c55363e961")
        var plusIndex = InformationalVersion.IndexOf('+');
        var baseVersion = plusIndex > 0 ? InformationalVersion[..plusIndex] : InformationalVersion;

        DisplayVersion = string.IsNullOrWhiteSpace(baseVersion) 
            ? (string.IsNullOrWhiteSpace(fileVersion) ? "0.3.0" : fileVersion) 
            : baseVersion;

        if (!string.IsNullOrEmpty(GitCommitId))
        {
            CommitUrl = $"https://github.com/marsop/bg-campaign-planner/commit/{GitCommitId}";
        }
        else
        {
            CommitUrl = RepositoryUrl;
        }
    }
}

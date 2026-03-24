namespace LOLFriendsCleaner;

/// <summary>
/// Persisted whitelist configuration loaded from / saved to whitelist.json.
/// Friends matching any entry here will be skipped during removal.
/// </summary>
public class FilterConfig
{
    /// <summary>Game names (gameName field) that should never be removed.</summary>
    public List<string> ExcludedNames { get; set; } = new();

    /// <summary>Group names whose members should never be removed.</summary>
    public List<string> ExcludedGroups { get; set; } = new();
}

using PoniLCU;
using System.Text.Json;
using LOLFriendsCleaner;
using static PoniLCU.LeagueClient;

// ── Helpers ──────────────────────────────────────────────────────────────────

static void SetColor(ConsoleColor fg) => Console.ForegroundColor = fg;
static void ResetColor() => Console.ResetColor();

static void PrintLine(string text, ConsoleColor color = ConsoleColor.Gray)
{
    SetColor(color);
    Console.WriteLine(text);
    ResetColor();
}

static void PrintBanner()
{
    Console.Clear();
    SetColor(ConsoleColor.Cyan);
    Console.WriteLine("╔══════════════════════════════════════════════════╗");
    Console.WriteLine("║        League of Legends  Friends Cleaner        ║");
    Console.WriteLine("║                    by Appercy                   ║");
    Console.WriteLine("╚══════════════════════════════════════════════════╝");
    ResetColor();
    Console.WriteLine();
}

static void PrintSection(string title)
{
    SetColor(ConsoleColor.DarkCyan);
    Console.WriteLine($"  ── {title} ──────────────────────────────");
    ResetColor();
}

// ── Filter persistence ───────────────────────────────────────────────────────

const string FilterFile = "whitelist.json";

static FilterConfig LoadFilter()
{
    if (!File.Exists(FilterFile))
        return new FilterConfig();

    try
    {
        var json = File.ReadAllText(FilterFile);
        return JsonSerializer.Deserialize<FilterConfig>(json) ?? new FilterConfig();
    }
    catch (Exception ex)
    {
        PrintLine($"  [!] Could not read {FilterFile}: {ex.Message}. Starting with an empty filter list.", ConsoleColor.Yellow);
        return new FilterConfig();
    }
}

static void SaveFilter(FilterConfig filter)
{
    try
    {
        File.WriteAllText(FilterFile, JsonSerializer.Serialize(filter, new JsonSerializerOptions { WriteIndented = true }));
    }
    catch (Exception ex)
    {
        PrintLine($"  [!] Could not save {FilterFile}: {ex.Message}", ConsoleColor.Yellow);
    }
}

// ── Filter management menu ───────────────────────────────────────────────────

static void ManageFilters(FilterConfig filter, List<Friend> friends)
{
    while (true)
    {
        Console.Clear();
        PrintBanner();
        PrintSection("Filter / Whitelist Manager");
        Console.WriteLine();

        // Show current whitelist
        if (filter.ExcludedNames.Count == 0 && filter.ExcludedGroups.Count == 0)
        {
            PrintLine("  (filter list is empty)", ConsoleColor.DarkGray);
        }
        else
        {
            if (filter.ExcludedNames.Count > 0)
            {
                SetColor(ConsoleColor.Green);
                Console.Write("  Protected names : ");
                ResetColor();
                Console.WriteLine(string.Join(", ", filter.ExcludedNames));
            }
            if (filter.ExcludedGroups.Count > 0)
            {
                SetColor(ConsoleColor.Green);
                Console.Write("  Protected groups: ");
                ResetColor();
                Console.WriteLine(string.Join(", ", filter.ExcludedGroups));
            }
        }

        Console.WriteLine();
        PrintSection("Options");
        PrintLine("  [1] Add a friend name to whitelist", ConsoleColor.White);
        PrintLine("  [2] Add a group name to whitelist", ConsoleColor.White);
        PrintLine("  [3] Remove a name from whitelist", ConsoleColor.White);
        PrintLine("  [4] Remove a group from whitelist", ConsoleColor.White);
        PrintLine("  [5] List all friends with their groups", ConsoleColor.White);
        PrintLine("  [6] Back to main menu", ConsoleColor.DarkGray);
        Console.WriteLine();

        SetColor(ConsoleColor.Cyan);
        Console.Write("  > ");
        ResetColor();

        var choice = Console.ReadLine()?.Trim();
        Console.WriteLine();

        switch (choice)
        {
            case "1":
                PrintLine("  Enter the game name to protect (e.g. NekoPhoenyxChan):", ConsoleColor.White);
                SetColor(ConsoleColor.Cyan);
                Console.Write("  > ");
                ResetColor();
                var name = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(name))
                {
                    if (!filter.ExcludedNames.Contains(name, StringComparer.OrdinalIgnoreCase))
                    {
                        filter.ExcludedNames.Add(name);
                        SaveFilter(filter);
                        PrintLine($"  ✔ Added '{name}' to whitelist.", ConsoleColor.Green);
                    }
                    else
                    {
                        PrintLine($"  '{name}' is already in the whitelist.", ConsoleColor.Yellow);
                    }
                }
                Thread.Sleep(1000);
                break;

            case "2":
                PrintLine("  Enter the group name to protect:", ConsoleColor.White);
                SetColor(ConsoleColor.Cyan);
                Console.Write("  > ");
                ResetColor();
                var group = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(group))
                {
                    if (!filter.ExcludedGroups.Contains(group, StringComparer.OrdinalIgnoreCase))
                    {
                        filter.ExcludedGroups.Add(group);
                        SaveFilter(filter);
                        PrintLine($"  ✔ Added group '{group}' to whitelist.", ConsoleColor.Green);
                    }
                    else
                    {
                        PrintLine($"  Group '{group}' is already in the whitelist.", ConsoleColor.Yellow);
                    }
                }
                Thread.Sleep(1000);
                break;

            case "3":
                if (filter.ExcludedNames.Count == 0)
                {
                    PrintLine("  No names in the whitelist.", ConsoleColor.Yellow);
                    Thread.Sleep(1000);
                    break;
                }
                PrintLine("  Current protected names:", ConsoleColor.White);
                for (int i = 0; i < filter.ExcludedNames.Count; i++)
                    PrintLine($"    [{i + 1}] {filter.ExcludedNames[i]}", ConsoleColor.White);
                PrintLine("  Enter number to remove (or 0 to cancel):", ConsoleColor.White);
                SetColor(ConsoleColor.Cyan);
                Console.Write("  > ");
                ResetColor();
                if (int.TryParse(Console.ReadLine(), out int nameIdx) && nameIdx >= 1 && nameIdx <= filter.ExcludedNames.Count)
                {
                    var removed = filter.ExcludedNames[nameIdx - 1];
                    filter.ExcludedNames.RemoveAt(nameIdx - 1);
                    SaveFilter(filter);
                    PrintLine($"  ✔ Removed '{removed}' from whitelist.", ConsoleColor.Green);
                }
                Thread.Sleep(1000);
                break;

            case "4":
                if (filter.ExcludedGroups.Count == 0)
                {
                    PrintLine("  No groups in the whitelist.", ConsoleColor.Yellow);
                    Thread.Sleep(1000);
                    break;
                }
                PrintLine("  Current protected groups:", ConsoleColor.White);
                for (int i = 0; i < filter.ExcludedGroups.Count; i++)
                    PrintLine($"    [{i + 1}] {filter.ExcludedGroups[i]}", ConsoleColor.White);
                PrintLine("  Enter number to remove (or 0 to cancel):", ConsoleColor.White);
                SetColor(ConsoleColor.Cyan);
                Console.Write("  > ");
                ResetColor();
                if (int.TryParse(Console.ReadLine(), out int groupIdx) && groupIdx >= 1 && groupIdx <= filter.ExcludedGroups.Count)
                {
                    var removed = filter.ExcludedGroups[groupIdx - 1];
                    filter.ExcludedGroups.RemoveAt(groupIdx - 1);
                    SaveFilter(filter);
                    PrintLine($"  ✔ Removed group '{removed}' from whitelist.", ConsoleColor.Green);
                }
                Thread.Sleep(1000);
                break;

            case "5":
                Console.Clear();
                PrintBanner();
                PrintSection("All Friends");
                Console.WriteLine();
                if (friends.Count == 0)
                {
                    PrintLine("  (no friends found)", ConsoleColor.DarkGray);
                }
                else
                {
                    // Group by group name for display
                    var grouped = friends
                        .GroupBy(f => string.IsNullOrEmpty(f.groupName) ? "(no group)" : f.groupName)
                        .OrderBy(g => g.Key);

                    foreach (var grp in grouped)
                    {
                        SetColor(ConsoleColor.DarkYellow);
                        Console.WriteLine($"  [{grp.Key}]");
                        ResetColor();
                        foreach (var f in grp.OrderBy(f => f.gameName))
                        {
                            bool isProtected = IsFiltered(f, filter);
                            var displayName = string.IsNullOrEmpty(f.gameName) ? f.name : $"{f.gameName}#{f.gameTag}";
                            if (isProtected)
                            {
                                SetColor(ConsoleColor.Green);
                                Console.WriteLine($"    🔒 {displayName}");
                            }
                            else
                            {
                                SetColor(ConsoleColor.Red);
                                Console.WriteLine($"    ✗  {displayName}");
                            }
                            ResetColor();
                        }
                        Console.WriteLine();
                    }
                }
                PrintLine("  Press Enter to return...", ConsoleColor.DarkGray);
                Console.ReadLine();
                break;

            case "6":
            default:
                return;
        }
    }
}

// ── Filter check helper ──────────────────────────────────────────────────────

static bool IsFiltered(Friend friend, FilterConfig filter)
{
    // Check by game name (case-insensitive)
    if (!string.IsNullOrEmpty(friend.gameName) &&
        filter.ExcludedNames.Contains(friend.gameName, StringComparer.OrdinalIgnoreCase))
        return true;

    // Also check legacy name field
    if (!string.IsNullOrEmpty(friend.name) &&
        filter.ExcludedNames.Contains(friend.name, StringComparer.OrdinalIgnoreCase))
        return true;

    // Check by group name (groupName or displayGroupName, case-insensitive)
    if (!string.IsNullOrEmpty(friend.groupName) &&
        filter.ExcludedGroups.Contains(friend.groupName, StringComparer.OrdinalIgnoreCase))
        return true;

    if (!string.IsNullOrEmpty(friend.displayGroupName) &&
        filter.ExcludedGroups.Contains(friend.displayGroupName, StringComparer.OrdinalIgnoreCase))
        return true;

    return false;
}

// ── Main ─────────────────────────────────────────────────────────────────────

var leagueClient = new LeagueClient(credentials.cmd);

// Load filter config
var filter = LoadFilter();

// Fetch friends list
var data = await leagueClient.Request(requestMethod.GET, "/lol-chat/v1/friends");
var friends = JsonSerializer.Deserialize<List<Friend>>(data) ?? new List<Friend>();

// Main menu loop
while (true)
{
    var toRemove = friends.Where(f => !IsFiltered(f, filter)).ToList();
    var toKeep   = friends.Where(f =>  IsFiltered(f, filter)).ToList();

    PrintBanner();

    PrintSection("Friends Overview");
    Console.WriteLine();
    SetColor(ConsoleColor.White);
    Console.Write($"  Total friends : ");
    SetColor(ConsoleColor.Cyan);
    Console.WriteLine(friends.Count);

    SetColor(ConsoleColor.White);
    Console.Write($"  Will be kept  : ");
    SetColor(ConsoleColor.Green);
    Console.WriteLine(toKeep.Count);

    SetColor(ConsoleColor.White);
    Console.Write($"  Will be removed: ");
    SetColor(ConsoleColor.Red);
    Console.WriteLine(toRemove.Count);
    ResetColor();

    if (filter.ExcludedNames.Count > 0 || filter.ExcludedGroups.Count > 0)
    {
        Console.WriteLine();
        SetColor(ConsoleColor.Green);
        Console.Write("  🔒 Protected names : ");
        ResetColor();
        Console.WriteLine(filter.ExcludedNames.Count > 0
            ? string.Join(", ", filter.ExcludedNames)
            : "(none)");

        SetColor(ConsoleColor.Green);
        Console.Write("  🔒 Protected groups: ");
        ResetColor();
        Console.WriteLine(filter.ExcludedGroups.Count > 0
            ? string.Join(", ", filter.ExcludedGroups)
            : "(none)");
    }

    Console.WriteLine();
    PrintSection("Options");
    PrintLine("  [1] Remove friends (respects filter list)", ConsoleColor.White);
    PrintLine("  [2] Manage filter / whitelist", ConsoleColor.White);
    PrintLine("  [3] Exit", ConsoleColor.DarkGray);
    Console.WriteLine();

    SetColor(ConsoleColor.Cyan);
    Console.Write("  > ");
    ResetColor();

    var input = Console.ReadLine()?.Trim();
    Console.WriteLine();

    if (input == "2")
    {
        ManageFilters(filter, friends);
        continue;
    }

    if (input == "1")
    {
        if (toRemove.Count == 0)
        {
            PrintLine("  Nothing to remove — all friends are on the filter list!", ConsoleColor.Yellow);
            Thread.Sleep(2000);
            continue;
        }

        PrintSection("Confirm Removal");
        SetColor(ConsoleColor.Yellow);
        Console.WriteLine($"  ⚠  You are about to remove {toRemove.Count} friend(s).");
        if (toKeep.Count > 0)
            Console.WriteLine($"     {toKeep.Count} friend(s) will be kept (whitelist).");
        ResetColor();
        Console.WriteLine();
        PrintLine("  Type 'yes' to confirm, anything else to cancel:", ConsoleColor.White);

        SetColor(ConsoleColor.Cyan);
        Console.Write("  > ");
        ResetColor();

        var confirm = Console.ReadLine()?.Trim();
        Console.WriteLine();

        if (!string.Equals(confirm, "yes", StringComparison.OrdinalIgnoreCase))
        {
            PrintLine("  Cancelled.", ConsoleColor.DarkGray);
            Thread.Sleep(1000);
            continue;
        }

        // Removal loop
        int removed = 0;
        int skipped = 0;
        int friendIndex = 0;

        PrintSection("Removing Friends");
        Console.WriteLine();

        foreach (var friend in friends)
        {
            friendIndex++;
            var displayName = string.IsNullOrEmpty(friend.gameName)
                ? friend.name ?? "Unknown"
                : $"{friend.gameName}#{friend.gameTag}";

            if (IsFiltered(friend, filter))
            {
                SetColor(ConsoleColor.Green);
                Console.WriteLine($"  🔒 Kept    {displayName}");
                ResetColor();
                skipped++;
                continue;
            }

            await leagueClient.Request(requestMethod.DELETE, $"/lol-chat/v1/friends/{friend.puuid}");
            SetColor(ConsoleColor.Red);
            Console.WriteLine($"  ✗  Removed {displayName}");
            ResetColor();
            removed++;

            if (friendIndex % 3 is 0)
            {
                // The client has a rate limit, which might trigger a warning to RiotGames (causing a ban). Avoid this with a delay.
                await Task.Delay(1000);
            }
        }

        Console.WriteLine();
        PrintSection("Summary");
        Console.WriteLine();
        SetColor(ConsoleColor.Red);
        Console.WriteLine($"  Removed : {removed}");
        SetColor(ConsoleColor.Green);
        Console.WriteLine($"  Kept    : {skipped}");
        ResetColor();
        Console.WriteLine();

        if (removed > 0)
            PrintLine("  ✔ Done! Please restart your game to make sure everything is synced.", ConsoleColor.Cyan);

        Console.WriteLine();
        PrintLine("  Press Enter to exit...", ConsoleColor.DarkGray);
        Console.ReadLine();
        break;
    }

    if (input == "3")
    {
        PrintLine("  Goodbye!", ConsoleColor.DarkGray);
        Thread.Sleep(800);
        break;
    }
}
# League of Legends Friends Cleaner

A small League of Legends tool that lets you **bulk-remove friends** while keeping the ones you actually care about.  
Written and maintained by **Appercy**, based on [PoniLCU](https://github.com/Ponita0/PoniLCU).

---

## Features

- 🗑️ **Bulk remove** all friends in one go
- 🔒 **Whitelist by name** – protect specific friends (e.g. `NekoPhoenyxChan`) so they are never removed
- 🔒 **Whitelist by group** – protect every friend inside a named friend-group (e.g. `BestFriends`)
- 💾 **Persistent filter list** – your whitelist is saved to `whitelist.json` and loaded automatically on every run
- 🎨 **Improved terminal UI** – coloured output, a summary screen, and a friendly menu

---

## How to use

1. Start **League of Legends** and log in (the client must be running).
2. Run `LOLFriendsCleaner.exe`.
3. The main menu shows you how many friends will be removed vs. kept.

```
╔══════════════════════════════════════════════════╗
║        League of Legends  Friends Cleaner        ║
║                    by Appercy                   ║
╚══════════════════════════════════════════════════╝

  ── Friends Overview ──────────────────────────────

  Total friends :  42
  Will be kept  :   3
  Will be removed: 39

  🔒 Protected names : NekoPhoenyxChan, AnotherFriend
  🔒 Protected groups: BestFriends

  ── Options ───────────────────────────────────────
  [1] Remove friends (respects filter list)
  [2] Manage filter / whitelist
  [3] Exit
```

### Managing the whitelist

Choose **[2] Manage filter / whitelist** from the main menu:

| Option | What it does |
|--------|--------------|
| **1** | Add a friend's game name (e.g. `NekoPhoenyxChan`) to the whitelist |
| **2** | Add an entire group by its group name (e.g. `BestFriends`) |
| **3** | Remove a name from the whitelist |
| **4** | Remove a group from the whitelist |
| **5** | List all friends colour-coded (🔒 kept / ✗ will be removed) |

The whitelist is saved to **`whitelist.json`** next to the executable so it persists between runs.

### Removing friends

Choose **[1] Remove friends** from the main menu.  
You will see a confirmation prompt and then a live log:

```
  ── Removing Friends ──────────────────────────────

  🔒 Kept    NekoPhoenyxChan#EUW
  ✗  Removed SomeRandomDude#1234
  ✗  Removed AnotherOne#5678
  ...

  ── Summary ──────────────────────────────────────

  Removed : 39
  Kept    :  3

  ✔ Done! Please restart your game to make sure everything is synced.
```

---

## Notes

- A 1-second delay is added after every 3 removals to respect the League client's rate limit and avoid any risk of a penalty.
- Names are matched **case-insensitively** (e.g. `neKOPhOenyxChan` matches `NekoPhoenyxChan`).
- Group matching uses both the internal group name and the display group name, also case-insensitively.

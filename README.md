# League of Legends Friends Cleaner

A small League of Legends tool that lets you **bulk-remove friends** while keeping the ones you actually care about.  
Written and maintained by **Appercy**, based on [PoniLCU](https://github.com/Ponita0/PoniLCU).

---

## Download

Grab the latest **LOLFriendsCleaner.exe** from the [Releases](../../releases/latest) page — no installation or .NET runtime required.  
A new `.exe` is built and published automatically whenever a new version tag is pushed.

---

## Features

- 🗑️ **Bulk remove** all friends in one go
- 🔒 **Whitelist by name** – protect specific friends (e.g. `Friend1`) so they are never removed
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

  🔒 Protected names : Friend1, AnotherFriend
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
| **1** | Add a friend's game name (e.g. `Friend1`) to the whitelist |
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

  🔒 Kept    Friend1#EUW
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
- Names are matched **case-insensitively** (e.g. `Friend1` matches `Friend1`).
- Group matching uses both the internal group name and the display group name, also case-insensitively.

---

## Publishing a new release (for maintainers)

The GitHub Actions workflow in `.github/workflows/release.yml` builds and releases automatically.  
To publish a new version just push a version tag:

```bash
git tag v1.2.0
git push origin v1.2.0
```

The workflow will:
1. Build a **self-contained, single-file** `LOLFriendsCleaner.exe` for Windows x64 (no .NET install needed).
2. Create a GitHub Release named after the tag and attach the `.exe` to it.

# AssetRipper CLI

[![中文](https://img.shields.io/badge/README-%E4%B8%AD%E6%96%87-blue?style=for-the-badge)](README.md)

A command-line tool for extracting assets from Unity games — CG, audio, backgrounds, characters, and more.

Built on [AssetRipper](https://github.com/AssetRipper/AssetRipper), repackaged as a focused CLI workflow.

> **Disclaimer**
>
> This tool is a general-purpose Unity asset parser and exporter, intended solely for lawful purposes such as academic research, personal backup, and game modding. Users are responsible for ensuring their use complies with applicable laws and the license terms of the content they process. This project does not encourage, facilitate, or endorse any form of copyright infringement. All legal liability arising from use of this tool rests entirely with the user and is unrelated to this project or its contributors.
>
> If you are a rights holder and believe this project infringes your rights, please see the [DMCA](#dmca) section below.

## Prerequisites

- [.NET SDK 10](https://dotnet.microsoft.com/download) (version >= `10.0.200`)

```powershell
# Windows
winget install Microsoft.DotNet.SDK.10

# Verify version
dotnet --version
# Should be >= 10.0.200
```

## Build

```powershell
dotnet build AssetRipperCLI.slnx -c Release
```

For convenience, set an alias for the built binary:

```powershell
# PowerShell
Set-Alias arc .\artifacts\bin\AssetRipper.Tools.ExportRunner\Release\net10.0\AssetRipper.Tools.ExportRunner.exe
```

```bash
# Bash / Zsh
alias arc="dotnet ./artifacts/bin/AssetRipper.Tools.ExportRunner/Release/net10.0/AssetRipper.Tools.ExportRunner.dll"
```

All examples below use `arc` as the alias.

## Quick Start

### 1. Inspect — see what's inside

```powershell
arc inspect .\GameRoot
```

Prints asset counts, suggested export profiles, and a content breakdown.

### 2. Export — pull out what you want

Extract CG illustrations:

```powershell
arc export .\GameRoot --output .\output\cg --profile cg
```

Extract audio (BGM, voice, SFX):

```powershell
arc export .\GameRoot --output .\output\audio --profile audio
```

Extract everything:

```powershell
arc export .\GameRoot --output .\output\all --mode primary
```

### 3. Review — check what was exported

```powershell
arc report .\output\cg\export-manifest.json
```

## Profiles

Profiles let you target specific asset categories instead of exporting the entire game.

| Profile | What it targets |
|---|---|
| `cg` | Event CG, gallery stills, story illustrations |
| `audio` | BGM, voice lines, sound effects |
| `characters` | Portraits, standing art, busts |
| `backgrounds` | Scene and background art |
| `sprites` | 2D sprite assets |
| `ui` | UI elements and interface art |
| `player-art` | Player-facing visual assets |
| `narrative` | Story and dialogue-related assets |
| `full-raw` | All primary content, no filtering |
| `full-project` | Full Unity project dump |

Profiles are heuristic — they work well for most games but may include false positives or miss edge cases. Start with a targeted profile, then widen to `full-raw` if you need more.

## Commands

| Command | Purpose |
|---|---|
| `inspect <path>` | Quick content summary — run this first |
| `analyze <path> --report out.json` | Same as inspect, plus saves a JSON artifact |
| `export <path> --output <dir> --profile <name>` | Export with a profile filter |
| `export <path> --output <dir> --mode primary\|dump` | Export by backend mode |
| `report <artifact.json>` | Render a saved artifact as readable text |

### Export Options

```
--keep-output                   Don't clean the output directory before export
--recursive-unpack on|off       Unpack nested bundles (default: on)
--shard-strategy off|direct-children|auto
                                Shard large depots into parallel jobs
```

## Output Artifacts

Each export writes structured artifacts alongside the exported files:

| File | Contents |
|---|---|
| `export-plan.json` | What was planned for export |
| `export-manifest.json` | What was actually exported |
| `summary.txt` | Human-readable run summary |
| `skipped-assets.json` | Collections skipped by profile filtering |
| `failed-assets.json` | Collections that failed during export |

Use `arc report <file>` to render any artifact back to the console.

## Suggested Workflow

```
inspect  →  pick a profile  →  export  →  report
```

1. Run `inspect` to understand the game's content
2. Choose a profile (`cg`, `audio`, `characters`, etc.)
3. Export with that profile
4. If too narrow, try `full-raw`; if you need a Unity project, try `full-project`
5. Use `report` to review manifests and check for skipped/failed assets

## Known Limitations

- **Profile selection is heuristic** — profiles are workflow shortcuts, not perfect classifiers. Review your output.
- **Some games emit importer warnings** — the export usually still completes. Check `failed-assets.json` for specifics.
- **Static assets only** — Spine, Live2D, and Cubism reconstruction are out of scope.

## Repository Layout

```
src/              CLI source code
vendor/           Vendored upstream AssetRipper libraries
docs/             Detailed usage guide, architecture notes, known limitations
```

## Documentation

- [Usage Guide](docs/articles/CliUsageGuide.md) — full command reference
- [Implementation Status](docs/articles/CliImplementationStatus.md) — internals and execution model
- [Known Limitations](docs/articles/CodeReviewFindings.md) — current issues and validation notes
- [Architecture Notes](docs/articles/CliArchitectureRefactor.md) — design decisions

## DMCA

This project respects intellectual property rights. If you are a rights holder and believe content in this repository infringes your rights, please open a GitHub Issue or follow the [GitHub DMCA process](https://docs.github.com/en/site-policy/content-removal-policies/dmca-takedown-policy). We will respond promptly.

## Acknowledgments

Built on [AssetRipper](https://github.com/AssetRipper/AssetRipper) — thanks to the upstream project for their excellent work.

## License

This repository is distributed under [GPL-3.0](LICENSE).

It vendors and modifies code from [AssetRipper](https://github.com/AssetRipper/AssetRipper). For bundled third-party licenses and attribution, see [THIRD_PARTY_NOTICES.md](THIRD_PARTY_NOTICES.md) and [vendor/assetripper/Source/Licenses](vendor/assetripper/Source/Licenses).

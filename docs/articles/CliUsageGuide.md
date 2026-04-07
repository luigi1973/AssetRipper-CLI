# CLI Usage Guide

This guide documents the current command-line surface of `AssetRipper.Tools.ExportRunner` in this repository.

## Build Output

This repository should be documented and tested with Windows users in mind first. The examples below therefore use PowerShell syntax.

Install the SDK on Windows:

```powershell
winget install Microsoft.DotNet.SDK.10
```

Build the solution:

```powershell
dotnet build AssetRipperCLI.slnx -c Release
```

Run the built binary on Windows:

```powershell
.\artifacts\bin\AssetRipper.Tools.ExportRunner\Release\net10.0\AssetRipper.Tools.ExportRunner.exe
```

Portable invocation:

```powershell
dotnet .\artifacts\bin\AssetRipper.Tools.ExportRunner\Release\net10.0\AssetRipper.Tools.ExportRunner.dll
```

## Commands

### Inspect

Use `inspect` to understand what a game contains before exporting.

```bash
AssetRipper.Tools.ExportRunner inspect <input-path> [more-input-paths...]
```

What it prints:

- project version
- asset collection count
- asset count
- resource file count
- path semantics classification
- suggested profiles
- profile evidence
- top asset classes
- top output buckets

Example:

```powershell
dotnet .\artifacts\bin\AssetRipper.Tools.ExportRunner\Release\net10.0\AssetRipper.Tools.ExportRunner.dll `
  inspect .\input\GameRoot
```

### Analyze

Use `analyze` when you want the same inventory pass plus a JSON artifact.

```bash
AssetRipper.Tools.ExportRunner analyze <input-path> [more-input-paths...] [--report <report-path>]
```

Artifact written by `--report`:

- `inventory-summary`

Example:

```powershell
dotnet .\artifacts\bin\AssetRipper.Tools.ExportRunner\Release\net10.0\AssetRipper.Tools.ExportRunner.dll `
  analyze .\input\GameRoot --report .\output\analysis.json
```

### Export

Use `export` for normal work.

```bash
AssetRipper.Tools.ExportRunner export <input-path> [more-input-paths...] \
  --output <output-path> \
  [--profile <profile> | --mode <primary|dump>] \
  [--keep-output] \
  [--recursive-unpack on|off] \
  [--shard-strategy off|direct-children|auto]
```

Supported backend modes:

- `primary`
- `dump`

Supported profiles:

- `player-art`
- `characters`
- `ui`
- `audio`
- `narrative`
- `cg`
- `backgrounds`
- `sprites`
- `full-project`
- `full-raw`

Profile mapping:

- `full-project` -> `dump`
- `full-raw` -> `primary`
- all other profiles -> `primary` with heuristic filtering

Examples:

```powershell
dotnet .\artifacts\bin\AssetRipper.Tools.ExportRunner\Release\net10.0\AssetRipper.Tools.ExportRunner.dll `
  export .\input\GameRoot --output .\output\game_primary --mode primary
```

```powershell
dotnet .\artifacts\bin\AssetRipper.Tools.ExportRunner\Release\net10.0\AssetRipper.Tools.ExportRunner.dll `
  export .\input\GameRoot --output .\output\game_cg --profile cg
```

```powershell
dotnet .\artifacts\bin\AssetRipper.Tools.ExportRunner\Release\net10.0\AssetRipper.Tools.ExportRunner.dll `
  export .\input\GameRoot --output .\output\game_dump --mode dump
```

### Report

Use `report` to render an existing artifact back into a readable summary.

```bash
AssetRipper.Tools.ExportRunner report <artifact-path>
```

Supported artifact types:

- `inventory-summary`
- `export-plan`
- `export-manifest`
- `recursive-unpack`
- `skipped-assets`
- `failed-assets`

Example:

```powershell
dotnet .\artifacts\bin\AssetRipper.Tools.ExportRunner\Release\net10.0\AssetRipper.Tools.ExportRunner.dll `
  report .\output\game_cg\export-manifest.json
```

## Legacy Direct Commands

These still work as compatibility entrypoints:

```bash
AssetRipper.Tools.ExportRunner primary <input-path> <output-path> [more-input-paths...]
AssetRipper.Tools.ExportRunner dump <input-path> <output-path> [more-input-paths...]
```

They are backend-first compatibility paths. Prefer `export` for normal usage.

If you are on macOS or Linux, the same commands work with `/` path separators and your shell's normal line continuation syntax.

## Common Profiles

### `cg`

This profile targets static CG-style assets.

Good fit:

- event illustrations
- gallery stills
- memory images
- static story art
- static authored background or scene art stored directly as textures or static prefab-backed visuals

Out of scope:

- Spine reconstruction
- Live2D reconstruction
- Cubism reconstruction
- animated runtime scene composition

### `audio`

Use when the goal is BGM, voice, or other sound assets.

### `backgrounds`

Use when the goal is scene and background art rather than character-focused stills.

### `characters`

Use when the goal is portraits, standing art, busts, and similar character assets.

### `full-raw`

Use when you want broad primary-content extraction without heuristic filtering.

### `full-project`

Use when you want a dump-style export path rather than immediate filtered asset extraction.

## Output Behavior

By default:

- output is cleaned before export
- recursive unpack is on
- shard strategy is off

Useful flags:

- `--keep-output`
- `--recursive-unpack off`
- `--shard-strategy direct-children`
- `--shard-strategy auto`
- `--shard-direct-children`

`--shard-direct-children` is shorthand for `--shard-strategy direct-children`.

Artifacts commonly written by export runs:

- `export-plan.json`
- `export-manifest.json`
- `summary.txt`
- `skipped-assets.json` when profile filtering skips collections
- `failed-assets.json` when per-collection exporter failures are recorded
- `recursive-unpack.json` when recursive unpack runs and writes a summary artifact

## Suggested Workflow

1. Run `inspect` first.
2. If needed, run `analyze --report ...`.
3. Start with a targeted profile such as `audio`, `cg`, or `backgrounds`.
4. If filtering is too narrow, widen to `full-raw`.
5. Use `dump` or `full-project` when the goal is project-style export rather than filtered primary content.

## Notes

- `cg` is heuristic and static-only.
- `audio` can still be broad on some games.
- some games will emit importer warnings while still exporting usable content.
- broken streamed textures are now skipped instead of aborting the entire primary export.

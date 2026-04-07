# CLI Usage Guide

This guide documents the current command-line surface of the export runner in this repository.

The CLI host lives at:

- `upstream/assetripper_dotnet10/Source/AssetRipper.Tools.ExportRunner`

Typical built binary path:

```bash
/home/m/.local/dotnet-10.0.200/dotnet \
  upstream/assetripper_dotnet10/Source/0Bins/AssetRipper.Tools.ExportRunner/Release/AssetRipper.Tools.ExportRunner.dll
```

## Commands

### Inspect

Use `inspect` to quickly understand what a game contains before exporting.

```bash
AssetRipper.Tools.ExportRunner inspect <input-path> [more-input-paths...]
```

What it prints:

- project version
- asset collection count
- asset count
- resource file count
- top asset classes
- top output buckets
- profile evidence
- suggested profiles

Example:

```bash
/home/m/.local/dotnet-10.0.200/dotnet \
  upstream/assetripper_dotnet10/Source/0Bins/AssetRipper.Tools.ExportRunner/Release/AssetRipper.Tools.ExportRunner.dll \
  inspect "./input/Game"
```

### Analyze

Use `analyze` when you want the same inventory pass plus a JSON artifact.

```bash
AssetRipper.Tools.ExportRunner analyze <input-path> [more-input-paths...] [--report <report-path>]
```

Example:

```bash
/home/m/.local/dotnet-10.0.200/dotnet \
  upstream/assetripper_dotnet10/Source/0Bins/AssetRipper.Tools.ExportRunner/Release/AssetRipper.Tools.ExportRunner.dll \
  analyze "./input/Game" --report ./analysis.json
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

```bash
/home/m/.local/dotnet-10.0.200/dotnet \
  upstream/assetripper_dotnet10/Source/0Bins/AssetRipper.Tools.ExportRunner/Release/AssetRipper.Tools.ExportRunner.dll \
  export "./input/Game" --output ./outputs/game_primary --mode primary
```

```bash
/home/m/.local/dotnet-10.0.200/dotnet \
  upstream/assetripper_dotnet10/Source/0Bins/AssetRipper.Tools.ExportRunner/Release/AssetRipper.Tools.ExportRunner.dll \
  export "./input/Game" --output ./outputs/game_cg --profile cg
```

```bash
/home/m/.local/dotnet-10.0.200/dotnet \
  upstream/assetripper_dotnet10/Source/0Bins/AssetRipper.Tools.ExportRunner/Release/AssetRipper.Tools.ExportRunner.dll \
  export "./input/Game" --output ./outputs/game_dump --mode dump
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

```bash
/home/m/.local/dotnet-10.0.200/dotnet \
  upstream/assetripper_dotnet10/Source/0Bins/AssetRipper.Tools.ExportRunner/Release/AssetRipper.Tools.ExportRunner.dll \
  report ./outputs/game_cg/export-manifest.json
```

## Legacy Direct Commands

These still work as compatibility entrypoints:

```bash
AssetRipper.Tools.ExportRunner primary <input-path> <output-path> [more-input-paths...]
AssetRipper.Tools.ExportRunner dump <input-path> <output-path> [more-input-paths...]
```

They are equivalent to direct backend export and bypass profile naming.

## Common Profiles

### `cg`

This profile targets static CG-style assets.

Good fit:

- event illustrations
- gallery stills
- memory images
- static story art
- static authored background/scene art that is stored directly as textures or static prefab-backed visuals

Out of scope:

- Spine reconstruction
- Live2D reconstruction
- Cubism reconstruction
- animated runtime scene composition

This profile is heuristic. It is intended to find static CG assets, not to restore an entire runtime presentation stack.

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

`--shard-direct-children` is kept as shorthand for `--shard-strategy direct-children`.

## Suggested Workflow

1. Run `inspect` first.
2. If needed, run `analyze --report ...`.
3. Start with a targeted profile like `audio`, `cg`, or `backgrounds`.
4. If filtering is too narrow, widen to `full-raw`.
5. Use `dump` when the goal is project-style output rather than filtered asset extraction.

## Notes

- `cg` is heuristic and static-only.
- primary texture export now prefers streamed texture payloads over embedded preview-style image bytes when both exist.
- some games will still produce false positives in `cg`, especially when UI or gallery-related assets share naming conventions with scene art.

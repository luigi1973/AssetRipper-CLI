# CLI Implementation Status

This document describes the current implementation in this repository. It is a snapshot, not a roadmap.

## Scope

The current CLI supports four workflows:

- inspect inputs before export
- analyze inputs and optionally write a JSON artifact
- export with either `primary` or `dump` backends
- report previously written artifacts

The main project lives in:

- `src/AssetRipper.Tools.ExportRunner`

## Commands

### Inspect

```bash
AssetRipper.Tools.ExportRunner inspect <input-path> [more-input-paths...]
```

Behavior:

- loads and processes the input
- prints a compact inventory summary
- shows project version, counts, path semantics, top classes, top buckets, and suggested profiles

### Analyze

```bash
AssetRipper.Tools.ExportRunner analyze <input-path> [more-input-paths...] [--report <report-path>]
```

Behavior:

- runs the same inventory pass as `inspect`
- prints profile evidence and suggested profiles
- can write `inventory-summary` as JSON

### Export

```bash
AssetRipper.Tools.ExportRunner export <input-path> [more-input-paths...] --output <output-path> [--profile <profile> | --mode <primary|dump>]
```

Supported options:

- `--profile`
- `--mode`
- `--keep-output`
- `--recursive-unpack=on|off`
- `--shard-strategy=off|direct-children|auto`
- `--shard-direct-children`

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
- other profiles -> `primary` with heuristic collection filtering

Compatibility entrypoints still exist:

```bash
AssetRipper.Tools.ExportRunner primary <input-path> <output-path> [more-input-paths...]
AssetRipper.Tools.ExportRunner dump <input-path> <output-path> [more-input-paths...]
```

### Report

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

## Execution Notes

- load, export, unpack, and job scheduling each have separate worker-count environment variables
- sharded exports are planned through `ShardPlanner` and executed through `ExportScheduler`
- recursive unpack currently runs after both `primary` and `dump` exports when enabled

## Known Gaps

- profile selection is heuristic, not evidence-complete
- CLI behavior still has known issues documented in [Code Review Findings](CodeReviewFindings.md)
- dedicated automated tests for CLI routing, shard behavior, and artifact semantics are not present yet

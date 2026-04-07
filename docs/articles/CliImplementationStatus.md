# CLI Implementation Status

This document describes the current implementation in this repository. It is a status snapshot, not a roadmap.

## Repository Shape

Main project:

- `src/AssetRipper.Tools.ExportRunner`

Supporting code:

- `vendor/assetripper/Source/...`

Solution file:

- `AssetRipperCLI.slnx`

Primary documentation posture:

- Windows-first command examples
- PowerShell snippets in user-facing docs
- portable `dotnet <dll>` invocation kept as the default documented path

## Current Workflow Model

The CLI is organized around four top-level workflows:

- `inspect`
- `analyze`
- `export`
- `report`

This is implemented in:

- `Program.cs`
- `InventoryWorkflow.cs`
- `InventorySummaryBuilder.cs`
- `ExportProfileResolver.cs`
- `ProfileSelection.cs`
- `ExportPlanBuilder.cs`
- `ShardPlanner.cs`
- `CliExportExecutor.cs`
- `ArtifactReportWorkflow.cs`

## Export Modes

Backend modes:

- `primary`
- `dump`

Primary export is also used underneath heuristic profiles such as `cg`, `audio`, and `characters`.

## Profiles

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

Current implementation details:

- profile matching is heuristic
- selection happens at collection level
- profile evidence is derived from asset class, path buckets, and naming tokens
- suggested profiles are derived from both coarse inventory counts and profile-evidence signals

## Execution Model

### Load

Load worker settings:

- `ASSETRIPPER_LOAD_WORKERS`
- fallback: `ASSETRIPPER_WORKERS`

### Primary export

Primary export worker settings:

- `ASSETRIPPER_EXPORT_WORKERS`
- fallback: `ASSETRIPPER_WORKERS`

Current behavior:

- exports collections in parallel
- records skipped collections and failed collections
- now treats broken streamed textures as per-collection failures instead of aborting the whole export run

### Recursive unpack

Recursive unpack worker settings:

- `ASSETRIPPER_UNPACK_WORKERS`
- fallback: `ASSETRIPPER_WORKERS`

Current behavior:

- runs after both `primary` and `dump` exports when enabled
- handles nested bundle-like payloads discovered during export
- now avoids reusing an existing output directory name when unpacking nested content

### Job scheduling

Job scheduler settings:

- `ASSETRIPPER_JOB_WORKERS`

Current behavior:

- builds an `export-plan` before execution
- routes planned jobs through a bounded scheduler
- supports single-job and sharded execution
- treats `skipped` shard jobs as successful terminal outcomes for process exit purposes

## Artifacts

Analyze:

- `inventory-summary`

Export:

- `export-plan`
- `export-manifest`
- `recursive-unpack`
- `skipped-assets`
- `failed-assets`
- `summary.txt`

## Manual Validation Status

Manual batch validation has been run against several local Windows game samples.

Observed behavior:

- `cg`, `audio`, and `primary` all completed successfully on multiple games
- one previously failing `primary` case was retested successfully after export hardening changes
- importer warnings still appear on some games while the export remains usable

## Current Risks

- profile quality is still heuristic rather than evidence-complete
- some games still emit importer read warnings for specific asset types
- automated CLI-level tests are still missing

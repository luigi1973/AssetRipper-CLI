# CLI Architecture Notes

This file is retained as a short design note rather than a long refactor plan.

## Current Direction

The CLI is being shaped around user workflows instead of exporter internals:

- `inspect`
- `analyze`
- `export`
- `report`

The implementation follows the same broad split described in `.agents`:

- command model and user-facing workflow
- inventory and profile evidence
- export planning and execution
- scheduler and shard behavior
- delivery and validation

## Current Boundaries

- `Program.cs` owns command routing and argument normalization
- `InventoryWorkflow` and `InventorySummaryBuilder` own inspection/analyze summaries
- `ExportProfileResolver`, `ProfileSelection`, `ExportPlanBuilder`, and `ShardPlanner` own export intent and planning
- `CliExportExecutor` and `ExportScheduler` own execution and artifacts
- `ArtifactReportWorkflow` owns rendering existing artifacts back to the console

## Practical Constraints

- large bundle depots may require shard-oriented execution
- the CLI must explain what it exported, skipped, and failed
- compatibility entrypoints still exist, but the workflow surface should stay centered on `export`
- user-facing docs should remain Windows-friendly by default

## Recent Stabilization

Recent work improved launch-readiness:

- recursive unpack now avoids directory collisions instead of reusing an existing export path
- sharded reruns no longer report process failure when work is intentionally skipped
- `--keep-output` now propagates correctly into sharded jobs
- broken streamed textures are skipped instead of aborting the entire primary export job

## Next Useful Work

- add CLI-level tests for shard reruns, keep-output behavior, and recursive unpack safety
- continue importer hardening for warning-heavy games
- tighten profile documentation using more validated examples
- prepare CI and release packaging expectations for GitHub launch

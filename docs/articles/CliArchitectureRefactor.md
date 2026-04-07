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

## Next Useful Work

- fix the known issues in [Code Review Findings](CodeReviewFindings.md)
- add CLI-level tests for shard reruns, keep-output behavior, and recursive unpack safety
- trim remaining docs and examples that still describe the broader upstream website product

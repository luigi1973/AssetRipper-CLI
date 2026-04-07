# Code Review Findings

This document records the current review findings for the CLI workspace as of April 7, 2026. These are known issues, not resolved work.

## High Severity

### Recursive unpack can delete an existing export directory

Affected files:

- `src/AssetRipper.Tools.ExportRunner/RecursiveBundleUnpacker.cs`
- `src/AssetRipper.Tools.ExportRunner/CliTextAssetContentExtractor.cs`

Current behavior:

- nested unpack picks an output directory based on the input file stem
- if a directory with that stem already exists, the code does not treat that as a collision
- the target path is then cleaned before the nested export is proven to succeed

Why this is a problem:

- an existing export directory such as `foo/` can be deleted while unpacking `foo.bundle` or `foo.bytes`
- this can destroy already-exported data before the nested export has completed

Required fix direction:

- treat existing directories as collisions, not just existing files
- use a guaranteed unique temp/staging directory before replacing any prior output
- only delete superseded bundle payloads after successful nested export

### Successful shard cache hits still return exit code 1

Affected file:

- `src/AssetRipper.Tools.ExportRunner/CliExportExecutor.cs`

Current behavior:

- a shard with a `.done` marker returns status `skipped`
- the overall process returns success only when every job status is `success`

Why this is a problem:

- a valid rerun of a sharded export can exit nonzero even when nothing is actually wrong
- CI and shell scripts will treat cache-hit reruns as failures

Required fix direction:

- treat `skipped` as a successful terminal state for process exit purposes
- keep `failed` as the only status that forces a nonzero exit code

## Medium Severity

### `--keep-output` is ignored for sharded jobs

Affected files:

- `src/AssetRipper.Tools.ExportRunner/Program.cs`
- `src/AssetRipper.Tools.ExportRunner/ShardPlanner.cs`

Current behavior:

- the root export command accepts `--keep-output`
- sharded jobs are still created with `CleanOutput: true`

Why this is a problem:

- the CLI contract implies output preservation
- sharded reruns still wipe shard directories unless `.done` causes a skip

Required fix direction:

- carry the root clean/keep policy into shard job planning
- document any exceptions explicitly if shard behavior must differ

## Notes

- `dotnet build AssetRipperCLI.slnx -c Release` succeeded during review
- these findings are based on code inspection; scenario tests with real bundle fixtures are still pending

# AssetRipper CLI

This repository is a focused CLI workspace built around `AssetRipper.Tools.ExportRunner`.

What is here:

- `src/AssetRipper.Tools.ExportRunner`: the CLI host and workflow code
- `vendor/assetripper`: retained upstream libraries needed by the CLI
- `docs/articles/CliUsageGuide.md`: command reference
- `docs/articles/CliImplementationStatus.md`: current implementation notes
- `docs/articles/CodeReviewFindings.md`: current limitations and resolved review items

Start with:

- [CLI Usage Guide](articles/CliUsageGuide.md)
- [Implementation Status](articles/CliImplementationStatus.md)
- [Known Limitations](articles/CodeReviewFindings.md)
- [Release Readiness](articles/ReleaseReadiness.md)

Current state:

- the solution builds successfully
- the main April 7, 2026 export-handling review items have been fixed
- local batch validation has been run against several sample games
- dedicated automated tests for CLI workflows have not been added yet

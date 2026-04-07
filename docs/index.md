# AssetRipper CLI

This repository is a focused CLI workspace built around `AssetRipper.Tools.ExportRunner`.

What is here:

- `src/AssetRipper.Tools.ExportRunner`: the CLI host and workflow code
- `vendor/assetripper`: retained upstream libraries needed by the CLI
- `docs/articles/CliUsageGuide.md`: command reference
- `docs/articles/CliImplementationStatus.md`: current implementation notes
- `docs/articles/CodeReviewFindings.md`: known problems found in review

Start with:

- [CLI Usage Guide](articles/CliUsageGuide.md)
- [Implementation Status](articles/CliImplementationStatus.md)
- [Code Review Findings](articles/CodeReviewFindings.md)

Current state:

- the solution builds successfully
- review findings are documented but not fixed yet
- test coverage for the CLI workflows has not been added yet

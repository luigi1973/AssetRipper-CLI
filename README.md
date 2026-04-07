# AssetRipper CLI

`AssetRipper CLI` is a focused repository for a command-line Unity asset export workflow built around `AssetRipper.Tools.ExportRunner`.

This repo is not the full upstream AssetRipper monorepo. It keeps a reduced CLI-centered workspace:

- `src/AssetRipper.Tools.ExportRunner`: the CLI host
- `vendor/assetripper`: retained upstream libraries needed by the CLI
- `docs/`: repo docs and command reference

## Current Scope

The CLI currently supports four workflows:

- `inspect`: quick inventory summary for a game or asset root
- `analyze`: inventory summary plus JSON artifact output
- `export`: primary-content or dump-style export
- `report`: human-readable rendering of saved artifacts

The command surface is workflow-first:

- inspect first
- analyze when you want a machine-readable artifact
- export with a profile or backend mode
- report artifacts after the run

## Status

Current state of the repo:

- builds successfully with `.NET 10`
- includes sharded export planning and execution
- supports heuristic profiles such as `cg`, `audio`, and `characters`
- writes export artifacts such as `export-plan.json`, `export-manifest.json`, and `summary.txt`
- has been manually exercised against several local game samples

Current gaps:

- profile selection remains heuristic
- importer warnings still occur on some real-world games
- dedicated automated tests for CLI behavior are not in place yet

## Build

Prerequisite:

- `.NET SDK 10`

Windows install example:

```powershell
winget install Microsoft.DotNet.SDK.10
```

Build the solution:

```powershell
dotnet build AssetRipperCLI.slnx -c Release
```

Built binary on Windows:

```powershell
.\artifacts\bin\AssetRipper.Tools.ExportRunner\Release\net10.0\AssetRipper.Tools.ExportRunner.exe
```

Portable invocation:

```powershell
dotnet .\artifacts\bin\AssetRipper.Tools.ExportRunner\Release\net10.0\AssetRipper.Tools.ExportRunner.dll
```

## Quick Start

Windows examples:

Inspect a game:

```powershell
dotnet .\artifacts\bin\AssetRipper.Tools.ExportRunner\Release\net10.0\AssetRipper.Tools.ExportRunner.dll `
  inspect .\input\GameRoot
```

Analyze and save an inventory artifact:

```powershell
dotnet .\artifacts\bin\AssetRipper.Tools.ExportRunner\Release\net10.0\AssetRipper.Tools.ExportRunner.dll `
  analyze .\input\GameRoot --report .\output\analysis.json
```

Export a targeted profile:

```powershell
dotnet .\artifacts\bin\AssetRipper.Tools.ExportRunner\Release\net10.0\AssetRipper.Tools.ExportRunner.dll `
  export .\input\GameRoot --output .\output\game_cg --profile cg
```

Export broad primary content:

```powershell
dotnet .\artifacts\bin\AssetRipper.Tools.ExportRunner\Release\net10.0\AssetRipper.Tools.ExportRunner.dll `
  export .\input\GameRoot --output .\output\game_primary --mode primary
```

Render an artifact back to the console:

```powershell
dotnet .\artifacts\bin\AssetRipper.Tools.ExportRunner\Release\net10.0\AssetRipper.Tools.ExportRunner.dll `
  report .\output\game_primary\export-manifest.json
```

Unix-style invocation is also supported; adjust path separators and line continuations for your shell.

## Repository Layout

- `src/`: authored CLI code
- `vendor/`: vendored upstream AssetRipper libraries used by the CLI
- `docs/`: usage, implementation notes, and known limitations
- `input/`: ignored local test inputs
- `output/`: ignored local test outputs

## Documentation

- [docs/index.md](docs/index.md)
- [docs/articles/CliUsageGuide.md](docs/articles/CliUsageGuide.md)
- [docs/articles/CliImplementationStatus.md](docs/articles/CliImplementationStatus.md)
- [docs/articles/CodeReviewFindings.md](docs/articles/CodeReviewFindings.md)
- [docs/articles/ReleaseReadiness.md](docs/articles/ReleaseReadiness.md)

## Launch Notes

Before publishing broadly on GitHub, the main remaining work is:

- add automated tests for CLI routing, shard behavior, and artifact semantics
- tighten profile documentation with more tested examples
- decide how much release packaging and CI should live in this reduced repo

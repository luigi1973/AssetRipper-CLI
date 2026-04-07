# Release Readiness

This document is a practical pre-GitHub-launch checklist for this repository.

## What Is Ready

- the solution builds successfully with `.NET 10`
- the repository now has a GitHub-facing `README.md`
- command docs reflect the current repo layout and binary paths
- docs are written with Windows and PowerShell examples first
- the CLI has been manually exercised against several local game samples
- recent export-handling fixes have been validated against a previously failing case

## What Has Been Manually Validated

Manual validation covered:

- `inspect`
- `analyze`
- `export --profile cg`
- `export --profile audio`
- `export --mode primary`
- artifact generation and manifest review

Observed outcome:

- multiple sample games completed successfully under `cg`, `audio`, and `primary`
- one prior `primary` failure was fixed by skipping broken streamed textures instead of aborting the entire job

## What Still Needs Work

- automated tests for CLI routing, shard behavior, artifact serialization, and exit codes
- stronger profile-quality documentation with more tested examples
- a decision on release packaging and CI expectations for this reduced repo

## Suggested GitHub Launch Checklist

1. Review the root `README.md` for naming, positioning, and installation wording.
2. Confirm whether the repo should include release binaries or source-only instructions.
3. Add CI for `dotnet build AssetRipperCLI.slnx -c Release`.
4. Add at least a small initial automated test set.
5. Decide whether `.agents/` should remain published as repository process docs.
6. Confirm license and attribution expectations for the reduced vendored layout.

## Handoff Notes

The repo is in a good state for a documentation and release-process handoff.

The highest-value next engineering work is:

- automated tests
- profile tuning
- importer robustness for warning-heavy games

# Known Limitations

This document records the current known limitations and recent stability findings for the CLI workspace.

## Recently Fixed

The following issues were fixed on April 7, 2026:

- recursive unpack no longer reuses an existing export directory name and clobbers prior output
- sharded reruns with cached `.done` markers no longer force a failing process exit code
- `--keep-output` now applies to sharded jobs
- a broken streamed texture no longer aborts the entire primary export job

## Current Limitations

### Heuristic profile selection

Profile selection is still heuristic.

Practical consequence:

- `cg` is useful for static illustration-style assets but can still include false positives
- `audio` can be broader than expected on some games
- profiles are best treated as workflow shortcuts, not perfect semantic classifiers

### Importer warnings on real-world games

Some games still emit importer warnings while remaining exportable.

Examples seen during local validation:

- `BuildSettings` read errors
- `UnityConnectSettings` read errors
- unknown or partially-read serialized asset types
- short-read warnings for specific asset classes such as `Cubemap`

Practical consequence:

- a warning-free import should not currently be assumed
- manifest and output review still matter after a run

### Limited automated validation

Current testing is still manual-first.

Practical consequence:

- manual spot checks have been valuable
- the repository still needs automated tests for:
  - CLI routing
  - shard behavior
  - artifact serialization
  - exit-code behavior
  - failure handling for broken assets

## Validation Notes

Recent local batch validation covered:

- multiple Windows Unity game samples
- `cg`, `audio`, and `primary` variants
- artifact generation and manifest inspection

Observed result:

- the export pipeline now handles a previously failing streamed-texture case by completing the run instead of aborting it
- recursive unpack remained quiet on the tested samples, producing zero candidate nested bundle files

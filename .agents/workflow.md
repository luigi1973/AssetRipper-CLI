# Engineering Workflow

This file defines how the agent roles collaborate on substantial CLI and export work.

## Standard Sequence

1. `cli-architect`
   - defines the user-facing workflow, command shape, invariants, and migration constraints
2. `asset-inventory-analyst`
   - defines what must be known about the game before selection and export
3. `export-pipeline-engineer`
   - implements execution contracts, backend integration, manifests, and path policy
4. `performance-and-concurrency`
   - hardens the execution path for large projects and validates scheduler behavior
5. `delivery-and-validation`
   - verifies real behavior, docs alignment, and release risk

## Escalation Rules

- If a design changes command semantics, the CLI architect must review it.
- If a change alters classification logic, the inventory analyst must review it.
- If a change alters output naming or skip reasons, export engineering and validation must review it.
- If a change alters worker behavior or scheduler policy, performance and validation must review it.

## Required Artifacts For Major Changes

- architecture note or updated docs
- implementation changes
- validation notes
- explicit list of deferred work

## Repository Priorities

- player-facing workflows over internal mode names
- explainability over silent best-effort behavior
- bounded concurrency over ad hoc parallel loops
- compatibility during migration, then deliberate cleanup

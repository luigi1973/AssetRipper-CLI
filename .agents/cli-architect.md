# CLI Architect

## Mission

Own the command model, boundaries between layers, migration sequencing, and compatibility strategy.

## Primary Responsibilities

- define top-level commands and argument model
- prevent the CLI from leaking exporter internals into user workflows
- define orchestration boundaries between frontend, planning, execution, and reporting
- preserve compatibility where required during migration
- ensure user-facing docs and examples are coherent for Windows-first usage

## Inputs

- current CLI entry points
- current exporter capabilities and limitations
- observed user workflows
- reporting and manifest requirements

## Outputs

- command specifications
- architecture proposals
- migration plans
- compatibility and deprecation notes

## Required Questions

- What user intent is being expressed?
- Is the command naming workflow-oriented?
- Can the requested workflow be inspected before export?
- Does the design reduce divergence between `unity` and `primary` style paths?

## Constraints

- do not redesign around hypothetical future exporters only
- do not bury player-facing behavior inside infrastructure abstractions
- do not remove compatibility paths without an explicit migration note
- treat large AssetBundle depots as a first-class product case, including shard-oriented execution when one-shot loading is not safe
- do not ship repo docs that assume a Unix shell when the likely end user is on Windows

## Typical Deliverables

- new command tree
- refactor boundary docs
- option naming cleanup
- compatibility shims

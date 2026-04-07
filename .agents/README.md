# Agent System

This directory defines the working agreement for agentic engineering in this repository.

The purpose is simple:

- keep architectural work coherent
- split discovery, implementation, and validation cleanly
- avoid duplicate investigation and conflicting edits
- keep export behavior aligned with player-facing use cases

## Operating Principles

- Work from the code that exists, not from assumptions.
- Treat CLI behavior as a product surface, not just a developer convenience.
- Prefer workflow-oriented design over exporter-oriented design.
- Every meaningful export change should improve inspectability, coverage reporting, or execution predictability.
- Do not silently broaden scope. Document follow-on work instead.

## Core Workstreams

- CLI architecture and command model
- asset inventory and semantic classification
- export pipeline and scheduler behavior
- reporting, manifests, and validation

## Role Documents

- `cli-architect.md`
- `asset-inventory-analyst.md`
- `export-pipeline-engineer.md`
- `performance-and-concurrency.md`
- `delivery-and-validation.md`
- `workflow.md`

## Handoff Rules

- Architecture decisions define intent, boundaries, invariants, and migration sequencing.
- Inventory analysis defines how assets are described before export.
- Export engineering implements execution against the agreed plan.
- Performance work changes scheduling and scaling behavior, not product semantics, unless explicitly coordinated.
- Delivery and validation verifies user-facing behavior, reports gaps, and blocks weak assumptions.

## Definition Of Done

A substantial CLI/export change is not done until it includes:

- code changes
- updated docs if behavior or architecture changed
- observable verification notes
- clear statement of known gaps or deferred work

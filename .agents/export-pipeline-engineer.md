# Export Pipeline Engineer

## Mission

Own the translation from export plan to concrete exported output.

## Primary Responsibilities

- implement `ExportPlan` execution
- integrate exporter backends behind a shared execution contract
- enforce path reservation and collision policy
- produce manifests and per-job outcomes

## Inputs

- approved command and architecture model
- asset inventory
- selection rules
- scheduler contracts

## Outputs

- executable export jobs
- exporter integration points
- manifest records
- failure and skip handling

## Required Questions

- What is the smallest stable execution contract shared by both export styles?
- Can this job be retried or skipped deterministically?
- How are output collisions prevented?
- Does the implementation leave behind enough evidence to debug failures?

## Constraints

- do not embed product selection logic inside low-level exporters
- do not rely on global mutable state when a pipeline-scoped object is sufficient
- do not introduce hidden output naming rules
- recursive AssetBundle unpack must delete superseded bundle payloads only after successful nested export

## Typical Deliverables

- export execution services
- path reservation services
- manifest writer
- backend adapters

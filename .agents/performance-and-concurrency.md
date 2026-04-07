# Performance And Concurrency

## Mission

Own runtime behavior for large projects, including bounded parallelism, backpressure, and predictable throughput.

## Primary Responsibilities

- define and implement the shared scheduler model
- measure serial bottlenecks
- choose safe default worker policies
- ensure concurrency changes preserve correctness and reporting fidelity

## Inputs

- export plan size
- exporter cost profile
- filesystem behavior
- current serial and parallel execution paths

## Outputs

- scheduler design
- worker-count policy
- throughput findings
- concurrency safety notes

## Required Questions

- Is this workload CPU-bound, IO-bound, or mixed?
- Where can output collisions occur?
- What state must be isolated per job?
- What should happen when one worker fails?

## Constraints

- do not add parallelism without deterministic path and manifest handling
- do not optimize one backend while leaving the product model inconsistent
- do not hide worker-count assumptions
- if one-shot loading risks OOM, prefer shard-oriented execution over raising worker counts

## Typical Deliverables

- `ExportScheduler`
- bounded queue execution
- cancellation behavior
- performance benchmarks and notes

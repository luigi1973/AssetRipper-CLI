# Delivery And Validation

## Mission

Own release confidence for CLI/export changes.

## Primary Responsibilities

- validate command behavior from a user perspective
- check docs and examples against actual CLI behavior
- verify manifests, skipped reports, and output hierarchy consistency
- identify regressions, missing tests, and weak assumptions
- verify that Windows install and invocation guidance is accurate and easy to follow

## Inputs

- implemented changes
- generated output
- docs and examples
- sample game exports

## Outputs

- validation notes
- bug findings
- test coverage recommendations
- release-readiness summary

## Required Questions

- Does the command do what the docs say?
- Can a user understand what was exported and skipped?
- Are large-project behaviors visible and predictable?
- Are failures actionable?
- Can a Windows user build and run the documented commands without translating shell syntax first?

## Constraints

- do not sign off on architecture based on code reading alone
- do not treat missing reporting as acceptable on large exports
- do not collapse findings into vague summaries

## Typical Deliverables

- scenario-based validation checklist
- findings list with file references
- output consistency review
- test gap report

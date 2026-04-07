# Asset Inventory Analyst

## Mission

Own the understanding of what a loaded game contains before export selection happens.

## Primary Responsibilities

- design inventory models for loaded assets
- group assets by class, path, bundle, naming pattern, and dependency signals
- identify whether a game is path-rich or path-poor
- support profile selection with evidence instead of guesswork

## Inputs

- loaded `GameData`
- asset metadata
- original paths
- bundle structure
- prior export observations

## Outputs

- inventory schemas
- classification heuristics
- analysis reports
- confidence notes and fallback rules

## Required Questions

- Which assets are likely to matter to a player or archivist?
- Which assets can be confidently classified?
- Which assets require fallback export because semantics are weak?
- What evidence supports each classification?

## Constraints

- do not hardcode game-specific assumptions without documenting them
- do not conflate asset type with user value
- do not hide uncertainty; surface confidence and fallback behavior

## Typical Deliverables

- inventory report design
- profile selection heuristics
- asset grouping rules
- skipped-classification notes

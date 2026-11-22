# Supported API and exclusions

Reconciliation source: `docs/api-model.json` compared against `.Client` and `.Mapper`.

## Coverage matrix (exact)

- **Operations**: **222 / 222** covered (**100%**)
- **Schemas**: **120 / 120** covered (**100%**)
- Evidence artifact:  
  `C:\Users\Arius-dev\.copilot\session-state\a6a9000b-eb02-48ef-a676-4fc1cb95fc0e\files\wave2-global-matrix-docs-coverage.json`

## Coverage policy

- **Stable** operations/schemas are part of the normal compatibility-oriented public surface.
- **Unstable** operations/schemas are included and flagged only in XML docs.
- **Deprecated** operations/schemas are included and annotated with `[Obsolete]`.

## Normalized alias handling included in the matrix

- OpenAPI bracket aliases `/[number,address,url,email]/...` are counted via implemented concrete routes (`/number`, `/address`, `/url`, `/email`).
- `/contact/{contact_id}/[numbers|addresses|urls|emails]/` is counted via concrete implemented collections.
- `/parental/filter/{id}/planning` is counted via the dedicated planning sub-resource.
- `/ws/upload` is counted via WebSocket uploads route coverage.
- `/api_version` is counted via discovery transport coverage.

# Waves — device-fleet-operations

Each wave IS an iteration: a time-boxed, independently shippable
increment — one branch, one PR, green tests, demoable outcome.
Do not start wave N+1 until wave N merges.

## Wave 1 — iteration 1

Scenarios proven by this wave:
- Add devices to a company
- Add devices to unknown company fails
- Query location by id
- Query unknown location returns nothing

| Item | Title | Size | Deps | Notes |
|------|-------|------|------|-------|
| US1 | As an operator, I want to add devices to a company and query locations by id, so that I can grow and inspect fleets after creation | M | — | AddDevicesMessage + QueryLocationById request/result messages, MessageProcessor routing, AddDevicesToCompanyUseCase (continues Location numbering), unit tests. |

## Wave 2 — iteration 2

Scenarios proven by this wave:
- API exposes fleet operations

| Item | Title | Size | Deps | Notes |
|------|-------|------|------|-------|
| US2 | As an API consumer, I want HTTP endpoints for fleet operations, so that I can manage devices without touching the message bus | M | US1 | EasyNetQ handlers (add-devices fire-and-forget, location query request/response), POST /api/companies/{id}/devices + GET /api/locations/{id} with RequestId correlation, README/AGENTS contract update. |

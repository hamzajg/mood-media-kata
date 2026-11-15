# Backlog: device-fleet-operations

Items are USER STORIES in the parseable format below — keep the heading
shape `## US<n> — <story>` and the field lines intact so `wave_plan` can
parse them. Write the story as: "As a <role>, I want <goal>, so that
<value>" — the title after the dash IS the story.

```
## US<n> — As a <role>, I want <goal>, so that <value>
- Scenarios: @w1, @w2
- Depends: US1, US3        (optional; omit for wave-1 items)
- Size: S|M|L
- Notes: <one line>
```

Order stories by user value; dependencies express technical order only.

## US1 — As an operator, I want to add devices to a company and query locations by id, so that I can grow and inspect fleets after creation
- Scenarios: @w1
- Size: M
- Notes: AddDevicesMessage + QueryLocationById request/result messages, MessageProcessor routing, AddDevicesToCompanyUseCase (continues Location numbering), unit tests.

## US2 — As an API consumer, I want HTTP endpoints for fleet operations, so that I can manage devices without touching the message bus
- Scenarios: @w2
- Depends: US1
- Size: M
- Notes: EasyNetQ handlers (add-devices fire-and-forget, location query request/response), POST /api/companies/{id}/devices + GET /api/locations/{id} with RequestId correlation, README/AGENTS contract update.

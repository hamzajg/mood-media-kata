# Spec: device-fleet-operations

> Spec-driven development artifact. This file is the contract: implementation
> is done when every acceptance criterion maps to a scenario in
> `EXAMPLES.feature` and every scenario has a passing check.

## Problem

The domain already knows how to add devices to a company
(`AddDevicesUseCase`) and how to fetch a location (`QueryLocationByIdUseCase`),
but neither is reachable: there is no message type, no subscriber handler,
and no REST endpoint for them. The public API can create a company, query it
by id, and delete devices by serial — nothing else. Operators cannot grow a
company's fleet after creation or inspect where a device lives.

## Users

- API consumers (the htmx web app today, ops tooling tomorrow) that manage
  company device fleets through `MoodMediaKata.APIs`.

## Scope (in)

- `AddDevicesMessage` (company id + device list) processed through
  `MessageProcessor` into a use case that appends devices to an existing
  company — new `Location {index}` entries continue numbering after the
  company's existing locations, serial numbers generated app-side.
- `QueryLocationByIdMessage` / result message pair following the existing
  `QueryCompanyById` request/response pattern (dedicated result queue,
  `RequestId` correlation).
- REST endpoints: `POST /api/companies/{id}/devices`,
  `GET /api/locations/{id}`.
- Unit tests for the new use case/message routing; docs updated.

## Scope (out)

- Device lookup by serial number (needs `FindBySerialNumber` on
  `IDeviceRepository` across all backends — separate feature).
- Company listing/search, licensing-tier rules, web app UI pages.
- Changes to persistence backends or message-bus topology.

## Acceptance criteria

<!-- Scenario(s): use scenario names verbatim from EXAMPLES.feature and/or
     @w<N> tags — ship_validate resolves both for traceability. -->

| # | Criterion | Scenario(s) |
|---|-----------|-------------|
| 1 | Adding devices to an existing company creates one new location per device, numbered after existing locations, with app-generated serials | Add devices to a company |
| 2 | Adding devices to an unknown company is rejected, not silently dropped | Add devices to unknown company fails |
| 3 | Querying a location by id returns the location with its device details | Query location by id |
| 4 | Querying an unknown location returns no result | Query unknown location returns nothing |
| 5 | POST /api/companies/{id}/devices publishes AddDevicesMessage; GET /api/locations/{id} returns the location over HTTP | API exposes fleet operations |

## Decisions

- Reuse the existing message-bus request/response pattern (dedicated result
  queue + `RequestId` correlation) rather than inventing a new transport.
- Location numbering continues from the company's current location count
  (Location N+1, N+2, ...) — devices never share a location.
- In-memory repository behaviour is the correctness target; SQL/Mongo
  backends must compile and behave identically through the same use cases.

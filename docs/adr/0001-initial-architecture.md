# ADR 0001 — Initial architecture

Date: 2026-11-13 (baseline recorded retroactively)
Status: accepted (describes what is built)

## Context

A coding-kata solution: ingest `NewCompany`/`DeleteDevices` messages and
maintain Company→Location→Device aggregates. The exercise explicitly asks
to show data-access skill ("No Entity Framework. We'd like to see how you
write queries") and performance is explicitly not the goal.

## Decisions

1. **Ports & adapters / clean layering.** Domain (`MoodMediaKata.Company`)
   → shared abstractions (`MoodMediaKata.App`: `IRepository`,
   `IMessageProcessor`, `IdGenerator`) → hosts (`MoodMediaKata` worker,
   `MoodMediaKata.APIs` REST, `MoodMediaKata.WebApp` htmx). Rationale:
   every infrastructure choice is swappable — which is exactly what the
   kata demonstrates.
2. **Runtime-selected backends via CLI flags.** `--db-orm`,
   `--repository`, `--message-processor` choose implementations at
   startup. Rationale: one binary, many topologies; also enables the
   server-free dev mode (`none/InMemory/Console`).
3. **Use-case classes over controller logic.** `CreateCompanyUseCase`,
   `AddDevicesUseCase`, `DeleteDevicesUseCase`, `QueryCompanyByIdUseCase`,
   `QueryLocationByIdUseCase` registered via `UseCaseRegistration`.
   Rationale: behaviour is named, testable, framework-free.
4. **Dapper/hand-written SQL as the primary ORM path.** EF Core exists as
   an optional flag only. Rationale: the kata's stated goal — demonstrate
   real query writing.
5. **htmx + server-rendered fragments for the web app.** No JS framework,
   no SPA build. Rationale: KISS — the UI is a thin view over the API.
6. **App-side serial-number generation.** `IdGenerator` creates device
   serials; payloads don't carry them. Rationale: keeps the message
   contract minimal.
7. **net10.0 with CI on `10.0.x`.** Modernised during the fleet SDK sweep.

## Consequences

- Many combinations of flags × backends — each is a supported path that
  must keep working; changes to abstractions affect all of them.
- Performance was never a goal; don't "optimise" without a reason.
- Integration tests need Postgres; unit tests stay infra-free.

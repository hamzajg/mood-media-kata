# Architecture — mood-media-kata

Current-state map. Descriptive, not aspirational; see `docs/adr/` for why.

## System shape

```
                 docker-compose.infra.yml (Postgres/SQLServer/Mongo/RabbitMQ)
                                    │
   ┌────────────────┐   ┌──────────┴───────────┐
   │ MoodMediaKata. │   │ MoodMediaKata.APIs   │
   │ APIs (REST)    │   │ (message endpoints   │
   │ /api/companies │   │  + Swagger in dev)   │
   └───────┬────────┘   └──────────┬───────────┘
           │                       │
           ▼                       ▼
   ┌───────────────────────────────────────┐
   │ MoodMediaKata (worker host)           │
   │  Program.cs + InfrastructureRegistration│
   │  Messaging/: subscriber + bus          │
   │  Repository/: Sql | MongoDb            │
   │  Database/: Dapper | EF contexts       │
   └───────────────────┬───────────────────┘
                       ▼
        MoodMediaKata.Company  use cases (CreateCompany, AddDevices,
        MoodMediaKata.App      DeleteDevices, QueryCompany/Location)
                               shared: Entity, IdGenerator, IRepository,
                               InMemoryRepository, IMessageProcessor
                       ▲
        MoodMediaKata.WebApp (htmx) ── calls /api/*
```

## Runtime selection

Backends are **chosen at startup by CLI flags**, not config files:

- `--db-orm=none|dapper|ef` — no ORM / Dapper / EF Core
- `--repository=InMemory|PostgreSql|Sql|MongoDb`
- `--message-processor=Console|RabbitMq`

`none + InMemory + Console` runs with zero external services (the dev
mode merged previously); everything else needs infra from
`docker-compose.infra.yml`.

## Domain flow

1. Message arrives via REST (`/api/companies`, `/api/company/{id}/devices`)
   or the message bus (RabbitMQ/Console subscriber).
2. Use case in `MoodMediaKata.Company` validates and applies it:
   - `NewCompany` → Company + `Location {index}` per device + generated
     serials (Standard/Custom type).
   - `DeleteDevices` → remove by serial number.
3. Repository abstraction (`IRepository`/`IDeviceRepository`) persists —
   InMemory, PostgreSql (Dapper), SqlServer (EF), or MongoDb.
4. htmx WebApp renders company views from the same API.

## Data

Kata schema (README mermaid): `COMPANY` → `LOCATION` → `DEVICE`, with
Licensing int, unique codes/serials. Queries are hand-written (Dapper) or
EF — deliberately no "just use EF" default.

## Tests

- `MoodMediaKata.Tests.Unit` — 12 tests, no infra.
- `MoodMediaKata.Tests.Integration` — Postgres service container in CI.

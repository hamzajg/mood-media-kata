# AGENTS.md — mood-media-kata

Guidance for AI agents (and humans) working in this repository.

## Project

Mood Media Kata — a take-home-style .NET solution: process `NewCompany` /
`DeleteDevices` JSON messages into Company→Location→Device aggregates,
exposed via a REST API, an htmx web app, pluggable ORM/repository/message-
processor backends selected by CLI flags, and Docker Compose services.
Stack: C# `net10.0`, ASP.NET Core minimal APIs, Dapper/EF Core, RabbitMQ or
Console message processor, htmx web app, PostgreSQL/SQL Server/MongoDB/
InMemory options, xUnit + integration tests.

## Commands

Verified 2026-11-13 against CI (`.github/workflows/ci.yml`) and README.

| Task | Command |
|---|---|
| Restore | `dotnet restore` |
| Build | `dotnet build --no-restore --configuration Release` |
| Unit tests | `dotnet test MoodMediaKata.Tests.Unit/...csproj --no-build -c Release` (12) |
| Integration tests | `dotnet test MoodMediaKata.Tests.Integration/...csproj --no-build -c Release` — needs Postgres on :5432 |
| Infra | `docker compose -f docker-compose.infra.yml up` |
| APIs | `cd MoodMediaKata.APIs && dotnet run` |
| Message processor | `cd MoodMediaKata && dotnet run --db-orm=none --repository=InMemory --message-processor=Console` (server-free dev mode) |

Requires: .NET 10 SDK (CI `10.0.x`); integration tests need PostgreSQL
(`postgres:14.2`, db `kata_db`). Flags: `--db-orm=none|dapper|ef`,
`--repository=InMemory|PostgreSql|Sql|MongoDb`,
`--message-processor=Console|RabbitMq`.

## Architecture

```
MoodMediaKata.sln
├── MoodMediaKata/          host/worker: Program.cs, Startup-style
│   │                       InfrastructureRegistration, Messaging/ (bus,
│   │                       subscriber), Repository/ (Sql/MongoDb),
│   │                       Database/ — backend selected by CLI flags
│   └── Dockerfile
├── MoodMediaKata.APIs/     ASP.NET minimal API (port per launchSettings)
│   │                       GET /api/companies/{id}
│   │                       POST /api/companies          (CreateNewCompanyMessage)
│   │                       DELETE /api/company/{id}/devices (DeleteDevicesMessage)
│   └── Dockerfile
├── MoodMediaKata.App/      shared kernel: Entity, IdGenerator, Message,
│   │                       IRepository, InMemoryRepository, IMessageProcessor
├── MoodMediaKata.Company/  domain: Company, use cases (CreateCompany,
│   │                       AddDevices, DeleteDevices, QueryCompany/Location),
│   │                       IDeviceRepository + InMemory impl, MessageProcessor
├── MoodMediaKata.WebApp/   htmx client (index.html, add-company/, companies/,
│   │                       company/ routes) + Dockerfile
├── MoodMediaKata.Tests.Unit/         12 unit tests
└── MoodMediaKata.Tests.Integration/  Postgres-backed integration tests
docker-compose.yml / docker-compose.infra.yml
```

### API contract (APIs ↔ WebApp ↔ messaging)

| Method | Path | Body | Purpose |
|---|---|---|---|
| GET | `/api/companies/{id}` | — | company detail |
| POST | `/api/companies` | `CreateNewCompanyMessage` (Id, MessageType `NewCompany`, CompanyName, CompanyCode, Licensing, Devices[]) | create company + locations + devices |
| DELETE | `/api/company/{id}/devices` | `DeleteDevicesMessage` (SerialNumbers[]) | delete devices |

Message payloads per README (`createCompany.json`, `deleteDevices.json`
examples): device `Type` ∈ {`Standard`,`Custom`}; each device gets its own
`Location {index}`; serial numbers are app-generated. The same messages
also flow through the message bus (RabbitMQ or Console processor).

## Conventions

- **Ports & adapters**: domain (`MoodMediaKata.Company`) knows nothing of
  SQL/RabbitMQ; `MoodMediaKata.App` holds the shared abstractions
  (`IRepository`, `IMessageProcessor`, `IdGenerator`); `MoodMediaKata`
  wires concrete backends by flag.
- Use cases are classes (`CreateCompanyUseCase`, …) registered via
  `UseCaseRegistration` — keep new behaviour as use-case classes, not
  controller logic.
- **No EF Core by design of the kata** (README: "No Entity Framework. We'd
  like to see how you write queries") — Dapper/hand-written SQL is the
  expected style for DB access; EF exists only as an optional flag.
- htmx for the web app — server-rendered fragments, no JS framework.

## Landmines

- **Default branch is `main`.**
- Working tree may contain uncommitted Dockerfile edits (net10 base-image
  bumps) — check `git status` before assuming clean; don't sweep them
  into docs commits.
- `--db-orm=none --repository=InMemory --message-processor=Console` is the
  only fully server-free combo — merged in a previous pass; other combos
  need Postgres/SQL Server/Mongo/RabbitMQ.
- Serial-number generation is app-side (`IdGenerator`) — devices' serials
  are NOT in the payload.
- Integration tests hard-require Postgres; unit tests don't.
- README's mermaid diagram uses non-ASCII box chars — cosmetic only.

## Agent rules

- Branch from `main`; name branches `feat/*`, `fix/*`, `chore/*`, `docs/*`.
- Docs-only changes still go through a PR; never commit to `main` directly.
- Verify before claiming: `dotnet build` + unit tests locally; integration
  tests only if Postgres is available — say which you ran.
- Keep changes minimal and consistent with surrounding code. No drive-by
  refactors.
- Update AGENTS.md / docs/ARCHITECTURE.md when routes, message shapes,
  flags, or project layout change.

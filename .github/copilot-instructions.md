# Project Guidelines

## Code Style
- Use C# file-scoped namespaces and nullable reference types, consistent with existing projects.
- Preserve current WinForms patterns in Designer-backed forms; avoid manual edits in generated designer regions unless required.
- Keep changes scoped to the target app boundary: `ServerAdmin`, `ClientApp`, or `SharedModels`.

## Architecture
- Solution structure:
  - `SharedModels`: shared POCO models used by both applications.
  - `ServerAdmin`: WinForms admin app, TCP server, SQLite + Dapper data access.
  - `ClientApp`: WinForms kiosk/client app, TCP client, lock screen and widget UX.
- Communication is JSON-over-TCP line messages (`Action` + `Payload`). Keep protocol changes backward-compatible when possible and update both server/client handlers together.
- Database initialization is performed on server startup in `ServerAdmin/Program.cs` via `DatabaseHelper.InitializeDatabase()`.

## Build And Test
- Restore/build from repo root:
  - `dotnet restore`
  - `dotnet build`
- Run apps:
  - `dotnet run --project ServerAdmin/ServerAdmin.csproj`
  - `dotnet run --project ClientApp/ClientApp.csproj`
- There is currently no dedicated test project in this repository. If adding tests, keep them isolated and do not break existing run commands.

## Conventions
- Default network behavior:
  - Server listens on TCP port `5000` by default.
  - Client/server message readers and writers use UTF-8 and line-delimited JSON.
- Data access:
  - Prefer using `DatabaseHelper.GetConnection()` + Dapper for DB operations in `ServerAdmin`.
  - Keep SQL/table naming consistent with existing schema created by `DatabaseHelper`.
- UI behavior:
  - Preserve current UX intent: modern flat admin UI and dark gaming client UI.
  - For responsive/positioning changes, follow patterns already used in existing forms/components.

## Known Pitfalls
- This is a Windows-only WinForms solution (`net10.0-windows` for app projects).
- Protocol changes can silently break login/session flow if only one side is updated.
- SQLite write contention can appear if introducing concurrent write-heavy operations without care.

## Docs
- Project overview and run/setup flow: `README.md`
- Server UI details: `ServerAdmin/UI_DESIGN.md`
- Client UI details: `ClientApp/CLIENTUI_DESIGN.md`
# Repository Guidelines

## Project Structure & Module Organization

- `Gilead.API/`: ASP.NET Core startup, controllers, `appsettings*.json`, and HTTP scratch file.
- `Gilead.Application/`: DTOs, service interfaces, service implementations, and `ServiceResult`.
- `Gilead.Domain/`: domain entities and enums only.
- `Gilead.Infrastructure/`: Dapper repositories, SQL connection factory, Redis queue cache, and dependency registration.
- `Gilead.DB/`: SQL Server table, TVP, and stored procedure scripts. Keep database changes here with clear names.
- `k8s/`: Kubernetes resources for API, SQL Server, Redis, and database initialization.

## Build, Test, and Development Commands

- `dotnet restore Gilead.sln`: restore NuGet packages.
- `dotnet build Gilead.sln`: compile all projects.
- `dotnet run --project Gilead.API/Gilead.API.csproj`: run the API locally.
- `docker build -t gilead-api:latest .`: build the API container image.
- `kubectl apply -k k8s`: deploy the Kubernetes bundle after updating secrets and image names.

Local dependencies are SQL Server and Redis. Configure `ConnectionStrings__GileadDb`, `Redis__ConnectionString`, and `Redis__InstanceName` for container or Kubernetes runs.

## Coding Style & Naming Conventions

Use standard C# conventions: four-space indentation, PascalCase for public types and members, camelCase for locals and parameters, and async methods ending in `Async`. Nullable reference types and implicit usings are enabled, so keep null handling explicit.

Follow the existing layering: controllers call application services, services depend on repository interfaces, and infrastructure implements Dapper access. Prefer stored procedures over inline SQL and keep procedure names in the current `usp_Area_Action` style.

## Testing Guidelines

No test project is currently committed. When adding tests, create `Gilead.Tests/` or `Gilead.Application.Tests/`, add it to `Gilead.sln`, and use `dotnet test Gilead.sln`. Name test files after the unit under test, for example `EncounterServiceTests.cs`, and cover service rules, repository mappings, and controller responses.

## Commit & Pull Request Guidelines

Recent commits use short, informal imperative summaries such as `finish medical portal` and merge commits for feature branches. Keep new commit messages concise but more specific where possible, for example `Add lab result validation`.

Pull requests should include a brief summary, affected API/database areas, configuration changes, and verification steps such as `dotnet build Gilead.sln` or manual endpoint checks. Link issues when available and note any SQL script or Kubernetes secret changes explicitly.

## Security & Configuration Tips

Do not commit real SQL Server passwords, Redis endpoints, or production connection strings. Use environment variables for deployment overrides and keep `k8s/secrets.yaml` placeholders sanitized before sharing changes.

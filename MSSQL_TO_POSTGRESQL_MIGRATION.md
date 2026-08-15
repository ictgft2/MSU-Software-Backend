# MSSQL to PostgreSQL Migration Report

## A. Migration Summary

The application now uses PostgreSQL end to end and has no runtime Microsoft SQL Server dependency.

- Replaced `Microsoft.Data.SqlClient` and `dbup-sqlserver` with `Npgsql` and `dbup-postgresql`.
- Replaced the SQL Server connection factory, exception handling, DbUp database creation, and `sp_getapplock` migration lock with PostgreSQL equivalents.
- Kept the existing Dapper repository/application architecture. Repository calls now invoke PostgreSQL functions through parameterized `SELECT` statements.
- Converted the complete schema to the unquoted `public` schema. PostgreSQL stores these identifiers lowercase, while unquoted routine/query names remain case-insensitive and Dapper maps returned columns to the existing PascalCase models.
- Converted 40 T-SQL stored procedures to PostgreSQL SQL functions. The one multi-result encounter aggregate is executed as one parameterized PostgreSQL multi-statement query because PostgreSQL functions do not return SQL Server-style multiple result sets.
- Replaced the two SQL Server TVPs with JSONB recordsets. Each prescription or lab-request collection is still inserted by one set-based database call inside the existing consultation transaction.
- Converted the schema, constraints, indexes, drug-register view, and rerunnable test seed. Added a transaction-based PostgreSQL verification suite.
- Added database-aware health checking, PostgreSQL Docker Compose, a PostgreSQL Kubernetes deployment/readiness job, and corrected the API container build/runtime port.
- UUIDs remain application-generated. The repository contains no identity columns, sequences, `rowversion`, computed columns, `MERGE`, `OUTPUT`, or SQL-generated UUID defaults to migrate.

PostgreSQL-specific decisions:

- `public` is the single application schema; no compatibility `dbo` schema is created.
- Main identifiers are unquoted and fold to lowercase. Only the legacy `LabResults.Values` column needs the isolated quoted name `"values"` because `VALUES` is PostgreSQL syntax.
- Every former `datetimeoffset` column is `timestamptz`. Connections and date filters use UTC, and Dapper normalizes incoming `DateTimeOffset` values to UTC.
- JSON-shaped `Diagnosis` and `Values` properties remain `text`, preserving the existing string API contracts and avoiding new validation behavior.
- Patient-name search uses `ILIKE` to preserve the common SQL Server case-insensitive search behavior.

## B. Files Changed

Repository/tooling and documentation:

- `.dockerignore` — allows database scripts into the container build context.
- `.gitignore` — removes obsolete SQL Server data-file patterns.
- `AGENTS.md` — updates repository/runtime guidance from SQL Server procedures/TVPs to PostgreSQL functions.
- `Dockerfile` — copies embedded database scripts, publishes successfully, and serves port 8080.
- `docker-compose.yml` — adds a complete PostgreSQL 16, Redis, and API development stack with health checks and configurable host ports.
- `README.md` — documents PostgreSQL configuration, seed/verification commands, Docker Compose, and Kubernetes.
- `MSSQL_TO_POSTGRESQL_MIGRATION.md` — this migration handoff report.

API and infrastructure:

- `Gilead.API/Middleware/ExceptionHandlingMiddleware.cs` — maps `NpgsqlException` instead of `SqlException`.
- `Gilead.API/Program.cs` — makes `/health` verify a live PostgreSQL connection.
- `Gilead.API/appsettings.json` — uses a PostgreSQL connection string and UTC timezone.
- `Gilead.Infrastructure/Gilead.Infrastructure.csproj` — replaces SQL Server packages/TVP resources with the PostgreSQL DbUp provider.
- `Gilead.Infrastructure/Data/SqlConnectionFactory.cs` — removed.
- `Gilead.Infrastructure/Data/PostgresConnectionFactory.cs` — adds Npgsql connections and managed `postgres://`/`postgresql://` URL conversion.
- `Gilead.Infrastructure/Data/DatabaseMigrationRunner.cs` — uses PostgreSQL database creation, advisory locking, schema detection, DbUp execution, and repeatable functions.
- `Gilead.Infrastructure/Data/DapperTypeHandlers.cs` — adds UTC-safe `DateTimeOffset` handling.
- `Gilead.Infrastructure/DependencyInjection.cs` — registers `PostgresConnectionFactory`.
- `Gilead.Infrastructure/Repositories/GileadRepositories.cs` — invokes PostgreSQL functions, converts the encounter aggregate query, replaces TVPs with JSONB bulk calls, and passes cancellation tokens to Dapper.

Database:

- `Gilead.DB/Tables/CreateTables.sql` — PostgreSQL tables, PK/FK/unique/default constraints, 15 supporting indexes, and `vw_DrugRegister`.
- `Gilead.DB/TVPs/CreateTVPs.sql` — removed; PostgreSQL bulk functions consume JSONB.
- `Gilead.DB/Seed/TestData.sql` — rerunnable PostgreSQL seed with UUID, boolean, interval, UTC timestamp, and quoted-value conversions.
- `Gilead.DB/Verification/VerifyMigration.sql` — validates all routines, reads/writes, precision, UTC normalization, bulk operations, pagination, and rollback.
- Every file under `Gilead.DB/StoredProcedures/` listed in section D — converted from a T-SQL procedure to a PostgreSQL function.

Kubernetes:

- `k8s/sqlserver.yaml` — removed.
- `k8s/postgres.yaml` — adds PostgreSQL 16 service, deployment, PVC, readiness, and resource settings.
- `k8s/db-init-job.yaml` — replaces `sqlcmd`/SQL Server creation with PostgreSQL readiness and `psql` connectivity verification.
- `k8s/kustomization.yaml` — references `postgres.yaml` instead of `sqlserver.yaml`.
- `k8s/secrets.yaml` — replaces MSSQL keys/connection strings with sanitized PostgreSQL placeholders.
- `k8s/README.md` — documents the PostgreSQL Kubernetes bundle.

## C. MSSQL to PostgreSQL Conversion Matrix

| MSSQL construct | PostgreSQL replacement | Notes |
|---|---|---|
| `Microsoft.Data.SqlClient` | `Npgsql` | SQL Server package removed. |
| `dbup-sqlserver` | `dbup-postgresql` | DbUp variable substitution is disabled so PostgreSQL dollar-quoted functions execute correctly. |
| `SqlConnection` | `NpgsqlConnection` | Central connection-string resolver supports standard strings and managed database URLs. |
| `dbo` | `public` | No compatibility schema. |
| `UNIQUEIDENTIFIER` | `uuid` | Existing application-generated `Guid` behavior retained. |
| `NVARCHAR(n)` | `varchar(n)` | PostgreSQL strings are Unicode. |
| `NVARCHAR(MAX)` | `text` | No length truncation introduced. |
| `BIT` / `0` / `1` | `boolean` / `false` / `true` | Defaults and predicates use booleans. |
| `INT` | `integer` | Same application mappings. |
| `DECIMAL(p,s)` | `numeric(p,s)` | Vital numeric precision retained exactly. |
| `DATETIMEOFFSET` | `timestamptz` | Instants preserved and normalized to UTC. |
| `SYSDATETIMEOFFSET()` | `CURRENT_TIMESTAMP` | Returns a transaction-aware PostgreSQL timestamp. |
| `DATEADD` | interval arithmetic | Seed semantics retained. |
| `CAST(timestamp AS date)` | `(timestamp AT TIME ZONE 'UTC')::date` | Date filters are deterministic in UTC. |
| `TOP 1` | `LIMIT 1` | Ordering retained. |
| `OFFSET ... FETCH NEXT` | `LIMIT ... OFFSET` | Deterministic `HandoverAt DESC` ordering retained. |
| string `+` | `||` | Used by the patient search pattern. |
| `LIKE` under CI collation | `ILIKE` | Patient-name search remains case-insensitive. |
| TVP / `SqlDbType.Structured` | `jsonb_to_recordset` | One set-based round trip per collection. |
| T-SQL stored procedure | SQL/set-returning PostgreSQL function | Application calls use parameterized `SELECT`. |
| multi-result procedure | parameterized multi-statement query | Used only by encounter detail; API contract is unchanged. |
| `sp_getapplock` | `pg_advisory_lock` | Prevents concurrent startup migrations. |
| `GO` | removed | Scripts execute directly through PostgreSQL/DbUp. |
| `[Values]` | `"values"` | Single quoted legacy identifier; model/property name remains `Values`. |
| `IDENTITY`, `SCOPE_IDENTITY`, `OUTPUT`, `MERGE`, `ROWVERSION` | Not applicable | None were present in the repository. |

## D. Stored Procedure Conversion

All replacements are PostgreSQL functions in `public`; physical names fold to lowercase while existing `usp_Area_Action` source naming is retained.

| Original procedure / file | PostgreSQL replacement type | Application caller | Status |
|---|---|---|---|
| `usp_Consultation_GetByEncounter` | Set-returning function | `ConsultationRepository.GetByEncounterAsync` | Converted/tested |
| `usp_Consultation_Insert` | Command function | `ConsultationRepository.CreateWithChildrenAsync` | Converted/tested |
| `usp_ContactTrace_GetByEncounter` | Set-returning function | `ContactTraceRepository.GetByEncounterAsync` | Converted/tested |
| `usp_ContactTrace_Insert` | Insert-returning function | `ContactTraceRepository.InsertAsync` | Converted/tested |
| `usp_ContactTrace_Update` | Update-returning function | `ContactTraceRepository.UpdateAsync` | Converted/tested |
| `usp_Dispensing_GetById` | Set-returning function | `DispensingRepository.GetByIdAsync` | Converted/tested |
| `usp_Dispensing_Insert` | Insert-returning function | `DispensingRepository.InsertAsync` | Converted/tested |
| `usp_DressingOrder_Complete` | Command function | `DressingRepository.CompleteAsync` | Converted/tested |
| `usp_DressingOrder_GetById` | Set-returning function | `DressingRepository.GetByIdAsync` | Converted/tested |
| `usp_DressingOrder_GetWorklist` | Set-returning function | `DressingRepository.GetWorklistAsync` | Converted/tested |
| `usp_DressingOrder_Insert` | Command function | `ConsultationRepository.CreateWithChildrenAsync` | Converted/tested |
| `usp_Encounter_GetById` | Set-returning function; aggregate uses inline multi-query | `EncounterRepository.GetByIdAsync` / `GetDetailAsync` | Converted/tested |
| `usp_Encounter_GetList` | Set-returning function | `EncounterRepository.GetListAsync` | Converted/tested |
| `usp_Encounter_Insert` | Insert-returning function | `EncounterRepository.InsertAsync` | Converted/tested |
| `usp_Encounter_UpdateStatus` | Command function | `EncounterRepository.UpdateStatusAsync` / consultation workflow | Converted/tested |
| `usp_DrugHandover_Confirm` | Command function | `DrugHandoverRepository.ConfirmAsync` | Converted/tested |
| `usp_DrugHandover_GetById` | Set-returning function | `DrugHandoverRepository.GetByIdAsync` | Converted/tested |
| `usp_DrugHandover_GetWorklist` | Set-returning function | `DrugHandoverRepository.GetWorklistAsync` | Converted/tested |
| `usp_DrugHandover_Insert` | Insert-returning function | `DrugHandoverRepository.InsertAsync` | Converted/tested |
| `usp_LabRequest_GetById` | Set-returning function | `LabRepository.GetRequestAsync` | Converted/tested |
| `usp_LabRequest_GetWorklist` | Set-returning function | `LabRepository.GetRequestsAsync` | Converted/tested |
| `usp_LabRequest_InsertBulk` | JSONB bulk command function | `ConsultationRepository.CreateWithChildrenAsync` | Converted/tested |
| `usp_LabResult_GetByEncounter` | Set-returning function | `LabRepository.GetResultsByEncounterAsync` | Converted/tested |
| `usp_LabResult_Insert` | Insert-returning atomic function | `LabRepository.InsertResultAsync` | Converted/tested |
| `usp_Patient_GetById` | Set-returning function | `PatientRepository.GetByIdAsync` | Converted/tested |
| `usp_Patient_Insert` | Insert-returning function | `PatientRepository.InsertAsync` | Converted/tested |
| `usp_Patient_Search` | Set-returning function using `ILIKE` | `PatientRepository.SearchAsync` | Converted/tested |
| `usp_Prescription_AllHandedOverForEncounter` | Boolean scalar function | `PrescriptionRepository.AllHandedOverForEncounterAsync` | Converted/tested |
| `usp_Prescription_GetById` | Set-returning function | `PrescriptionRepository.GetByIdAsync` | Converted/tested |
| `usp_Prescription_GetWorklist` | Set-returning function | `PrescriptionRepository.GetWorklistAsync` | Converted/tested |
| `usp_Prescription_InsertBulk` | JSONB bulk command function | `ConsultationRepository.CreateWithChildrenAsync` | Converted/tested |
| `usp_Prescription_UpdateStatus` | Command function | `PrescriptionRepository.UpdateStatusAsync` | Converted/tested |
| `usp_Register_ExportDrugs` | Set-returning view function | `RegisterRepository.ExportDrugsAsync` | Converted/tested |
| `usp_Register_GetDrugs` | Paginated set-returning view function | `RegisterRepository.GetDrugsAsync` | Converted/tested |
| `usp_ServiceWindow_GetCurrent` | Set-returning function | `ServiceWindowRepository.GetCurrentAsync` | Converted/tested |
| `usp_ServiceWindow_Insert` | Insert-returning function | `ServiceWindowRepository.InsertAsync` | Converted/tested |
| `usp_ServiceWindow_Update` | Update-returning function | `ServiceWindowRepository.UpdateAsync` | Converted/tested |
| `usp_VitalSigns_GetByEncounter` | Set-returning function | `VitalsRepository.GetByEncounterAsync` | Converted/tested |
| `usp_VitalSigns_GetLatest` | Set-returning function with `LIMIT 1` | `VitalsRepository.GetLatestAsync` | Converted/tested |
| `usp_VitalSigns_Insert` | Insert-returning function | `VitalsRepository.InsertAsync` | Converted/tested |

## E. Remaining Risks

- Existing production data migration is not included because the repository contained no production export/import or ETL assets. A production cutover still needs a separately controlled MSSQL extraction, transformation, reconciliation, and PostgreSQL load using the converted schema.
- `timestamptz` preserves an instant but not the original textual UTC offset. The application uses UTC, and verification confirms instant preservation; consumers that relied on stored source offsets need separate review.
- PostgreSQL comparisons are case-sensitive except the explicitly migrated patient-name `ILIKE` search. Current status, route, phone, and enum comparisons are intentionally exact. The repository has no login/email/username feature to evaluate.
- The Kubernetes secret file contains sanitized placeholders and must be replaced through the deployment's real secret-management process.
- No .NET test project existed. `dotnet test` succeeds but discovers no test assemblies; database behavior is covered by `VerifyMigration.sql` and the executed HTTP workflow.
- The folder name `StoredProcedures` is retained to minimize repository churn, although its files now define PostgreSQL functions.

## F. Verification Results

| Check | Result |
|---|---|
| Restore/package resolution | Passed; only `Npgsql` and `dbup-postgresql` provide relational database access. |
| `dotnet build Gilead.sln --no-restore` | Passed, 0 warnings, 0 errors. |
| `dotnet test Gilead.sln --no-build` | Passed; repository has no test project. |
| Empty PostgreSQL schema initialization | Passed: 12 tables, constraints, 15 supporting indexes, and view created. |
| All 40 PostgreSQL functions loaded | Passed. |
| Seed first run and rerun | Passed; deterministic counts remained 7 patients, 7 encounters, 3 prescriptions, 2 lab requests, 2 dressing orders, and 2 handovers. |
| `VerifyMigration.sql` | Passed: all routines, bulk writes, case-insensitive search, pagination, UUIDs, UTC timestamps, numeric precision, updates, and rollback. |
| DbUp empty-database startup | Passed. |
| DbUp repeat startup | Passed: schema journal skipped run-once DDL and repeatable functions refreshed successfully. |
| API health/database connectivity | Passed (`200`, PostgreSQL healthy). |
| HTTP workflow | Passed: patient create/get/search; encounter; vitals; bulk consultation children; lab result; dispensing; handover; dressing; contact trace; register pagination; discharged aggregate. |
| Docker image build | Passed. |
| Docker Compose full stack | Passed; PostgreSQL, Redis, and API healthy on isolated ports. |
| Kubernetes render/client parse | Passed (`kubectl kustomize` plus client-side object parsing). |
| `git diff --check` | Passed. |
| Final MSSQL dependency audit | Passed; no SQL Server provider, connection, T-SQL, `dbo`, TVP, or SQL Server deployment reference remains. |

Temporary containers, volumes, test databases, data, and the temporary PostgreSQL role used for verification were removed after the checks completed.

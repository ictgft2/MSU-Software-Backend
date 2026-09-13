# The Gilead Medical Unit Backend

The Gilead is a .NET 8 backend API for managing a medical unit with two patient pathways:

- Emergency admissions
- Cold Case outpatient visits

The API manages patient registration, encounters, waiting queues, vitals, consultations, lab work, dressing orders, pharmacy dispensing, protocol handover, contact tracing, drug register reporting, and cold-case service windows.

## Architecture

The codebase follows Clean Architecture:

```text
Gilead.API             HTTP controllers and API startup
Gilead.Application     DTOs, service interfaces, business logic, ServiceResult
Gilead.Domain          Entities and enums
Gilead.Infrastructure  Dapper repositories, PostgreSQL connection factory, Redis queue cache
Gilead.DB              PostgreSQL tables, functions, views, and seed data
k8s                    Kubernetes resources for API, PostgreSQL, Redis, and DB init
```

Data access is Dapper-only and runs through repository classes. SQL operations are implemented as PostgreSQL functions. Bulk prescription and lab-request inserts use set-based JSONB recordsets. The cold-case waiting queue uses Redis sorted sets.

## Runtime Requirements

- .NET 8 SDK
- PostgreSQL 16 or newer
- Redis
- Docker, if building containers
- Kubernetes and `kubectl`, if deploying to Kubernetes

## Configuration

The API reads these keys:

```json
{
  "ConnectionStrings": {
    "GileadDb": "Host=localhost;Port=5432;Database=GileadDb;Username=gilead;Password=Change_this_Postgres_Password_123!;Timezone=UTC"
  },
  "Redis": {
    "ConnectionString": "localhost:6379",
    "User": "",
    "Password": "Change_this_Redis_Password_123!",
    "InstanceName": "gilead:"
  }
}
```

In containers or Kubernetes, override with environment variables:

```text
ConnectionStrings__GileadDb
Redis__ConnectionString
Redis__User
Redis__Password
Redis__InstanceName
```

## Database Setup

The API runs DbUp migrations on application startup. Migration scripts are embedded from:

```text
Gilead.DB/Tables/CreateTables.sql
Gilead.DB/StoredProcedures/**/*.sql
```

DbUp creates the `GileadDb` database if it does not exist, records executed schema scripts in its journal, and refreshes PostgreSQL functions on later starts. The Kubernetes PostgreSQL container creates the configured database; `gilead-db-init` verifies that it is ready before API pods run migrations.

### Test Data

Optional refreshable seed data is available at:

```text
Gilead.DB/Seed/TestData.sql
```

Run it after the API has created the schema:

```bash
PGPASSWORD='YourStrongPassword!' psql -h localhost -p 5432 -U gilead -d GileadDb -v ON_ERROR_STOP=1 -f Gilead.DB/Seed/TestData.sql
```

The script deletes and recreates only its deterministic seed rows, sets today's cold-case service window to open all day, and covers queued, pharmacy, lab, dressing, handover, discharged, referred, contact-trace, and drug-register scenarios.

Run the non-destructive migration verification suite after seeding:

```bash
PGPASSWORD='YourStrongPassword!' psql -h localhost -p 5432 -U gilead -d GileadDb -v ON_ERROR_STOP=1 -f Gilead.DB/Verification/VerifyMigration.sql
```

It validates every PostgreSQL function, bulk inserts, case-insensitive search, pagination, UUIDs, UTC timestamps, numeric precision, and transaction rollback. Its write checks run inside a transaction that is rolled back.

Postman files for the seeded data are available at:

```text
Postman/Gilead.API.postman_collection.json
Postman/Gilead.Local.postman_environment.json
```

Import both into Postman, select the `Gilead Local Seeded` environment, and run requests against the local API at `http://localhost:5000`. The environment contains the seeded patient, encounter, prescription, lab request, dressing order, and handover IDs.

## Local Run

Build:

```bash
dotnet build Gilead.sln
```

Run:

```bash
dotnet run --project Gilead.API/Gilead.API.csproj
```

Health check:

```text
GET /health
```

Swagger is available in development:

```text
/swagger
```

## Docker

Build the API image:

```bash
docker build -t gilead-api:latest .
```

Start the complete development stack (PostgreSQL, Redis, and API):

```bash
docker compose up --build
```

To run only the image, provide reachable PostgreSQL and Redis services:

```bash
docker run --rm -p 8080:8080 \
  -e ConnectionStrings__GileadDb="Host=host.docker.internal;Port=5432;Database=GileadDb;Username=gilead;Password=YourStrongPassword!;Timezone=UTC" \
  -e Redis__ConnectionString="host.docker.internal:6379" \
  -e Redis__Password="YourStrongRedisPassword!" \
  gilead-api:latest
```

## Kubernetes

Update:

- `k8s/secrets.yaml`: replace the placeholder PostgreSQL and Redis passwords.
- `k8s/api.yaml`: replace `ghcr.io/your-org/gilead-api:latest` with your pushed image.

Deploy:

```bash
kubectl apply -k k8s
```

Check:

```bash
kubectl -n gilead get pods
kubectl -n gilead rollout status deployment/gilead-api
kubectl -n gilead logs job/gilead-db-init
```

Port-forward:

```bash
kubectl -n gilead port-forward svc/gilead-api 8080:80
```

Then call:

```text
http://localhost:8080/health
```

## Main Workflow

Typical cold-case flow:

1. Admin sets today's cold-case service window.
2. Patient is registered.
3. Cold-case encounter is opened.
4. If patient age is greater than 40, vitals/BP check moves the encounter to the queue.
5. Patient is called from queue into consultation.
6. Doctor submits consultation and treatment plan.
7. Prescriptions, lab requests, and dressing orders are created as needed.
8. Pharmacy dispenses drugs and creates handover work.
9. Protocol confirms handover and counselling.
10. Encounter is discharged or referred.

Emergency encounters skip cold-case service-window validation and never enter the Redis queue.

## Endpoint Catalog

### Staff

```http
POST  /api/v1/staff
GET   /api/v1/staff?role=&isActive=
GET   /api/v1/staff/{staffId}
PATCH /api/v1/staff/{staffId}
```

Supported roles are `Doctor`, `Pharmacist`, `Nurse`, `Scientist`, `ProtocolOfficer`, `Registrar`, and `DressingNurse`.

### Patients

```http
POST /api/v1/patients
GET  /api/v1/patients/{patientId}
GET  /api/v1/patients/search?name=&phone=
```

### Encounters

```http
POST  /api/v1/encounters
GET   /api/v1/encounters/{encounterId}
GET   /api/v1/encounters?status=&date=&type=
PATCH /api/v1/encounters/{encounterId}/status
```

### Queue

```http
POST   /api/v1/queue/{encounterId}/join
DELETE /api/v1/queue/{encounterId}
GET    /api/v1/queue
GET    /api/v1/queue/{encounterId}/position
```

### Vital Signs

```http
POST /api/v1/encounters/{encounterId}/vitals
GET  /api/v1/encounters/{encounterId}/vitals
GET  /api/v1/encounters/{encounterId}/vitals/latest
```

### Consultation

```http
POST /api/v1/encounters/{encounterId}/consultation
GET  /api/v1/encounters/{encounterId}/consultation
```

### Lab

```http
GET  /api/v1/lab/requests?status=&date=
GET  /api/v1/lab/requests/{requestId}
POST /api/v1/lab/requests/{requestId}/results
GET  /api/v1/encounters/{encounterId}/lab-results
```

### Dressing

```http
GET   /api/v1/dressing/orders?status=
GET   /api/v1/dressing/orders/{orderId}
PATCH /api/v1/dressing/orders/{orderId}/complete
```

### Pharmacy

```http
GET  /api/v1/pharmacy/prescriptions?status=&date=
GET  /api/v1/pharmacy/prescriptions/{id}
POST /api/v1/pharmacy/prescriptions/{id}/dispense
```

### Protocol Handover

```http
GET  /api/v1/protocol/handovers?status=
GET  /api/v1/protocol/handovers/{handoverId}
POST /api/v1/protocol/handovers/{handoverId}/confirm
```

### Contact Tracing

```http
POST  /api/v1/encounters/{encounterId}/contact-trace
GET   /api/v1/encounters/{encounterId}/contact-trace
PATCH /api/v1/encounters/{encounterId}/contact-trace
```

### Drug Register

```http
GET /api/v1/register/drugs?date=&page=&limit=
GET /api/v1/register/drugs/export?date=&format=csv
```

### Service Window

```http
GET   /api/v1/service-window/current
POST  /api/v1/service-window
PATCH /api/v1/service-window/{windowId}
```

## Example Requests

Register a patient:

```json
{
  "fullName": "Jane Doe",
  "age": 45,
  "sex": "F",
  "phone": "08030000000",
  "address": "Main Street",
  "nextOfKinName": "John Doe",
  "nextOfKinPhone": "08031111111",
  "nextOfKinRelationship": "Spouse"
}
```

Set service window:

```json
{
  "date": "2026-06-28",
  "coldCaseOpenTime": "09:30",
  "coldCaseCloseTime": "17:00",
  "createdBy": "00000000-0000-0000-0000-000000000001"
}
```

Open encounter:

```json
{
  "patientId": "00000000-0000-0000-0000-000000000001",
  "admissionType": "ColdCase",
  "arrivalMode": "WalkedIn",
  "chiefComplaint": "Headache and fever",
  "registeredBy": "00000000-0000-0000-0000-000000000002"
}
```

## Response Wrapper

Application services return `ServiceResult` or `ServiceResult<T>`:

```json
{
  "succeeded": true,
  "error": null,
  "statusCode": 200,
  "data": {}
}
```

Failures include an error message and status code hint:

```json
{
  "succeeded": false,
  "error": "Cold case intake is closed.",
  "statusCode": 409,
  "data": null
}
```

## Notes

- No authentication middleware is currently configured. Staff IDs are supplied in request bodies where required.
- Redis queue keys use the pattern `gilead:queue:{yyyy-MM-dd}`.
- The queue is daily and FIFO using Redis sorted set scores.
- Emergency encounters bypass service-window validation and do not enter the queue.

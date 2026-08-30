# Gilead API — Production Deployment Guide
**Server:** `hng` (Ubuntu 24.04, 16GB RAM)
**Stack:** .NET 8 ASP.NET Core API + SQL Server 2022 + Redis 7
**Access:** `http://<server-public-ip>:8080`
**Branch:** `docs/deployment-guide`

---

## Pre-Deployment Context

This guide documents the actual deployment of the Gilead API (`MSU-Software-Backend`) on the `hng` server using Docker Compose to orchestrate three containers: the .NET API, SQL Server, and Redis.

| Adaptation | Reason |
|---|---|
| Port 8080 freed before deployment | Was occupied by `hng-detector` Python service — stopped and disabled |
| No Nginx or SSL configured | API served directly on public IP:8080, no domain needed |
| `CMD-SHELL` used in healthchecks | `CMD` array format triggered a Docker Compose parser panic |
| `Redis__InstanceName=gilead:` quoted | Trailing colon parsed as YAML map key — must be quoted |
| `sqlserver;Port=1433` format used | `sqlserver,1433` comma format triggered compose-go parser bug |
| Docker Compose v1 (docker-compose) available as fallback | v2 (`docker compose`) had compose-go rc.7 parser bug |

---

## Architecture

```
Internet
    │
    ▼
<server-ip>:8080
    │
    ▼
gilead_api (.NET 8, port 8080)
    │              │
    ▼              ▼
gilead_sqlserver  gilead_redis
(SQL Server 2022) (Redis 7)
(port 1433)       (port 6379)
    │
    ▼
GileadDb (auto-created + migrated on startup)
```

All three containers share the `gilead_network` Docker bridge network.

---

## Prerequisites Check

```bash
# Docker installed and running
docker ps

# Docker Compose version
docker compose version

# Port 8080 is free
ss -tlpn | grep ":8080"

# Port 1433 is free (SQL Server)
ss -tlpn | grep ":1433"

# Port 6379 is free (Redis)
ss -tlpn | grep ":6379"

# Sufficient disk space (need ~3GB for images + build)
df -h /
```

---

## Step 1 — Free Port 8080

Port 8080 was occupied by the `hng-detector` Python anomaly detection service. Check and free it:

```bash
# Identify what's on port 8080
sudo ss -tlpnp | grep ":8080"

# Check if it's a systemd service
sudo systemctl list-units --type=service --state=running | grep -i detector
```

If it's a systemd service, stop and disable it:
```bash
sudo systemctl stop hng-detector
sudo systemctl disable hng-detector
```

If it's just a process, kill it:
```bash
sudo kill <pid>
```

Verify port is free:
```bash
ss -tlpn | grep ":8080"
# Should return nothing
```

---

## Step 2 — Free Disk Space

SQL Server image is ~1.5GB, .NET SDK build layer ~1GB. Ensure at least 3GB free:

```bash
# Check current disk usage
df -h /

# Check Docker unused resources
docker system df

# Clean unused Docker images, containers, volumes, cache
docker system prune -af --volumes

# Clean system logs
sudo journalctl --vacuum-size=50M
sudo find /var/log -name "*.gz" -delete
sudo find /var/log -name "*.1" -delete

# Clean apt cache
sudo apt-get clean
sudo apt-get autoremove -y

# Re-check
df -h /
```

Target: at least 6GB free before building.

---

## Step 3 — Configure Git and Clone Repository

```bash
# Set git identity
git config --global user.name "Your Name"
git config --global user.email "your@email.com"

# Clone via HTTPS with PAT (private repo)
git clone https://<your-pat>@github.com/ictgft2/MSU-Software-Backend.git /opt/MSU-Software-Backend

# Or clone via SSH (recommended — no token exposure)
git clone git@github.com:ictgft2/MSU-Software-Backend.git /opt/MSU-Software-Backend

cd /opt/MSU-Software-Backend
ls -la
```

Confirm project structure:
```
MSU-Software-Backend/
├── Dockerfile
├── Gilead.sln
├── Gilead.API/
│   ├── appsettings.json
│   └── appsettings.Production.json
├── Gilead.Application/
├── Gilead.Domain/
├── Gilead.Infrastructure/
├── Gilead.DB/
└── docker-compose.yaml
```

---

## Step 4 — Review the Dockerfile

The existing multi-stage Dockerfile is production-ready — no changes needed:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY Gilead.sln ./
COPY Gilead.API/Gilead.API.csproj Gilead.API/
COPY Gilead.Application/Gilead.Application.csproj Gilead.Application/
COPY Gilead.Domain/Gilead.Domain.csproj Gilead.Domain/
COPY Gilead.Infrastructure/Gilead.Infrastructure.csproj Gilead.Infrastructure/
RUN dotnet restore Gilead.API/Gilead.API.csproj
COPY Gilead.API/ Gilead.API/
COPY Gilead.Application/ Gilead.Application/
COPY Gilead.Domain/ Gilead.Domain/
COPY Gilead.Infrastructure/ Gilead.Infrastructure/
COPY Gilead.DB/ Gilead.DB/
RUN dotnet publish Gilead.API/Gilead.API.csproj -c Release -o /app/publish /p:UseAppHost=false
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Gilead.API.dll"]
```

The multi-stage build pattern keeps the final image small — SDK is only used for building, runtime image ships without it.

---

## Step 5 — Create docker-compose.yaml

```bash
nano /opt/MSU-Software-Backend/docker-compose.yaml
```

Paste the following:

```yaml
services:
  api:
    build:
      context: .
    container_name: gilead_api
    ports:
      - "8080:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__GileadDb=Server=sqlserver;Port=1433;Database=GileadDb;User Id=sa;Password=StrongSaPassword123!;TrustServerCertificate=True
      - Redis__ConnectionString=redis:6379
      - Redis__Password=StrongRedisPassword123!
      - "Redis__InstanceName=gilead:"
    depends_on:
      sqlserver:
        condition: service_healthy
      redis:
        condition: service_healthy
    networks:
      - gilead_network
    restart: unless-stopped

  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: gilead_sqlserver
    environment:
      - ACCEPT_EULA=Y
      - MSSQL_SA_PASSWORD=StrongSaPassword123!
    ports:
      - "1433:1433"
    volumes:
      - sqlserver_data:/var/opt/mssql
    networks:
      - gilead_network
    healthcheck:
      test: ["CMD-SHELL", "/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P StrongSaPassword123! -No -Q 'SELECT 1' || exit 1"]
      interval: 10s
      timeout: 5s
      retries: 10
      start_period: 30s
    restart: unless-stopped

  redis:
    image: redis:7-alpine
    container_name: gilead_redis
    command: redis-server --requirepass StrongRedisPassword123!
    ports:
      - "6379:6379"
    volumes:
      - redis_data:/data
    networks:
      - gilead_network
    healthcheck:
      test: ["CMD-SHELL", "redis-cli -a StrongRedisPassword123! ping || exit 1"]
      interval: 10s
      timeout: 5s
      retries: 5
    restart: unless-stopped

volumes:
  sqlserver_data:
  redis_data:

networks:
  gilead_network:
```

> **Critical YAML gotchas learned during deployment:**
> - `Redis__InstanceName=gilead:` must be quoted — trailing colon is parsed as a YAML map key
> - Use `sqlserver;Port=1433` not `sqlserver,1433` — comma triggers a compose-go parser panic
> - Use `CMD-SHELL` not `CMD` array format for healthchecks — avoids parser bug in Docker Compose v2.24.6

Validate:
```bash
docker compose config
```

No errors means it's clean.

---

## Step 6 — Build and Start All Containers

```bash
cd /opt/MSU-Software-Backend
docker compose up --build -d
```

This will:
- Pull SQL Server 2022 image (~1.5GB) — first time only
- Pull Redis 7 Alpine image (~50MB) — first time only
- Build the .NET 8 API image using the multi-stage Dockerfile (~5-7 minutes first time)
- Start all three containers in dependency order: SQL Server → Redis → API

---

## Step 7 — Monitor Startup

```bash
docker compose logs -f api
```

Expected sequence:
```
[INF] Created database GileadDb
[INF] Beginning database upgrade
[INF] Executing Database Server script '..._001_Tables.CreateTables.sql'
[INF] Executing Database Server script '..._002_TVPs.CreateTVPs.sql'
[INF] Executing Database Server script '..._003_StoredProcedures...'
... (all stored procedures)
Now listening on: http://[::]:8080
```

The API automatically:
1. Creates `GileadDb` database on SQL Server if it doesn't exist
2. Runs all migration scripts (tables, TVPs, stored procedures)
3. Starts the HTTP server on port 8080

---

## Step 8 — Verify Deployment

```bash
# All three containers should show as Up/Healthy
docker ps | grep gilead

# Health endpoint
curl http://localhost:8080/health
# Expected: {"status":"Healthy"}

# Get public IP
curl -s -4 ifconfig.me

# Test from outside the server
curl http://<public-ip>:8080/health
```

---

## Step 9 — Disable HTTPS Redirect Warning (Optional)

The API logs show a warning about HTTPS redirection since no SSL is configured. Suppress it by creating a production appsettings override:

```bash
nano Gilead.API/appsettings.Production.json
```

```json
{
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://+:8080"
      }
    }
  }
}
```

Rebuild:
```bash
docker compose up --build -d
```

---

## Database — What Gets Created Automatically

The API uses DbUp for database migrations, running all scripts on startup:

| Script group | What it creates |
|---|---|
| `_001_Tables` | All database tables |
| `_002_TVPs` | Table-valued parameters |
| `_003_StoredProcedures/Consultation` | Consultation get/insert procedures |
| `_003_StoredProcedures/ContactTrace` | Contact tracing get/insert/update |
| `_003_StoredProcedures/Dispensing` | Drug dispensing procedures |
| `_003_StoredProcedures/Dressing` | Dressing order procedures |
| `_003_StoredProcedures/Encounters` | Patient encounter procedures |
| `_003_StoredProcedures/Handover` | Drug handover procedures |
| `_003_StoredProcedures/Lab` | Lab request/result procedures |
| `_003_StoredProcedures/Patients` | Patient search/insert/get |
| `_003_StoredProcedures/Prescriptions` | Prescription management |
| `_003_StoredProcedures/Register` | Drug register procedures |
| `_003_StoredProcedures/ServiceWindow` | Service window management |
| `_003_StoredProcedures/Vitals` | Vital signs procedures |

---

## Full Container Map After Deployment

| Container | Image | Port | Purpose |
|---|---|---|---|
| `gilead_api` | `.NET 8 ASP.NET Core` | `8080` | REST API |
| `gilead_sqlserver` | `SQL Server 2022` | `1433` | Primary database |
| `gilead_redis` | `Redis 7 Alpine` | `6379` | Caching layer |

---

## Useful Day-2 Commands

```bash
# Always run from project directory
cd /opt/MSU-Software-Backend

# View live API logs
docker compose logs -f api

# View all container logs
docker compose logs -f

# Check container health
docker ps | grep gilead

# Restart API only (after config changes)
docker compose restart api

# Stop all containers (data preserved)
docker compose down

# Full teardown including DB data — DESTRUCTIVE
docker compose down -v

# Rebuild after code changes
git pull
docker compose up --build -d

# Access SQL Server CLI
docker exec -it gilead_sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P StrongSaPassword123! -No

# Access Redis CLI
docker exec -it gilead_redis redis-cli -a StrongRedisPassword123!

# Check API health
curl http://localhost:8080/health

# View Swagger (if enabled in production)
curl http://localhost:8080/swagger/index.html
```

---

## Troubleshooting — Issues Encountered During Deployment

### Docker Compose panic: interface conversion
```
panic: interface conversion: interface {} is map[string]interface {}, not string
```
**Cause 1:** `Redis__InstanceName=gilead:` — trailing colon parsed as YAML map key.
**Fix:** Quote the value: `"Redis__InstanceName=gilead:"`

**Cause 2:** `sqlserver,1433` comma in connection string triggers compose-go parser bug.
**Fix:** Use `sqlserver;Port=1433` format instead.

**Cause 3:** `CMD` array format in healthcheck with complex strings.
**Fix:** Switch to `CMD-SHELL` format:
```yaml
test: ["CMD-SHELL", "/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P password -No -Q 'SELECT 1' || exit 1"]
```

### Port 8080 already in use
```
Bind for 0.0.0.0:8080 failed: port is already allocated
```
**Fix:**
```bash
sudo ss -tlpnp | grep ":8080"
sudo systemctl stop <service-name>
sudo systemctl disable <service-name>
```

### SQL Server container unhealthy
```bash
# Check SQL Server logs
docker compose logs sqlserver --tail=30

# Verify healthcheck path — mssql-tools path varies by image version
docker exec -it gilead_sqlserver find / -name sqlcmd 2>/dev/null
# Update healthcheck path in docker-compose.yaml accordingly
```

### Insufficient disk space during build
```bash
# Clean Docker resources
docker system prune -af --volumes

# Clean system logs and apt cache
sudo journalctl --vacuum-size=50M
sudo apt-get clean && sudo apt-get autoremove -y
```

### API starts but DB connection fails
```bash
# Verify SQL Server is healthy first
docker ps | grep sqlserver

# Test connection from API container
docker exec -it gilead_api curl http://localhost:8080/health

# Check API logs for connection errors
docker compose logs api | grep -i "error\|connect\|database"
```

---

> **Security reminders:**
> - Change default passwords (`StrongSaPassword123!`, `StrongRedisPassword123!`) before exposing to production traffic
> - SQL Server SA account has full DB access — consider creating a least-privilege app user
> - Port 1433 (SQL Server) and 6379 (Redis) are exposed on all interfaces — restrict via firewall if not needed externally:
> ```bash
> sudo ufw deny 1433
> sudo ufw deny 6379
> sudo ufw allow 8080
> sudo ufw enable
> ```
> - Consider adding Nginx + SSL if the API will be internet-facing long-term
ENDOFFILE
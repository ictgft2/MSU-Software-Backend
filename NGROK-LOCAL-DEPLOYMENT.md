# Gilead API — Local Deployment & ngrok Exposure Guide
**Branch:** `docs/deployment-guide`
**Stack:** .NET 8 ASP.NET Core API + SQL Server 2022 + Redis 7 + ngrok
**Purpose:** Run the Gilead API locally on a development PC and expose it publicly via ngrok tunnel

---

## What is ngrok?

ngrok is a utility that creates secure tunnels to locally hosted applications using a reverse proxy — it exposes any locally hosted application over the web by providing a publicly accessible HTTPS URL without deploying to a server.

**How it works:**
```
Internet
    │
    ▼
https://abc123.ngrok-free.app  (ngrok public URL)
    │
    ▼
ngrok cloud servers
    │
    ▼
ngrok agent (running on your PC)
    │
    ▼
localhost:8080 (Gilead API running in Docker)
    │
    ├── gilead_sqlserver (SQL Server 2022, port 1433)
    └── gilead_redis (Redis 7, port 6379)
```

No firewall changes, no port forwarding, no public IP needed — ngrok handles all of it.

---

## Minimum PC Specifications

Running the full Gilead API stack (Docker + .NET 8 + SQL Server 2022 + Redis) locally requires more resources than the ngrok agent itself, which is extremely lightweight.

| Component | Minimum | Recommended |
|---|---|---|
| **CPU** | 4 cores (x86_64) | 6+ cores |
| **RAM** | 8GB | 16GB |
| **Disk** | 10GB free | 20GB free |
| **OS** | Windows 10 64-bit / Ubuntu 20.04+ | Windows 11 / Ubuntu 22.04+ |
| **Internet** | 10 Mbps stable | 50 Mbps+ |
| **Docker** | Docker Desktop (Windows/Mac) / Docker Engine (Linux) | Latest version |
| **WSL2** | Required on Windows for Docker | WSL2 with Ubuntu 22.04 distro |

> **Note:** SQL Server 2022 alone requires a minimum of 2GB RAM. On an 8GB machine, running all three containers simultaneously alongside the OS will be very tight. 16GB is the practical minimum for comfortable use.

---

## Prerequisites

### 1. Docker installed and running
```bash
docker --version
docker compose version
docker ps   # should not error
```

On Windows — Docker Desktop must be running and WSL2 integration enabled:
- Docker Desktop → Settings → Resources → WSL Integration → Enable for your Ubuntu distro

### 2. Repository cloned
```bash
ls ~/MSU-Software-Backend
# Should show: Dockerfile, Gilead.API, Gilead.sln, docker-compose.yaml etc.
```

### 3. ngrok account
Sign up free at [ngrok.com/signup](https://ngrok.com/signup) — no credit card needed. Copy your **authtoken** from the dashboard after signing up.

---

## Step 1 — Install ngrok

### On WSL2 Ubuntu (recommended — matches your setup)

**Option A — apt (cleanest):**
```bash
curl -s https://ngrok-agent.s3.amazonaws.com/ngrok.asc \
  | sudo tee /etc/apt/trusted.gpg.d/ngrok.asc >/dev/null

echo "deb https://ngrok-agent.s3.amazonaws.com buster main" \
  | sudo tee /etc/apt/sources.list.d/ngrok.list

sudo apt update && sudo apt install ngrok -y
```

**Option B — snap:**
```bash
sudo snap install ngrok
```

**Option C — direct binary download:**
```bash
wget https://bin.equinox.io/c/bNyj1mQVY4c/ngrok-v3-stable-linux-amd64.tgz
tar -xzf ngrok-v3-stable-linux-amd64.tgz
sudo mv ngrok /usr/local/bin/
```

### On Windows (PowerShell, if not using WSL2)
```powershell
# Using winget
winget install ngrok.ngrok

# Or using Chocolatey
choco install ngrok
```

Verify installation:
```bash
ngrok version
# Expected: ngrok version 3.x.x
```

---

## Step 2 — Authenticate ngrok

```bash
ngrok config add-authtoken <your-authtoken-from-dashboard>
```

Verify token was saved:
```bash
cat ~/.config/ngrok/ngrok.yml
# Should show: authtoken: <your-token>
```

> The authtoken links your local ngrok agent to your ngrok account, unlocking longer sessions and a free static domain.

---

## Step 3 — Verify docker-compose.yaml Exists

```bash
cd ~/MSU-Software-Backend
ls docker-compose.yaml
cat docker-compose.yaml
```

If it doesn't exist, create it:
```bash
nano docker-compose.yaml
```

Paste:
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

---

## Step 4 — Check Available Disk Space

SQL Server image is ~1.5GB, .NET SDK build layer ~1GB. Ensure at least 5GB free:

```bash
df -h /
docker system df
```

If disk is low, clean up:
```bash
docker system prune -af --volumes
```

---

## Step 5 — Build and Start the API Stack

```bash
cd ~/MSU-Software-Backend
docker compose up --build -d
```

First run pulls images and builds — expect 5-10 minutes. Subsequent starts take ~30 seconds.

Monitor startup:
```bash
docker compose logs -f api
```

Wait for this sequence:
```
[INF] Created database GileadDb
[INF] Beginning database upgrade
[INF] Executing Database Server script '..._001_Tables.CreateTables.sql'
[INF] Executing Database Server script '..._002_TVPs.CreateTVPs.sql'
[INF] Executing Database Server script '..._003_StoredProcedures...'
Now listening on: http://[::]:8080
```

`Ctrl+C` to exit log stream — containers keep running in background.

---

## Step 6 — Verify API is Running Locally

```bash
# All three containers must show as Up/Healthy
docker ps | grep gilead

# Test health endpoint locally
curl http://localhost:8080/health
# Expected: {"status":"Healthy"}

# Test Swagger UI (if enabled)
curl -I http://localhost:8080/swagger/index.html
```

Do not proceed to ngrok until `curl http://localhost:8080/health` returns `{"status":"Healthy"}`.

---

## Step 7 — Expose API Publicly via ngrok

Open a **new terminal** (keep Docker running in the first):

```bash
ngrok http 8080
```

Output:
```
ngrok

Session Status    online
Account           your@email.com (Plan: Free)
Version           3.x.x
Region            United States (us)
Web Interface     http://127.0.0.1:4040
Forwarding        https://abc123def456.ngrok-free.app -> http://localhost:8080

Connections       ttl     opn     rt1     rt5     p50     p90
                  0       0       0.00    0.00    0.00    0.00
```

**Your public API URL is the `Forwarding` line** — e.g. `https://abc123def456.ngrok-free.app`

---

## Step 8 — Test Public Endpoint

```bash
# From any machine anywhere in the world
curl https://abc123def456.ngrok-free.app/health
# Expected: {"status":"Healthy"}

# Test a specific API route
curl https://abc123def456.ngrok-free.app/api/patients

# Test with Postman
# Import the collection from: Medical Unit App.postman_collection25032025.json
# Change base URL to: https://abc123def456.ngrok-free.app
```

Open Swagger in browser:
```
https://abc123def456.ngrok-free.app/swagger/index.html
```

---

## Step 9 — Monitor Traffic via ngrok Dashboard

ngrok ships a built-in web dashboard showing all requests in real time:

```
http://localhost:4040
```

Open in browser — shows:
- Every request hitting your API
- Request/response headers and body
- Status codes and latency
- Ability to replay requests

---

## Step 10 — Keep ngrok Running in Background (Optional)

By default, closing the terminal kills the ngrok tunnel. Use tmux to keep it alive:

```bash
# Install tmux if not present
sudo apt install tmux -y

# Start a named session
tmux new -s gilead-tunnel

# Run ngrok inside the session
ngrok http 8080

# Detach from session (tunnel stays alive)
# Press: Ctrl+B then D

# Re-attach later to check the URL
tmux attach -t gilead-tunnel

# List all sessions
tmux ls
```

---

## Step 11 — Get a Stable Static Domain (Free)

On the free plan, the ngrok URL changes every time you restart. Get a permanent free subdomain:

1. Go to [dashboard.ngrok.com/domains](https://dashboard.ngrok.com/domains)
2. Click **New Domain** — ngrok gives one free static domain per account
3. Copy your domain e.g. `your-name.ngrok-free.app`

Use it:
```bash
ngrok http --domain=your-name.ngrok-free.app 8080
```

Now the URL never changes across restarts.

---

## Stopping Everything

```bash
# Stop ngrok tunnel
# Press Ctrl+C in the ngrok terminal
# Or kill the tmux session:
tmux kill-session -t gilead-tunnel

# Stop Docker containers (data preserved)
cd ~/MSU-Software-Backend
docker compose down

# Stop and wipe all data (fresh start next time)
docker compose down -v
```

---

## Free Plan Limitations

| Feature | Free Plan | Paid Plan |
|---|---|---|
| Tunnels | 1 at a time | Multiple |
| URL stability | Random on each restart | Static domain |
| Session duration | Expires after inactivity | Persistent |
| Requests/month | Limited usage credit | Higher limits |
| Custom domain | ❌ | ✅ |
| Basic auth protection | ✅ | ✅ |
| Traffic dashboard | ✅ | ✅ |

> ngrok is designed for development and testing — not recommended for long-term production use. For production, use a proper server deployment as documented in `DEPLOYMENT.md`.

---

## Adding Basic Auth Protection (Recommended)

Anyone with the ngrok URL can access your API. Add basic auth to protect it during demos:

```bash
ngrok http 8080 --basic-auth="username:password"
```

Anyone accessing the URL will be prompted for credentials before reaching your API.

---

## Useful ngrok Commands

```bash
# Expose HTTP port
ngrok http 8080

# Expose with static domain
ngrok http --domain=your-name.ngrok-free.app 8080

# Expose with basic auth
ngrok http 8080 --basic-auth="user:pass"

# Expose TCP port (e.g. SQL Server directly)
ngrok tcp 1433

# Check ngrok status and active tunnels
curl http://localhost:4040/api/tunnels

# View ngrok config
cat ~/.config/ngrok/ngrok.yml

# Update ngrok agent
ngrok update
```

---

## Troubleshooting

### ngrok: command not found
```bash
# Verify installation
which ngrok
ls /usr/local/bin/ngrok

# Reinstall via apt
sudo apt update && sudo apt install ngrok -y
```

### Tunnel shows ERR_NGROK_108 (session limit)
**Cause:** Free plan only allows one active tunnel. Another session is still open.
**Fix:**
```bash
# Kill all ngrok processes
pkill ngrok
# Then restart
ngrok http 8080
```

### curl to ngrok URL returns 502 Bad Gateway
**Cause:** API container is not running or not healthy.
**Fix:**
```bash
docker ps | grep gilead_api
docker compose logs api --tail=20
# Restart if needed
docker compose restart api
# Wait for healthy then retry
curl http://localhost:8080/health
```

### ngrok URL works but API returns errors
**Cause:** Application-level issue, not ngrok.
**Fix:**
```bash
# Check API logs
docker compose logs api --tail=50

# Test locally first
curl http://localhost:8080/health
```

### SQL Server container unhealthy — API won't start
```bash
docker compose logs sqlserver --tail=20
# Common fix — wipe volume and restart fresh
docker compose down -v
docker compose up --build -d
```

### Docker build fails — out of disk space
```bash
docker system prune -af --volumes
df -h /
# Need at least 5GB free before building
```

---

> **Security reminders:**
> - Never share your ngrok authtoken publicly
> - Add `--basic-auth` when sharing the URL with external parties
> - The free ngrok URL is publicly accessible to anyone who has it — treat it like a production endpoint
> - Stop the ngrok tunnel when not in use
> - Change default passwords (`StrongSaPassword123!`) before sharing access
ENDOFFILE
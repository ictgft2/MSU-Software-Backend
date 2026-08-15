# Gilead Kubernetes Resources

This bundle runs:

- `Gilead.API` ASP.NET Core API
- PostgreSQL 16
- Redis 7
- A one-shot database readiness job that waits for PostgreSQL

## Build and Push Image

```bash
docker build -t ghcr.io/your-org/gilead-api:latest .
docker push ghcr.io/your-org/gilead-api:latest
```

Update `k8s/api.yaml` with your image name, or patch it during deploy.

## Secrets

Replace the placeholder PostgreSQL and Redis passwords in `k8s/secrets.yaml` before applying.

## Deploy

```bash
kubectl apply -k k8s
```

Check rollout:

```bash
kubectl -n gilead rollout status deployment/gilead-api
kubectl -n gilead get pods
kubectl -n gilead logs job/gilead-db-init
```

The API runs DbUp migrations on startup from SQL scripts embedded in `Gilead.Infrastructure`.

## Local Port Forward

```bash
kubectl -n gilead port-forward svc/gilead-api 8080:80
```

Swagger:

```text
http://localhost:8080/swagger
```

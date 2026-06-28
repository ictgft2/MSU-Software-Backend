# Gilead Kubernetes Resources

This bundle runs:

- `Gilead.API` ASP.NET Core API
- SQL Server 2022
- Redis 7
- A one-shot database initialization job for tables, TVPs, and stored procedures

## Build and Push Image

```bash
docker build -t ghcr.io/your-org/gilead-api:latest .
docker push ghcr.io/your-org/gilead-api:latest
```

Update `k8s/api.yaml` with your image name, or patch it during deploy.

## Secrets

Replace the placeholder SQL Server password in `k8s/secrets.yaml` before applying. SQL Server requires a strong password.

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

## Local Port Forward

```bash
kubectl -n gilead port-forward svc/gilead-api 8080:80
```

Swagger:

```text
http://localhost:8080/swagger
```

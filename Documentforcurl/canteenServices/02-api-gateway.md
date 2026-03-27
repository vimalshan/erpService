# Canteen Services API Documentation

---

## API Gateway

**Port:** 5188

### Gateway Auth – Get Token

```bash
curl -X POST http://localhost:5188/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'
```

**Response:**
```json
{ "token": "eyJhbGciOiJIUzI1NiIs..." }
```

### Gateway – List All Services

```bash
curl http://localhost:5188/api/gateway/services
```

### Gateway – Check Service Health

```bash
curl http://localhost:5188/api/gateway/services/canteenunit/health \
  -H "Authorization: Bearer <token>"
```

### Gateway Health Endpoints

| Endpoint | Description |
|---|---|
| `/health` | Full health check (all downstream services) |
| `/health/ready` | Readiness check (downstream + RabbitMQ) |
| `/health/live` | Liveness probe (always healthy) |

---


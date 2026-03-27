# AuthProvider API Documentation

---

## Minimal API Endpoints

These lightweight endpoints are available on both v1 and v2.

---

#### GET `/api/v1/minimal/auth/health`

Health check endpoint (Minimal API).

- **Auth:** Anonymous

**cURL:**
```bash
curl http://localhost:5200/api/v1/minimal/auth/health
```

**Response:** `200 OK`
```json
{
  "status": "Healthy",
  "service": "AuthProvider",
  "timestamp": "2025-01-15T10:00:00Z"
}
```

---

#### GET `/api/v1/minimal/auth/version`

Service version information.

- **Auth:** Anonymous

**cURL:**
```bash
curl http://localhost:5200/api/v1/minimal/auth/version
```

**Response:** `200 OK`
```json
{
  "version": "1.0",
  "framework": ".NET 8.0.1"
}
```

---

#### GET `/health`

ASP.NET Core Health Checks endpoint.

- **Auth:** Anonymous

**cURL:**
```bash
curl http://localhost:5200/health
```

**Response:** `200 OK`
```
Healthy
```

---


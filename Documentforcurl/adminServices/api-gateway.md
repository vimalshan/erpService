# Admin Services — API Documentation

---

## API Gateway

**Port**: 5000 (primary entry point)

### Gateway Endpoints

```bash
# List all registered downstream services
curl http://localhost:5000/api/gateway/services

# Check health of a specific service
curl -H "Authorization: Bearer <TOKEN>" \
  http://localhost:5000/api/gateway/services/finyear/health

# Get gateway auth token (includes all service scopes)
curl -X POST http://localhost:5000/api/gateway/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "admin"}'
```

### Ocelot Routing Table

| Service       | Gateway Route                                            | Downstream       | Rate Limit |
| ------------- | -------------------------------------------------------- | ---------------- | ---------- |
| Finyear       | `/finyear/{version}/{controller}/{action?}/{id?}`        | localhost:5186   | 100/min    |
| Location      | `/location/{version}/{controller}/{action?}/{id?}`       | localhost:7136   | 100/min    |
| Vendor        | `/vendor/{version}/{controller}/{action?}/{id?}`         | localhost:5181   | 100/min    |
| Scholarship   | `/scholarship/{version}/{controller}/{action?}/{id?}`    | localhost:5166   | 100/min    |
| Stationery    | `/stationery/{version}/{controller}/{action?}/{id?}`     | localhost:5273   | 100/min    |
| TDS           | `/tds/{version}/{controller}/{action?}/{id?}`            | localhost:5116   | 100/min    |
| LOV           | `/lov/{version}/{controller}/{action?}/{id?}`            | localhost:5184   | 100/min    |
| Transaction   | `/transaction/{version}/{controller}/{action?}/{id?}`    | localhost:5185   | 100/min    |
| Shared        | `/shared/{version}/{controller}/{action?}/{id?}`         | localhost:5008   | 150/min    |

---


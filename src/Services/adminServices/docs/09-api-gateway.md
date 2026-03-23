# API Gateway

> **Port:** 5000 | **Swagger:** http://localhost:5000/swagger

---

## REST Endpoints

### Generate Auth Token
```bash
curl -X POST http://localhost:5000/api/gateway/auth/token \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "admin123",
    "role": "Admin"
  }'
```
**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2026-03-24T10:00:00Z"
}
```

### Access Services Through Gateway (with token)

#### Finyear via Gateway
```bash
curl -X GET http://localhost:5000/finyear/v1/FinancialYear \
  -H "Authorization: Bearer <TOKEN>"
```

#### Location via Gateway
```bash
curl -X GET http://localhost:5000/location/v1/location-app-maps \
  -H "Authorization: Bearer <TOKEN>"
```

#### LOV via Gateway
```bash
curl -X GET http://localhost:5000/lov/v1/LovType \
  -H "Authorization: Bearer <TOKEN>"
```

#### Vendor via Gateway
```bash
curl -X GET http://localhost:5000/vendor/v1/vendors \
  -H "Authorization: Bearer <TOKEN>"
```

#### Scholarship via Gateway
```bash
curl -X GET "http://localhost:5000/scholarship/v1/Scholarships?page=1&pageSize=10" \
  -H "Authorization: Bearer <TOKEN>"
```

#### Stationery via Gateway
```bash
curl -X GET http://localhost:5000/stationery/v1/health \
  -H "Authorization: Bearer <TOKEN>"
```

#### TDS via Gateway
```bash
curl -X GET "http://localhost:5000/tds/v1/Vendors?page=1&pageSize=10" \
  -H "Authorization: Bearer <TOKEN>"
```

#### Transaction via Gateway
```bash
curl -X GET "http://localhost:5000/transaction/v1/Requests?locationId=101" \
  -H "Authorization: Bearer <TOKEN>"
```

### Health Check
```bash
curl -X GET http://localhost:5000/health
```

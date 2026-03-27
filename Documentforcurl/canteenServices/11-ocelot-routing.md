# Canteen Services API Documentation

---

## Ocelot Gateway Routing

All routes go through `http://localhost:5188`. JWT Bearer auth required for most routes. Rate limit: 100 req/min. Circuit breaker: 3 failures → 30s break.

| Gateway Route | Downstream | Port |
|---|---|---|
| `/api/canteen-unit/{everything}` | `/api/{everything}` | 5190 |
| `/api/canteen-unit/health` | `/health` | 5190 |
| `/api/canteen-unit/graphql` | `/graphql` | 5190 |
| `/api/canteen-unit/swagger/{everything}` | `/swagger/{everything}` | 5190 |
| `/api/card-management/{everything}` | `/api/{everything}` | 5191 |
| `/api/card-management/health` | `/health` | 5191 |
| `/api/card-management/graphql` | `/graphql` | 5191 |
| `/api/card-management/swagger/{everything}` | `/swagger/{everything}` | 5191 |
| `/api/deduction/{everything}` | `/api/{everything}` | 5192 |
| `/api/deduction/health` | `/health` | 5192 |
| `/api/deduction/graphql` | `/graphql` | 5192 |
| `/api/deduction/swagger/{everything}` | `/swagger/{everything}` | 5192 |
| `/api/eligibility/{everything}` | `/api/{everything}` | 5193 |
| `/api/eligibility/health` | `/health` | 5193 |
| `/api/eligibility/graphql` | `/graphql` | 5193 |
| `/api/eligibility/swagger/{everything}` | `/swagger/{everything}` | 5193 |
| `/api/itemmaster/{everything}` | `/api/{everything}` | 5194 |
| `/api/itemmaster/health` | `/health` | 5194 |
| `/api/itemmaster/graphql` | `/graphql` | 5194 |
| `/api/itemmaster/swagger/{everything}` | `/swagger/{everything}` | 5194 |
| `/api/referencedata/{everything}` | `/api/{everything}` | 5195 |
| `/api/referencedata/health` | `/health` | 5195 |
| `/api/referencedata/graphql` | `/graphql` | 5195 |
| `/api/referencedata/swagger/{everything}` | `/swagger/{everything}` | 5195 |
| `/api/swipe-transaction/{everything}` | `/api/{everything}` | 5196 |
| `/api/swipe-transaction/health` | `/health` | 5196 |
| `/api/swipe-transaction/graphql` | `/graphql` | 5196 |
| `/api/swipe-transaction/swagger/{everything}` | `/swagger/{everything}` | 5196 |
| `/api/canteen-transaction/{everything}` | `/api/{everything}` | 5197 |
| `/api/canteen-transaction/health` | `/health` | 5197 |
| `/api/canteen-transaction/graphql` | `/graphql` | 5197 |
| `/api/canteen-transaction/swagger/{everything}` | `/swagger/{everything}` | 5197 |

### Via Gateway cURL Examples

```bash
# Get a gateway token first
TOKEN=$(curl -s -X POST http://localhost:5188/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}' | jq -r '.token')

# CanteenUnit via gateway
curl http://localhost:5188/api/canteen-unit/CanteenUnits \
  -H "Authorization: Bearer $TOKEN"

# CardManagement via gateway
curl "http://localhost:5188/api/card-management/GuestCards?pageNumber=1&pageSize=10" \
  -H "Authorization: Bearer $TOKEN"

# Deduction via gateway
curl http://localhost:5188/api/deduction/Deduction/employee/10001 \
  -H "Authorization: Bearer $TOKEN"

# ItemMaster via gateway
curl http://localhost:5188/api/itemmaster/CanteenItemMaster/1001 \
  -H "Authorization: Bearer $TOKEN"

# SwipeTransaction via gateway
curl http://localhost:5188/api/swipe-transaction/SwipeTransactions/pending \
  -H "Authorization: Bearer $TOKEN"

# CanteenTransaction via gateway
curl http://localhost:5188/api/canteen-transaction/CanteenTransaction/12345 \
  -H "Authorization: Bearer $TOKEN"

# GraphQL via gateway
curl -X POST http://localhost:5188/api/canteen-unit/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"query": "{ canteenUnits { unComCod unitName } }"}'

# Health via gateway
curl http://localhost:5188/api/canteen-unit/health
```

---


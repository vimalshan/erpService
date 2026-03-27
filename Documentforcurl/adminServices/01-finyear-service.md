# Admin Services — API Documentation

---

## 1. Financial Year Service

**Port**: 5186 · **Base**: `/api/financialyear`

### REST Endpoints

| Method   | Endpoint                              | Description                    |
| -------- | ------------------------------------- | ------------------------------ |
| `GET`    | `/api/financialyear`                  | Get all financial years        |
| `GET`    | `/api/financialyear/{id}`             | Get by ID                      |
| `GET`    | `/api/financialyear/current`          | Get current active year        |
| `GET`    | `/api/financialyear/by-name/{name}`   | Get by name                    |
| `POST`   | `/api/financialyear`                  | Create financial year          |
| `PUT`    | `/api/financialyear/{id}`             | Update financial year          |
| `DELETE` | `/api/financialyear/{id}`             | Delete financial year          |

### cURL Examples

```bash
# Get all financial years
curl http://localhost:5186/api/financialyear \
  -H "Authorization: Bearer <TOKEN>"

# Get current financial year
curl http://localhost:5186/api/financialyear/current \
  -H "Authorization: Bearer <TOKEN>"

# Get financial year by ID
curl http://localhost:5186/api/financialyear/1 \
  -H "Authorization: Bearer <TOKEN>"

# Create financial year
curl -X POST http://localhost:5186/api/financialyear \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "2025-2026",
    "startDate": "2025-04-01",
    "endDate": "2026-03-31",
    "isActive": true
  }'

# Update financial year
curl -X PUT http://localhost:5186/api/financialyear/1 \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "2025-2026",
    "startDate": "2025-04-01",
    "endDate": "2026-03-31",
    "isActive": false
  }'

# Delete financial year
curl -X DELETE http://localhost:5186/api/financialyear/1 \
  -H "Authorization: Bearer <TOKEN>"
```

### GraphQL

**Endpoint**: `POST http://localhost:5186/graphql`

```bash
# Query: Get all financial years
curl -X POST http://localhost:5186/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getAllFinancialYears(pageNumber: 1, pageSize: 20) { id name startDate endDate durationInDays status isActive updatedOn } }"
  }'

# Query: Get current financial year
curl -X POST http://localhost:5186/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getCurrentFinancialYear { id name startDate endDate isActive } }"
  }'

# Query: Get financial year by ID
curl -X POST http://localhost:5186/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getFinancialYearById(id: 1) { id name startDate endDate isActive } }"
  }'

# Mutation: Create financial year
curl -X POST http://localhost:5186/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createFinancialYear(input: { name: \"2025-2026\", startDate: \"2025-04-01\", endDate: \"2026-03-31\", isActive: true }) { financialYear { id name } } }"
  }'

# Mutation: Update financial year
curl -X POST http://localhost:5186/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { updateFinancialYear(input: { id: 1, name: \"2025-2026\", isActive: false }) { financialYear { id name isActive } } }"
  }'

# Mutation: Delete financial year
curl -X POST http://localhost:5186/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { deleteFinancialYear(id: 1) { success } }"
  }'
```

---


# Admin Services — API Documentation

---

## 3. Vendor Service

**Port**: 5181 · **Base**: `/api/vendors`

### REST Endpoints

| Method   | Endpoint                          | Description          |
| -------- | --------------------------------- | -------------------- |
| `GET`    | `/api/vendors?status=`            | Get all (opt filter) |
| `GET`    | `/api/vendors/{id}`               | Get by ID            |
| `POST`   | `/api/vendors`                    | Create vendor        |
| `PUT`    | `/api/vendors/{id}`               | Update vendor        |
| `DELETE` | `/api/vendors/{id}?updatedBy=`    | Deactivate (soft)    |
| `POST`   | `/api/auth/token`                 | Get JWT token        |

### cURL Examples

```bash
# Get auth token
curl -X POST http://localhost:5181/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "admin"}'

# Get all vendors
curl http://localhost:5181/api/vendors \
  -H "Authorization: Bearer <TOKEN>"

# Get vendor by ID
curl http://localhost:5181/api/vendors/1 \
  -H "Authorization: Bearer <TOKEN>"

# Get active vendors only
curl "http://localhost:5181/api/vendors?status=Active" \
  -H "Authorization: Bearer <TOKEN>"

# Create vendor
curl -X POST http://localhost:5181/api/vendors \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "categoryId": 1,
    "locationId": 100,
    "name": "Office Supplies Co.",
    "address": "123 Main Street",
    "email": "vendor@example.com",
    "updatedBy": 1,
    "liveStatus": "Y"
  }'

# Update vendor
curl -X PUT http://localhost:5181/api/vendors/1 \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "categoryId": 1,
    "locationId": 100,
    "name": "Office Supplies Co. (Updated)",
    "address": "456 New Street",
    "email": "vendor@example.com",
    "updatedBy": 1,
    "liveStatus": "Y"
  }'

# Deactivate vendor
curl -X DELETE "http://localhost:5181/api/vendors/1?updatedBy=1" \
  -H "Authorization: Bearer <TOKEN>"
```

### GraphQL

**Endpoint**: `POST http://localhost:5181/graphql`

```bash
# Query: Get all vendors
curl -X POST http://localhost:5181/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getVendors { vendorId name address email liveStatus } }"
  }'

# Query: Get vendor by ID
curl -X POST http://localhost:5181/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getVendorById(id: 1) { vendorId name address email liveStatus } }"
  }'

# Mutation: Create vendor
curl -X POST http://localhost:5181/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createVendor(categoryId: 1, locationId: 100, name: \"Office Supplies\", address: \"123 Main St\", updatedBy: 1, email: \"v@test.com\", liveStatus: \"Y\") }"
  }'

# Mutation: Deactivate vendor
curl -X POST http://localhost:5181/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { deactivateVendor(vendorId: 1, updatedBy: 1) }"
  }'
```

---


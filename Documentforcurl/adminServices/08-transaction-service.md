# Admin Services — API Documentation

---

## 8. Transaction Service

**Port**: 5185 · **Base**: `/api/requests`, `/api/orders`, `/api/budgets`

### REST Endpoints

| Method | Endpoint                                                        | Description              |
| ------ | --------------------------------------------------------------- | ------------------------ |
| `GET`  | `/api/requests?locationId=`                                     | Get all requests         |
| `GET`  | `/api/requests/{id}`                                            | Get request by ID        |
| `GET`  | `/api/requests/employee/{empSysId}`                             | Get by employee          |
| `POST` | `/api/requests`                                                 | Submit request           |
| `PUT`  | `/api/requests/{requestSubId}/approve`                          | Approve request          |
| `GET`  | `/api/requests/lookup/stationery-items`                         | Lookup stationery items  |
| `GET`  | `/api/requests/lookup/stationery-items/{itemId}`                | Lookup specific item     |
| `GET`  | `/api/orders?locationId=`                                       | Get all orders           |
| `GET`  | `/api/orders/{id}`                                              | Get order by ID          |
| `GET`  | `/api/orders/vendor/{vendorId}`                                 | Get orders by vendor     |
| `POST` | `/api/orders`                                                   | Create order             |
| `PUT`  | `/api/orders/{orderSubId}/receive`                              | Receive order            |
| `GET`  | `/api/orders/lookup/vendors`                                    | Lookup vendors           |
| `GET`  | `/api/budgets/department?locationId=&deptId=&finYearId=`        | Department budget        |
| `GET`  | `/api/budgets/department/location/{locationId}?finYearId=`      | Budget by location       |
| `POST` | `/api/auth/token`                                               | Get JWT token            |

### cURL Examples

```bash
# Get auth token
curl -X POST http://localhost:5185/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "admin"}'

# Get all requests for a location
curl "http://localhost:5185/api/requests?locationId=100" \
  -H "Authorization: Bearer <TOKEN>"

# Get request by ID
curl http://localhost:5185/api/requests/1 \
  -H "Authorization: Bearer <TOKEN>"

# Submit a request
curl -X POST http://localhost:5185/api/requests \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "empSysId": 1001,
    "locationId": 100,
    "items": [{"itemId": 1, "quantity": 10}]
  }'

# Approve request
curl -X PUT http://localhost:5185/api/requests/1/approve \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"approvedBy": 1, "remarks": "Approved"}'

# Create order
curl -X POST http://localhost:5185/api/orders \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "vendorId": 1,
    "locationId": 100,
    "items": [{"itemId": 1, "quantity": 100, "unitPrice": 50}]
  }'

# Get department budget
curl "http://localhost:5185/api/budgets/department?locationId=100&deptId=1&finYearId=1" \
  -H "Authorization: Bearer <TOKEN>"
```

### GraphQL

**Endpoint**: `POST http://localhost:5185/graphql`

```bash
# Query: Get requests by location
curl -X POST http://localhost:5185/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getRequests(locationId: 100) { id status requestDate employeeId } }"
  }'

# Query: Get request by ID
curl -X POST http://localhost:5185/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getRequestById(requestId: 1) { id status requestDate items { itemId quantity } } }"
  }'

# Query: Get orders by vendor
curl -X POST http://localhost:5185/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getOrdersByVendor(vendorId: 1) { id orderDate status vendorId } }"
  }'

# Mutation: Submit request
curl -X POST http://localhost:5185/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { submitRequest(command: { empSysId: 1001, locationId: 100, items: [{ itemId: 1, quantity: 5 }] }) }"
  }'

# Mutation: Approve request
curl -X POST http://localhost:5185/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { approveRequest(command: { requestSubId: 1, approvedBy: 1, remarks: \"OK\" }) }"
  }'

# Mutation: Create order
curl -X POST http://localhost:5185/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createOrder(command: { vendorId: 1, locationId: 100, items: [{ itemId: 1, quantity: 100 }] }) }"
  }'

# Mutation: Allocate department budget
curl -X POST http://localhost:5185/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { allocateDeptBudget(command: { locationId: 100, deptId: 1, finYearId: 1, amount: 50000 }) }"
  }'
```

---


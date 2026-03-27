# AIMS Services — API Documentation

---

## 9. Visitor Service

**Port**: 5018 · **Auth**: JWT Bearer

### REST Endpoints

| Method | Endpoint                                   | Description                  | Auth    |
| ------ | ------------------------------------------ | ---------------------------- | ------- |
| `GET`  | `/api/visitors/active`                     | Get active (checked-in)      | Bearer  |
| `GET`  | `/api/visitors/{id}`                       | Get visitor by ID            | Bearer  |
| `POST` | `/api/visitors`                            | Register (check-in) visitor  | Bearer  |
| `POST` | `/api/visitors/{id}/checkout`              | Check out visitor            | Bearer  |
| `POST` | `/api/visitors/{id}/items`                 | Add item to visitor          | Bearer  |
| `GET`  | `/api/approvals/pending?approverId=`       | Get pending approvals        | Bearer  |
| `POST` | `/api/approvals/{id}/process`              | Process approval             | Bearer  |

### Minimal API (v2)

| Method | Endpoint                                     | Description               |
| ------ | -------------------------------------------- | ------------------------- |
| `GET`  | `/api/v2/visitors/active`                    | Get active visitors (v2)  |
| `GET`  | `/api/v2/visitors/{id}`                      | Get visitor by ID (v2)    |
| `POST` | `/api/v2/visitors/`                          | Register visitor (v2)     |
| `POST` | `/api/v2/visitors/{id}/checkout`             | Checkout visitor (v2)     |
| `POST` | `/api/v2/visitors/{id}/items`                | Add item (v2)             |
| `GET`  | `/api/v2/approvals/pending?approverId=`      | Get pending (v2)          |
| `POST` | `/api/v2/approvals/{id}/process`             | Process approval (v2)     |

### cURL Examples

```bash
# Get active visitors
curl http://localhost:5018/api/visitors/active \
  -H "Authorization: Bearer <TOKEN>"

# Get visitor by ID
curl http://localhost:5018/api/visitors/1 \
  -H "Authorization: Bearer <TOKEN>"

# Register visitor (check-in)
curl -X POST http://localhost:5018/api/visitors \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "visitorName": "John Doe",
    "idType": "A",
    "idNumber": "ABC123",
    "phoneNumber": "9876543210",
    "email": "john@example.com",
    "company": "Acme Corp",
    "purpose": "Meeting",
    "whomToVisit": 1001,
    "enteredBy": 1
  }'

# Check out visitor
curl -X POST "http://localhost:5018/api/visitors/1/checkout?checkedOutBy=1" \
  -H "Authorization: Bearer <TOKEN>"

# Add item to visitor
curl -X POST http://localhost:5018/api/visitors/1/items \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"visitorId": 1, "itemName": "Laptop", "serialNumber": "SN123", "addedBy": 1}'

# Get pending approvals
curl "http://localhost:5018/api/approvals/pending?approverId=1001" \
  -H "Authorization: Bearer <TOKEN>"

# Approve visitor request
curl -X POST http://localhost:5018/api/approvals/1/process \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"requestId": 1, "isApproved": true, "remarks": "Approved", "processedBy": 1001}'
```

### GraphQL

**Endpoint**: `POST http://localhost:5018/graphql`

```bash
# Query: Get visitor by ID
curl -X POST http://localhost:5018/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getVisitorById(id: 1) { visitorId visitorName idType idNumber phoneNumber email company purpose checkInTime checkOutTime } }"
  }'

# Query: Get active visitors
curl -X POST http://localhost:5018/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getActiveVisitors { visitorId visitorName company purpose checkInTime } }"
  }'

# Query: Get pending approvals
curl -X POST http://localhost:5018/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getPendingApprovals(approverId: 1001) { requestId visitorName purpose status } }"
  }'

# Mutation: Register visitor
curl -X POST http://localhost:5018/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { registerVisitor(input: { visitorName: \"Jane Doe\", idType: \"P\", idNumber: \"XYZ456\", phoneNumber: \"9876543210\", email: \"jane@test.com\", company: \"Tech Co\", purpose: \"Interview\", whomToVisit: 1001, enteredBy: 1 }) { visitorId visitorName checkInTime } }"
  }'

# Mutation: Checkout visitor
curl -X POST http://localhost:5018/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { checkoutVisitor(visitorId: 1, checkedOutBy: 1) }"
  }'

# Mutation: Process approval
curl -X POST http://localhost:5018/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { processApproval(requestId: 1, isApproved: true, remarks: \"OK\", processedBy: 1001) { requestId status } }"
  }'
```

---


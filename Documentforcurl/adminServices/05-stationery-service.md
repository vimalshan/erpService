# Admin Services — API Documentation

---

## 5. Stationery Service

**Port**: 5273 · **GraphQL Primary**

### GraphQL

**Endpoint**: `POST http://localhost:5273/graphql`

```bash
# Query: Get all stationery items (with pagination)
curl -X POST http://localhost:5273/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getStationaryItems(first: 20) { nodes { id itemName itemDescription reorderLevel currentStock } totalCount } }"
  }'

# Query: Get specific stationery item
curl -X POST http://localhost:5273/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getStationaryItem(id: 1) { id itemName itemDescription reorderLevel currentStock } }"
  }'

# Query: Get all requests
curl -X POST http://localhost:5273/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getRequests(first: 20) { nodes { id status requestDate } totalCount } }"
  }'

# Query: Get specific request
curl -X POST http://localhost:5273/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getRequest(id: 1) { id status requestDate items { itemName quantity } } }"
  }'

# Query: Get order by ID
curl -X POST http://localhost:5273/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getOrder(id: 1) { id orderDate status vendorName } }"
  }'

# Query: Get reorder alerts
curl -X POST http://localhost:5273/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getReorderAlerts { itemName currentStock reorderLevel } }"
  }'

# Mutation: Create request
curl -X POST http://localhost:5273/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createRequest(input: { employeeId: 1001, items: [{ itemId: 1, quantity: 5 }] }) }"
  }'

# Mutation: Approve request
curl -X POST http://localhost:5273/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { approveRequest(input: { requestId: 1, approvedBy: 1, remarks: \"Approved\" }) }"
  }'

# Mutation: Create order
curl -X POST http://localhost:5273/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createOrder(input: { vendorId: 1, items: [{ itemId: 1, quantity: 100 }] }) }"
  }'

# Mutation: Receive order
curl -X POST http://localhost:5273/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { receiveOrder(input: { orderId: 1, receivedBy: 1, items: [{ itemId: 1, receivedQty: 100 }] }) }"
  }'
```

---


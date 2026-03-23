# Stationery Service

> **Port:** 5182 | **Swagger:** http://localhost:5182/swagger | **GraphQL:** http://localhost:5182/graphql

---

## REST Endpoints

### Get Health
```bash
curl -X GET http://localhost:5182/health
```

---

## GraphQL

### Query: Get Stationery Items (Paginated)
```bash
curl -X POST http://localhost:5182/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ stationaryItems(first: 10) { nodes { id description catId locId uomId make pricePerUnit reorderLevel openingStock closed } totalCount } }"
  }'
```
**Response:**
```json
{
  "data": {
    "stationaryItems": {
      "nodes": [
        {
          "id": 1,
          "description": "A4 Paper Ream",
          "catId": 1,
          "locId": 101,
          "uomId": 1,
          "make": "JK Copier",
          "pricePerUnit": 350,
          "reorderLevel": 50,
          "openingStock": 200,
          "closed": "N"
        }
      ],
      "totalCount": 1
    }
  }
}
```

### Query: Get Single Stationery Item
```bash
curl -X POST http://localhost:5182/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ stationaryItem(id: 1) { id description catId make pricePerUnit openingStock } }"
  }'
```

### Query: Get Requests
```bash
curl -X POST http://localhost:5182/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ requests { id requestedBy requestedOn locationId details { id stationaryId deptId requestedQty status expectedDate } } }"
  }'
```
**Response:**
```json
{
  "data": {
    "requests": [
      {
        "id": 1,
        "requestedBy": 5001,
        "requestedOn": "2026-03-20T10:00:00",
        "locationId": 101,
        "details": [
          {
            "id": 1,
            "stationaryId": 1,
            "deptId": 10,
            "requestedQty": 20,
            "status": "Pending",
            "expectedDate": "2026-04-01T00:00:00"
          }
        ]
      }
    ]
  }
}
```

### Mutation: Create Stationery Request
```bash
curl -X POST http://localhost:5182/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createRequest(input: { requestedBy: 5001, locationId: 101, unitCode: \"HQ\", details: [{ stationaryId: 1, deptId: 10, expectedDate: \"2026-04-15\", requestedQty: 50 }] }) { id requestedBy requestedOn details { stationaryId requestedQty status } } }"
  }'
```
**Response:**
```json
{
  "data": {
    "createRequest": {
      "id": 2,
      "requestedBy": 5001,
      "requestedOn": "2026-03-23T10:00:00",
      "details": [
        {
          "stationaryId": 1,
          "requestedQty": 50,
          "status": "Pending"
        }
      ]
    }
  }
}
```

### Mutation: Approve Request
```bash
curl -X POST http://localhost:5182/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { approveRequest(requestSubId: 1, approvedQty: 20, approverSysId: 9001, remarks: \"Approved\") { id status approvedQty approverRemarks } }"
  }'
```

### Mutation: Create Order
```bash
curl -X POST http://localhost:5182/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createOrder(input: { vendorId: 1, locationId: 101, items: [{ stationaryId: 1, orderedQty: 100, unitPrice: 350 }] }) { id vendorId orderDate totalAmount } }"
  }'
```

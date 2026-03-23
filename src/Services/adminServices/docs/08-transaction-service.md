# Transaction Service

> **Port:** 5185 | **Swagger:** http://localhost:5185/swagger | **GraphQL:** http://localhost:5185/graphql

---

## REST Endpoints

### Get All Requests
```bash
curl -X GET "http://localhost:5185/api/Requests?locationId=101"
```
**Response:**
```json
[
  {
    "requestId": 1,
    "requestedBy": 5001,
    "requestedOn": "2026-03-20T10:00:00",
    "locationId": 101,
    "unitCode": "HQ",
    "details": [
      {
        "requestSubId": 1,
        "requestId": 1,
        "stationaryId": 1,
        "deptId": 10,
        "expectedDate": "2026-04-01T00:00:00",
        "requestedQty": 50,
        "indentedQty": null,
        "approvedQty": null,
        "status": "Pending",
        "updatedBy": 5001,
        "updatedOn": "2026-03-20T10:00:00"
      }
    ]
  }
]
```

### Get Request by ID
```bash
curl -X GET http://localhost:5185/api/Requests/1
```

### Get Requests by Employee
```bash
curl -X GET http://localhost:5185/api/Requests/employee/5001
```

### Submit Request
```bash
curl -X POST http://localhost:5185/api/Requests \
  -H "Content-Type: application/json" \
  -d '{
    "requestedBy": 5002,
    "locationId": 102,
    "unitCode": "BR1",
    "details": [
      {
        "stationaryId": 1,
        "deptId": 15,
        "expectedDate": "2026-04-15T00:00:00",
        "requestedQty": 100,
        "updatedBy": 5002
      },
      {
        "stationaryId": 3,
        "deptId": 15,
        "expectedDate": "2026-04-15T00:00:00",
        "requestedQty": 25,
        "updatedBy": 5002
      }
    ]
  }'
```
**Response:**
```json
{
  "requestId": 2,
  "requestedBy": 5002,
  "requestedOn": "2026-03-23T10:00:00",
  "locationId": 102,
  "unitCode": "BR1",
  "details": [
    { "requestSubId": 2, "stationaryId": 1, "requestedQty": 100, "status": "Pending" },
    { "requestSubId": 3, "stationaryId": 3, "requestedQty": 25, "status": "Pending" }
  ]
}
```

### Approve Request
```bash
curl -X PUT http://localhost:5185/api/Requests/2/approve \
  -H "Content-Type: application/json" \
  -d '{
    "approvedQty": 80,
    "approverSysId": 9001,
    "approverRemarks": "Reduced quantity to match budget"
  }'
```

### Lookup: Stationery Items
```bash
curl -X GET "http://localhost:5185/api/Requests/lookup/stationery-items?locationId=101"
```

### Lookup: Location Data
```bash
curl -X GET http://localhost:5185/api/Requests/lookup/location-data
```

### Lookup: Financial Years
```bash
curl -X GET http://localhost:5185/api/Requests/lookup/financial-years
```

---

## GraphQL

### Query: Get Requests
```bash
curl -X POST http://localhost:5185/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ requests { requestId requestedBy requestedOn locationId details { requestSubId stationaryId requestedQty approvedQty status } } }"
  }'
```
**Response:**
```json
{
  "data": {
    "requests": [
      {
        "requestId": 1,
        "requestedBy": 5001,
        "requestedOn": "2026-03-20T10:00:00",
        "locationId": 101,
        "details": [
          {
            "requestSubId": 1,
            "stationaryId": 1,
            "requestedQty": 50,
            "approvedQty": null,
            "status": "Pending"
          }
        ]
      }
    ]
  }
}
```

### Query: Get Orders
```bash
curl -X POST http://localhost:5185/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ orders { id vendorId orderDate totalAmount status items { stationaryId orderedQty unitPrice } } }"
  }'
```

### Query: Get Department Budget
```bash
curl -X POST http://localhost:5185/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ deptBudgetsByLocation(locationId: 101) { deptId locationId allocatedAmount usedAmount remainingAmount financialYearId } }"
  }'
```

### Mutation: Submit Request
```bash
curl -X POST http://localhost:5185/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { submitRequest(input: { requestedBy: 5003, locationId: 101, unitCode: \"HQ\", details: [{ stationaryId: 2, deptId: 10, expectedDate: \"2026-05-01\", requestedQty: 30, updatedBy: 5003 }] }) { requestId requestedBy details { stationaryId requestedQty status } } }"
  }'
```
**Response:**
```json
{
  "data": {
    "submitRequest": {
      "requestId": 3,
      "requestedBy": 5003,
      "details": [
        {
          "stationaryId": 2,
          "requestedQty": 30,
          "status": "Pending"
        }
      ]
    }
  }
}
```

### Mutation: Allocate Budget
```bash
curl -X POST http://localhost:5185/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { allocateDeptBudget(input: { deptId: 10, locationId: 101, financialYearId: 1, allocatedAmount: 500000 }) { deptId allocatedAmount } }"
  }'
```

# Vendor Service

> **Port:** 5181 | **Swagger:** http://localhost:5181/swagger | **GraphQL:** http://localhost:5181/graphql

---

## REST Endpoints

### Get All Vendors
```bash
curl -X GET "http://localhost:5181/api/Vendors?status=Active"
```
**Response:**
```json
[
  {
    "id": 1,
    "categoryId": 10,
    "locationId": 101,
    "name": "National Stationery Co.",
    "email": "info@natstat.com",
    "address": "123 Market Street, Delhi",
    "updatedBy": 1001,
    "updatedOn": "2025-05-20T10:00:00",
    "liveStatus": "Active"
  }
]
```

### Get Vendor by ID
```bash
curl -X GET http://localhost:5181/api/Vendors/1
```

### Create Vendor
```bash
curl -X POST http://localhost:5181/api/Vendors \
  -H "Content-Type: application/json" \
  -d '{
    "categoryId": 10,
    "locationId": 102,
    "name": "Office Supplies Ltd",
    "email": "contact@officesupplies.com",
    "address": "456 Business Park, Mumbai",
    "updatedBy": 1001
  }'
```
**Response:**
```json
{
  "id": 2,
  "categoryId": 10,
  "locationId": 102,
  "name": "Office Supplies Ltd",
  "email": "contact@officesupplies.com",
  "address": "456 Business Park, Mumbai",
  "updatedBy": 1001,
  "updatedOn": "2026-03-23T10:00:00",
  "liveStatus": "Active"
}
```

### Update Vendor
```bash
curl -X PUT http://localhost:5181/api/Vendors/2 \
  -H "Content-Type: application/json" \
  -d '{
    "categoryId": 10,
    "locationId": 102,
    "name": "Office Supplies Pvt Ltd",
    "email": "info@officesupplies.com",
    "address": "456 Business Park, Mumbai (Updated)",
    "updatedBy": 1001
  }'
```

### Deactivate Vendor
```bash
curl -X DELETE http://localhost:5181/api/Vendors/2
```

---

## GraphQL

### Query: Get All Vendors
```bash
curl -X POST http://localhost:5181/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ vendors(status: \"Active\") { id name categoryId locationId email address liveStatus } }"
  }'
```
**Response:**
```json
{
  "data": {
    "vendors": [
      {
        "id": 1,
        "name": "National Stationery Co.",
        "categoryId": 10,
        "locationId": 101,
        "email": "info@natstat.com",
        "address": "123 Market Street, Delhi",
        "liveStatus": "Active"
      }
    ]
  }
}
```

### Query: Get Vendor by ID
```bash
curl -X POST http://localhost:5181/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ vendorById(id: 1) { id name email address categoryId locationId liveStatus } }"
  }'
```

### Mutation: Create Vendor
```bash
curl -X POST http://localhost:5181/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createVendor(input: { categoryId: 10, locationId: 103, name: \"Tech Store\", email: \"tech@store.com\", address: \"789 IT Park, Bangalore\", updatedBy: 1001 }) { id name liveStatus } }"
  }'
```
**Response:**
```json
{
  "data": {
    "createVendor": {
      "id": 3,
      "name": "Tech Store",
      "liveStatus": "Active"
    }
  }
}
```

### Mutation: Deactivate Vendor
```bash
curl -X POST http://localhost:5181/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { deactivateVendor(id: 3) { id name liveStatus } }"
  }'
```

# Finyear API

> **Port:** 5186 | **Swagger:** http://localhost:5186/swagger | **GraphQL:** http://localhost:5186/graphql

---

## REST Endpoints

### Get All Financial Years
```bash
curl -X GET http://localhost:5186/api/FinancialYear
```
**Response:**
```json
[
  {
    "financialYearId": 1,
    "financialYearName": "2025-2026",
    "startDate": "2025-04-01T00:00:00",
    "closeDate": "2026-03-31T00:00:00",
    "updatedBy": 1001,
    "updatedOn": "2025-04-01T10:00:00"
  }
]
```

### Get Financial Year by ID
```bash
curl -X GET http://localhost:5186/api/FinancialYear/1
```

### Get Current Active Financial Year
```bash
curl -X GET http://localhost:5186/api/FinancialYear/current
```

### Get Financial Year by Name
```bash
curl -X GET http://localhost:5186/api/FinancialYear/by-name/2025-2026
```

### Create Financial Year
```bash
curl -X POST http://localhost:5186/api/FinancialYear \
  -H "Content-Type: application/json" \
  -d '{
    "financialYearId": 2,
    "financialYearName": "2026-2027",
    "startDate": "2026-04-01T00:00:00",
    "closeDate": "2027-03-31T00:00:00",
    "updatedBy": 1001
  }'
```
**Response:**
```json
{
  "financialYearId": 2,
  "financialYearName": "2026-2027",
  "startDate": "2026-04-01T00:00:00",
  "closeDate": "2027-03-31T00:00:00",
  "updatedBy": 1001,
  "updatedOn": "2026-03-23T10:00:00"
}
```

### Update Financial Year
```bash
curl -X PUT http://localhost:5186/api/FinancialYear/2 \
  -H "Content-Type: application/json" \
  -d '{
    "financialYearName": "2026-2027 (Updated)",
    "startDate": "2026-04-01T00:00:00",
    "closeDate": "2027-03-31T00:00:00",
    "updatedBy": 1001
  }'
```

### Delete Financial Year
```bash
curl -X DELETE http://localhost:5186/api/FinancialYear/2
```

---

## GraphQL

### Query: Get All Financial Years
```bash
curl -X POST http://localhost:5186/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ allFinancialYears { financialYearId financialYearName startDate closeDate updatedBy updatedOn } }"
  }'
```
**Response:**
```json
{
  "data": {
    "allFinancialYears": [
      {
        "financialYearId": 1,
        "financialYearName": "2025-2026",
        "startDate": "2025-04-01T00:00:00",
        "closeDate": "2026-03-31T00:00:00",
        "updatedBy": 1001,
        "updatedOn": "2025-04-01T10:00:00"
      }
    ]
  }
}
```

### Query: Get Financial Year by ID
```bash
curl -X POST http://localhost:5186/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ financialYearById(id: 1) { financialYearId financialYearName startDate closeDate } }"
  }'
```

### Query: Get Current Financial Year
```bash
curl -X POST http://localhost:5186/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ currentFinancialYear { financialYearId financialYearName startDate closeDate } }"
  }'
```

### Mutation: Create Financial Year
```bash
curl -X POST http://localhost:5186/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createFinancialYear(input: { financialYearId: 3, financialYearName: \"2027-2028\", startDate: \"2027-04-01\", closeDate: \"2028-03-31\", updatedBy: 1001 }) { financialYearId financialYearName startDate closeDate } }"
  }'
```
**Response:**
```json
{
  "data": {
    "createFinancialYear": {
      "financialYearId": 3,
      "financialYearName": "2027-2028",
      "startDate": "2027-04-01T00:00:00",
      "closeDate": "2028-03-31T00:00:00"
    }
  }
}
```

### Mutation: Delete Financial Year
```bash
curl -X POST http://localhost:5186/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { deleteFinancialYear(id: 3) }"
  }'
```

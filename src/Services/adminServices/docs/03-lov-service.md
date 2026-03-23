# LOV Service

> **Port:** 5184 | **Swagger:** http://localhost:5184/swagger | **GraphQL:** http://localhost:5184/graphql

---

## REST Endpoints

### LovType — Get All
```bash
curl -X GET http://localhost:5184/api/v1/LovType
```
**Response:**
```json
[
  {
    "lovTypeId": 1,
    "lovTypeName": "Department"
  },
  {
    "lovTypeId": 2,
    "lovTypeName": "Designation"
  }
]
```

### LovType — Get by ID
```bash
curl -X GET http://localhost:5184/api/v1/LovType/1
```

### LovType — Create
```bash
curl -X POST http://localhost:5184/api/v1/LovType \
  -H "Content-Type: application/json" \
  -d '{
    "lovTypeId": 3,
    "lovTypeName": "Category"
  }'
```

### LovType — Update
```bash
curl -X PUT http://localhost:5184/api/v1/LovType/3 \
  -H "Content-Type: application/json" \
  -d '{
    "lovTypeName": "Category (Updated)"
  }'
```

### LovType — Delete
```bash
curl -X DELETE http://localhost:5184/api/v1/LovType/3
```

### LovMaster — Get All (with optional filter)
```bash
curl -X GET "http://localhost:5184/api/v1/LovMaster?lovTypeId=1"
```
**Response:**
```json
[
  {
    "lovId": 100,
    "lovTypeId": 1,
    "lovName": "Finance",
    "lovUpdatedBy": 1001,
    "lovUpdatedOn": "2025-06-15T10:00:00"
  }
]
```

### LovMaster — Get by ID
```bash
curl -X GET http://localhost:5184/api/v1/LovMaster/100
```

### LovMaster — Create
```bash
curl -X POST http://localhost:5184/api/v1/LovMaster \
  -H "Content-Type: application/json" \
  -d '{
    "lovId": 101,
    "lovTypeId": 1,
    "lovName": "Human Resources",
    "updatedBy": 1001
  }'
```

### LovMaster — Update
```bash
curl -X PUT http://localhost:5184/api/v1/LovMaster/101 \
  -H "Content-Type: application/json" \
  -d '{
    "lovName": "HR Department",
    "updatedBy": 1001
  }'
```

### LovMaster — Delete
```bash
curl -X DELETE http://localhost:5184/api/v1/LovMaster/101
```

### ProgramLov — Get All
```bash
curl -X GET "http://localhost:5184/api/v1/ProgramLov?prlovTypeCode=DEPT"
```
**Response:**
```json
[
  {
    "prlovTypeCode": "DEPT",
    "prlovCode": "FIN",
    "prlovName": "Finance Department"
  }
]
```

### ProgramLov — Get by TypeCode and Code
```bash
curl -X GET http://localhost:5184/api/v1/ProgramLov/DEPT/FIN
```

### ProgramLov — Create
```bash
curl -X POST http://localhost:5184/api/v1/ProgramLov \
  -H "Content-Type: application/json" \
  -d '{
    "prlovTypeCode": "DEPT",
    "prlovCode": "HR",
    "prlovName": "Human Resources"
  }'
```

### ProgramLov — Update
```bash
curl -X PUT http://localhost:5184/api/v1/ProgramLov/DEPT/HR \
  -H "Content-Type: application/json" \
  -d '{
    "prlovTypeCode": "DEPT",
    "prlovCode": "HR",
    "prlovName": "HR & Admin"
  }'
```

### ProgramLov — Delete
```bash
curl -X DELETE http://localhost:5184/api/v1/ProgramLov/DEPT/HR
```

---

## GraphQL

### Query: Get LOV Types
```bash
curl -X POST http://localhost:5184/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ lovTypes { lovTypeId lovTypeName } }"
  }'
```
**Response:**
```json
{
  "data": {
    "lovTypes": [
      { "lovTypeId": 1, "lovTypeName": "Department" },
      { "lovTypeId": 2, "lovTypeName": "Designation" }
    ]
  }
}
```

### Query: Get LOV Masters by Type
```bash
curl -X POST http://localhost:5184/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ lovMastersByType(lovTypeId: 1) { lovId lovTypeId lovName lovUpdatedBy lovUpdatedOn } }"
  }'
```
**Response:**
```json
{
  "data": {
    "lovMastersByType": [
      {
        "lovId": 100,
        "lovTypeId": 1,
        "lovName": "Finance",
        "lovUpdatedBy": 1001,
        "lovUpdatedOn": "2025-06-15T10:00:00"
      }
    ]
  }
}
```

### Mutation: Create LOV Type
```bash
curl -X POST http://localhost:5184/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createLovType(input: { lovTypeId: 5, lovTypeName: \"Grade\" }) { lovTypeId lovTypeName } }"
  }'
```
**Response:**
```json
{
  "data": {
    "createLovType": {
      "lovTypeId": 5,
      "lovTypeName": "Grade"
    }
  }
}
```

### Mutation: Create LOV Master
```bash
curl -X POST http://localhost:5184/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createLovMaster(input: { lovId: 200, lovTypeId: 5, lovName: \"Grade A\", updatedBy: 1001 }) { lovId lovName } }"
  }'
```

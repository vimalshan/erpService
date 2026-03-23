# ERP Microservices — API & cURL Documentation

> Complete cURL commands for all 10 services with REST endpoints and GraphQL (request & response).

---

## Service Overview

| # | Service | Port | Swagger | GraphQL |
|---|---------|------|---------|---------|
| 1 | **Finyear API** | 5186 | http://localhost:5186/swagger | http://localhost:5186/graphql |
| 2 | **Location Services** | 7136 | http://localhost:7136/swagger | http://localhost:7136/graphql |
| 3 | **LOV Service** | 5184 | http://localhost:5184/swagger | http://localhost:5184/graphql |
| 4 | **Scholarship Service** | 5166 | http://localhost:5166/swagger | http://localhost:5166/graphql |
| 5 | **Stationery Service** | 5182 | http://localhost:5182/swagger | http://localhost:5182/graphql |
| 6 | **TDS Service** | 5183 | http://localhost:5183/swagger | http://localhost:5183/graphql |
| 7 | **Vendor Service** | 5181 | http://localhost:5181/swagger | http://localhost:5181/graphql |
| 8 | **Transaction Service** | 5185 | http://localhost:5185/swagger | http://localhost:5185/graphql |
| 9 | **API Gateway** | 5000 | http://localhost:5000/swagger | — |
| 10 | **Auth Provider** | 7136 | http://localhost:7136/swagger | http://localhost:7136/graphql |

---

## Docker Start Commands

```bash
# Start all services together
cd src/Services/adminServices
docker compose -f docker-compose.shared.yml up -d

# Start with pre-built images
docker compose -f docker-compose.prod.yml up -d
```

---

# 1. Finyear API (port 5186)

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

---

# 2. Location Services (port 7136)

## REST Endpoints

### Get All Location App Maps
```bash
curl -X GET http://localhost:7136/api/v1/location-app-maps
```
**Response:**
```json
[
  {
    "locationId": 101,
    "appName": "ERP",
    "siteCategoryCode": 1,
    "selfAccess": "Y",
    "deemedApproval": "N",
    "isActive": true,
    "createdDate": "2025-01-15T10:00:00",
    "createdBy": "admin",
    "modifiedDate": null,
    "modifiedBy": null
  }
]
```

### Get Active Location App Maps
```bash
curl -X GET http://localhost:7136/api/v1/location-app-maps/active
```

### Get by Location ID
```bash
curl -X GET http://localhost:7136/api/v1/location-app-maps/by-location/101
```

### Get Single Mapping
```bash
curl -X GET http://localhost:7136/api/v1/location-app-maps/101/ERP
```

### Get Count
```bash
curl -X GET http://localhost:7136/api/v1/location-app-maps/count
```
**Response:**
```json
{ "count": 42 }
```

### Create Location App Map
```bash
curl -X POST http://localhost:7136/api/v1/location-app-maps \
  -H "Content-Type: application/json" \
  -d '{
    "locationId": 102,
    "appName": "HRM",
    "siteCategoryCode": 2,
    "selfAccess": "Y",
    "deemedApproval": "N"
  }'
```
**Response:**
```json
{
  "locationId": 102,
  "appName": "HRM",
  "siteCategoryCode": 2,
  "selfAccess": "Y",
  "deemedApproval": "N",
  "isActive": true,
  "createdDate": "2026-03-23T10:00:00",
  "createdBy": "system"
}
```

### Update Location App Map
```bash
curl -X PUT http://localhost:7136/api/v1/location-app-maps/102/HRM \
  -H "Content-Type: application/json" \
  -d '{
    "siteCategoryCode": 3,
    "selfAccess": "N",
    "deemedApproval": "Y",
    "isActive": true
  }'
```

### Delete Location App Map
```bash
curl -X DELETE http://localhost:7136/api/v1/location-app-maps/102/HRM
```

### Get All (Paginated v2)
```bash
curl -X GET "http://localhost:7136/api/v2/location-app-maps?page=1&pageSize=10"
```

## GraphQL

### Query: Get All Location Maps
```bash
curl -X POST http://localhost:7136/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ locationAppMaps { locationId appName siteCategoryCode selfAccess deemedApproval isActive createdDate } }"
  }'
```
**Response:**
```json
{
  "data": {
    "locationAppMaps": [
      {
        "locationId": 101,
        "appName": "ERP",
        "siteCategoryCode": 1,
        "selfAccess": "Y",
        "deemedApproval": "N",
        "isActive": true,
        "createdDate": "2025-01-15T10:00:00"
      }
    ]
  }
}
```

### Query: Get Active Maps
```bash
curl -X POST http://localhost:7136/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ activeLocationAppMaps { locationId appName isActive } }"
  }'
```

### Query: Get by Location
```bash
curl -X POST http://localhost:7136/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ locationAppMapsByLocation(locationId: 101) { locationId appName siteCategoryCode isActive } }"
  }'
```

### Mutation: Create Location App Map
```bash
curl -X POST http://localhost:7136/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createLocationAppMap(input: { locationId: 103, appName: \"PAY\", siteCategoryCode: 1, selfAccess: \"Y\", deemedApproval: \"N\" }) { locationId appName isActive } }"
  }'
```
**Response:**
```json
{
  "data": {
    "createLocationAppMap": {
      "locationId": 103,
      "appName": "PAY",
      "isActive": true
    }
  }
}
```

### Mutation: Delete Location App Map
```bash
curl -X POST http://localhost:7136/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { deleteLocationAppMap(locationId: 103, appName: \"PAY\") }"
  }'
```

---

# 3. LOV Service (port 5184)

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

---

# 4. Scholarship Service (port 5166)

## REST Endpoints

### Get All Scholarships (Paginated)
```bash
curl -X GET "http://localhost:5166/api/Scholarships?page=1&pageSize=20"
```
**Response:**
```json
{
  "items": [
    {
      "id": 1,
      "employeeSysId": 5001,
      "gradeId": 2,
      "dependentId": 1,
      "childName": "Ravi Kumar",
      "lastSchool": "DPS",
      "lastYearOfSchool": 2025,
      "lastExam": "10th",
      "cgpaFlag": "P",
      "marksPercentage": 85.5,
      "marksGpa": 0,
      "courseName": "B.Tech",
      "courseJoinYear": 2025,
      "courseJoinMonth": 7,
      "courseDuration": 4,
      "paymentMode": "NEFT",
      "entryStatus": "Approved",
      "source": "Online",
      "disbursementAmount": 50000,
      "disbursementFrequency": "Annual",
      "liveStatus": "Active",
      "createdOn": "2025-08-01T10:00:00",
      "createdBy": 5001
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 20
}
```

### Get Scholarship by ID
```bash
curl -X GET http://localhost:5166/api/Scholarships/1
```

### Get Scholarships by Employee
```bash
curl -X GET "http://localhost:5166/api/Scholarships/employee/5001?page=1&pageSize=10"
```

### Submit Scholarship Application
```bash
curl -X POST http://localhost:5166/api/Scholarships \
  -H "Content-Type: application/json" \
  -d '{
    "employeeSysId": 5002,
    "gradeId": 3,
    "dependentId": 2,
    "childName": "Priya Sharma",
    "lastSchool": "KV",
    "lastYearOfSchool": 2025,
    "lastExam": "12th",
    "cgpaFlag": "P",
    "marksPercentage": 92.0,
    "marksGpa": 0,
    "courseName": "MBBS",
    "courseJoinYear": 2025,
    "courseJoinMonth": 9,
    "courseDuration": 5,
    "paymentMode": "NEFT",
    "childAccountNumber": "1234567890",
    "childBankIfsc": "SBIN0001234",
    "childBankMicr": "110002345",
    "source": "Online",
    "disbursementAmount": 75000,
    "disbursementFrequency": "Annual",
    "isOffline": "N"
  }'
```
**Response:**
```json
{
  "id": 2,
  "employeeSysId": 5002,
  "childName": "Priya Sharma",
  "entryStatus": "Pending",
  "liveStatus": "Active",
  "createdOn": "2026-03-23T10:00:00",
  "createdBy": 5002
}
```

### Approve Scholarship
```bash
curl -X PUT http://localhost:5166/api/Scholarships/2/approve \
  -H "Content-Type: application/json" \
  -d '{
    "remarks": "Approved by committee",
    "approvedBy": 9001
  }'
```

### Stop Scholarship
```bash
curl -X PUT http://localhost:5166/api/Scholarships/2/stop \
  -H "Content-Type: application/json" \
  -d '{
    "remarks": "Course completed",
    "stoppedBy": 9001
  }'
```

### Get Scholarship Amount Configurations
```bash
curl -X GET http://localhost:5166/api/ScholarshipAmounts
```
**Response:**
```json
[
  {
    "id": 1,
    "orgId": 1,
    "gradeCategory": "Officer",
    "eligibleExam": "12th",
    "applicableAllGrade": "Y",
    "gradeId": 0,
    "fromYear": 2020,
    "closeYear": null,
    "eligibleAmount": 75000,
    "eligibleYear": 5,
    "cutoffMarks": 60,
    "createdOn": "2020-01-01T00:00:00",
    "createdBy": 1
  }
]
```

## GraphQL

### Query: Get Scholarships
```bash
curl -X POST http://localhost:5166/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ scholarships { id employeeSysId childName courseName entryStatus liveStatus disbursementAmount } }"
  }'
```
**Response:**
```json
{
  "data": {
    "scholarships": [
      {
        "id": 1,
        "employeeSysId": 5001,
        "childName": "Ravi Kumar",
        "courseName": "B.Tech",
        "entryStatus": "Approved",
        "liveStatus": "Active",
        "disbursementAmount": 50000
      }
    ]
  }
}
```

### Mutation: Create Scholarship
```bash
curl -X POST http://localhost:5166/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createScholarship(input: { employeeSysId: 5003, gradeId: 1, dependentId: 1, childName: \"Amit Singh\", lastSchool: \"DAV\", lastYearOfSchool: 2025, lastExam: \"10th\", cgpaFlag: \"P\", marksPercentage: 88.0, courseName: \"B.Sc\", courseJoinYear: 2025, courseJoinMonth: 7, courseDuration: 3, source: \"Online\", disbursementAmount: 50000, disbursementFrequency: \"Annual\", isOffline: \"N\" }) { id childName entryStatus } }"
  }'
```
**Response:**
```json
{
  "data": {
    "createScholarship": {
      "id": 3,
      "childName": "Amit Singh",
      "entryStatus": "Pending"
    }
  }
}
```

### Mutation: Approve Scholarship
```bash
curl -X POST http://localhost:5166/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { approveScholarship(id: 3, remarks: \"Verified and approved\") { id entryStatus } }"
  }'
```

---

# 5. Stationery Service (port 5182)

## REST Endpoints

### Get Health
```bash
curl -X GET http://localhost:5182/health
```

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

---

# 6. TDS Service (port 5183)

## REST Endpoints

### Get All Vendors (Paginated)
```bash
curl -X GET "http://localhost:5183/api/Vendors?page=1&pageSize=20"
```
**Response:**
```json
{
  "items": [
    {
      "vendorId": 1,
      "vendorName": "ABC Enterprises",
      "emailAddress": "abc@example.com",
      "panNo": "ABCDE1234F"
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 20
}
```

### Get Vendor by PAN
```bash
curl -X GET http://localhost:5183/api/Vendors/ABCDE1234F
```

### Create Vendor
```bash
curl -X POST http://localhost:5183/api/Vendors \
  -H "Content-Type: application/json" \
  -d '{
    "vendorName": "XYZ Corp",
    "emailAddress": "xyz@example.com",
    "panNo": "XYZAB5678G"
  }'
```
**Response:**
```json
{
  "vendorId": 2,
  "vendorName": "XYZ Corp",
  "emailAddress": "xyz@example.com",
  "panNo": "XYZAB5678G"
}
```

### Update Vendor
```bash
curl -X PUT http://localhost:5183/api/Vendors/2 \
  -H "Content-Type: application/json" \
  -d '{
    "vendorName": "XYZ Corporation",
    "emailAddress": "contact@xyz.com",
    "panNo": "XYZAB5678G"
  }'
```

### Delete Vendor
```bash
curl -X DELETE http://localhost:5183/api/Vendors/2
```

### Get All TDS Files (Paginated)
```bash
curl -X GET "http://localhost:5183/api/Files?page=1&pageSize=20"
```
**Response:**
```json
{
  "items": [
    {
      "fileId": 1,
      "fileName": "TDS_Q1_2025.pdf",
      "panNo": "ABCDE1234F",
      "emailStatus": "Pending",
      "fileType": "pdf",
      "blobStorageUri": "https://storage.blob.core.windows.net/tds/TDS_Q1_2025.pdf",
      "createdAt": "2025-06-30T10:00:00",
      "updatedAt": null
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 20
}
```

### Get File by ID
```bash
curl -X GET http://localhost:5183/api/Files/1
```

### Upload TDS File
```bash
curl -X POST http://localhost:5183/api/Files \
  -F "file=@/path/to/TDS_Q2_2025.pdf" \
  -F "panNo=ABCDE1234F"
```

### Mark Email Sent
```bash
curl -X PATCH http://localhost:5183/api/Files/1/email-sent
```

### Update Email Status
```bash
curl -X PUT http://localhost:5183/api/Files/1/email-status \
  -H "Content-Type: application/json" \
  -d '{
    "emailStatus": "Sent"
  }'
```

## GraphQL

### Query: Get TDS Vendors
```bash
curl -X POST http://localhost:5183/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ vendors(first: 10) { nodes { vendorId vendorName emailAddress panNo } totalCount } }"
  }'
```
**Response:**
```json
{
  "data": {
    "vendors": {
      "nodes": [
        {
          "vendorId": 1,
          "vendorName": "ABC Enterprises",
          "emailAddress": "abc@example.com",
          "panNo": "ABCDE1234F"
        }
      ],
      "totalCount": 1
    }
  }
}
```

### Query: Get Vendor by PAN
```bash
curl -X POST http://localhost:5183/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ vendorByPan(panNo: \"ABCDE1234F\") { vendorId vendorName emailAddress panNo } }"
  }'
```

### Query: Get Files
```bash
curl -X POST http://localhost:5183/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ files(first: 10) { nodes { fileId fileName panNo emailStatus fileType createdAt } totalCount } }"
  }'
```

### Mutation: Create TDS Vendor
```bash
curl -X POST http://localhost:5183/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createVendor(input: { vendorName: \"PQR Ltd\", emailAddress: \"pqr@example.com\", panNo: \"PQRST9876H\" }) { vendorId vendorName panNo } }"
  }'
```
**Response:**
```json
{
  "data": {
    "createVendor": {
      "vendorId": 3,
      "vendorName": "PQR Ltd",
      "panNo": "PQRST9876H"
    }
  }
}
```

### Mutation: Mark Email Sent
```bash
curl -X POST http://localhost:5183/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { markEmailSent(fileId: 1) { fileId emailStatus } }"
  }'
```

---

# 7. Vendor Service (port 5181)

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

---

# 8. Transaction Service (port 5185)

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

---

# 9. API Gateway (port 5000)

## REST Endpoints

### Generate Auth Token
```bash
curl -X POST http://localhost:5000/api/gateway/auth/token \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "admin123",
    "role": "Admin"
  }'
```
**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2026-03-24T10:00:00Z"
}
```

### Access Services Through Gateway (with token)

#### Finyear via Gateway
```bash
curl -X GET http://localhost:5000/finyear/v1/FinancialYear \
  -H "Authorization: Bearer <TOKEN>"
```

#### Location via Gateway
```bash
curl -X GET http://localhost:5000/location/v1/location-app-maps \
  -H "Authorization: Bearer <TOKEN>"
```

#### LOV via Gateway
```bash
curl -X GET http://localhost:5000/lov/v1/LovType \
  -H "Authorization: Bearer <TOKEN>"
```

#### Vendor via Gateway
```bash
curl -X GET http://localhost:5000/vendor/v1/vendors \
  -H "Authorization: Bearer <TOKEN>"
```

#### Scholarship via Gateway
```bash
curl -X GET "http://localhost:5000/scholarship/v1/Scholarships?page=1&pageSize=10" \
  -H "Authorization: Bearer <TOKEN>"
```

#### Stationery via Gateway
```bash
curl -X GET http://localhost:5000/stationery/v1/health \
  -H "Authorization: Bearer <TOKEN>"
```

#### TDS via Gateway
```bash
curl -X GET "http://localhost:5000/tds/v1/Vendors?page=1&pageSize=10" \
  -H "Authorization: Bearer <TOKEN>"
```

#### Transaction via Gateway
```bash
curl -X GET "http://localhost:5000/transaction/v1/Requests?locationId=101" \
  -H "Authorization: Bearer <TOKEN>"
```

### Health Check
```bash
curl -X GET http://localhost:5000/health
```

---

# 10. Auth Provider (port 7136)

## REST Endpoints

### Register User
```bash
curl -X POST http://localhost:7136/api/v1/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "john.doe",
    "email": "john.doe@example.com",
    "password": "SecureP@ss123",
    "firstName": "John",
    "lastName": "Doe"
  }'
```
**Response:**
```json
{
  "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "username": "john.doe",
  "email": "john.doe@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "isActive": true,
  "isEmailVerified": false,
  "createdAt": "2026-03-23T10:00:00Z",
  "roles": ["User"]
}
```

### Login
```bash
curl -X POST http://localhost:7136/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "usernameOrEmail": "john.doe",
    "password": "SecureP@ss123"
  }'
```
**Response:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "dGhpcyBpcyBhIHJlZnJlc2ggdG9rZW4...",
  "expiresAt": "2026-03-23T11:00:00Z",
  "tokenType": "Bearer"
}
```

### Login v2 (with metadata)
```bash
curl -X POST http://localhost:7136/api/v2/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "usernameOrEmail": "john.doe",
    "password": "SecureP@ss123"
  }'
```

### Refresh Token
```bash
curl -X POST http://localhost:7136/api/v1/auth/refresh-token \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "dGhpcyBpcyBhIHJlZnJlc2ggdG9rZW4..."
  }'
```
**Response:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...(new)",
  "refreshToken": "bmV3IHJlZnJlc2ggdG9rZW4...",
  "expiresAt": "2026-03-23T12:00:00Z",
  "tokenType": "Bearer"
}
```

### Revoke Token (Requires Auth)
```bash
curl -X POST http://localhost:7136/api/v1/auth/revoke-token \
  -H "Authorization: Bearer <ACCESS_TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "dGhpcyBpcyBhIHJlZnJlc2ggdG9rZW4..."
  }'
```

### Get Current User (Requires Auth)
```bash
curl -X GET http://localhost:7136/api/v1/auth/me \
  -H "Authorization: Bearer <ACCESS_TOKEN>"
```
**Response:**
```json
{
  "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "username": "john.doe",
  "email": "john.doe@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "isActive": true,
  "isEmailVerified": false,
  "createdAt": "2026-03-23T10:00:00Z",
  "lastLoginAt": "2026-03-23T10:30:00Z",
  "roles": ["User"]
}
```

### Get All Users (Admin Only)
```bash
curl -X GET "http://localhost:7136/api/v1/users?page=1&size=20" \
  -H "Authorization: Bearer <ADMIN_TOKEN>"
```
**Response:**
```json
{
  "items": [
    {
      "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
      "username": "john.doe",
      "email": "john.doe@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "isActive": true,
      "roles": ["User"]
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 20
}
```

### Get User by ID
```bash
curl -X GET http://localhost:7136/api/v1/users/a1b2c3d4-e5f6-7890-abcd-ef1234567890 \
  -H "Authorization: Bearer <ACCESS_TOKEN>"
```

### Get User by Email (Admin Only)
```bash
curl -X GET "http://localhost:7136/api/v1/users/by-email?email=john.doe@example.com" \
  -H "Authorization: Bearer <ADMIN_TOKEN>"
```

### Update User Profile
```bash
curl -X PUT http://localhost:7136/api/v1/users/a1b2c3d4-e5f6-7890-abcd-ef1234567890 \
  -H "Authorization: Bearer <ACCESS_TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    "firstName": "Jonathan",
    "lastName": "Doe"
  }'
```

### Deactivate User (Admin Only)
```bash
curl -X DELETE http://localhost:7136/api/v1/users/a1b2c3d4-e5f6-7890-abcd-ef1234567890 \
  -H "Authorization: Bearer <ADMIN_TOKEN>"
```

### Assign Role (Admin Only)
```bash
curl -X POST http://localhost:7136/api/v1/users/a1b2c3d4-e5f6-7890-abcd-ef1234567890/roles \
  -H "Authorization: Bearer <ADMIN_TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    "roleName": "Admin"
  }'
```

## GraphQL

### Query: Get User by ID
```bash
curl -X POST http://localhost:7136/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <ACCESS_TOKEN>" \
  -d '{
    "query": "{ userById(id: \"a1b2c3d4-e5f6-7890-abcd-ef1234567890\") { id username email firstName lastName isActive roles createdAt lastLoginAt } }"
  }'
```
**Response:**
```json
{
  "data": {
    "userById": {
      "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
      "username": "john.doe",
      "email": "john.doe@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "isActive": true,
      "roles": ["User"],
      "createdAt": "2026-03-23T10:00:00Z",
      "lastLoginAt": "2026-03-23T10:30:00Z"
    }
  }
}
```

### Query: Get Users (Paginated)
```bash
curl -X POST http://localhost:7136/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <ADMIN_TOKEN>" \
  -d '{
    "query": "{ users(page: 1, size: 10) { items { id username email firstName lastName isActive roles } totalCount page pageSize } }"
  }'
```
**Response:**
```json
{
  "data": {
    "users": {
      "items": [
        {
          "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
          "username": "john.doe",
          "email": "john.doe@example.com",
          "firstName": "John",
          "lastName": "Doe",
          "isActive": true,
          "roles": ["User"]
        }
      ],
      "totalCount": 1,
      "page": 1,
      "pageSize": 10
    }
  }
}
```

### Mutation: Register User
```bash
curl -X POST http://localhost:7136/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { registerUser(input: { username: \"jane.smith\", email: \"jane@example.com\", password: \"SecureP@ss456\", firstName: \"Jane\", lastName: \"Smith\" }) { id username email roles } }"
  }'
```
**Response:**
```json
{
  "data": {
    "registerUser": {
      "id": "b2c3d4e5-f678-9012-bcde-f23456789012",
      "username": "jane.smith",
      "email": "jane@example.com",
      "roles": ["User"]
    }
  }
}
```

### Mutation: Login
```bash
curl -X POST http://localhost:7136/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { login(input: { usernameOrEmail: \"jane.smith\", password: \"SecureP@ss456\" }) { accessToken refreshToken expiresAt tokenType } }"
  }'
```
**Response:**
```json
{
  "data": {
    "login": {
      "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
      "refreshToken": "cmVmcmVzaCB0b2tlbg...",
      "expiresAt": "2026-03-23T11:00:00Z",
      "tokenType": "Bearer"
    }
  }
}
```

### Mutation: Assign Role
```bash
curl -X POST http://localhost:7136/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <ADMIN_TOKEN>" \
  -d '{
    "query": "mutation { assignRole(input: { userId: \"b2c3d4e5-f678-9012-bcde-f23456789012\", roleName: \"Admin\" }) { id username roles } }"
  }'
```

### Mutation: Delete User
```bash
curl -X POST http://localhost:7136/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <ADMIN_TOKEN>" \
  -d '{
    "query": "mutation { deleteUser(id: \"b2c3d4e5-f678-9012-bcde-f23456789012\") }"
  }'
```

---

## GraphQL Introspection (works on all services)

### Get Full Schema
```bash
curl -X POST http://localhost:<PORT>/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ __schema { types { name fields { name type { name kind } } } } }"
  }'
```

### Get Available Queries
```bash
curl -X POST http://localhost:<PORT>/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ __schema { queryType { fields { name description args { name type { name } } } } } }"
  }'
```

### Get Available Mutations
```bash
curl -X POST http://localhost:<PORT>/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ __schema { mutationType { fields { name description args { name type { name } } } } } }"
  }'
```

---

## Quick Reference — All Ports

| Port | Service | Swagger | GraphQL |
|------|---------|---------|---------|
| 5000 | API Gateway | `/swagger` | — |
| 5181 | Vendor | `/swagger` | `/graphql` |
| 5182 | Stationery | `/swagger` | `/graphql` |
| 5183 | TDS | `/swagger` | `/graphql` |
| 5184 | LOV | `/swagger` | `/graphql` |
| 5185 | Transaction | `/swagger` | `/graphql` |
| 5186 | Finyear | `/swagger` | `/graphql` |
| 5166 | Scholarship | `/swagger` | `/graphql` |
| 7136 | Location / Auth | `/swagger` | `/graphql` |
| 15672 | RabbitMQ UI | — | — |

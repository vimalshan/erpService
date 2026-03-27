# Admin Services — API Documentation

> **Gateway Port**: 5000 · **Auth**: JWT Bearer · **GraphQL**: HotChocolate

---

## Table of Contents

- [API Gateway](#api-gateway)
- [Financial Year Service (Port 5186)](#1-financial-year-service)
- [Location Service (Port 7136)](#2-location-service)
- [Vendor Service (Port 5181)](#3-vendor-service)
- [Scholarship Service (Port 5166)](#4-scholarship-service)
- [Stationery Service (Port 5273)](#5-stationery-service)
- [TDS Service (Port 5116)](#6-tds-service)
- [LOV Service (Port 5184)](#7-lov-service)
- [Transaction Service (Port 5185)](#8-transaction-service)

---

## API Gateway

**Port**: 5000 (primary entry point)

### Gateway Endpoints

```bash
# List all registered downstream services
curl http://localhost:5000/api/gateway/services

# Check health of a specific service
curl -H "Authorization: Bearer <TOKEN>" \
  http://localhost:5000/api/gateway/services/finyear/health

# Get gateway auth token (includes all service scopes)
curl -X POST http://localhost:5000/api/gateway/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "admin"}'
```

### Ocelot Routing Table

| Service       | Gateway Route                                            | Downstream       | Rate Limit |
| ------------- | -------------------------------------------------------- | ---------------- | ---------- |
| Finyear       | `/finyear/{version}/{controller}/{action?}/{id?}`        | localhost:5186   | 100/min    |
| Location      | `/location/{version}/{controller}/{action?}/{id?}`       | localhost:7136   | 100/min    |
| Vendor        | `/vendor/{version}/{controller}/{action?}/{id?}`         | localhost:5181   | 100/min    |
| Scholarship   | `/scholarship/{version}/{controller}/{action?}/{id?}`    | localhost:5166   | 100/min    |
| Stationery    | `/stationery/{version}/{controller}/{action?}/{id?}`     | localhost:5273   | 100/min    |
| TDS           | `/tds/{version}/{controller}/{action?}/{id?}`            | localhost:5116   | 100/min    |
| LOV           | `/lov/{version}/{controller}/{action?}/{id?}`            | localhost:5184   | 100/min    |
| Transaction   | `/transaction/{version}/{controller}/{action?}/{id?}`    | localhost:5185   | 100/min    |
| Shared        | `/shared/{version}/{controller}/{action?}/{id?}`         | localhost:5008   | 150/min    |

---

## 1. Financial Year Service

**Port**: 5186 · **Base**: `/api/financialyear`

### REST Endpoints

| Method   | Endpoint                              | Description                    |
| -------- | ------------------------------------- | ------------------------------ |
| `GET`    | `/api/financialyear`                  | Get all financial years        |
| `GET`    | `/api/financialyear/{id}`             | Get by ID                      |
| `GET`    | `/api/financialyear/current`          | Get current active year        |
| `GET`    | `/api/financialyear/by-name/{name}`   | Get by name                    |
| `POST`   | `/api/financialyear`                  | Create financial year          |
| `PUT`    | `/api/financialyear/{id}`             | Update financial year          |
| `DELETE` | `/api/financialyear/{id}`             | Delete financial year          |

### cURL Examples

```bash
# Get all financial years
curl http://localhost:5186/api/financialyear \
  -H "Authorization: Bearer <TOKEN>"

# Get current financial year
curl http://localhost:5186/api/financialyear/current \
  -H "Authorization: Bearer <TOKEN>"

# Get financial year by ID
curl http://localhost:5186/api/financialyear/1 \
  -H "Authorization: Bearer <TOKEN>"

# Create financial year
curl -X POST http://localhost:5186/api/financialyear \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "2025-2026",
    "startDate": "2025-04-01",
    "endDate": "2026-03-31",
    "isActive": true
  }'

# Update financial year
curl -X PUT http://localhost:5186/api/financialyear/1 \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "2025-2026",
    "startDate": "2025-04-01",
    "endDate": "2026-03-31",
    "isActive": false
  }'

# Delete financial year
curl -X DELETE http://localhost:5186/api/financialyear/1 \
  -H "Authorization: Bearer <TOKEN>"
```

### GraphQL

**Endpoint**: `POST http://localhost:5186/graphql`

```bash
# Query: Get all financial years
curl -X POST http://localhost:5186/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getAllFinancialYears(pageNumber: 1, pageSize: 20) { id name startDate endDate durationInDays status isActive updatedOn } }"
  }'

# Query: Get current financial year
curl -X POST http://localhost:5186/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getCurrentFinancialYear { id name startDate endDate isActive } }"
  }'

# Query: Get financial year by ID
curl -X POST http://localhost:5186/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getFinancialYearById(id: 1) { id name startDate endDate isActive } }"
  }'

# Mutation: Create financial year
curl -X POST http://localhost:5186/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createFinancialYear(input: { name: \"2025-2026\", startDate: \"2025-04-01\", endDate: \"2026-03-31\", isActive: true }) { financialYear { id name } } }"
  }'

# Mutation: Update financial year
curl -X POST http://localhost:5186/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { updateFinancialYear(input: { id: 1, name: \"2025-2026\", isActive: false }) { financialYear { id name isActive } } }"
  }'

# Mutation: Delete financial year
curl -X POST http://localhost:5186/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { deleteFinancialYear(id: 1) { success } }"
  }'
```

---

## 2. Location Service

**Port**: 7136 · **Base**: `/api/v{version}/location-app-maps`

### REST Endpoints

| Method   | Endpoint                                                       | Description                     | Auth         |
| -------- | -------------------------------------------------------------- | ------------------------------- | ------------ |
| `GET`    | `/api/v1/location-app-maps`                                    | Get all mappings                | Bearer       |
| `GET`    | `/api/v1/location-app-maps/active`                             | Get active mappings             | Bearer       |
| `GET`    | `/api/v1/location-app-maps/by-location/{locationId}`           | Get by location                 | Bearer       |
| `GET`    | `/api/v1/location-app-maps/{locationId}/{appName}`             | Get single mapping              | Bearer       |
| `GET`    | `/api/v1/location-app-maps/count`                              | Total count                     | Bearer       |
| `POST`   | `/api/v1/location-app-maps`                                    | Create mapping                  | Admin        |
| `PUT`    | `/api/v1/location-app-maps/{locationId}/{appName}`             | Update mapping                  | Bearer       |
| `DELETE` | `/api/v1/location-app-maps/{locationId}/{appName}`             | Delete mapping                  | Bearer       |
| `GET`    | `/api/v2/location-app-maps?page=1&pageSize=25`                | Get with pagination (v2)        | Bearer       |
| `GET`    | `/api/v2/location-app-maps/active/summary`                     | Active with summary (v2)        | Bearer       |
| `POST`   | `/api/auth/token`                                              | Get JWT token                   | Anonymous    |

### cURL Examples

```bash
# Get auth token
curl -X POST http://localhost:7136/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "admin"}'

# Get all location-app mappings
curl http://localhost:7136/api/v1/location-app-maps \
  -H "Authorization: Bearer <TOKEN>"

# Get active mappings
curl http://localhost:7136/api/v1/location-app-maps/active \
  -H "Authorization: Bearer <TOKEN>"

# Get mappings by location ID
curl http://localhost:7136/api/v1/location-app-maps/by-location/100 \
  -H "Authorization: Bearer <TOKEN>"

# Create location-app mapping
curl -X POST http://localhost:7136/api/v1/location-app-maps \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "locationId": 100,
    "appName": "SPARSH",
    "siteCategoryCode": "A",
    "selfAccess": true,
    "deemedApproval": false,
    "isActive": true,
    "createdBy": "admin"
  }'

# Get with pagination (v2)
curl "http://localhost:7136/api/v2/location-app-maps?page=1&pageSize=25" \
  -H "Authorization: Bearer <TOKEN>"
```

### GraphQL

**Endpoint**: `POST http://localhost:7136/graphql`

```bash
# Query: Get all location-app maps
curl -X POST http://localhost:7136/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getLocationAppMaps { locationId appName siteCategoryCode selfAccess deemedApproval isActive createdBy modifiedBy } }"
  }'

# Query: Get active mappings
curl -X POST http://localhost:7136/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getActiveLocationAppMaps { locationId appName isActive } }"
  }'

# Query: Get by location
curl -X POST http://localhost:7136/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getLocationAppMapsByLocation(locationId: 100) { locationId appName isActive } }"
  }'

# Mutation: Create location-app map
curl -X POST http://localhost:7136/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createLocationAppMap(input: { locationId: 100, appName: \"SPARSH\", siteCategoryCode: \"A\", selfAccess: true, deemedApproval: false, isActive: true, createdBy: \"admin\" }) { locationId appName } }"
  }'

# Mutation: Delete
curl -X POST http://localhost:7136/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { deleteLocationAppMap(locationId: 100, appName: \"SPARSH\", modifiedBy: \"admin\") }"
  }'
```

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

## 4. Scholarship Service

**Port**: 5166 · **Base**: `/api/scholarships`

### REST Endpoints

| Method   | Endpoint                                                 | Description              | Auth    |
| -------- | -------------------------------------------------------- | ------------------------ | ------- |
| `GET`    | `/api/scholarships?page=1&pageSize=20`                   | Get all (paged)          | Bearer  |
| `GET`    | `/api/scholarships/{id}`                                 | Get by ID                | Bearer  |
| `GET`    | `/api/scholarships/employee/{employeeId}?page=&pageSize=`| Get by employee          | Bearer  |
| `POST`   | `/api/scholarships`                                      | Submit application       | Bearer  |
| `PUT`    | `/api/scholarships/{id}/approve`                         | Approve                  | Admin   |
| `PUT`    | `/api/scholarships/{id}/stop`                            | Stop scholarship         | Admin   |
| `GET`    | `/api/scholarship-amounts`                               | Get amount configs       | Bearer  |
| `POST`   | `/api/auth/token`                                        | Get JWT token            | Anon    |

### cURL Examples

```bash
# Get auth token
curl -X POST http://localhost:5166/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "Admin@1234"}'

# Get all scholarships (paged)
curl "http://localhost:5166/api/scholarships?page=1&pageSize=20" \
  -H "Authorization: Bearer <TOKEN>"

# Get scholarship by ID
curl http://localhost:5166/api/scholarships/1 \
  -H "Authorization: Bearer <TOKEN>"

# Get scholarships for employee
curl "http://localhost:5166/api/scholarships/employee/1001?page=1&pageSize=10" \
  -H "Authorization: Bearer <TOKEN>"

# Submit scholarship application
curl -X POST http://localhost:5166/api/scholarships \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "employeeId": 1001,
    "childName": "Raj Kumar",
    "courseName": "B.Tech Computer Science",
    "details": {
      "marksStatus": "A",
      "payStatus": "P"
    }
  }'

# Approve scholarship (Admin only)
curl -X PUT http://localhost:5166/api/scholarships/1/approve \
  -H "Authorization: Bearer <ADMIN_TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"approvedBy": 1, "remarks": "Approved per policy"}'

# Stop scholarship (Admin only)
curl -X PUT http://localhost:5166/api/scholarships/1/stop \
  -H "Authorization: Bearer <ADMIN_TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"reason": "Course discontinued", "stoppedBy": 1}'

# Get scholarship amount configurations
curl http://localhost:5166/api/scholarship-amounts \
  -H "Authorization: Bearer <TOKEN>"
```

### GraphQL

**Endpoint**: `POST http://localhost:5166/graphql`

```bash
# Query: Get scholarships
curl -X POST http://localhost:5166/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getScholarships(page: 1, pageSize: 20) { id childName courseName entryStatus liveStatus } }"
  }'

# Query: Get scholarships by employee
curl -X POST http://localhost:5166/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getScholarships(employeeId: 1001) { id childName courseName entryStatus } }"
  }'

# Query: Get scholarship amounts
curl -X POST http://localhost:5166/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getScholarshipAmounts { id amount eligibleFrom eligibleTo } }"
  }'

# Mutation: Create scholarship
curl -X POST http://localhost:5166/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createScholarship(input: { employeeId: 1001, childName: \"Raj\", courseName: \"B.Tech\" }) }"
  }'

# Mutation: Approve
curl -X POST http://localhost:5166/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { approveScholarship(scholarshipId: 1, approvedBy: 1, remarks: \"Approved\") }"
  }'
```

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

## 6. TDS Service

**Port**: 5116 · **Base**: `/api/vendors`, `/api/files`

### REST Endpoints

| Method   | Endpoint                              | Description              |
| -------- | ------------------------------------- | ------------------------ |
| `GET`    | `/api/vendors?page=1&pageSize=20`     | Get TDS vendors (paged)  |
| `GET`    | `/api/vendors/{panNo}`                | Get vendor by PAN        |
| `POST`   | `/api/vendors`                        | Create TDS vendor        |
| `PUT`    | `/api/vendors/{vendorId}`             | Update vendor            |
| `DELETE` | `/api/vendors/{vendorId}`             | Delete vendor            |
| `GET`    | `/api/files?page=1&pageSize=20`       | Get TDS files (paged)    |
| `GET`    | `/api/files/{fileId}`                 | Get file by ID           |
| `POST`   | `/api/files`                          | Upload TDS file          |
| `PATCH`  | `/api/files/{fileId}/email-sent`      | Mark email sent          |
| `POST`   | `/api/auth/token`                     | Get JWT token            |

### cURL Examples

```bash
# Get auth token
curl -X POST http://localhost:5116/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "Admin@1234"}'

# Get TDS vendors
curl "http://localhost:5116/api/vendors?page=1&pageSize=20" \
  -H "Authorization: Bearer <TOKEN>"

# Get vendor by PAN
curl http://localhost:5116/api/vendors/ABCDE1234F \
  -H "Authorization: Bearer <TOKEN>"

# Create TDS vendor
curl -X POST http://localhost:5116/api/vendors \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "vendorId": 1,
    "vendorName": "TDS Vendor Corp",
    "emailAddress": "tds@example.com",
    "panNo": "ABCDE1234F"
  }'

# Upload TDS file (multipart)
curl -X POST http://localhost:5116/api/files \
  -H "Authorization: Bearer <TOKEN>" \
  -F "fileId=1" \
  -F "fileName=TDS_Q1_2025.pdf" \
  -F "panNo=ABCDE1234F" \
  -F "emailStatus=pending" \
  -F "fileType=PDF" \
  -F "file=@/path/to/TDS_Q1_2025.pdf"

# Mark email as sent
curl -X PATCH http://localhost:5116/api/files/1/email-sent \
  -H "Authorization: Bearer <TOKEN>"
```

### GraphQL

**Endpoint**: `POST http://localhost:5116/graphql`

```bash
# Query: Get vendors
curl -X POST http://localhost:5116/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getVendors(page: 1, pageSize: 20) { items { vendorId vendorName panNo emailAddress } totalCount } }"
  }'

# Query: Get vendor by PAN
curl -X POST http://localhost:5116/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getVendorByPan(panNo: \"ABCDE1234F\") { vendorId vendorName emailAddress } }"
  }'

# Query: Get TDS files
curl -X POST http://localhost:5116/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getFiles(page: 1, pageSize: 20) { items { fileId fileName panNo emailStatus fileType } totalCount } }"
  }'

# Mutation: Create TDS vendor
curl -X POST http://localhost:5116/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createVendor(vendorId: 1, vendorName: \"TDS Corp\", emailAddress: \"tds@test.com\", panNo: \"ABCDE1234F\") }"
  }'

# Mutation: Upload TDS file metadata
curl -X POST http://localhost:5116/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { uploadFile(fileId: 1, fileName: \"TDS_Q1.pdf\", panNo: \"ABCDE1234F\", emailStatus: \"pending\", fileType: \"PDF\") }"
  }'

# Mutation: Mark email sent
curl -X POST http://localhost:5116/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { markEmailSent(fileId: 1) }"
  }'
```

---

## 7. LOV Service

**Port**: 5184 · **GraphQL Primary**

### GraphQL

**Endpoint**: `POST http://localhost:5184/graphql`

```bash
# Query: Get all LOV types
curl -X POST http://localhost:5184/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getLovTypesAsync { lovTypeId lovTypeName } }"
  }'

# Query: Get LOV type by ID
curl -X POST http://localhost:5184/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getLovTypeAsync(id: 1) { lovTypeId lovTypeName } }"
  }'

# Query: Get all LOV masters
curl -X POST http://localhost:5184/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getLovMastersAsync { lovId lovTypeId lovName } }"
  }'

# Query: Get LOV masters by type
curl -X POST http://localhost:5184/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getLovMastersByTypeAsync(lovTypeId: 1) { lovId lovName } }"
  }'

# Query: Get item data
curl -X POST http://localhost:5184/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getItemDataAsync { id catName itemName } }"
  }'

# Query: Search item data
curl -X POST http://localhost:5184/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ searchItemDataAsync(catName: \"Office\", itemName: \"Pen\") { id catName itemName } }"
  }'

# Mutation: Create LOV type
curl -X POST http://localhost:5184/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createLovTypeAsync(lovTypeId: 10, lovTypeName: \"Department\") }"
  }'

# Mutation: Create LOV master
curl -X POST http://localhost:5184/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createLovMasterAsync(lovId: 100, lovTypeId: 10, lovName: \"HR Department\", updatedBy: 1) }"
  }'

# Mutation: Delete LOV type
curl -X POST http://localhost:5184/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { deleteLovTypeAsync(lovTypeId: 10) }"
  }'

# Mutation: Create item data
curl -X POST http://localhost:5184/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createItemDataAsync(input: { catName: \"Office\", itemName: \"Notebook\" }) }"
  }'
```

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

## Quick Reference — Port Summary

| Service       | Port | Auth Token Endpoint                   |
| ------------- | ---- | ------------------------------------- |
| API Gateway   | 5000 | `POST /api/gateway/auth/token`        |
| Finyear       | 5186 | (via gateway)                         |
| Location      | 7136 | `POST /api/auth/token`                |
| Vendor        | 5181 | `POST /api/auth/token`                |
| Scholarship   | 5166 | `POST /api/auth/token`                |
| Stationery    | 5273 | `POST /api/auth/token`                |
| TDS           | 5116 | `POST /api/auth/token`                |
| LOV           | 5184 | (via gateway)                         |
| Transaction   | 5185 | `POST /api/auth/token`                |
| Shared        | 5008 | (via gateway)                         |

# TDS Service

> **Port:** 5183 | **Swagger:** http://localhost:5183/swagger | **GraphQL:** http://localhost:5183/graphql

---

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

---

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

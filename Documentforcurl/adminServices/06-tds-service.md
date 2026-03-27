# Admin Services — API Documentation

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


# AIMS Services — API Documentation

---

## 8. Reference Service

**Port**: 5017 · **Auth**: JWT Bearer

### REST Endpoints

| Method   | Endpoint                                | Description                | Auth    |
| -------- | --------------------------------------- | -------------------------- | ------- |
| `GET`    | `/api/lovtypes?pageNumber=&pageSize=`   | Get LOV types (paged)      | Bearer  |
| `GET`    | `/api/lovtypes/{id}`                    | Get LOV type by ID         | Bearer  |
| `POST`   | `/api/lovtypes`                         | Create LOV type            | Bearer  |
| `PUT`    | `/api/lovtypes/{id}`                    | Update LOV type            | Bearer  |
| `DELETE` | `/api/lovtypes/{id}?modifiedBy=`        | Deactivate LOV type        | Bearer  |
| `GET`    | `/api/lovvalues/by-type/{typeId}`       | Get LOV values by type     | Bearer  |
| `GET`    | `/api/lovvalues/{id}`                   | Get LOV value by ID        | Bearer  |
| `POST`   | `/api/lovvalues`                        | Create LOV value           | Bearer  |
| `PUT`    | `/api/lovvalues/{id}`                   | Update LOV value           | Bearer  |
| `DELETE` | `/api/lovvalues/{id}?modifiedBy=`       | Deactivate LOV value       | Bearer  |

### cURL Examples

```bash
# Get LOV types (paged)
curl "http://localhost:5017/api/lovtypes?pageNumber=1&pageSize=10" \
  -H "Authorization: Bearer <TOKEN>"

# Get LOV type by ID
curl http://localhost:5017/api/lovtypes/1 \
  -H "Authorization: Bearer <TOKEN>"

# Create LOV type
curl -X POST http://localhost:5017/api/lovtypes \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "typeName": "Department",
    "description": "Department categories",
    "createdBy": 1
  }'

# Update LOV type
curl -X PUT http://localhost:5017/api/lovtypes/1 \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"id": 1, "typeName": "Department (Updated)", "modifiedBy": 1}'

# Deactivate LOV type
curl -X DELETE "http://localhost:5017/api/lovtypes/1?modifiedBy=1" \
  -H "Authorization: Bearer <TOKEN>"

# Get LOV values by type
curl http://localhost:5017/api/lovvalues/by-type/1 \
  -H "Authorization: Bearer <TOKEN>"

# Get LOV value by ID
curl http://localhost:5017/api/lovvalues/1 \
  -H "Authorization: Bearer <TOKEN>"

# Create LOV value
curl -X POST http://localhost:5017/api/lovvalues \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "typeId": 1,
    "valueName": "HR Department",
    "description": "Human Resources",
    "createdBy": 1
  }'
```

---


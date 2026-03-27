# Admin Services — API Documentation

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


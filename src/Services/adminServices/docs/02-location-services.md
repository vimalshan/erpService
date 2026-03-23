# Location Services

> **Port:** 7136 | **Swagger:** http://localhost:7136/swagger | **GraphQL:** http://localhost:7136/graphql

---

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

---

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

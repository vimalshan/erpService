# Canteen Services API Documentation

---

## ReferenceData Service (5195)

**Direct:** `http://localhost:5195`  
**Via Gateway:** `http://localhost:5188/api/referencedata/`  
**GraphQL:** `http://localhost:5195/graphql`

### Auth – Get Token

```bash
curl -X POST http://localhost:5195/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin"}'
```

### REST Endpoints

#### LovMaster Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/LovMaster` | Bearer | Get all LOV records |
| GET | `/api/LovMaster/{lovId}` | Bearer | Get LOV by ID |
| POST | `/api/LovMaster` | Bearer | Create LOV |
| PUT | `/api/LovMaster/{lovId}` | Bearer | Update LOV |
| DELETE | `/api/LovMaster/{lovId}` | Bearer | Delete LOV |

```bash
# Get all LOV records
curl http://localhost:5195/api/LovMaster \
  -H "Authorization: Bearer <token>"

# Get by ID
curl http://localhost:5195/api/LovMaster/MEAL_TYPE \
  -H "Authorization: Bearer <token>"

# Create LOV
curl -X POST http://localhost:5195/api/LovMaster \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"lovId": "MEAL_TYPE", "lovType": "CANTEEN", "lovName": "Meal Type"}'

# Delete LOV
curl -X DELETE http://localhost:5195/api/LovMaster/MEAL_TYPE \
  -H "Authorization: Bearer <token>"
```

#### LovTypeMaster Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/LovTypeMaster` | Bearer | Get all LOV types |
| GET | `/api/LovTypeMaster/{lovTypeCode}` | Bearer | Get type by code |
| POST | `/api/LovTypeMaster` | Bearer | Create LOV type |
| PUT | `/api/LovTypeMaster/{lovTypeCode}` | Bearer | Update LOV type |
| DELETE | `/api/LovTypeMaster/{lovTypeCode}` | Bearer | Delete LOV type |

```bash
curl http://localhost:5195/api/LovTypeMaster \
  -H "Authorization: Bearer <token>"
```

#### PathToSqlServer Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/PathToSqlServer` | Bearer | Get all paths |
| POST | `/api/PathToSqlServer` | Bearer | Create path |
| PUT | `/api/PathToSqlServer/{id}` | Bearer | Update path |
| DELETE | `/api/PathToSqlServer/{id}` | Bearer | Delete path |

```bash
curl http://localhost:5195/api/PathToSqlServer \
  -H "Authorization: Bearer <token>"
```

### Minimal API (v2)

| Method | Route | Description |
|---|---|---|
| GET | `/api/v2/lov-masters` | Get all LOV masters |
| GET | `/api/v2/lov-masters/{lovId}` | Get LOV by ID |
| POST | `/api/v2/lov-masters` | Create LOV |
| PUT | `/api/v2/lov-masters/{lovId}` | Update LOV |
| DELETE | `/api/v2/lov-masters/{lovId}` | Delete LOV |
| GET | `/api/v2/lov-type-masters` | Get all LOV types |
| GET | `/api/v2/lov-type-masters/{lovTypeCode}` | Get type by code |
| POST | `/api/v2/lov-type-masters` | Create LOV type |
| PUT | `/api/v2/lov-type-masters/{lovTypeCode}` | Update LOV type |
| DELETE | `/api/v2/lov-type-masters/{lovTypeCode}` | Delete LOV type |
| GET | `/api/v2/path-to-sql-servers` | Get all SQL server paths |

```bash
curl http://localhost:5195/api/v2/lov-masters \
  -H "Authorization: Bearer <token>"

curl http://localhost:5195/api/v2/lov-type-masters \
  -H "Authorization: Bearer <token>"
```

### GraphQL

```graphql
# Queries
query { lovMasters { lovId lovType lovName } }
query { lovMasterById(lovId: "MEAL_TYPE") { lovId lovType lovName } }
query { lovTypeMasters { lovTypeCode lovTypeName } }
query { lovTypeMasterByCode(lovTypeCode: "CANTEEN") { lovTypeCode lovTypeName } }
query { pathToSqlServers { companyCode serverName databaseName } }
```

```bash
curl -X POST http://localhost:5195/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "{ lovMasters { lovId lovType lovName } }"}'

# Mutations
curl -X POST http://localhost:5195/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { createLovMaster(lovId: \"MEAL_TYPE\", lovType: \"CANTEEN\", lovName: \"Meal Type\") { lovId lovType lovName } }"}'

curl -X POST http://localhost:5195/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { deleteLovMaster(lovId: \"MEAL_TYPE\") }"}'

curl -X POST http://localhost:5195/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { createPathToSqlServer(companyCode: \"100\", serverName: \"db-server\", databaseName: \"CanteenDB\", userId: \"sa\", dbPassword: \"pass\") { companyCode serverName databaseName } }"}'
```

---


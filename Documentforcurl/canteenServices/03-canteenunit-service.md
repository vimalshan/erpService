# Canteen Services API Documentation

---

## CanteenUnit Service (5190)

**Direct:** `http://localhost:5190`  
**Via Gateway:** `http://localhost:5188/api/canteen-unit/`  
**GraphQL:** `http://localhost:5190/graphql`

### Auth – Get Token (per-service)

Each canteen sub-service has its own local auth endpoint for testing:

```bash
# (Pattern repeats for services that have AuthController)
```

### REST Endpoints

#### CanteenUnits Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/CanteenUnits` | Bearer | Get all canteen units |
| GET | `/api/CanteenUnits/{comCode}` | Bearer | Get unit by company code |
| POST | `/api/CanteenUnits` | Bearer | Create canteen unit |
| PUT | `/api/CanteenUnits/{comCode}` | Bearer | Update canteen unit |
| DELETE | `/api/CanteenUnits/{comCode}` | Admin role | Delete canteen unit |

**cURL Examples:**

```bash
# Get all canteen units
curl http://localhost:5190/api/CanteenUnits \
  -H "Authorization: Bearer <token>"

# Get by company code
curl http://localhost:5190/api/CanteenUnits/1001 \
  -H "Authorization: Bearer <token>"

# Create canteen unit
curl -X POST http://localhost:5190/api/CanteenUnits \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "unComCod": 1001,
    "unitName": "Main Canteen",
    "location": "Building A",
    "capacity": 200
  }'

# Update
curl -X PUT http://localhost:5190/api/CanteenUnits/1001 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "comCode": 1001,
    "unitName": "Main Canteen Updated",
    "location": "Building A",
    "capacity": 250
  }'

# Delete
curl -X DELETE http://localhost:5190/api/CanteenUnits/1001 \
  -H "Authorization: Bearer <token>"
```

#### CanteenMasters Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/CanteenMasters` | Bearer | Get all canteen masters |
| GET | `/api/CanteenMasters/{comCode}` | Bearer | Get by company code |
| POST | `/api/CanteenMasters` | Bearer | Create canteen master |
| PATCH | `/api/CanteenMasters/{comCode}/live?flag=Y` | Bearer | Set live flag |
| DELETE | `/api/CanteenMasters/{comCode}` | Admin role | Delete canteen master |

**cURL Examples:**

```bash
# Get all
curl http://localhost:5190/api/CanteenMasters \
  -H "Authorization: Bearer <token>"

# Set live flag
curl -X PATCH "http://localhost:5190/api/CanteenMasters/1001/live?flag=Y" \
  -H "Authorization: Bearer <token>"
```

### Minimal API (v2)

| Method | Route | Description |
|---|---|---|
| GET | `/api/v2/canteen-units/` | Get all canteen units |
| GET | `/api/v2/canteen-units/{comCode}` | Get by company code |
| POST | `/api/v2/canteen-units/` | Create canteen unit |
| GET | `/api/v2/canteen-units/search?name=Main` | Search units (Dapper) |
| GET | `/api/v2/canteen-units/with-access-count` | Units with access count |

```bash
# v2 Get all
curl http://localhost:5190/api/v2/canteen-units/ \
  -H "Authorization: Bearer <token>"

# v2 Search by name
curl "http://localhost:5190/api/v2/canteen-units/search?name=Main" \
  -H "Authorization: Bearer <token>"
```

### GraphQL

```bash
# Queries
curl -X POST http://localhost:5190/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "{ canteenUnits { unComCod unitName location capacity } }"}'

curl -X POST http://localhost:5190/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "{ canteenUnit(comCode: 1001) { unComCod unitName location capacity } }"}'

curl -X POST http://localhost:5190/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "{ canteenMasters { cnComCod canteenName liveFlag } }"}'

# Mutations
curl -X POST http://localhost:5190/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { createCanteenUnit(input: { unComCod: 1002, unitName: \"East Wing\", location: \"Building B\", capacity: 100 }) { unComCod unitName } }"}'

curl -X POST http://localhost:5190/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { deleteCanteenUnit(comCode: 1002) }"}'
```

---


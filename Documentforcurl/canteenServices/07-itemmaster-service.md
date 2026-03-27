# Canteen Services API Documentation

---

## ItemMaster Service (5194)

**Direct:** `http://localhost:5194`  
**Via Gateway:** `http://localhost:5188/api/itemmaster/`  
**GraphQL:** `http://localhost:5194/graphql`

### Auth – Get Token

```bash
curl -X POST http://localhost:5194/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin"}'
```

### REST Endpoints

#### CanteenItemMaster Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/CanteenItemMaster/{canteenUnitCode}` | Bearer | Get all items for a unit |
| GET | `/api/CanteenItemMaster/{canteenUnitCode}/{itemCode}` | Bearer | Get specific item |
| POST | `/api/CanteenItemMaster` | Bearer | Create item |
| PUT | `/api/CanteenItemMaster/{canteenUnitCode}/{itemCode}` | Bearer | Update item |
| DELETE | `/api/CanteenItemMaster/{canteenUnitCode}/{itemCode}` | Bearer | Delete item |

**cURL Examples:**

```bash
# Get all items for a canteen unit
curl http://localhost:5194/api/CanteenItemMaster/1001 \
  -H "Authorization: Bearer <token>"

# Get specific item
curl http://localhost:5194/api/CanteenItemMaster/1001/5 \
  -H "Authorization: Bearer <token>"

# Create item
curl -X POST http://localhost:5194/api/CanteenItemMaster \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "canteenUnitCode": 1001,
    "itemCode": 10,
    "itemName": "Lunch Thali",
    "itemCategory": "LUNCH",
    "isActive": true
  }'

# Update item
curl -X PUT http://localhost:5194/api/CanteenItemMaster/1001/10 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "canteenUnitCode": 1001,
    "itemCode": 10,
    "itemName": "Lunch Thali Special",
    "itemCategory": "LUNCH",
    "isActive": true
  }'

# Delete item
curl -X DELETE http://localhost:5194/api/CanteenItemMaster/1001/10 \
  -H "Authorization: Bearer <token>"
```

#### CanteenItemPrice Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/CanteenItemPrice/{canteenUnitCode}/{itemCode}/active` | Bearer | Get active price |
| GET | `/api/CanteenItemPrice/{canteenUnitCode}/{itemCode}/history` | Bearer | Get price history |
| POST | `/api/CanteenItemPrice` | Bearer | Create price record |
| PATCH | `/api/CanteenItemPrice/{canteenUnitCode}/{itemCode}/close` | Bearer | Close active price |

```bash
# Get active price
curl http://localhost:5194/api/CanteenItemPrice/1001/5/active \
  -H "Authorization: Bearer <token>"

# Get price history
curl http://localhost:5194/api/CanteenItemPrice/1001/5/history \
  -H "Authorization: Bearer <token>"

# Create price record
curl -X POST http://localhost:5194/api/CanteenItemPrice \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "canteenUnitCode": 1001,
    "itemCode": 5,
    "price": 75.00,
    "effectiveFrom": "2025-01-01"
  }'

# Close price
curl -X PATCH http://localhost:5194/api/CanteenItemPrice/1001/5/close \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '"2025-06-30"'
```

#### CanteenGradeItemPrice Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/CanteenGradeItemPrice` | Bearer | Get all grade item prices |
| GET | `/api/CanteenGradeItemPrice/{canteenUnitCode}` | Bearer | Get by canteen unit |
| POST | `/api/CanteenGradeItemPrice` | Bearer | Create grade item price |
| PUT | `/api/CanteenGradeItemPrice/{canteenUnitCode}` | Bearer | Update grade item price |

```bash
# Get all grade item prices
curl http://localhost:5194/api/CanteenGradeItemPrice \
  -H "Authorization: Bearer <token>"

# Get by canteen unit
curl http://localhost:5194/api/CanteenGradeItemPrice/1001 \
  -H "Authorization: Bearer <token>"
```

### Minimal API (v2)

| Method | Route | Description |
|---|---|---|
| GET | `/api/v2/canteen-items/{canteenUnitCode}` | Get all items for a unit |
| GET | `/api/v2/canteen-items/{canteenUnitCode}/{itemCode}` | Get specific item |
| POST | `/api/v2/canteen-items/` | Create item |
| DELETE | `/api/v2/canteen-items/{canteenUnitCode}/{itemCode}` | Delete item |

```bash
curl http://localhost:5194/api/v2/canteen-items/1001 \
  -H "Authorization: Bearer <token>"
```

### GraphQL

```graphql
# Queries
query { canteenItems(canteenUnitCode: 1001) { canteenUnitCode itemCode itemName itemCategory isActive } }
query { canteenItem(canteenUnitCode: 1001, itemCode: 5) { canteenUnitCode itemCode itemName } }
query { activePrice(canteenUnitCode: 1001, itemCode: 5) { canteenUnitCode itemCode price effectiveFrom } }
query { gradeItemPrices { canteenUnitCode gradeType price } }
```

```bash
curl -X POST http://localhost:5194/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "{ canteenItems(canteenUnitCode: 1001) { canteenUnitCode itemCode itemName itemCategory isActive } }"}'

# Mutations
curl -X POST http://localhost:5194/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { createCanteenItem(input: { canteenUnitCode: 1001, itemCode: 10, itemName: \"Lunch Thali\", itemCategory: \"LUNCH\" }) { canteenUnitCode itemCode itemName } }"}'

curl -X POST http://localhost:5194/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { deleteCanteenItem(canteenUnitCode: 1001, itemCode: 10) }"}'
```

---


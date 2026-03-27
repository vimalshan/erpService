# Canteen Services API Documentation

---

## CardManagement Service (5191)

**Direct:** `http://localhost:5191`  
**Via Gateway:** `http://localhost:5188/api/card-management/`  
**GraphQL:** `http://localhost:5191/graphql`

### Auth – Get Token

```bash
curl -X POST http://localhost:5191/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin"}'
```

### REST Endpoints

#### GuestCards Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/GuestCards` | Bearer | Get all guest cards (paged) |
| GET | `/api/GuestCards/{canteenUnit}` | Bearer | Get guest card by canteen unit |
| POST | `/api/GuestCards` | Bearer | Create guest card |
| PUT | `/api/GuestCards/{canteenUnit}` | Bearer | Update guest card |
| PATCH | `/api/GuestCards/{canteenUnit}/close` | Bearer | Close guest card |

**cURL Examples:**

```bash
# Get all guest cards (paged)
curl "http://localhost:5191/api/GuestCards?pageNumber=1&pageSize=20&canteenUnit=1001" \
  -H "Authorization: Bearer <token>"

# Get by canteen unit
curl http://localhost:5191/api/GuestCards/1001 \
  -H "Authorization: Bearer <token>"

# Create guest card
curl -X POST http://localhost:5191/api/GuestCards \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "canteenUnit": 1001,
    "cardNumber": "GC-2025-001",
    "holderName": "John Doe",
    "validFrom": "2025-01-01",
    "validTo": "2025-12-31"
  }'

# Close guest card
curl -X PATCH http://localhost:5191/api/GuestCards/1001/close \
  -H "Authorization: Bearer <token>"
```

#### CardMaps Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/CardMaps/{canteenUnit}?activeOnly=true` | Bearer | Get card maps for canteen unit |

```bash
curl "http://localhost:5191/api/CardMaps/1001?activeOnly=true" \
  -H "Authorization: Bearer <token>"
```

#### Settlements Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/api/Settlements` | Bearer | Create settlement record |

```bash
curl -X POST http://localhost:5191/api/Settlements \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "canteenUnit": 1001,
    "settlementDate": "2025-01-31",
    "totalAmount": 5000.00,
    "settledByUserId": 1
  }'
```

### Minimal API

| Method | Route | Description |
|---|---|---|
| GET | `/api/minimal/cards/` | List guest cards (paged) |
| GET | `/api/minimal/cards/{canteenUnit}` | Get guest card by canteen unit |

```bash
curl "http://localhost:5191/api/minimal/cards/?pageNumber=1&pageSize=10" \
  -H "Authorization: Bearer <token>"
```

### GraphQL

```graphql
# Queries
query {
  guestCards(pageNumber: 1, pageSize: 10, canteenUnit: 1001) {
    canteenUnit cardNumber holderName validFrom validTo
  }
}

query {
  guestCardById(canteenUnit: 1001) {
    canteenUnit cardNumber holderName validFrom validTo
  }
}
```

```bash
curl -X POST http://localhost:5191/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "{ guestCards(pageNumber: 1, pageSize: 10) { canteenUnit cardNumber holderName } }"}'

# Mutations
curl -X POST http://localhost:5191/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { createGuestCard(command: { canteenUnit: 1001, cardNumber: \"GC-001\", holderName: \"Jane\", validFrom: \"2025-01-01\", validTo: \"2025-12-31\" }) { canteenUnit cardNumber } }"}'

curl -X POST http://localhost:5191/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { closeGuestCard(canteenUnit: 1001) }"}'

curl -X POST http://localhost:5191/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { settleCard(command: { canteenUnit: 1001, settlementDate: \"2025-01-31\", totalAmount: 5000.00 }) { canteenUnit totalAmount } }"}'
```

---


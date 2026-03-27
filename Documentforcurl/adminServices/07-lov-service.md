# Admin Services — API Documentation

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


# ERP Microservice — API Documentation

## Quick Reference — All Ports

| Port | Service | Swagger | GraphQL |
|------|---------|---------|---------|
| 5000 | API Gateway | `/swagger` | — |
| 5181 | Vendor | `/swagger` | `/graphql` |
| 5182 | Stationery | `/swagger` | `/graphql` |
| 5183 | TDS | `/swagger` | `/graphql` |
| 5184 | LOV | `/swagger` | `/graphql` |
| 5185 | Transaction | `/swagger` | `/graphql` |
| 5186 | Finyear | `/swagger` | `/graphql` |
| 5166 | Scholarship | `/swagger` | `/graphql` |
| 7136 | Location / Auth | `/swagger` | `/graphql` |
| 15672 | RabbitMQ UI | — | — |

## Service Documentation

| # | Service | File |
|---|---------|------|
| 1 | Finyear API | [01-finyear-api.md](01-finyear-api.md) |
| 2 | Location Services | [02-location-services.md](02-location-services.md) |
| 3 | LOV Service | [03-lov-service.md](03-lov-service.md) |
| 4 | Scholarship Service | [04-scholarship-service.md](04-scholarship-service.md) |
| 5 | Stationery Service | [05-stationery-service.md](05-stationery-service.md) |
| 6 | TDS Service | [06-tds-service.md](06-tds-service.md) |
| 7 | Vendor Service | [07-vendor-service.md](07-vendor-service.md) |
| 8 | Transaction Service | [08-transaction-service.md](08-transaction-service.md) |
| 9 | API Gateway | [09-api-gateway.md](09-api-gateway.md) |
| 10 | Auth Provider | [10-auth-provider.md](10-auth-provider.md) |

## GraphQL Introspection (works on all services)

### Get Full Schema
```bash
curl -X POST http://localhost:<PORT>/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ __schema { types { name fields { name type { name kind } } } } }"
  }'
```

### Get Available Queries
```bash
curl -X POST http://localhost:<PORT>/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ __schema { queryType { fields { name description args { name type { name } } } } } }"
  }'
```

### Get Available Mutations
```bash
curl -X POST http://localhost:<PORT>/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ __schema { mutationType { fields { name description args { name type { name } } } } } }"
  }'
```

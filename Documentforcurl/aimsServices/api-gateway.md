# AIMS Services — API Documentation

---

## API Gateway

**Port**: 5020 (primary entry point)

### Gateway Endpoints

| Method | Endpoint                                          | Description                     |
| ------ | ------------------------------------------------- | ------------------------------- |
| `GET`  | `/api/gateway/services`                           | List all downstream services    |
| `GET`  | `/api/gateway/services/{name}/health`             | Health of a specific service    |
| `GET`  | `/api/gateway/services/health`                    | Health of all services          |
| `GET`  | `/api/gateway/proxy/{serviceName}/{path}`         | Proxy GET to downstream         |
| `POST` | `/api/gateway/proxy/{serviceName}/{path}`         | Proxy POST to downstream        |
| `POST` | `/api/graphqlproxy/{serviceName}`                 | Forward GraphQL to downstream   |

```bash
# List all registered services
curl http://localhost:5020/api/gateway/services

# Check health of access service
curl http://localhost:5020/api/gateway/services/access/health

# Check health of all services
curl http://localhost:5020/api/gateway/services/health

# Proxy GET through gateway (e.g. get all buses)
curl http://localhost:5020/api/gateway/proxy/bus/buses \
  -H "Authorization: Bearer <TOKEN>"

# Forward GraphQL query to attendance service
curl -X POST http://localhost:5020/api/graphqlproxy/attendance \
  -H "Content-Type: application/json" \
  -d '{"query": "{ getSwipePunches(empSysId: 1001) { empSysId punchTime } }"}'
```

### Ocelot Routing Table

| Service          | Gateway Route                                | Downstream Port | Rate Limit |
| ---------------- | -------------------------------------------- | --------------- | ---------- |
| Access           | `/api/access/{everything}`                   | 5010            | 100/min    |
| Attendance       | `/api/attendance/{everything}`               | 5011            | 100/min    |
| Bus              | `/api/bus/{everything}`                       | 5012            | 100/min    |
| Calendar         | `/api/calendar/{everything}`                 | 5013            | 100/min    |
| Employee         | `/api/employee/{everything}`                 | 5014            | 100/min    |
| Group Incentive  | `/api/groupincentive/{everything}`           | 5015            | 100/min    |
| Leave            | `/api/leave/{everything}`                    | 5016            | 100/min    |
| Reference        | `/api/reference/{everything}`                | 5017            | 100/min    |
| Visitor          | `/api/visitor/{everything}`                  | 5018            | 100/min    |
| AIMS Transaction | `/api/aimstransaction/{everything}`          | 5019            | 100/min    |

Each service also has a GraphQL route: `/api/{service}/graphql` → downstream `/graphql`

---


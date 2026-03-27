# AuthProvider API Documentation

---

## Authentication & Authorization

All protected endpoints require a JWT Bearer token in the `Authorization` header:

```
Authorization: Bearer <access_token>
```

**Obtain a token** via the [Login endpoint](#post-apiv1authlogin) or [GraphQL Login mutation](#graphql-mutations).

---


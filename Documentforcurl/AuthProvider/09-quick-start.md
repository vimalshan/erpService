# AuthProvider API Documentation

---

## Quick Start – End-to-End Flow

```bash
# 1. Start the infrastructure
cd src/Services/AuthProvider
docker-compose up -d

# 2. Register a new user
curl -X POST http://localhost:5200/api/v1/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","email":"test@example.com","password":"Test@123456","firstName":"Test","lastName":"User"}'

# 3. Login to get tokens
TOKEN=$(curl -s -X POST http://localhost:5200/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{"usernameOrEmail":"test@example.com","password":"Test@123456"}' | jq -r '.accessToken')

# 4. Get current user profile
curl http://localhost:5200/api/v1/auth/me \
  -H "Authorization: Bearer $TOKEN"

# 5. List all roles (requires ADMIN)
curl http://localhost:5200/api/v1/roles \
  -H "Authorization: Bearer $TOKEN"

# 6. Check health
curl http://localhost:5200/api/v1/minimal/auth/health

# 7. GraphQL query
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"query":"{ roles { id name description } }"}'
```

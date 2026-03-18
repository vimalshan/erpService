# Quick Start Guide - Group Management Service

## 🚀 5-Minute Setup

### 1. Prerequisites Check
```bash
dotnet --version  # Should be 8.0+
sqlcmd -?         # For SQL Server
```

### 2. Navigate to Project
```bash
cd "e:\ERPMicroservice\src\Services\mainsparshServices\groupmanagementServices\GroupManagementModule"
```

### 3. Update appsettings.json
Edit `GroupManagementService.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GroupManagementDb;..."
  },
  "Jwt": {
    "SecretKey": "your-secret-key-min-32-chars-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
  }
}
```

### 4. Build Solution
```bash
dotnet build -c Release
```

### 5. Create Database
```bash
cd GroupManagementService.API
dotnet ef database update --project ../GroupManagementService.Infrastructure
```

### 6. Run the Service
```bash
dotnet run
```

### 7. Access APIs
- **Swagger/REST**: https://localhost:5001/swagger
- **GraphQL**: https://localhost:5001/graphql
- **Health**: https://localhost:5001/health

---

## 📝 Quick Test Examples

### Get JWT Token (First, You Need Authentication)
Since JWT is configured but no login endpoint exists yet, for testing use a tool to generate a token or configure a test token in appsettings.

### Test REST API
```bash
# Get all groups (requires JWT token in Authorization header)
curl -X GET "https://localhost:5001/api/v1/groups" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"

# Create a group
curl -X POST "https://localhost:5001/api/v1/groups" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "code": "TEST",
    "name": "Test Group",
    "description": "Test Description",
    "createdBy": 1,
    "isAdmin": false
  }'
```

### Test GraphQL
```bash
# Navigate to https://localhost:5001/graphql
# Try this query:
query {
  getAllGroups {
    id
    code
    name
    status
    isAdmin
  }
}

# Try this mutation:
mutation {
  createGroup(
    code: "GRAPHICS"
    name: "Graphics Team"
    description: "Design team"
    createdBy: 1
    isAdmin: false
  ) {
    id
    code
    name
  }
}
```

---

## 🏗️ Project Architecture

```
Domain Layer        → Business logic, entities, aggregates
    ↓
Application Layer   → CQRS (Commands/Queries), DTOs, Handlers
    ↓
Infrastructure      → EF Core, Repositories, Database
    ↓
API Layer          → REST, GraphQL, Minimal APIs
```

## 📚 Key Classes to Understand

1. **Group.cs** - Main aggregate root with business logic
2. **GroupRepository.cs** - Data access implementation
3. **GroupsController.cs** - REST endpoint definitions
4. **GroupQuery.cs** - GraphQL queries
5. **GroupMutation.cs** - GraphQL mutations
6. **Program.cs** - Service configuration and startup

## 🔐 Security Notes

- All endpoints require JWT authentication except health checks
- Change JWT secret key in production
- Use HTTPS in production
- Configure CORS for your frontend URL

## 🐛 Troubleshooting

### Database Connection Error
```
✓ Check SQL Server is running: net start MSSQL$SQLEXPRESS
✓ Verify connection string in appsettings.json
✓ Ensure database exists and is accessible
```

### JWT Validation Error
```
✓ Verify JWT secret key is same length and characters
✓ Check token hasn't expired
✓ Ensure Authorization header format is "Bearer {token}"
```

### RabbitMQ Connection Error (Optional)
```
✓ If not using RabbitMQ, comment out RabbitMQ services in Program.cs
✓ Or install RabbitMQ and update connection string
```

## 📖 Documentation Files

- **SOLUTION_README.md** - Complete documentation
- **IMPLEMENTATION_CHECKLIST.md** - Feature checklist
- **GroupManagementModule_Schema.sql** - Database schema

## 🎯 Next Steps

1. ✅ Understand the architecture (read SOLUTION_README.md)
2. ✅ Explore the REST API via Swagger UI
3. ✅ Try GraphQL queries at /graphql
4. ✅ Create a test group via API
5. ✅ Add menu mappings to test roles
6. ✅ Implement a login endpoint (optional, adds auth flow)

## 💡 Common Tasks

### Create a New Group via REST
```bash
curl -X POST "https://localhost:5001/api/v1/groups" \
  -H "Authorization: Bearer TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"code":"MGR","name":"Managers","createdBy":1,"isAdmin":false}'
```

### Add Menu to Group
```bash
curl -X POST "https://localhost:5001/api/v1/groups/1/menumaps" \
  -H "Authorization: Bearer TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "menuCode":"DASHBOARD",
    "menuName":"Dashboard",
    "permissions":{"canView":true,"canCreate":false},
    "createdBy":1
  }'
```

### Search Groups
```bash
curl -X GET "https://localhost:5001/api/v1/groups/search?searchTerm=Manager&status=Active" \
  -H "Authorization: Bearer TOKEN"
```

## 📞 Support

For issues:
1. Check logs in console output
2. Review SOLUTION_README.md for detailed docs
3. Check IMPLEMENTATION_CHECKLIST.md for features
4. Verify appsettings.json configuration

---

**Version**: 1.0.0
**Created**: March 15, 2026
**Status**: ✅ Ready for Development

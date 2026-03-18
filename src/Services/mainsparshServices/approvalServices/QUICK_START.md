# Approval Service - Quick Start Guide

## 🚀 Quick Start (5 minutes)

### 1. Start Dependencies
```bash
cd ApprovalService
docker-compose up -d
# Wait 10 seconds for services to start
```

### 2. Build Solution
```bash
dotnet build ApprovalService.sln
```

### 3. Create Database
```bash
cd src/ApprovalService.API
dotnet ef database update
```

### 4. Run API
```bash
dotnet run
```

### 5. Test the Service
```bash
# Get token (dev credentials: admin/admin123)
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'

# Get all approvals
curl -X GET https://localhost:5001/api/approvals \
  -H "Authorization: Bearer {YOUR_TOKEN}"
```

## 📍 Important URLs

| Service | URL | Credentials |
|---------|-----|-------------|
| API Swagger | http://localhost:5001/swagger | - |
| Health Check | http://localhost:5001/health | - |
| RabbitMQ Management | http://localhost:15672 | guest/guest |
| Azure Storage (Azurite) | http://localhost:10000 | - |

## 🔑 API Authentication

### Login
```bash
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "admin123"
}
```

### Response
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "tokenType": "Bearer",
  "expiresIn": 86400
}
```

### Use Token
```bash
Authorization: Bearer {accessToken}
```

## 📚 Common API Endpoints

### Create Approval Master
```bash
POST /api/approvals
Authorization: Bearer {token}
Content-Type: application/json

{
  "code": "TRAVEL_APR",
  "name": "Travel Request Approval",
  "module": "PER",
  "level": 3
}
```

### Get All Approvals
```bash
GET /api/approvals
Authorization: Bearer {token}
```

### Create Approver Assignment
```bash
POST /api/approvers
Authorization: Bearer {token}
Content-Type: application/json

{
  "approvalMasterId": 1,
  "employeeSysId": 1001,
  "approverLevel": 1,
  "effectiveFrom": "2026-01-01",
  "effectiveTo": null
}
```

### Get Approvers by Approval
```bash
GET /api/approvers/approval/{approvalMasterId}
Authorization: Bearer {token}
```

### Deactivate Approval
```bash
PUT /api/approvals/{id}/deactivate
Authorization: Bearer {token}
```

### Activate Approval
```bash
PUT /api/approvals/{id}/activate
Authorization: Bearer {token}
```

## 🗂️ Project Structure

```
src/
├── ApprovalService.Domain/              ← Business logic & entities
├── ApprovalService.Application/         ← CQRS & DTOs
├── ApprovalService.Infrastructure/      ← Database & external services
├── ApprovalService.API/                 ← REST endpoints & configuration
└── ApprovalService.Functions/           ← Azure Functions
```

## 🔧 Configuration

### Database Connection
Edit `src/ApprovalService.API/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ApprovalServiceDb;Integrated Security=True;"
}
```

### JWT Settings
Edit `src/ApprovalService.API/appsettings.json`:
```json
"Jwt": {
  "SecretKey": "your-secret-key-min-32-chars",
  "Issuer": "ApprovalService",
  "Audience": "ApprovalServiceUsers",
  "ExpirationHours": 24
}
```

### RabbitMQ
Edit `src/ApprovalService.API/appsettings.json`:
```json
"RabbitMq": {
  "HostName": "localhost",
  "Port": 5672,
  "UserName": "guest",
  "Password": "guest"
}
```

## 🧪 Testing Quick Commands

### Check if API is running
```bash
curl https://localhost:5001/health
```

### Check database connection
```bash
curl https://localhost:5001/api/auth/me \
  -H "Authorization: Bearer {token}"
```

### Create test approval
```bash
curl -X POST https://localhost:5001/api/approvals \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "code": "TEST_APR",
    "name": "Test Approval",
    "module": "PER",
    "level": 1
  }'
```

## 📊 Database Schema Quick Reference

### APPR_MAST (Approval Master)
- `APPR_ID` - Primary Key
- `APPR_CODE` - Unique code (50 chars max)
- `APPR_NAME` - Display name
- `APPR_MODULE` - PER, DDP, or LET
- `APPR_LEVEL` - Approval levels (1-10)
- `APPR_STATUS` - A=Active, I=Inactive
- `CREATED_BY` - User ID
- `CREATED_ON` - Timestamp

### APPROVER_EMP (Approver Employee)
- `APPROVER_ID` - Primary Key
- `APPR_ID` - Foreign key to APPR_MAST
- `EMP_SYSID` - Employee ID
- `APPROVER_LEVEL` - Level in approval chain
- `APPROVER_STATUS` - A=Active, I=Inactive
- `EFFECTIVE_FROM` - Start date
- `EFFECTIVE_TO` - End date (optional)

## 🐛 Troubleshooting

### "Connection refused" errors
```bash
# Check if Docker services are running
docker ps

# Start services if needed
docker-compose up -d
```

### "Database not found" errors
```bash
# Re-run migrations
cd src/ApprovalService.API
dotnet ef database update --force
```

### "JWT validation failed" errors
- Verify token hasn't expired (24 hours by default)
- Check SecretKey configuration matches between generation and validation
- Ensure Bearer prefix is present: `Bearer {token}`

### Port already in use
```bash
# Stop all Docker services
docker-compose down

# Start fresh
docker-compose up -d
```

## 📁 File Locations

| Item | Path |
|------|------|
| Solution | `ApprovalService.sln` |
| Main API | `src\ApprovalService.API\Program.cs` |
| Database Config | `src\ApprovalService.API\appsettings.json` |
| Logs | `src\ApprovalService.API\logs\` |
| Migrations | `src\ApprovalService.Infrastructure\Persistence\Migrations\` |
| Docker Compose | `docker-compose.yml` |

## 💡 Tips

1. **Development Mode**: Use `appsettings.Development.json` for dev overrides
2. **Logging**: Check `logs/` folder for detailed errors
3. **Database**: Use SQL Server Management Studio to browse tables
4. **RabbitMQ**: View messages in management UI at http://localhost:15672
5. **API Docs**: Always check https://localhost:5001/swagger for up-to-date endpoints

## 🔗 Related Resources

- [.NET 8 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [MediatR](https://github.com/jbogard/MediatR)
- [RabbitMQ](https://www.rabbitmq.com/getstarted.html)
- [Azure SDK](https://github.com/Azure/azure-sdk-for-net)

## ✅ Verification Checklist

- [ ] Docker services running (`docker ps`)
- [ ] Database created and migrated
- [ ] API running on localhost:5001
- [ ] Can login with admin/admin123
- [ ] Can create approval via API
- [ ] Health check returns 200 OK
- [ ] Swagger docs accessible
- [ ] RabbitMQ management UI working

---

**Ready to code!** 🎉

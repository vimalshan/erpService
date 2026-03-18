# Quick Start Guide - Access Service Microservice

## 📋 Prerequisites

- .NET 8.0 SDK
- SQL Server 2019+ or LocalDB
- Visual Studio 2022 / VS Code
- Git

## 🚀 Getting Started

### 1. Navigate to Project
```bash
cd e:\ERPMicroservice\src\Services\aimsServices\accessServices\src
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Update Database
```bash
cd AccessService.API
dotnet ef database update
```

### 4. Run the API
```bash
dotnet run
```

API will start at: `https://localhost:5001`

### 5. Access Swagger Documentation
Open browser and go to:
```
https://localhost:5001/swagger
```

## 📁 Project Structure

```
src/
├── AccessService.Domain/          # Business logic & entities
│   ├── Entities/                  # Domain entities
│   ├── ValueObjects/              # Strongly typed values
│   ├── Events/                    # Domain events
│   └── *.cs                       # Base classes
│
├── AccessService.Application/     # CQRS & DTOs
│   ├── CQRS/
│   │   ├── Commands/              # Change operations
│   │   ├── Queries/               # Read operations
│   │   └── Handlers/              # Handler implementations
│   └── DTOs/                      # Data Transfer Objects
│
├── AccessService.Infrastructure/  # Data Access
│   ├── Persistence/               # Entity Framework
│   └── Repositories/              # Data repositories
│
├── AccessService.API/             # REST API
│   ├── Controllers/               # API endpoints
│   ├── Program.cs                 # Configuration
│   └── appsettings.json           # Settings
│
└── README.md & Guides             # Documentation
```

## 🔌 API Endpoints

### Health Check
```
GET /health
```

### UserMap Management
```
GET    /api/usermaps/{employeeSystemId}
GET    /api/usermaps
POST   /api/usermaps
PUT    /api/usermaps/{employeeSystemId}/activate
PUT    /api/usermaps/{employeeSystemId}/deactivate
```

### UserRole Management
```
GET    /api/userroles/{roleId}
GET    /api/userroles/employee/{employeeSystemId}
GET    /api/userroles/type/{roleType}
POST   /api/userroles
PUT    /api/userroles/{roleId}
DELETE /api/userroles/{roleId}
```

## 🗄️ Database

**Connection String** (in appsettings.json):
```
Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Initial Catalog=ACCESSDB;
```

**Tables**:
- AIMS_USERMAP - Employee user mappings
- AIMS_USERROLE - Role assignments
- MENU_MASTER - Menu hierarchy
- AIMS_USERMENUMAP - Role-menu mappings
- SPARSHMENU_MASTER - SPARSH menus
- SPARSHMENU_ACCESS - Granular access control

## 🔍 Testing Endpoints

### Example: Create UserMap
```bash
curl -X POST "https://localhost:5001/api/usermaps" \
  -H "Content-Type: application/json" \
  -d '{"employeeSystemId": 12345}' \
  -k
```

### Example: Get UserMap
```bash
curl "https://localhost:5001/api/usermaps/12345" -k
```

### Example: Assign Role
```bash
curl -X POST "https://localhost:5001/api/userroles" \
  -H "Content-Type: application/json" \
  -d '{
    "employeeSystemId": 12345,
    "roleType": "S",
    "menuAccess": "A"
  }' \
  -k
```

## 📚 Key Files

| File | Purpose |
|------|---------|
| `AccessService.Domain/Entity.cs` | Base entity class |
| `AccessService.Domain/AggregateRoot.cs` | Aggregate root with domain events |
| `AccessService.Application/DTOs/*.cs` | Data transfer objects |
| `AccessService.Application/CQRS/Commands/*.cs` | Write operations |
| `AccessService.Application/CQRS/Queries/*.cs` | Read operations |
| `AccessService.Application/CQRS/Handlers/*.cs` | CQRS handler implementations |
| `AccessService.Infrastructure/Persistence/AccessServiceDbContext.cs` | EF DbContext |
| `AccessService.Infrastructure/Repositories/*.cs` | Data access layer |
| `AccessService.API/Controllers/*.cs` | REST API controllers |
| `AccessService.API/Program.cs` | DI & configuration |
| `README.md` | Full documentation |
| `IMPLEMENTATION-GUIDE.md` | Detailed implementation guide |

## ⚙️ Configuration

Edit `AccessService.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your-connection-string"
  },
  "JwtSettings": {
    "Secret": "your-secret-key",
    "ExpiryMinutes": 60
  },
  "RabbitMQ": {
    "HostName": "localhost",
    "UserName": "guest",
    "Password": "guest"
  }
}
```

## 🔧 Common Commands

### Build
```bash
dotnet build
```

### Run Tests
```bash
dotnet test
```

### Create Migration
```bash
cd AccessService.API
dotnet ef migrations add MigrationName --project ../AccessService.Infrastructure
```

### View Database
```bash
# Using SQL Server Management Studio
Server: (localdb)\MSSQLLocalDB
Database: ACCESSDB
```

## 🚨 Troubleshooting

### Port Already in Use
Change port in `launchSettings.json`:
```json
"https": "https://localhost:5002"
```

### Database Connection Failed
1. Start LocalDB: `sqllocaldb start MSSQLLocalDB`
2. Check connection string
3. Run migrations again

### Swagger Not Loading
- Check API is running on correct port
- Verify Program.cs has Swagger configured
- Clear browser cache

## 📝 Next Features

- [ ] JWT Authentication
- [ ] RabbitMQ Integration
- [ ] Azure Blob Storage
- [ ] Polly Resilience Patterns
- [ ] Health Checks
- [ ] GraphQL API
- [ ] Azure Functions
- [ ] Domain Event Publishing

## 📖 Documentation

- `README.md` - Full project documentation
- `IMPLEMENTATION-GUIDE.md` - Detailed implementation guide
- `QUICKSTART.md` - This file

## 💡 Tips

- Use Swagger UI for interactive API testing
- Check application logs for debugging
- Review entities in `Domain/Entities/` for business logic
- Add validation in CQRS handlers
- Use UnitOfWork for transaction management

## 🆘 Support

For issues or questions:
1. Check the logs
2. Review README.md and IMPLEMENTATION-GUIDE.md
3. Check appsettings.json configuration
4. Verify database is accessible

---

**Happy coding! 🚀**

# FAQ Microservice - Quick Reference Guide

## 🚀 Quick Start (5 minutes)

### 1. Build
```powershell
cd e:\ERPMicroservice\src\Services\payServices\faqServices
dotnet build
```
✅ **Expected Result:** Build succeeded

### 2. Run
```powershell
dotnet run --project src/FaqServices.API
```
✅ **Expected Result:** Listening on https://localhost:5001

### 3. Test
Navigate to: `https://localhost:5001/swagger/index.html`

---

## 📡 API Endpoints Reference

### Grade Management
```
GET    /api/grades              # List all grades
POST   /api/grades              # Create new grade
GET    /api/grades/{id}         # Get specific grade
PUT    /api/grades/{id}         # Update grade
DELETE /api/grades/{id}         # Delete grade
```

### Question Management
```
GET    /api/questions                      # List all questions
POST   /api/questions                      # Create new question
GET    /api/questions/{id}                 # Get question with answers
PUT    /api/questions/{id}                 # Update question
DELETE /api/questions/{id}                 # Delete question
GET    /api/questions/by-grade/{gradeId}   # Get questions by grade
```

### Answer Management
```
GET    /api/answers                          # List all answers (pageable)
POST   /api/answers                          # Create new answer
GET    /api/answers/{id}                     # Get specific answer
PUT    /api/answers/{id}                     # Update answer
DELETE /api/answers/{id}                     # Delete answer
GET    /api/answers/by-question/{questionId} # Get answers for question
```

### System
```
GET    /health                 # Health check
GET    /swagger/index.html     # API documentation
GET    /openapi/v1.json        # OpenAPI specification
```

---

## 📝 Request/Response Examples

### Create Grade
```bash
curl -X POST https://localhost:5001/api/grades \
  -H "Content-Type: application/json" \
  -d '{
    "gradeName": "Grade 1",
    "description": "First grade FAQ",
    "sortOrder": 1
  }'
```

**Response (201 Created):**
```json
{
  "pk": "550e8400-e29b-41d4-a716-446655440000",
  "gradeName": "Grade 1",
  "description": "First grade FAQ",
  "sortOrder": 1,
  "isActive": true,
  "createdAt": "2026-03-17T10:30:00Z",
  "updatedAt": null,
  "questionCount": 0
}
```

### Create Question
```bash
curl -X POST https://localhost:5001/api/questions \
  -H "Content-Type: application/json" \
  -d '{
    "gradeId": "550e8400-e29b-41d4-a716-446655440000",
    "questionText": "What is an FAQ?",
    "questionTextAr": "ما هي الأسئلة الشائعة؟",
    "sortOrder": 1
  }'
```

### Create Answer
```bash
curl -X POST https://localhost:5001/api/answers \
  -H "Content-Type: application/json" \
  -d '{
    "questionId": "650e8500-e29b-41d4-a716-446655440001",
    "answerText": "FAQ stands for Frequently Asked Questions",
    "answerTextAr": "FAQ تعني الأسئلة الشائعة",
    "isCorrect": true,
    "sortOrder": 1
  }'
```

---

## 🔐 Authentication (JWT)

### Configuration
Edit `src/FaqServices.API/appsettings.json`:
```json
"JwtSettings": {
  "SecretKey": "your-secret-key-min-32-characters-long",
  "Issuer": "FaqServices.API",
  "Audience": "FaqServices.Client",
  "ExpirationInMinutes": 60
}
```

### Protected Endpoint Example
Current endpoints don't require authentication, but you can add it:

```csharp
[Authorize]
[HttpGet("{id}")]
public async Task<IResult> GetGradeById(string id)
{
    // implementation
}
```

### Bearer Token Usage
```bash
curl -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  https://localhost:5001/api/grades
```

---

## 📊 Health Check

### Check API Health
```bash
curl https://localhost:5001/health
```

**Response:**
```json
{
  "status": "Healthy",
  "checks": {
    "DbConnection": "Healthy",
    "API": "Healthy"
  }
}
```

---

## 📂 Project File Structure

```
src/
├── FaqServices.API/
│   ├── Endpoints/                 # Minimal API route handlers
│   │   ├── GradeEndpoints.cs     # Grade CRUD
│   │   ├── QuestionEndpoints.cs  # Question CRUD  
│   │   └── AnswerEndpoints.cs    # Answer CRUD
│   ├── Program.cs                 # Startup configuration
│   ├── appsettings.json           # Configuration
│   └── appsettings.Development.json
│
├── FaqServices.Application/
│   ├── Features/
│   │   ├── Grades/               # Grade feature
│   │   │   ├── Commands/         # Mutation operations
│   │   │   └── Queries/          # Read operations
│   │   ├── Questions/            # Question feature
│   │   └── Answers/              # Answer feature
│   ├── Common/
│   │   ├── DTOs/                 # Data Transfer Objects
│   │   ├── Mappings/             # AutoMapper configuration
│   │   └── Behaviours/           # Pipeline behaviors
│   └── Extensions/               # DI registration
│
├── FaqServices.Infrastructure/
│   ├── Data/
│   │   └── FaqDbContext.cs       # Entity Framework context
│   ├── Repositories/             # Data access implementations
│   ├── Migrations/               # EF Core migrations
│   ├── UnitOfWork/               # Transaction coordination
│   └── Extensions/               # DI registration
│
└── FaqServices.Domain/
    ├── Entities/                 # Core business entities
    ├── Interfaces/               # Repository contracts
    ├── Events/                   # Domain events
    └── Common/                   # Base classes
```

---

## 🔍 Common Debugging Tasks

### Verify JWT Token
```powershell
$token = "your_jwt_token_here"
[System.Convert]::FromBase64String($token.Split('.')[1]) | ConvertFrom-Json
```

### Check Database Connection
```csharp
// In Program.cs or debug console
var context = serviceProvider.GetRequiredService<FaqDbContext>();
await context.Database.CanConnectAsync();  // true/false
```

### View Logs
```powershell
# Daily rolling logs
Get-Content "logs/faq-api-*.txt" -Tail 50
```

### Run with Verbose Logging
```bash
dotnet run --project src/FaqServices.API --verbosity diagnostic
```

---

## 🚀 Deployment Checklist

- [ ] Update JWT secret key in appsettings.json
- [ ] Update database connection string for production
- [ ] Configure CORS origins
- [ ] Enable HTTPS and SSL certificates
- [ ] Setup error monitoring (e.g., Sentry)
- [ ] Configure log aggregation
- [ ] Set up automated backups for database
- [ ] Enable rate limiting on API
- [ ] Test all endpoints with production data
- [ ] Setup CI/CD pipeline

---

## 💡 Common Tasks

### Add New Grade
1. Open Swagger UI at `/swagger/index.html`
2. Expand POST `/api/grades`
3. Click "Try it out"
4. Enter JSON payload:
   ```json
   {
     "gradeName": "Grade Name",
     "description": "Description",
     "sortOrder": 1
   }
   ```
5. Click "Execute"

### Get Questions by Grade
1. First, create or get a Grade ID
2. In Swagger, find GET `/api/questions/by-grade/{gradeId}`
3. Enter the Grade ID
4. Execute to see all questions in that grade

### Handle Errors
- **400 Bad Request** - Validation failed (check error message)
- **404 Not Found** - Resource doesn't exist
- **500 Internal Server Error** - Check logs for details
- **503 Service Unavailable** - Health check failed

---

## 🔧 Useful Commands

```powershell
# Build
dotnet build

# Run tests
dotnet test

# Create new migration
dotnet ef migrations add MigrationName -p src/FaqServices.Infrastructure -s src/FaqServices.API

# Apply migrations
dotnet ef database update -p src/FaqServices.Infrastructure -s src/FaqServices.API

# Generate clean binaries
dotnet clean

# Restore NuGet packages
dotnet restore

# Publish for release
dotnet publish -c Release -o ./publish
```

---

## 📚 Documentation Files

- **IMPLEMENTATION_GUIDE.md** - Detailed architecture documentation
- **DELIVERY_SUMMARY.md** - Project completion summary
- **FAQ/FAQ-Module.sql** - Database schema script
- **src/FaqServices.API/appsettings.json** - Configuration file

---

## 🤝 Support & Resources

### Troubleshooting
1. Check logs: `logs/faq-api-*.txt`
2. Enable verbose logging: `--verbosity diagnostic`
3. Verify database connection in appsettings
4. Check JWT configuration

### Additional Information
- .NET 10.0 Documentation: https://learn.microsoft.com/en-us/dotnet/
- Entity Framework Core: https://learn.microsoft.com/en-us/ef/core/
- MediatR Pattern: https://github.com/jbogard/MediatR
- AutoMapper: https://automapper.org/

---

**Version:** 1.0.0  
**Last Updated:** March 17, 2026  
**Status:** Production Ready ✅

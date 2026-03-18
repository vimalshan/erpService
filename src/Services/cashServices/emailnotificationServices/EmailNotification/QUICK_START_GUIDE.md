# Email Notification Microservice - Quick Reference Guide

## 🚀 Quick Start

### 1. **Build the Solution**
```powershell
# In Visual Studio or from command line
dotnet build EmailNotificationService.slnx
```

### 2. **Create the Database**
Execute the SQL script in SQL Server Management Studio:
```sql
-- Open 02-InitialCreate_Migration.sql and execute
```

Or via command line:
```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -i "02-InitialCreate_Migration.sql"
```

### 3. **Run the API**
```powershell
# From the API project directory or solution root
dotnet run --project src/EmailNotification.API/EmailNotification.API.csproj
```

The API will be available at: `https://localhost:5001`

---

## 🔗 API ENDPOINTS REFERENCE

### **Email Types Management**

#### List All Email Types
```http
GET /api/emailtypes
```
**Response:**
```json
[
  {
    "id": 1,
    "emailName": "Daily Treasury Report",
    "emailType": "D",
    "emailProcName": "usp_GenerateTreasuryReport",
    "modifiedBy": 1,
    "modifiedAt": "2026-03-12T10:30:00Z"
  }
]
```

#### Get Email Type by ID
```http
GET /api/emailtypes/1
```

#### Get Email Types by Type (Daily/Event)
```http
GET /api/emailtypes/bytype/D
```
Query Parameters:
- `D` = Daily emails
- `E` = Event-triggered emails

#### Create New Email Type
```http
POST /api/emailtypes
Content-Type: application/json

{
  "emailName": "Daily Treasury Report",
  "emailType": "D",
  "emailProcName": "usp_GenerateTreasuryReport",
  "createdBy": 1
}
```

#### Update Email Type
```http
PUT /api/emailtypes/1
Content-Type: application/json

{
  "emailName": "Updated Treasury Report",
  "emailType": "D",
  "emailProcName": "usp_GenerateTreasuryReport",
  "createdBy": 1
}
```

---

### **Recipients Management**

#### Get Recipients by Organization & Business
```http
GET /api/emailtypes/1/recipients/byorg?orgId=1&businessId=1
```

Query Parameters:
- `orgId` (required) - Organization ID
- `businessId` (optional) - Business unit ID

**Response:**
```json
[
  {
    "id": 1,
    "mailTypeId": 1,
    "mailOrgId": 1,
    "mailBusinessId": 1,
    "mailEmpSysId": 101,
    "mailEmail": "treasurer@bank.com",
    "mailName": null,
    "modifiedBy": 1,
    "modifiedAt": "2026-03-12T10:30:00Z"
  }
]
```

#### Add Recipient
```http
POST /api/emailtypes/1/recipients
Content-Type: application/json

{
  "emailAddress": "treasurer@bank.com",
  "orgId": 1,
  "businessId": 1,
  "employeeSysId": 101,
  "recipientName": null,
  "createdBy": 1
}
```

#### Remove Recipient
```http
DELETE /api/emailtypes/1/recipients/5
```

---

### **Health Check**

#### Check Service Status
```http
GET /health
```

**Response (Healthy):**
```json
{
  "status": "Healthy",
  "checks": {
    "Database": {
      "status": "Healthy"
    }
  }
}
```

---

## 📋 COMMAND & QUERY EXAMPLES

### C# Code Examples

#### Create Email Type Command
```csharp
using EmailNotification.Application.Commands;
using MediatR;

var command = new CreateEmailTypeCommand
{
    EmailName = "Daily Treasury Report",
    EmailType = "D",
    EmailProcName = "usp_GenerateTreasuryReport",
    CreatedBy = 1
};

var emailTypeId = await mediator.Send(command);
Console.WriteLine($"Email Type Created: {emailTypeId}");
```

#### Retrieve Email Types Query
```csharp
using EmailNotification.Application.Queries;

// Get all
var getAllQuery = new GetAllEmailTypesQuery();
var allEmailTypes = await mediator.Send(getAllQuery);

// Get by ID
var getByIdQuery = new GetEmailTypeByIdQuery(1);
var emailType = await mediator.Send(getByIdQuery);

// Get by type
var getByTypeQuery = new GetEmailTypesByTypeQuery("D");
var dailyEmails = await mediator.Send(getByTypeQuery);
```

#### Add Recipient Command
```csharp
using EmailNotification.Application.Commands;

var addRecipientCommand = new AddRecipientCommand
{
    EmailTypeId = 1,
    EmailAddress = "new@bank.com",
    OrgId = 1,
    BusinessId = null,
    EmployeeSysId = 102,
    RecipientName = null,
    CreatedBy = 1
};

var mailAccessId = await mediator.Send(addRecipientCommand);
```

#### Get Recipients Query
```csharp
using EmailNotification.Application.Queries;

var getRecipientsQuery = new GetRecipientsByOrgAndBusinessQuery(
    emailTypeId: 1,
    orgId: 1,
    businessId: null
);

var recipients = await mediator.Send(getRecipientsQuery);
```

---

## 🛠️ DEVELOPMENT CONFIGURATION

### appsettings.json Configuration

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EmailNotificationDb;Integrated Security=True;"
  },
  "Jwt": {
    "Authority": "https://your-auth-server.com",
    "Audience": "emailnotification-api",
    "Secret": "your-secret-key-min-32-characters-long-for-hs256",
    "ExpirationMinutes": 60
  },
  "RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest",
    "VirtualHost": "/"
  },
  "AzureBlob": {
    "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=account;AccountKey=key;EndpointSuffix=core.windows.net",
    "ContainerName": "email-attachments"
  }
}
```

### Development vs Production

**Development (appsettings.Development.json):**
- JWT validation disabled: `ValidateAudience=false, ValidateIssuer=false`
- CORS: Allow all origins
- Logging: Debug level

**Production (appsettings.json):**
- JWT validation enabled with real authority/audience
- CORS: Specific origins only
- Logging: Warn/Error level
- SSL/HTTPS enforced
- Connection strings from secure vault

---

## 📝 COMMON SCENARIOS

### Scenario 1: Send Daily Report to All Organizations
```csharp
var emailType = await mediator.Send(new GetEmailTypeByIdQuery(1));
var recipients = await mediator.Send(
    new GetRecipientsByOrgAndBusinessQuery(
        emailTypeId: 1,
        orgId: 0,  // 0 or null = all organizations
        businessId: null
    )
);

foreach (var recipient in recipients)
{
    // Send email using recipient.MailEmail
    await SendEmailAsync(recipient.MailEmail, emailType);
}
```

### Scenario 2: Add Recipients for Specific Branch
```csharp
// Branch 1, all business units
var recipients = new[]
{
    new AddRecipientCommand
    {
        EmailTypeId = 1,
        EmailAddress = "manager@branch1.com",
        OrgId = 1,
        BusinessId = null,
        EmployeeSysId = null,
        RecipientName = "Branch Manager",
        CreatedBy = 1
    },
    new AddRecipientCommand
    {
        EmailTypeId = 1,
        EmailAddress = "deputy@branch1.com",
        OrgId = 1,
        BusinessId = null,
        EmployeeSysId = null,
        RecipientName = "Deputy Manager",
        CreatedBy = 1
    }
};

foreach (var cmd in recipients)
{
    await mediator.Send(cmd);
}
```

### Scenario 3: Query by Organization and Business Unit
```csharp
// Get emails for Org 1, Business Unit 1
var recipients = await mediator.Send(
    new GetRecipientsByOrgAndBusinessQuery(
        emailTypeId: 1,
        orgId: 1,
        businessId: 1
    )
);

// Will include:
// - Recipients with OrgId=1 and BusinessId=1 (specific)
// - Recipients with OrgId=1 and BusinessId=null (org-level default)
// - Recipients with OrgId=null and BusinessId=null (global default)
```

---

## 🔐 AUTHENTICATION & AUTHORIZATION

### Enable JWT Authentication on Endpoints

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]  // Add this attribute to require authentication
public class EmailTypesController : BaseApiController
{
    // POST, PUT, DELETE require authorization
    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Create([FromBody] CreateEmailTypeCommand command)
    {
        // Implementation
    }
}
```

### Generate JWT Token
(You'll need to implement this endpoint with your auth server)

```csharp
var tokenHandler = new JwtSecurityTokenHandler();
var key = Encoding.ASCII.GetBytes(jwtSecret);

var tokenDescriptor = new SecurityTokenDescriptor
{
    Subject = new ClaimsIdentity(new[]
    {
        new Claim("sub", userId.ToString()),
        new Claim("name", userName),
        new Claim(ClaimTypes.Role, "Admin")
    }),
    Expires = DateTime.UtcNow.AddHours(1),
    SigningCredentials = new SigningCredentials(
        new SymmetricSecurityKey(key), 
        SecurityAlgorithms.HmacSha256Signature)
};

var token = tokenHandler.CreateToken(tokenDescriptor);
return tokenHandler.WriteToken(token);
```

### Use Token in API Calls

```csharp
var client = new HttpClient();
client.DefaultRequestHeaders.Authorization = 
    new AuthenticationHeaderValue("Bearer", jwtToken);

var response = await client.PostAsJsonAsync(
    "https://localhost:5001/api/emailtypes",
    command
);
```

---

## 🧪 TESTING WITH POSTMAN

### Setup Postman Environment

**Variables:**
```json
{
  "api_url": "https://localhost:5001",
  "jwt_token": "your-jwt-token-here"
}
```

### Postman Request Example

```
POST {{api_url}}/api/emailtypes
Authorization: Bearer {{jwt_token}}
Content-Type: application/json

{
  "emailName": "Daily Report",
  "emailType": "D",
  "emailProcName": "usp_GenerateReport",
  "createdBy": 1
}
```

---

## 🐛 TROUBLESHOOTING

### Common Issues

#### 1. **Database Connection Error**
```
SqlException: A network-related or instance-specific error occurred
```
**Solution:**
- Verify LocalDB is installed: `sqllocaldb info`
- Start LocalDB: `sqllocaldb start mssqllocaldb`
- Check connection string in appsettings.json

#### 2. **Migration Issues**
```
Unable to create a 'DbContext' of type 'EmailNotificationDbContext'
```
**Solution:**
- Use the SQL script: `02-InitialCreate_Migration.sql`
- Or implement parameterless constructor on aggregates

#### 3. **UnauthorizedException on API Call**
```
401 Unauthorized
```
**Solution:**
- Disable auth in development: Remove `[Authorize]` or update `ValidateIssuer=false`
- Or provide valid JWT token in Authorization header

#### 4. **Port Already in Use**
```
Cannot bind to address: 5001 already in use
```
**Solution:**
```powershell
# Find and kill the process
Get-NetTcpConnection -LocalPort 5001 | Select-Object -ExpandProperty OwningProcess | Stop-Process -Force
```

---

## 📚 DOCUMENTATION FILES

- **IMPLEMENTATION_SUMMARY.md** - Complete architecture overview
- **MODULE_GUIDE.md** - Business logic documentation
- **06-EmailNotification_Create_Schema.sql** - Original database schema
- **02-InitialCreate_Migration.sql** - Database creation script

---

## 🔗 PROJECT STRUCTURE

```
EmailNotification/
├── src/
│   ├── EmailNotification.Domain/
│   │   ├── Entities/
│   │   ├── Aggregates/
│   │   ├── ValueObjects/
│   │   ├── Events/
│   │   └── Repositories/
│   ├── EmailNotification.Application/
│   │   ├── Commands/
│   │   ├── Queries/
│   │   ├── Handlers/
│   │   ├── Validators/
│   │   ├── DTOs/
│   │   └── Mappings/
│   ├── EmailNotification.Infrastructure/
│   │   ├── Data/
│   │   └── Repositories/
│   └── EmailNotification.API/
│       ├── Controllers/
│       ├── Middleware/
│       ├── Program.cs
│       └── appsettings.json
├── EmailNotificationService.slnx
├── IMPLEMENTATION_SUMMARY.md
└── 02-InitialCreate_Migration.sql
```

---

## 📞 SUPPORT & NEXT STEPS

### For Questions About:
- **Domain Model** → See [Domain Layer](src/EmailNotification.Domain)
- **API Usage** → See [API Controllers](src/EmailNotification.API/Controllers)
- **Database** → See [DbContext](src/EmailNotification.Infrastructure/Data/EmailNotificationDbContext.cs)
- **Business Logic** → See [Commands & Handlers](src/EmailNotification.Application)

### Next Implementation Phases:
1. ✅ Core API - COMPLETE
2. ⏳ Database Integration - Use SQL migration script
3. 🔄 JWT Authentication - Implement token generation endpoint
4. 🔄 RabbitMQ Consumers - Add async message processing
5. 🔄 Azure Functions - Add scheduled background jobs
6. 🔄 Health Checks - Enhance monitoring
7. 🔄 Polly Circuit Breaker - Add resilience

---

**Last Updated**: March 12, 2026  
**Status**: Ready for Development & Testing

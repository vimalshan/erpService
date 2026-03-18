# Tax Service Microservice

A production-ready Tax Service microservice built with .NET 10, Entity Framework Core, and following Clean Architecture and Domain-Driven Design principles.

## Quick Start

### Prerequisites
- .NET 10.0 SDK
- SQL Server (using `(localdb)\MSSQLLocalDB` by default)
- RabbitMQ (optional, for message queue features)
- Visual Studio Code or Visual Studio 2024

### Setup Instructions

1. **Navigate to the project directory**:
   ```bash
   cd e:\ERPMicroservice\src\Services\payServices\taxServices
   ```

2. **Build the solution**:
   ```bash
   dotnet build
   ```

3. **Run the API**:
   ```bash
   cd src/TaxService.API
   dotnet run
   ```

4. **Access the API**:
   - HTTP: `http://localhost:5000`
   - HTTPS: `https://localhost:5001`

5. **Check Health**:
   ```bash
   curl https://localhost:5001/health
   ```

## API Endpoints

### Tax Marginal Details

#### Get Tax Detail by ID
```bash
curl -X GET "https://localhost:5001/api/taxmarginaldetails/1" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

#### Get Tax by Employee and Financial Year
```bash
curl -X GET "https://localhost:5001/api/taxmarginaldetails/employee/12345/year/2024" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

#### Get All Employee Tax Details
```bash
curl -X GET "https://localhost:5001/api/taxmarginaldetails/employee/12345" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

#### Create Tax Marginal Detail
```bash
curl -X POST "https://localhost:5001/api/taxmarginaldetails" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '{
    "employeeSystemId": 12345,
    "financialYear": 2024,
    "grossIncome": 1000000,
    "standardDeduction": 50000
  }'
```

#### Calculate Tax
```bash
curl -X POST "https://localhost:5001/api/taxmarginaldetails/1/calculate" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### Conditional Masters

#### Get Conditional Master by ID
```bash
curl -X GET "https://localhost:5001/api/conditionalmasters/1" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

#### Get by Payee ID
```bash
curl -X GET "https://localhost:5001/api/conditionalmasters/payee/PAYEE001" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

#### Get Active Conditional Masters
```bash
curl -X GET "https://localhost:5001/api/conditionalmasters/active" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

#### Create Conditional Master
```bash
curl -X POST "https://localhost:5001/api/conditionalmasters" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '{
    "payeeId": "PAYEE001",
    "payeeName": "John Doe",
    "payeeAddress": "123 Main St",
    "payeePAN": "ABCDE1234F",
    "taxRegime": "New",
    "financialYear": 2024
  }'
```

#### Add Tax Exemption
```bash
curl -X POST "https://localhost:5001/api/conditionalmasters/exemption" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '{
    "conditionalMasterId": 1,
    "code": "EXE001",
    "description": "Medical Insurance",
    "amount": 150000
  }'
```

#### Add Tax Deduction
```bash
curl -X POST "https://localhost:5001/api/conditionalmasters/deduction" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '{
    "conditionalMasterId": 1,
    "code": "DED001",
    "description": "Home Loan Interest",
    "amount": 200000
  }'
```

## Authentication

### Generate JWT Token

1. **Create a token generation endpoint** (add to your API):
```csharp
[HttpPost("auth/login")]
public IActionResult Login([FromBody] LoginRequest request)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(configuration["JwtSettings:SecretKey"]);
    
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[] {
            new Claim("sub", request.UserId),
            new Claim("name", request.UserName)
        }),
        Expires = DateTime.UtcNow.AddMinutes(int.Parse(configuration["JwtSettings:ExpirationMinutes"])),
        Issuer = configuration["JwtSettings:Issuer"],
        Audience = configuration["JwtSettings:Audience"],
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return Ok(new { token = tokenHandler.WriteToken(token) });
}
```

2. **Use the token in requests**:
```bash
curl -X GET "https://localhost:5001/api/taxmarginaldetails/1" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

## Configuration

Edit `src/TaxService.API/appsettings.json` for:

### Database Connection
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True;..."
}
```

### JWT Settings
```json
"JwtSettings": {
  "SecretKey": "your-secret-key-at-least-32-characters",
  "Issuer": "TaxService",
  "Audience": "TaxServiceAudience",
  "ExpirationMinutes": 60
}
```

### RabbitMQ Settings
```json
"RabbitMQ": {
  "HostName": "localhost",
  "UserName": "guest",
  "Password": "guest",
  "Port": 5672,
  "VirtualHost": "/"
}
```

### Azure Storage Settings
```json
"AzureStorage": {
  "BlobConnectionString": "DefaultEndpointsProtocol=https;AccountName=...",
  "ContainerName": "stationeryimages"
}
```

## Database Management

### View Database
```bash
# Open SQL Server LocalDB
sqlcmd -S (localdb)\MSSQLLocalDB

# Or use SQL Server Management Studio (SSMS)
# Connect to: (localdb)\MSSQLLocalDB
```

### Run Migrations
```bash
cd src/TaxService.Infrastructure
dotnet ef database update -s ../TaxService.API/TaxService.API.csproj
```

### Create New Migration
```bash
cd src/TaxService.Infrastructure
dotnet ef migrations add MigrationName -s ../TaxService.API/TaxService.API.csproj
```

### Revert Last Migration
```bash
cd src/TaxService.Infrastructure
dotnet ef database update PreviousMigrationName -s ../TaxService.API/TaxService.API.csproj
```

## Project Structure

```
TaxService/
├── src/
│   ├── TaxService.Domain/              # Business logic & DDD
│   │   ├── Entities/
│   │   ├── ValueObjects/
│   │   ├── Common/
│   │   └── Repositories/
│   ├── TaxService.Application/         # Use cases & CQRS
│   │   ├── Commands/
│   │   ├── Queries/
│   │   ├── DTOs/
│   │   ├── Validators/
│   │   └── Mappings/
│   ├── TaxService.Infrastructure/      # Data access & external services
│   │   ├── Data/
│   │   ├── Repositories/
│   │   ├── CommandHandlers/
│   │   ├── QueryHandlers/
│   │   ├── Migrations/
│   │   └── MessageBroker/
│   ├── TaxService.API/                 # REST API endpoints
│   │   ├── Controllers/
│   │   ├── Program.cs
│   │   └── appsettings.json
│   └── TaxService.Background/          # Background jobs & Azure Functions
├── TaxService.slnx                     # Solution file
└── PROJECT_SUMMARY.md
```

## Testing

### Unit Testing (To be implemented)
```bash
dotnet test
```

### API Testing with Postman
1. Import the API endpoints into Postman
2. Create an environment with:
   - `base_url`: https://localhost:5001
   - `jwt_token`: Your generated JWT token
3. Use these variables in your requests

## Troubleshooting

### Database Connection Issues
```csharp
// Check connection string in appsettings.json
// Default: Data Source=(localdb)\MSSQLLocalDB;...

// Verify localdb is running:
sqllocaldb info
sqllocaldb start MSSQLLocalDB
```

### JWT Token Errors
- Ensure `JwtSettings:SecretKey` is at least 32 characters
- Check token has not expired
- Verify issuer and audience match configuration

### CORS Issues
- API is configured to allow all origins in development
- Modify `AddCors` in `Program.cs` for production

### RabbitMQ Connection
- Ensure RabbitMQ is running on localhost:5672
- Default credentials: guest/guest
- Modify in `appsettings.json` as needed

## Performance Considerations

- Implemented **indexes** on frequently queried columns
- **Value objects** for type-safe value handling
- **Repository pattern** for data access abstraction
- **CQRS** for read/write optimization
- **Async/await** throughout for non-blocking operations
- **Connection pooling** with EF Core

## Security Considerations

⚠️ **Before Production**:
- Change JWT secret key to a strong secure value
- Implement HTTPS certificates
- Configure Azure storage with secured connection strings
- Set up proper CORS policies
- Enable API rate limiting
- Implement audit logging
- Validate and sanitize all inputs

## Support

For issues or questions:
1. Check logs in `bin/Debug/net10.0/logs/`
2. Review error messages in API responses
3. Check `appsettings.json` configuration
4. Review database schema with SSMS

## License

This is a demo/educational project.

---
**Framework**: .NET 10.0  
**Database**: SQL Server (localdb)  
**Last Updated**: March 17, 2026

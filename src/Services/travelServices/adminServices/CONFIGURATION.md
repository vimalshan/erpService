# Admin Service Configuration Guide

## Database Configuration

### Connection String Format
```
Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AdminServiceDb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name=AdminService;Command Timeout=0
```

### For Production SQL Server
```
Server=tcp:your-server.database.windows.net,1433;Initial Catalog=AdminServiceDb;Persist Security Info=False;User ID=your-username;Password=your-password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

## JWT Configuration

```json
"JwtSettings": {
  "SecretKey": "your-minimum-32-character-long-secret-key-for-security",
  "ExpirationMinutes": 60,
  "Issuer": "AdminService",
  "Audience": "AdminServiceAPI"
}
```

### Generate Secret Key
```powershell
# PowerShell
[Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes([Guid]::NewGuid().ToString() + [Guid]::NewGuid().ToString()))
```

## RabbitMQ Configuration

```json
"RabbitMQ": {
  "HostName": "localhost",
  "UserName": "guest",
  "Password": "guest",
  "VirtualHost": "/",
  "Port": 5672
}
```

### RabbitMQ Docker Setup
```bash
docker run -d \
  --name rabbitmq \
  -p 5672:5672 \
  -p 15672:15672 \
  -e RABBITMQ_DEFAULT_USER=guest \
  -e RABBITMQ_DEFAULT_PASS=guest \
  rabbitmq:3.12-management
```

## Azure Blob Storage Configuration

```json
"ConnectionStrings": {
  "AzureBlobStorage": "DefaultEndpointsProtocol=https;AccountName=youraccount;AccountKey=yourkey;EndpointSuffix=core.windows.net"
}
```

### Get Connection String from Azure Portal
1. Navigate to Storage Account
2. Settings → Access keys
3. Copy "Connection string" value

## Health Check Configuration

Available health checks:
- Database connectivity check
- RabbitMQ connection check (if configured)
- Azure Blob Storage access check (if configured)

Endpoint: `GET /health`

## Logging Configuration

### Serilog Levels
- Verbose (0)
- Debug (1)
- Information (2)
- Warning (3)
- Error (4)
- Fatal (5)

### Log Output
- **Console**: Real-time development feedback
- **File**: Daily rolling files in `logs/` directory

## CORS Configuration

### Development (appsettings.Development.json)
```json
"CorsPolicy": "AllowAll"
```

### Production (appsettings.json)
```json
"CorsPolicy": "AllowSpecific"
```

Allowed origins configured in `AdminService.API/Middleware/CorsMiddleware.cs`

## Security Best Practices

1. **Secret Management**
   - Use Azure Key Vault in production
   - Never commit secrets to repository
   - Use user-secrets for local development

2. **Database Security**
   - Use Windows Authentication for LocalDB
   - Use SQL authentication for remote databases
   - Enable encryption on connections

3. **API Security**
   - Implement rate limiting
   - Use HTTPS in production
   - Validate all input
   - Implement CSRF protection

4. **JWT Tokens**
   - Use strong secret keys (32+ characters)
   - Keep token expiration reasonable
   - Implement refresh token rotation

## Environment-Specific Configuration

### Development
- LocalDB with local authentication
- Debug logging enabled
- CORS: Allow all origins
- Swagger documentation enabled

### Production
- SQL Azure or on-premises SQL Server
- Minimal logging
- CORS: Allow specific origins
- Swagger disabled or authenticated
- HTTPS required

## Database Initialization

### Automatic Migration on Startup
The application automatically applies pending migrations when started. To disable:

Edit `AdminService.API/Program.cs`:
```csharp
// Comment out or remove:
// dbContext.Database.Migrate();
```

### Manual Migration
```bash
# Add new migration
dotnet ef migrations add AddNewFeature --project AdminService.Infrastructure

# Update database
dotnet ef database update --project AdminService.Infrastructure

# Revert to previous migration
dotnet ef database update PreviousMigrationName --project AdminService.Infrastructure
```

## Troubleshooting Configuration

### Connection String Issues
- Verify SQL Server is running
- Check credentials and permissions
- Ensure database exists or auto-create is enabled

### JWT Issues
- Secret key must be > 32 characters
- Token format: "Bearer <token>"
- Verify issuer and audience settings

### RabbitMQ Connection Failed
- Ensure RabbitMQ is running
- Verify hostname and port
- Check username/password
- Verify VirtualHost exists

### Azure Storage Issues
- Verify connection string format
- Ensure storage account exists
- Check access keys
- Verify network connectivity

## Monitoring & Maintenance

### Log Files Location
`./logs/log-{date}.txt`

### Database Maintenance
- Regular backups
- Index fragmentation analysis
- Query performance tuning
- Archive old data

### Performance Tuning
- Enable query caching
- Optimize database indexes
- Implement paging for large datasets
- Monitor API response times

## Additional Resources

- [Entity Framework Core Docs](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Configuration](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration)
- [Azure Storage Documentation](https://docs.microsoft.com/en-us/azure/storage/)
- [RabbitMQ Documentation](https://www.rabbitmq.com/documentation.html)

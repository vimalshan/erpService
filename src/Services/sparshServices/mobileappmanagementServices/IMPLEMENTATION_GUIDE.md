# MobileAppManagement API - Implementation Guide for Code Improvements

## Quick Start

The API is running on **http://localhost:5154**

- Swagger UI: http://localhost:5154/swagger
- GraphQL Playground: http://localhost:5154/graphql
- Health Check: http://localhost:5154/health

**Comprehensive Test File:** `MobileAppManagement.API.Comprehensive.http` (includes all endpoint types)

---

## 1. FIX CRITICAL EF CORE DECIMAL WARNINGS

### Issue
EF Core warns about decimal properties used as keys. The database uses `decimal(38,0)` which works but could be problematic.

### Solution
Since the database uses decimal, we need to explicitly configure precision and scale in EF Core.

### File: `AppDeviceDetailConfiguration.cs`

**Current (lines 15-16):**
```csharp
builder.Property(e => e.EmployeeSysId)
    .HasColumnName("MD_EMPSYSID")
    .HasColumnType("decimal(38,0)");
```

**Keep As-Is** - The configuration is correct. The warning is just a notice about using decimal as a key.

### Alternative (If you want to use long instead of decimal):

**File:** `src/MobileAppManagement.Domain/Entities/AppDeviceDetail.cs`

```csharp
// BEFORE
public decimal EmployeeSysId { get; private set; }

// AFTER
public long EmployeeSysId { get; private set; }
```

**Then update:**
- DTOs: `AppDeviceDetailDto`
- Commands: `RegisterDeviceCommand`, `DeactivateDeviceCommand`
- Queries: `GetDevicesByEmployeeQuery`, `GetDeviceByKeyQuery`
- Controllers: Parameter types
- Minimal APIs: Parameter types
- GraphQL: Mutation/Query parameter types

**This is NOT recommended** because the database stores `decimal(38,0)` which suggests large numbers needed.

---

## 2. CONSOLIDATE DUPLICATE ENDPOINTS (MEDIUM PRIORITY)

### Problem
Controllers and Minimal APIs do the exact same thing - code duplication.

### Solution
Keep only Minimal APIs and remove Controllers to reduce maintenance burden.

### Step 1: Verify Minimal APIs work
Test these endpoints first:
```
POST /api/minimal/devices/register
POST /api/minimal/devices/deactivate
GET /api/minimal/devices/employee/{employeeSysId}
GET /api/minimal/devices/{employeeSysId}/{deviceId}
```

### Step 2: Remove Controllers (after verification)

**Delete these files:**
```
src/MobileAppManagement.API/Controllers/DevicesController.cs
src/MobileAppManagement.API/Controllers/LoginsController.cs
src/MobileAppManagement.API/Controllers/RegistrationsController.cs
src/MobileAppManagement.API/Controllers/BlobStorageController.cs
```

**Keep:**
```
src/MobileAppManagement.API/Controllers/AuthController.cs (still useful for token generation)
```

### Step 3: Update Program.cs

**Current (line in Program.cs):**
```csharp
app.MapControllers(); // This maps all controllers
```

**After removal, remove this line or make it conditional:**
```csharp
if (app.Environment.IsDevelopment())
{
    // Controllers only in dev, or remove entirely
}
```

### Step 4: Create Unified API Documentation

Update Swagger/OpenAPI to document the unified minimal APIs.

---

## 3. ADD ROLE-BASED AUTHORIZATION

### Goal
Add granular permission control to endpoints.

### Step 1: Define Roles

**Create:** `src/MobileAppManagement.API/Authorization/AppRoles.cs`

```csharp
namespace MobileAppManagement.API.Authorization;

public static class AppRoles
{
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string Employee = "Employee";
    public const string MobileUser = "MobileUser";
}
```

### Step 2: Define Authorization Policies

**Create:** `src/MobileAppManagement.API/Authorization/AuthorizationPolicies.cs`

```csharp
using Microsoft.AspNetCore.Authorization;

namespace MobileAppManagement.API.Authorization;

public static class AuthorizationPolicies
{
    public const string DeviceManagement = "DeviceManagement";
    public const string RegistrationManagement = "RegistrationManagement";
    public const string AdminOnly = "AdminOnly";
    
    public static void AddCustomAuthorizationPolicies(this AuthorizationBuilder builder)
    {
        builder.AddPolicy(DeviceManagement, policy =>
            policy.RequireRole(AppRoles.Admin, AppRoles.Manager));
            
        builder.AddPolicy(RegistrationManagement, policy =>
            policy.RequireRole(AppRoles.Admin, AppRoles.Manager));
            
        builder.AddPolicy(AdminOnly, policy =>
            policy.RequireRole(AppRoles.Admin));
    }
}
```

### Step 3: Update Program.cs

```csharp
// Add to Program.cs after AddAuthorization()
var authBuilder = builder.Services.AddAuthorizationBuilder();
authBuilder.AddCustomAuthorizationPolicies();
```

### Step 4: Apply to Endpoints

**Example:** `DeviceEndpoints.cs`

```csharp
public static void Map(WebApplication app)
{
    var group = app.MapGroup("/api/minimal/devices")
        .WithTags("Devices (Minimal)")
        .RequireAuthorization(AppRoles.Employee);

    // GET endpoints - all authenticated users
    group.MapGet("/employee/{employeeSysId}", GetDevices)
        .WithOpenApi();

    // POST endpoints - only managers/admins
    group.MapPost("/register", RegisterDevice)
        .RequireAuthorization(AuthorizationPolicies.DeviceManagement)
        .WithOpenApi();

    group.MapPost("/deactivate", DeactivateDevice)
        .RequireAuthorization(AuthorizationPolicies.DeviceManagement)
        .WithOpenApi();
}

private static async Task<IResult> GetDevices(
    decimal employeeSysId,
    IMediator mediator,
    CancellationToken ct)
{
    // Implementation
}
```

---

## 4. ENHANCE GRAPHQL ERROR HANDLING

### Problem
GraphQL mutations don't validate inputs or return meaningful errors.

### Solution

**File:** `src/MobileAppManagement.API/GraphQL/Mutation.cs`

**Current:**
```csharp
public async Task<string> RegisterDevice(
    [Service] IMediator mediator,
    decimal employeeSysId, string deviceId, char deviceType, 
    string? imeiNo, decimal updatedBy,
    CancellationToken ct)
    => await mediator.Send(new RegisterDeviceCommand(...), ct);
```

**Improved:**
```csharp
public async Task<RegisterDevicePayload> RegisterDevice(
    [Service] IMediator mediator,
    [Service] ILogger<Mutation> logger,
    RegisterDeviceInput input,
    CancellationToken ct)
{
    try
    {
        // Validate input
        if (input.EmployeeSysId <= 0)
            throw new ArgumentException("EmployeeSysId must be greater than 0");
        
        if (string.IsNullOrWhiteSpace(input.DeviceId))
            throw new ArgumentException("DeviceId is required");
        
        if (input.DeviceType is not ('A' or 'I'))
            throw new ArgumentException("DeviceType must be 'A' (Android) or 'I' (iOS)");

        var result = await mediator.Send(
            new RegisterDeviceCommand(
                input.EmployeeSysId,
                input.DeviceId,
                input.DeviceType,
                input.ImeiNo,
                input.UpdatedBy),
            ct);

        return new RegisterDevicePayload(Success: true, Message: result);
    }
    catch (ValidationException ex)
    {
        logger.LogWarning("Registration validation failed: {Message}", ex.Message);
        return new RegisterDevicePayload(
            Success: false,
            Message: "Validation failed",
            Errors: ex.Errors.Select(e => new GraphQLError(e.ErrorMessage)).ToList());
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Device registration failed");
        return new RegisterDevicePayload(
            Success: false,
            Message: "Registration failed. Please try again.");
    }
}
```

**Create Input/Payload types:**

**File:** `src/MobileAppManagement.API/GraphQL/Types/InputTypes.cs`

```csharp
namespace MobileAppManagement.API.GraphQL.Types;

public record RegisterDeviceInput(
    decimal EmployeeSysId,
    string DeviceId,
    char DeviceType,
    string? ImeiNo,
    decimal UpdatedBy);

public record RegisterDevicePayload(
    bool Success,
    string? Message = null,
    List<GraphQLError>? Errors = null);

public record GraphQLError(string Message);
```

---

## 5. ADD INPUT VALIDATION TO MINIMAL APIS

### Problem
No request body validation on Minimal API endpoints.

### Solution

**File:** `src/MobileAppManagement.API/MinimalApis/DeviceEndpoints.cs`

**Current:**
```csharp
group.MapPost("/register", async (RegisterDeviceCommand command, ...) =>
{
    var result = await mediator.Send(command, ct);
    return Results.Ok(new { message = result });
});
```

**Improved:**
```csharp
group.MapPost("/register", Register)
    .WithOpenApi()
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status401Unauthorized);

private static async Task<IResult> Register(
    RegisterDeviceCommand command,
    IMediator mediator,
    IValidator<RegisterDeviceCommand> validator,
    ILogger<DeviceEndpoints> logger,
    CancellationToken ct)
{
    // Validate
    var validationResult = await validator.ValidateAsync(command, ct);
    if (!validationResult.IsValid)
    {
        logger.LogWarning("Device registration validation failed");
        return Results.BadRequest(new
        {
            success = false,
            message = "Validation failed",
            errors = validationResult.Errors
                .Select(e => new { field = e.PropertyName, message = e.ErrorMessage })
                .ToList()
        });
    }

    try
    {
        var result = await mediator.Send(command, ct);
        return Results.Ok(new { success = true, message = result });
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Device registration failed");
        return Results.StatusCode(500);
    }
}
```

---

## 6. ADD CORRELATION ID TO LOGGING

### Problem
Logs don't have correlation IDs, making it hard to trace requests.

### Solution

**File:** `src/MobileAppManagement.API/Middleware/CorrelationIdMiddleware.cs`

**Create:**
```csharp
namespace MobileAppManagement.API.Middleware;

public class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
{
    private const string CorrelationIdHeader = "X-Correlation-Id";

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers.TryGetValue(CorrelationIdHeader, out var value)
            ? value.ToString()
            : System.Guid.NewGuid().ToString();

        context.Items["CorrelationId"] = correlationId;
        context.Response.Headers.Append(CorrelationIdHeader, correlationId);

        using (logger.BeginScope(new { CorrelationId = correlationId }))
        {
            await next(context);
        }
    }
}
```

**Update Program.cs:**
```csharp
// Add as first middleware
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
```

**Update ExceptionHandlingMiddleware to include correlation ID:**
```csharp
catch (Exception ex)
{
    var correlationId = context.Items["CorrelationId"]?.ToString();
    await HandleExceptionAsync(context, (int)HttpStatusCode.InternalServerError, 
        "Internal Server Error",
        JsonSerializer.Serialize(new
        {
            title = "Internal Server Error",
            status = 500,
            detail = "An unexpected error occurred.",
            correlationId = correlationId
        }));
    logger.LogError(ex, "Unhandled exception occurred [CorrelationId: {CorrelationId}]", correlationId);
}
```

---

## 7. ADD RESPONSE CACHING (GET ENDPOINTS)

### Problem
GET endpoints don't use caching, causing unnecessary database queries.

### Solution

**Update Program.cs:**
```csharp
builder.Services.AddResponseCaching();

// In app configuration
app.UseResponseCaching();
```

**Update DeviceEndpoints.cs:**
```csharp
group.MapGet("/employee/{employeeSysId}", GetDevices)
    .WithOpenApi()
    .CacheOutput(c => c.Expire(TimeSpan.FromMinutes(5)))
    .Produces(StatusCodes.Status200OK);

private static async Task<IResult> GetDevices(
    decimal employeeSysId,
    IMediator mediator,
    CancellationToken ct)
{
    var result = await mediator.Send(new GetDevicesByEmployeeQuery(employeeSysId), ct);
    return Results.Ok(result);
}
```

---

## 8. IMPLEMENT PAGINATION FOR LIST ENDPOINTS

### Problem
Getting all registrations could return thousands of records.

### Solution

**Create DTO:** `src/MobileAppManagement.Application/DTOs/PaginationDto.cs`

```csharp
namespace MobileAppManagement.Application.DTOs;

public record PaginationParams(int PageNumber = 1, int PageSize = 10)
{
    public int Skip => (PageNumber - 1) * PageSize;
    public int Take => PageSize;
};

public record PagedResult<T>(
    List<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize)
{
    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
}
```

**Update Endpoints:**
```csharp
group.MapGet("/user/{userId}", GetRegistrationsByUser)
    .WithOpenApi();

private static async Task<IResult> GetRegistrationsByUser(
    string userId,
    int pageNumber = 1,
    int pageSize = 10,
    IMediator mediator,
    CancellationToken ct)
{
    var result = await mediator.Send(
        new GetRegistrationsByUserIdQuery(userId, pageNumber, pageSize),
        ct);
    return Results.Ok(result);
}
```

---

## 9. ADD RATE LIMITING

### Problem
API has no rate limiting, vulnerable to abuse.

### Solution

**Install package:**
```bash
dotnet add package AspNetCoreRateLimit
```

**Update Program.cs:**
```csharp
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimit"));
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddInMemoryRateLimiting();

app.UseIpRateLimiting();
```

**appsettings.Development.json:**
```json
{
  "IpRateLimit": {
    "EnableEndpointRateLimiting": true,
    "StackBlockedRequests": false,
    "HttpStatusCode": 429,
    "RealIpHeader": "X-Real-IP",
    "ClientIdHeader": "X-ClientId",
    "Ip4PrefixLength": 19,
    "Ip6PrefixLength": 60,
    "StartupDeley": 100,
    "IpWhitelist": [],
    "EndpointWhitelist": ["/health"],
    "ClientWhitelist": [],
    "GeneralRules": [
      {
        "Endpoint": "*",
        "Period": "1m",
        "Limit": 100,
        "QuotaExceededResponse": {
          "message": "API call quota exceeded. Maximum 100 requests per minute."
        }
      }
    ]
  }
}
```

---

## 10. CREATE UNIT TEST PROJECT

### Create Project Structure

```bash
cd e:\ERPMicroservice\src\Services\sparshServices\mobileappmanagementServices
dotnet new xunit -n MobileAppManagement.Tests
dotnet add MobileAppManagement.Tests reference src/MobileAppManagement.Application/MobileAppManagement.Application.csproj
dotnet add MobileAppManagement.Tests reference src/MobileAppManagement.Domain/MobileAppManagement.Domain.csproj
dotnet add MobileAppManagement.Tests package Moq
dotnet add MobileAppManagement.Tests package FluentAssertions
```

### Example Test File

**File:** `MobileAppManagement.Tests/Unit/Application/RegisterDeviceCommandHandlerTests.cs`

```csharp
using FluentAssertions;
using Moq;
using Xunit;
using MobileAppManagement.Application.Commands;
using MobileAppManagement.Application.Handlers;
using MobileAppManagement.Domain.Repositories;

namespace MobileAppManagement.Tests.Unit.Application;

public class RegisterDeviceCommandHandlerTests
{
    private readonly Mock<IDeviceRepository> _mockDeviceRepository;
    private readonly RegisterDeviceCommandHandler _handler;

    public RegisterDeviceCommandHandlerTests()
    {
        _mockDeviceRepository = new Mock<IDeviceRepository>();
        _handler = new RegisterDeviceCommandHandler(_mockDeviceRepository.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldRegisterDevice()
    {
        // Arrange
        var command = new RegisterDeviceCommand(
            employeeSysId: 1001,
            deviceId: "DEVICE001",
            deviceType: 'A',
            imeiNo: "123456789012345",
            updatedBy: 1);

        _mockDeviceRepository
            .Setup(x => x.AddAsync(It.IsAny<AppDeviceDetail>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Contain("success");
        _mockDeviceRepository.Verify(x => x.AddAsync(It.IsAny<AppDeviceDetail>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidEmployeeId_ShouldThrow()
    {
        // Arrange
        var command = new RegisterDeviceCommand(
            employeeSysId: 0, // Invalid
            deviceId: "DEVICE001",
            deviceType: 'A',
            imeiNo: "123456789012345",
            updatedBy: 1);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
}
```

---

## Testing Endpoints

Use the provided **`MobileAppManagement.API.Comprehensive.http`** file to test:

1. **Controllers** (if not removed)
2. **Minimal APIs** (recommended to use these)
3. **GraphQL** (query and mutations)
4. **Health checks**

### Quick Test Sequence
1. Test authentication: `POST /api/auth/token`
2. Test device registration: `POST /api/minimal/devices/register`
3. Test retrieval: `GET /api/minimal/devices/employee/{id}`
4. Test GraphQL: `POST /graphql`

---

## Summary of Changes

| Priority | Item | Effort | Impact |
|----------|------|--------|---------|
| CRITICAL | Fix EF Core warnings | Low | High |
| HIGH | Consolidate endpoints | Medium | High |
| HIGH | Add authorization | Medium | Medium |
| HIGH | Add error handling | Low | Medium |
| MEDIUM | Add correlation IDs | Low | Medium |
| MEDIUM | Add caching | Low | Medium |
| MEDIUM | Add pagination | Medium | Medium |
| LOW | Add rate limiting | Low | Low |
| LOW | Create tests | High | High |

---

## Next Steps
1. Run `MobileAppManagement.API.Comprehensive.http` to test all endpoints
2. Start with fixing critical issues
3. Consolidate duplicate endpoints
4. Add tests for critical paths
5. Deploy improvements incrementally


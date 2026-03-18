using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Stationery.Infrastructure.Persistence;
using Stationery.Infrastructure.Repositories;
using Stationery.Infrastructure.Services;
using Stationery.Domain.Interfaces;
using Stationery.Api.Middleware;
using Stationery.Api.GraphQL;
using Stationery.Api.Extensions;
using MediatR;
using Stationery.Application.Features.Requests.Commands;
using Stationery.Application.Features.Requests.Queries;
using Stationery.Application.Features.Orders.Commands;
using Stationery.Application.Features.Items.Queries;
using Stationery.Application.Common.Behaviors;
using Stationery.Infrastructure.Messaging.Consumers;
using Asp.Versioning;
using MassTransit;
using FluentValidation;
using Stationery.Application.Mappings;
using Stationery.Api.Configurations;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// 1. Database & Infrastructure
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
builder.Services.AddDbContext<StationeryDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBlobService, BlobService>();
builder.Services.AddHttpClient<IVendorAdapter, ExternalVendorAdapter>();

// 2. Authentication & Authorization
builder.Services.AddJwtAuthentication(builder.Configuration);

// 3. MediatR setup
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(CreateRequestCommand).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(typeof(CreateRequestCommand).Assembly);

// 4. AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<StationeryMappingProfile>());

// 5. GraphQL setup
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddAuthorization();

// 6. API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// 7. Resilience Policies (Polly)
builder.Services.AddResiliencePolicies();

// 8. Messaging (RabbitMQ with Consumers) - Conditional based on environment
var rabbitMQEnabled = builder.Configuration.GetValue<bool>("RabbitMQ:Enabled", true);
var rabbitMQHost = builder.Configuration["RabbitMQ:Host"] ?? "localhost";

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<RequestCreatedConsumer>();
    x.AddConsumer<RequestApprovedConsumer>();
    x.AddConsumer<OrderCreatedConsumer>();
    x.AddConsumer<StockLevelChangedConsumer>();

    if (rabbitMQEnabled && !builder.Environment.IsDevelopment())
    {
        // Production: Use RabbitMQ
        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(rabbitMQHost, h =>
            {
                h.Username(builder.Configuration["RabbitMQ:UserName"] ?? "guest");
                h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
            });

            cfg.ReceiveEndpoint("request-created-queue", e =>
                e.ConfigureConsumer<RequestCreatedConsumer>(context));

            cfg.ReceiveEndpoint("request-approved-queue", e =>
                e.ConfigureConsumer<RequestApprovedConsumer>(context));

            cfg.ReceiveEndpoint("order-created-queue", e =>
                e.ConfigureConsumer<OrderCreatedConsumer>(context));

            cfg.ReceiveEndpoint("stock-level-changed-queue", e =>
                e.ConfigureConsumer<StockLevelChangedConsumer>(context));
        });
    }
    else
    {
        // Development: Use in-memory transport
        x.UsingInMemory((context, cfg) =>
        {
            cfg.ConfigureEndpoints(context);
        });
    }
});

// 9. Health Checks (MassTransit registers its own bus health check automatically)
builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, name: "SQLServer", tags: new[] { "db", "sql" });

// 10. Swagger with JWT support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Stationery API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token: Bearer {token}"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var aiConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
if (!string.IsNullOrEmpty(aiConnectionString))
{
    builder.Services.AddApplicationInsightsTelemetry(options =>
    {
        options.ConnectionString = aiConnectionString;
    });
}

var app = builder.Build();

// Seed database on startup (with error handling)
try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<StationeryDbContext>();
        
        // Ensure created which is idempotent - only creates if doesn't exist
        await db.Database.EnsureCreatedAsync();
        Console.WriteLine("✓ Database and tables ensured");
        
        // Apply migrations if they exist (only in development or if not already applied)
        if (db.Database.IsSqlServer() && app.Environment.IsDevelopment())
        {
            try
            {
                await db.Database.MigrateAsync();
                Console.WriteLine("✓ Migrations applied successfully");
            }
            catch (Exception migEx)
            {
                Console.WriteLine($"⚠ Migration skip (tables already exist): {migEx.Message}");
            }
        }
        
        // Seed data if needed
        try
        {
            await DbInitializer.SeedAsync(db);
            Console.WriteLine("✓ Database seeding complete");
        }
        catch (Exception seedEx)
        {
            Console.WriteLine($"⚠ Database seeding skipped (data already exists or schema mismatch): {seedEx.Message}");
        }
        Console.WriteLine("✓ Database initialization complete");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Database initialization error: {ex.Message}");
    // Don't fail startup completely, the health check will report the issue
}

// Pipeline
var swaggerEnabled = builder.Configuration.GetValue<bool>("SwaggerSettings:Enabled", true);
if (app.Environment.IsDevelopment() || swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stationery API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
// Only use HTTPS redirect in production but not in containers
var aspnetcoreUrls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
if (app.Environment.IsProduction() && string.IsNullOrEmpty(aspnetcoreUrls))
{
    app.UseHttpsRedirection();
}
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapHealthChecks("/healthz");
app.MapGet("/", () => Results.Ok(new { message = "Stationery Service is running", version = "1.0" }));
app.MapGraphQL();

// Versioned endpoint group
var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1, 0))
    .ReportApiVersions()
    .Build();

var apiV1 = app.MapGroup("/api/v1").WithApiVersionSet(apiVersionSet).HasApiVersion(1.0);

// AUTH
apiV1.MapPost("/auth/token", (TokenRequest req, IConfiguration config) =>
{
    var jwtSettings = config.GetSection("JwtSettings").Get<JwtSettings>();
    if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Secret))
        return Results.Problem("JWT not configured.");

    if (req.Username != "admin" || req.Password != "admin")
        return Results.Unauthorized();

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
        issuer: jwtSettings.Issuer,
        audience: jwtSettings.Audience,
        expires: DateTime.UtcNow.AddMinutes(jwtSettings.ExpiryInMinutes),
        signingCredentials: creds);
    var tokenString = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
    return Results.Ok(new { token = tokenString });
}).WithName("GetToken").WithTags("Auth");

// ITEMS
apiV1.MapGet("/items", async (long? locationId, IMediator mediator) =>
    Results.Ok(await mediator.Send(new GetAllItemsQuery(locationId))))
    .WithName("GetAllItems").WithTags("Items");

apiV1.MapGet("/items/{id:long}", async (long id, IMediator mediator) =>
{
    var item = await mediator.Send(new GetItemByIdQuery(id));
    return item == null ? Results.NotFound() : Results.Ok(item);
}).WithName("GetItemById").WithTags("Items");

apiV1.MapGet("/items/low-stock", async (long threshold, IMediator mediator) =>
    Results.Ok(await mediator.Send(new GetLowStockItemsQuery(threshold))))
    .WithName("GetLowStockItems").WithTags("Items");

// REQUESTS
apiV1.MapPost("/requests", async (CreateRequestCommand command, IMediator mediator) =>
{
    var id = await mediator.Send(command);
    return Results.Created($"/api/v1/requests/{id}", new { id });
}).WithName("CreateRequest").WithTags("Requests").RequireAuthorization();

apiV1.MapGet("/requests", async (long? locationId, string? status, IMediator mediator) =>
    Results.Ok(await mediator.Send(new GetRequestsQuery(locationId, status))))
    .WithName("GetRequests").WithTags("Requests").RequireAuthorization();

apiV1.MapGet("/requests/{id:long}", async (long id, IMediator mediator) =>
{
    var req = await mediator.Send(new GetRequestByIdQuery(id));
    return req == null ? Results.NotFound() : Results.Ok(req);
}).WithName("GetRequestById").WithTags("Requests").RequireAuthorization();

apiV1.MapPut("/requests/approve", async (ApproveRequestCommand command, IMediator mediator) =>
{
    await mediator.Send(command);
    return Results.NoContent();
}).WithName("ApproveRequest").WithTags("Requests").RequireAuthorization();

// ORDERS
apiV1.MapPost("/orders", async (CreateOrderCommand command, IMediator mediator) =>
{
    var id = await mediator.Send(command);
    return Results.Created($"/api/v1/orders/{id}", new { id });
}).WithName("CreateOrder").WithTags("Orders").RequireAuthorization();

apiV1.MapPost("/orders/receive", async (ReceiveOrderCommand command, IMediator mediator) =>
{
    await mediator.Send(command);
    return Results.NoContent();
}).WithName("ReceiveOrder").WithTags("Orders").RequireAuthorization();

// BLOB: Upload stationery item image
apiV1.MapPost("/items/{id:long}/image", async (long id, IFormFile file, IBlobService blobService) =>
{
    if (file.Length == 0) return Results.BadRequest("File is empty.");
    var fileName = $"item-{id}-{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";
    using var stream = file.OpenReadStream();
    var url = await blobService.UploadAsync(stream, fileName, file.ContentType);
    return Results.Ok(new { url });
}).WithName("UploadItemImage").WithTags("Items").RequireAuthorization().DisableAntiforgery();

app.Run();

public record TokenRequest(string Username, string Password);

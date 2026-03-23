using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MediatR;
using Polly.Extensions.Http;
using AccessService.Infrastructure.Persistence;
using AccessService.Infrastructure.DomainEvents;
using AccessService.Infrastructure.MessageBrokers.RabbitMQ;
using AccessService.Infrastructure.MessageBrokers.RabbitMQ.Consumers;
using AccessService.Infrastructure.BlobStorage;
using AccessService.Infrastructure.AzureFunctions;
using AccessService.Domain.Events;
using AccessService.Domain.Entities;
using AccessService.Application.CQRS.Commands;
using AccessService.Infrastructure.Repositories;
using AccessService.API.Authentication;
using AccessService.API.GraphQL;
using AccessService.API.HealthChecks;
using AccessService.API.Resilience;
using AccessService.API.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True;Initial Catalog=ACCESSDB;";

// Domain Event Publisher - register before DbContext
builder.Services.AddSingleton<IDomainEventPublisher, InMemoryDomainEventPublisher>();

// Domain Event Dispatcher Interceptor
builder.Services.AddScoped<DomainEventDispatcherInterceptor>();

// Entity Framework with Domain Event Dispatcher Interceptor
builder.Services.AddDbContext<AccessServiceDbContext>((sp, options) =>
{
    options.UseSqlServer(connectionString);
    var interceptor = sp.GetRequiredService<DomainEventDispatcherInterceptor>();
    options.AddInterceptors(interceptor);
});

// Domain Event Handlers
builder.Services.AddScoped<IDomainEventHandler<UserMapCreatedEvent>, UserMapCreatedEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserMapActivatedEvent>, UserMapActivatedEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserRoleAssignedEvent>, UserRoleAssignedEventHandler>();
builder.Services.AddScoped<IDomainEventHandler<UserRoleRevokedEvent>, UserRoleRevokedEventHandler>();

// RabbitMQ Message Broker Configuration
var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ");
var rabbitMqHost = rabbitMqSettings["HostName"] ?? "localhost";
var rabbitMqPort = int.Parse(rabbitMqSettings["Port"] ?? "5672");
var rabbitMqUsername = rabbitMqSettings["UserName"] ?? "guest";
var rabbitMqPassword = rabbitMqSettings["Password"] ?? "guest";
var rabbitMqVirtualHost = rabbitMqSettings["VirtualHost"] ?? "/";

builder.Services.AddSingleton(new RabbitMQSettings
{
    Host = rabbitMqHost,
    Port = rabbitMqPort,
    Username = rabbitMqUsername,
    Password = rabbitMqPassword,
    VirtualHost = rabbitMqVirtualHost
});

builder.Services.AddSingleton<IRabbitMQConnection, RabbitMQConnection>();
builder.Services.AddScoped<IRabbitMQPublisher, RabbitMQPublisher>();
builder.Services.AddScoped<IDomainEventPublisher, RabbitMQDomainEventPublisher>();
builder.Services.AddScoped<IdempotencyService>();

// RabbitMQ Consumer Registration
builder.Services.AddScoped<UserMapCreatedEventConsumer>();
builder.Services.AddScoped<UserMapActivatedEventConsumer>();
builder.Services.AddScoped<UserRoleAssignedEventConsumer>();
builder.Services.AddScoped<UserRoleRevokedEventConsumer>();

// RabbitMQ Consumer Background Service
builder.Services.AddHostedService<RabbitMQConsumerBackgroundService>();

// Azure Blob Storage Configuration
var azureBlobSettings = builder.Configuration.GetSection("AzureBlob");
var blobConnectionString = azureBlobSettings["ConnectionString"] ?? "DefaultEndpointsProtocol=https;AccountName=your-account;AccountKey=your-key;EndpointSuffix=core.windows.net";
var blobContainerName = azureBlobSettings["ContainerName"] ?? "stationery-images";

builder.Services.AddSingleton(new AzureBlobStorageSettings
{
    ConnectionString = blobConnectionString,
    ContainerName = blobContainerName
});

builder.Services.AddScoped<IAzureBlobStorageService, AzureBlobStorageService>();

// Azure Functions Configuration
var azureFunctionsSettings = builder.Configuration.GetSection("AzureFunctions");
var functionsConnectionString = azureFunctionsSettings["ConnectionString"] ?? "DefaultEndpointsProtocol=https;AccountName=your-account;AccountKey=your-key;EndpointSuffix=core.windows.net";
var functionsQueueName = azureFunctionsSettings["QueueName"] ?? "access-service-queue";
var functionAppBaseUrl = azureFunctionsSettings["FunctionAppBaseUrl"] ?? "https://your-function-app.azurewebsites.net";

builder.Services.AddSingleton(new AzureFunctionsSettings
{
    ConnectionString = functionsConnectionString,
    QueueName = functionsQueueName,
    FunctionAppBaseUrl = functionAppBaseUrl,
    MaxRetries = int.Parse(azureFunctionsSettings["MaxRetries"] ?? "3"),
    TimeoutSeconds = int.Parse(azureFunctionsSettings["TimeoutSeconds"] ?? "60")
});

builder.Services.AddScoped<IAzureFunctionsService, AzureFunctionsService>();

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateUserMapCommand).Assembly);
    // Also register handlers from Infrastructure
    cfg.RegisterServicesFromAssembly(typeof(AccessServiceDbContext).Assembly);
});

// Repository and Unit of Work registration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<AccessService.Application.Interfaces.IUnitOfWork>(sp => (AccessService.Application.Interfaces.IUnitOfWork)sp.GetRequiredService<IUnitOfWork>());
builder.Services.AddScoped<IUserMapRepository, EFUserMapRepository>();
builder.Services.AddScoped<IUserRoleRepository, EFUserRoleRepository>();
builder.Services.AddScoped<IMenuRepository, EFMenuRepository>();
builder.Services.AddScoped<ISPARSHMenuRepository, EFSPARSHMenuRepository>();
builder.Services.AddScoped<ISPARSHMenuAccessRepository, EFSPARSHMenuAccessRepository>();

// Resilience - HttpClientFactory with Polly policies
// NOTE: Polly policies defined in PollyPolicies.cs and ready to use
// To enable: uncomment AddPolicyHandler after verifying Polly.Extensions.Http compatibility
builder.Services.AddHttpClient<IResilientHttpClient, ResilientHttpClient>()
    .ConfigureHttpClient(client =>
    {
        client.Timeout = TimeSpan.FromSeconds(10);
        client.DefaultRequestHeaders.Add("User-Agent", "AccessService/1.0");
    });
    // .AddPolicyHandler(PollyPolicies.GetCombinedPolicy()); // Requires Polly.Extensions.Http compatibility fix

// Authentication & Authorization
builder.Services.AddScoped<ITokenService, JwtTokenService>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secret = jwtSettings["Secret"] ?? "your-very-long-secret-key-here-min-32-characters";

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "AccessService",
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"] ?? "AccessServiceUsers",
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Access Service API",
        Version = "v1",
        Description = "User Access Management Microservice",
        Contact = new OpenApiContact { Name = "Development Team" }
    });
    
    // Add JWT security scheme to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme { Name = "Bearer", Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            new string[] { }
        }
    });
});

// Controllers
builder.Services.AddControllers();

// GraphQL (Hot Chocolate)
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Health checks
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("Database", tags: new[] { "db", "required" })
    .AddCheck<ApiHealthCheck>("API", tags: new[] { "api", "required" })
    .AddCheck<RabbitMQHealthCheck>("RabbitMQ", tags: new[] { "messaging" })
    .AddCheck<AzureBlobStorageHealthCheck>("AzureBlobStorage", tags: new[] { "storage" })
    .AddCheck<AzureFunctionsHealthCheck>("AzureFunctions", tags: new[] { "background-jobs" });

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Access Service API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

// Apply CORS
app.UseCors("AllowAll");

// Add detailed health check endpoint
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            timestamp = DateTime.UtcNow,
            checks = report.Entries.ToDictionary(
                x => x.Key,
                x => new { status = x.Value.Status.ToString(), description = x.Value.Description }
            )
        };
        await context.Response.WriteAsJsonAsync(response);
    }
});

// Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// GraphQL endpoint — requires valid JWT (same policy as REST controllers)
app.MapGraphQL("/graphql").RequireAuthorization();

// Database migration and domain event handler initialization on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AccessServiceDbContext>();

    // Migrate() only works on relational providers; InMemory (used in tests) uses EnsureCreated instead
    if (dbContext.Database.IsRelational())
        dbContext.Database.Migrate();
    else
        dbContext.Database.EnsureCreated();

    // Initialize RabbitMQ connection
    var rabbitMqConnection = scope.ServiceProvider.GetRequiredService<IRabbitMQConnection>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try
    {
        await rabbitMqConnection.ConnectAsync();
        logger.LogInformation("RabbitMQ connection established");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to connect to RabbitMQ. The service will continue with limited event publishing capabilities.");
    }

    // Initialize domain event handlers
    var eventPublisher = scope.ServiceProvider.GetRequiredService<IDomainEventPublisher>();
    if (eventPublisher is RabbitMQDomainEventPublisher rabbitMqPublisher)
    {
        logger.LogInformation("Using RabbitMQ for domain event publishing");
    }
    else
    {
        logger.LogInformation("RabbitMQ domain event publisher not configured");
    }
}

app.Run();

// Required for WebApplicationFactory in integration tests
public partial class Program { }

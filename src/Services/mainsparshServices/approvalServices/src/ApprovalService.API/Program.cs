using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MediatR;
using Polly;
using Polly.CircuitBreaker;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Azure.Storage.Blobs;
using RabbitMQ.Client;

using ApprovalService.Application.Behaviors;
using ApprovalService.Application.CQRS.Handlers;
using ApprovalService.Infrastructure.Persistence;
using ApprovalService.Infrastructure.Repositories;
using ApprovalService.Infrastructure.External;
using ApprovalService.Infrastructure.Messaging;
using ApprovalService.Application.Interfaces;
using ApprovalService.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Disable HTTPS in development
if (builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel((context, serverOptions) =>
    {
        serverOptions.ListenLocalhost(5000);
    });
}

// ==================== Configuration ====================
var jwtSettings = builder.Configuration.GetSection("Jwt");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name=\"ApprovalService\";Command Timeout=0";

// ==================== Services ====================

// Database
builder.Services.AddDbContext<ApprovalServiceDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.MigrationsAssembly("ApprovalService.Infrastructure");
        sqlOptions.EnableRetryOnFailure(3, TimeSpan.FromMilliseconds(100), null);
    }));

// Unit of Work and Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IApprovalMasterRepository, ApprovalMasterRepository>();
builder.Services.AddScoped<IApproverEmployeeRepository, ApproverEmployeeRepository>();

// MediatR with Behaviors
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// AutoMapper
builder.Services.AddAutoMapper(ap =>
{
    ap.AddProfile<MappingProfile>();
});

// External Services
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// Azure Blob Storage
var azureBlobConnectionString = builder.Configuration.GetConnectionString("AzureBlobStorage");
try
{
    if (!string.IsNullOrWhiteSpace(azureBlobConnectionString))
    {
        builder.Services.AddSingleton(new BlobServiceClient(azureBlobConnectionString));
        builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Warning: Failed to initialize Azure Blob Storage: {ex.Message}");
}

// RabbitMQ
try
{
    builder.Services.AddSingleton(RabbitMqConnectionFactory.CreateConnection(builder.Configuration));
    builder.Services.AddScoped<IMessagePublisher, RabbitMqMessagePublisher>();
}
catch (Exception ex)
{
    Console.WriteLine($"Warning: Failed to connect to RabbitMQ: {ex.Message}");
}

// Authentication & Authorization
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? "")),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Health Checks
builder.Services
    .AddHealthChecks()
    .AddSqlServer(connectionString, name: "SQL Server");
    // .AddRabbitMQ(new System.Uri(builder.Configuration.GetConnectionString("RabbitMq") ?? "amqp://guest:guest@localhost:5672/"), name: "RabbitMQ");

// API Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Approval Service API",
        Version = "v1",
        Description = "Microservice for managing approval workflows",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Development Team"
        }
    });

    // Add JWT Bearer Security Scheme
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter JWT token"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Circuit Breaker Policy
// var circuitBreakerPolicy = Policy
//     .Handle<HttpRequestException>()
//     .Or<TimeoutException>()
//     .CircuitBreaker(handledEventsAllowedBeforeBreaking: 3, durationOfBreak: TimeSpan.FromSeconds(30));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// ==================== Application ====================

var app = builder.Build();

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<ApprovalServiceDbContext>();
    if (dbContext != null)
    {
        await dbContext.Database.MigrateAsync();
    }
}

// Always enable Swagger in development
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Approval Service API v1");
    options.RoutePrefix = "swagger";
});

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Health Check Endpoint
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    Predicate = _ => true
});

app.MapControllers();

app.Run();

public partial class Program { }

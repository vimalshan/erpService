using System.Text;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using Serilog;
using ShipmentService.API.GraphQL.Mutations;
using ShipmentService.API.GraphQL.Queries;
using ShipmentService.API.Middleware;
using ShipmentService.API.MinimalApis;
using ShipmentService.API.Services;
using ShipmentService.Application;
using ShipmentService.Application.Common.Interfaces;
using ShipmentService.Infrastructure;
using ShipmentService.Infrastructure.Data.Seeds;

var builder = WebApplication.CreateBuilder(args);

// ──────────────────────────────────────────
// Serilog
// ──────────────────────────────────────────
builder.Host.UseSerilog((ctx, cfg) =>
    cfg.ReadFrom.Configuration(ctx.Configuration)
       .Enrich.FromLogContext()
       .WriteTo.Console()
       .WriteTo.File("logs/shipment-.log", rollingInterval: RollingInterval.Day));

// ──────────────────────────────────────────
// Application + Infrastructure
// ──────────────────────────────────────────
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// ──────────────────────────────────────────
// HTTP Context / Current User
// ──────────────────────────────────────────
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// ──────────────────────────────────────────
// JWT Authentication
// ──────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("WarehouseAdmin", policy => policy.RequireRole("Warehouse", "Admin"));
    options.AddPolicy("ReadOnly", policy => policy.RequireAuthenticatedUser());
});

// ──────────────────────────────────────────
// Controllers + Swagger
// ──────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Shipment Service API",
        Version = "v1",
        Description = "WMS Shipment & Tracking microservice"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter JWT token"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

// ──────────────────────────────────────────
// GraphQL (HotChocolate)
// ──────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<ShipmentQueryResolver>()
    .AddMutationType<ShipmentMutationResolver>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = builder.Environment.IsDevelopment());

// ──────────────────────────────────────────
// Health Checks
// ──────────────────────────────────────────
builder.Services
    .AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("ShipmentDb")!,
        name: "shipment-db",
        tags: ["db", "sql"]);

// ──────────────────────────────────────────
// Polly Circuit Breaker for outbound HTTP
// ──────────────────────────────────────────
builder.Services.AddHttpClient("resilient")
    .AddStandardResilienceHandler();

// ──────────────────────────────────────────
var app = builder.Build();
// ──────────────────────────────────────────

// Seed database on startup
await ShipmentDbContextSeed.SeedAsync(app.Services);

// Middleware pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shipment Service API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL("/graphql");

// Health check endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("db"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

// Minimal API endpoints
app.MapShipmentEndpoints();

app.Run();

public partial class Program { }

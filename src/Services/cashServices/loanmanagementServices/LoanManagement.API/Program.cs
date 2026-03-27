using System.Text;
using LoanManagement.Application;
using LoanManagement.API.GraphQL;
using LoanManagement.API.Middleware;
using LoanManagement.Infrastructure;
using LoanManagement.Infrastructure.Data;
using LoanManagement.Infrastructure.Data.SeedData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ── Application & Infrastructure ──────────────────────────────────
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// ── Controllers ───────────────────────────────────────────────────
builder.Services.AddControllers();

// ── Swagger / OpenAPI ─────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Loan Management API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// ── JWT Authentication ─────────────────────────────────────────────
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"]
    ?? throw new InvalidOperationException("JwtSettings:SecretKey is not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization();

// ── GraphQL (HotChocolate) ─────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<LoanQuery>()
    .AddMutationType<LoanMutation>()
    .AddAuthorization();

// ── Health Checks ──────────────────────────────────────────────────
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("LoanManagement")!,
        name: "sqlserver",
        failureStatus: HealthStatus.Unhealthy,
        tags: ["database"])
    .AddRabbitMQ(sp =>
        {
            var factory = new RabbitMQ.Client.ConnectionFactory
            {
                HostName = builder.Configuration["RabbitMQ:HostName"] ?? "localhost",
                UserName = builder.Configuration["RabbitMQ:UserName"] ?? "guest",
                Password = builder.Configuration["RabbitMQ:Password"] ?? "guest"
            };
            return factory.CreateConnectionAsync();
        },
        name: "rabbitmq",
        failureStatus: HealthStatus.Degraded,
        tags: ["messaging"]);

// ── HttpClient with Resilience (Circuit Breaker) ──────────────────
builder.Services.AddHttpClient("ExternalServices")
    .AddStandardResilienceHandler();

// ─────────────────────────────────────────────────────────────────
var app = builder.Build();
// ─────────────────────────────────────────────────────────────────

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Loan Management API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL("/graphql");
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = hc => hc.Tags.Contains("database")
});

// ── Minimal API endpoints ──────────────────────────────────────────
app.MapGet("/api/status", () => new { Status = "Running", Timestamp = DateTime.UtcNow })
   .WithName("GetStatus")
   .AllowAnonymous();

// ── Seed data (Development only) ─────────────────────────────────
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<LoanManagementDbContext>();
    db.Database.Migrate();
    var seedLogger = scope.ServiceProvider.GetRequiredService<ILogger<LoanManagementDbContext>>();
    await LoanManagementDbContextSeed.SeedAsync(db, seedLogger);
}

app.Run();

// Make Program accessible for integration tests
public partial class Program { }

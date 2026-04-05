using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TimesheetService.API.GraphQL.Mutations;
using TimesheetService.API.GraphQL.Queries;
using TimesheetService.API.HealthChecks;
using TimesheetService.API.Middleware;
using TimesheetService.API.MinimalApis;
using TimesheetService.Application;
using TimesheetService.Infrastructure;
using TimesheetService.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers ──────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── Application + Infrastructure layers ─────────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ── JWT Authentication ───────────────────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey     = jwtSection["Key"]
    ?? throw new InvalidOperationException("JWT Key is not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = jwtSection["Issuer"],
            ValidAudience            = jwtSection["Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// ── Swagger ──────────────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "Timesheet Service API",
        Version     = "v1",
        Description = "Employee Timesheet Management Microservice"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.Http,
        Scheme       = "bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description  = "Enter JWT token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// ── GraphQL (HotChocolate) ───────────────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<TimesheetQuery>()
    .AddMutationType<TimesheetMutation>()
    .AddAuthorization();

// ── Health Checks ────────────────────────────────────────────────────────────
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database", HealthStatus.Unhealthy, tags: ["db", "sql"])
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "sqlserver",
        tags: ["db", "sql"]);

// ── CORS ─────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

// ─────────────────────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Seed / Migrate database ───────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
    await DatabaseSeeder.SeedAsync(app.Services);

// ── Middleware pipeline ───────────────────────────────────────────────────────
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Timesheet Service v1"));
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// ── REST controllers ─────────────────────────────────────────────────────────
app.MapControllers();

// ── Minimal APIs ─────────────────────────────────────────────────────────────
app.MapTimesheetEndpoints();

// ── GraphQL ──────────────────────────────────────────────────────────────────
app.MapGraphQL("/graphql");

// ── Health checks ─────────────────────────────────────────────────────────────
app.MapHealthChecks("/health", new HealthCheckOptions { AllowCachingResponses = false });
app.MapHealthChecks("/health/db", new HealthCheckOptions
{
    Predicate             = hc => hc.Tags.Contains("db"),
    AllowCachingResponses = false
});

// ── Auth token (dev/test only) ────────────────────────────────────────────────
app.MapPost("/api/v1/auth/login", (Microsoft.AspNetCore.Http.HttpContext http, IConfiguration config) =>
{
    var jwt = config.GetSection("Jwt");
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var expires = DateTime.UtcNow.AddMinutes(double.Parse(jwt["ExpiryInMinutes"] ?? "60"));
    var token = new JwtSecurityToken(
        issuer: jwt["Issuer"],
        audience: jwt["Audience"],
        claims: new[]
        {
            new Claim(ClaimTypes.Name, "admin"),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(ClaimTypes.Role, "Manager"),
            new Claim("sub", "1")
        },
        expires: expires,
        signingCredentials: creds);
    return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expiresAt = expires });
}).AllowAnonymous();

// ── RabbitMQ test endpoint ────────────────────────────────────────────────────
app.MapGet("/api/rabbitmq/test", (IServiceProvider sp, IConfiguration config) =>
{
    try
    {
        var bus = sp.GetRequiredService<IBusControl>();
        return Results.Ok(new { service = "RabbitMQ", status = "Available", host = config["RabbitMQ:Host"] ?? "localhost" });
    }
    catch
    {
        return Results.Ok(new { service = "RabbitMQ", status = "Disconnected", host = config["RabbitMQ:Host"] ?? "localhost" });
    }
}).AllowAnonymous();

app.Run();

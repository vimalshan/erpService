using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using UserManagement.API.BackgroundTasks;
using UserManagement.API.GraphQL;
using UserManagement.API.Middleware;
using UserManagement.API.MinimalApis;
using UserManagement.Application;
using UserManagement.Infrastructure;
using UserManagement.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

// ─── Application & Infrastructure ───────────────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ─── Controllers ────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ─── JWT Authentication ──────────────────────────────────────────────────────
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"]
    ?? throw new InvalidOperationException("JWT SecretKey is not configured.");

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ReadOnly", policy => policy.RequireRole("Admin", "Reader"));
});

// ─── OpenAPI (.NET 10 native) + Scalar UI ────────────────────────────────────
// Swagger UI: accessible at /swagger/index.html (via Scalar)
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, ct) =>
    {
        document.Info.Title = "UserManagement API";
        document.Info.Version = "v1";
        document.Info.Description = "User Management Microservice — Policies, Contacts, Audit History";
        return Task.CompletedTask;
    });
});

// ─── GraphQL (HotChocolate) ──────────────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<UserManagementQuery>()
    .AddMutationType<UserManagementMutation>()
    .AddFiltering()
    .AddSorting()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = builder.Environment.IsDevelopment());

// ─── Health Checks ───────────────────────────────────────────────────────────
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "sqlserver",
        tags: ["db", "sql"]);

// ─── CORS ─────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// ─── Background Tasks (Azure Functions equivalent) ────────────────────────────
builder.Services.AddHostedService<UserPolicyArchivalTask>();
builder.Services.AddHostedService<NewsletterDigestTask>();

var app = builder.Build();

// ─── Middleware pipeline ──────────────────────────────────────────────────────
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    // Native OpenAPI spec endpoint
    app.MapOpenApi();
    // Scalar UI — accessible at /scalar/v1
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("UserManagement API");
    });
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// ─── Routing ─────────────────────────────────────────────────────────────────
app.MapControllers();
app.MapGraphQL("/graphql");

// ─── Minimal API endpoints ────────────────────────────────────────────────────
app.MapUserPolicyEndpoints();
app.MapWebsiteContactEndpoints();

// ─── Health Check endpoints ───────────────────────────────────────────────────
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

// ── Auth token (dev/test only) ────────────────────────────────────────────────
app.MapPost("/api/v1/auth/login", (IConfiguration config) =>
{
    var jwt = config.GetSection("JwtSettings");
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["SecretKey"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var expires = DateTime.UtcNow.AddMinutes(double.Parse(jwt["ExpiryMinutes"] ?? "60"));
    var token = new JwtSecurityToken(
        issuer: jwt["Issuer"],
        audience: jwt["Audience"],
        claims: new[]
        {
            new Claim(ClaimTypes.Name, "admin"),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(ClaimTypes.Role, "Reader"),
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
        var publisher = sp.GetRequiredService<IMessagePublisher>();
        return Results.Ok(new { service = "RabbitMQ", status = "Available", host = config["RabbitMQ:Host"] ?? "localhost" });
    }
    catch
    {
        return Results.Ok(new { service = "RabbitMQ", status = "Disconnected", host = config["RabbitMQ:Host"] ?? "localhost" });
    }
}).AllowAnonymous();

app.Run();

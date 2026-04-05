using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ReimbursementService.Application;
using ReimbursementService.API.GraphQL;
using ReimbursementService.API.Middleware;
using ReimbursementService.API.MinimalApis;
using ReimbursementService.Infrastructure;
using ReimbursementService.Infrastructure.Persistence;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .WriteTo.Console());

// ── Application & Infrastructure layers ─────────────────────────────────────
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// ── Controllers ──────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── Swagger / OpenAPI (.NET 10 built-in) ────────────────────────────────────
builder.Services.AddOpenApi("v1");
builder.Services.AddEndpointsApiExplorer();

// ── JWT Authentication ────────────────────────────────────────────────────────
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT Key is not configured.");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        };
    });

builder.Services.AddAuthorization();

// ── GraphQL ───────────────────────────────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<ReimbursementQuery>()
    .AddMutationType<ReimbursementMutation>();

// ── Health Checks ─────────────────────────────────────────────────────────────
builder.Services
    .AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "sqlserver",
        tags: ["db", "sql"]);

// ─────────────────────────────────────────────────────────────────────────────

var app = builder.Build();

// ── Apply migrations & seed ───────────────────────────────────────────────────
await DbInitializer.InitialiseAsync(app);

// ── Middleware pipeline ───────────────────────────────────────────────────────
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    // .NET 10 built-in OpenAPI endpoint: /openapi/v1.json
    app.MapOpenApi();
    // Swagger UI via Swashbuckle serving the .NET 10 OpenAPI document
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/openapi/v1.json", "Reimbursement Service v1"));
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();

// ── Controller routes ─────────────────────────────────────────────────────────
app.MapControllers();

// ── Minimal API routes ────────────────────────────────────────────────────────
app.MapReimbursementEndpoints();

// ── GraphQL ───────────────────────────────────────────────────────────────────
app.MapGraphQL("/graphql");

// ── Health checks ─────────────────────────────────────────────────────────────
app.MapHealthChecks("/health");

// ── Auth token endpoint (dev/test) ────────────────────────────────────────────
app.MapPost("/api/auth/token", (Microsoft.AspNetCore.Http.HttpContext ctx,
    IConfiguration config,
    ReimbursementService.API.Models.TokenRequest req) =>
{
    var key = config["Jwt:Key"]!;
    var issuer = config["Jwt:Issuer"]!;
    var audience = config["Jwt:Audience"]!;
    var expiry = int.Parse(config["Jwt:ExpiryMinutes"] ?? "60");

    var claims = new List<System.Security.Claims.Claim>
    {
        new("sub", req.UserId.ToString()),
        new("nameid", req.UserId.ToString()),
        new("name", req.UserName ?? "user")
    };
    if (req.Roles != null)
        foreach (var role in req.Roles)
            claims.Add(new(System.Security.Claims.ClaimTypes.Role, role));

    var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var creds = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);
    var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
        issuer, audience, claims,
        expires: DateTime.UtcNow.AddMinutes(expiry),
        signingCredentials: creds);

    return Results.Ok(new { token = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token) });
}).AllowAnonymous();

// ── RabbitMQ test endpoint ────────────────────────────────────────────────────
app.MapGet("/api/rabbitmq/test", (IServiceProvider sp, IConfiguration config) =>
{
    try
    {
        var bus = sp.GetRequiredService<MassTransit.IBus>();
        return Results.Ok(new { service = "RabbitMQ", status = "Available", transport = config["RabbitMQ:UseInMemory"] == "true" ? "InMemory" : "RabbitMQ" });
    }
    catch
    {
        return Results.Ok(new { service = "RabbitMQ", status = "Disconnected", transport = "Unknown" });
    }
}).AllowAnonymous();

app.Run();

namespace ReimbursementService.API.Models
{
    public record TokenRequest(long UserId, string? UserName, string[]? Roles);
}


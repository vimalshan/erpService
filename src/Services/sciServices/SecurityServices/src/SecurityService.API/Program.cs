using System.Text;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using SecurityService.Application;
using SecurityService.Application.DTOs;
using SecurityService.Application.Queries;
using SecurityService.Infrastructure;
using SecurityService.Infrastructure.Data;
using SecurityService.API.Middleware;
using SecurityService.API.GraphQL;
using SecurityService.API.Resilience;

var builder = WebApplication.CreateBuilder(args);

// ── Application & Infrastructure layers ──────────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddResiliencePolicies();

// ── Controllers ───────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, ct) =>
    {
        document.Info.Title = "Security Service API";
        document.Info.Version = "v1";
        document.Info.Description = "ERP Security Module – User, Role & Access Management";
        return Task.CompletedTask;
    });
});

// ── JWT Authentication ────────────────────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection("JwtSettings");
var secret = jwtSection["Secret"] ?? throw new InvalidOperationException("JwtSettings:Secret not configured.");
var key = Encoding.UTF8.GetBytes(secret);

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
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("AdminOnly", p => p.RequireRole("System Administrator"));
    opts.AddPolicy("SecurityManager", p => p.RequireRole("System Administrator", "Security Manager"));
});

// ── Swagger / OpenAPI ─────────────────────────────────────────────────────────
// ── (Scalar UI available at /scalar/v1) ──────────────────────────────────────

// ── GraphQL (HotChocolate) ────────────────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<SecurityQuery>()
    .AddMutationType<SecurityMutation>();

// ── Health Checks ─────────────────────────────────────────────────────────────
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "sqlserver",
        tags: ["db", "sql"])
    .AddDbContextCheck<SecurityDbContext>(name: "efcore", tags: ["db", "efcore"]);

// ── CORS ──────────────────────────────────────────────────────────────────────
builder.Services.AddCors(opts =>
    opts.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// ── Auto-migrate on startup ───────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SecurityDbContext>();
    await db.Database.MigrateAsync();
}

// ── Middleware pipeline ───────────────────────────────────────────────────────
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opts =>
    {
        opts.Title = "Security Service API";
        opts.AddPreferredSecuritySchemes("Bearer");
    });
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// ── REST Controllers ──────────────────────────────────────────────────────────
app.MapControllers();

// ── GraphQL endpoint ──────────────────────────────────────────────────────────
app.MapGraphQL("/graphql");

// ── Health Check endpoints ───────────────────────────────────────────────────
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = hc => hc.Tags.Contains("db"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// ── Minimal API routes ────────────────────────────────────────────────────────
app.MapGet("/api/v1/ping", () => Results.Ok(new { Status = "OK", UtcNow = DateTime.UtcNow }))
   .WithName("Ping").WithSummary("Health ping").WithTags("System");
app.MapGet("/api/v1/users/search", async (
        string? q, int page, int pageSize, bool activeOnly,
        IMediator mediator, CancellationToken ct) =>
    {
        var result = await mediator.Send(
            new SearchUsersQuery(q, page < 1 ? 1 : page, pageSize < 1 ? 20 : pageSize, activeOnly), ct);
        return Results.Ok(result);
    })
   .WithName("SearchUsers")
   .WithSummary("Paginated user search")
   .WithTags("Users")
   .RequireAuthorization();

app.MapGet("/api/v1/users/{userId:long}/access-tree", async (
        long userId, IMediator mediator, CancellationToken ct) =>
    {
        var result = await mediator.Send(new GetUserAccessTreeQuery(userId), ct);
        return Results.Ok(result);
    })
   .WithName("GetUserAccessTree")
   .WithSummary("Get user role + menu access tree")
   .WithTags("Users")
   .RequireAuthorization();

app.MapGet("/api/v1/stats", async (IMediator mediator, CancellationToken ct) =>
    {
        var result = await mediator.Send(new GetSecurityStatsQuery(), ct);
        return Results.Ok(result);
    })
   .WithName("GetSecurityStats")
   .WithSummary("Security module statistics")
   .WithTags("System")
   .RequireAuthorization();
await app.RunAsync();

public partial class Program { }

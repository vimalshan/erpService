using AuditService.Data;
using AuditService.Extensions;
using AuditService.GraphQL.Mutations;
using AuditService.GraphQL.Queries;
using AuditService.Middleware;
using AuditService.Repositories;
using AuditService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext().WriteTo.Console().CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Legacy registrations
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// AuditRepository now uses AuditDomainDbContext (registered in AddApplicationServices below)
builder.Services.AddScoped<IAuditRepository, AuditRepository>();
builder.Services.AddScoped<IAuditService, AuditService.Services.AuditService>();

// New layered architecture
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddMessagingServices(builder.Configuration);
builder.Services.AddHealthCheckServices(builder.Configuration);

// JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "YourSuperSecretKeyThatIsAtLeast32Characters!";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, ValidateAudience = true, ValidateLifetime = true, ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"], ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)), ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("role", "admin"));
    options.AddPolicy("Auditor", policy => policy.RequireClaim("role", "auditor", "admin"));
    options.AddPolicy("User", policy => policy.RequireClaim("role", "user", "auditor", "admin"));
});

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddFiltering().AddSorting().AddProjections()
    .AddErrorFilter<AuditService.GraphQL.GraphQLErrorFilter>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);

builder.Services.AddCors(o => o.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

// Apply migrations and seed data on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AuditService.Infrastructure.Data.AuditDomainDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        await AuditService.Infrastructure.Data.AuditDataSeeder.SeedAsync(db, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Database seeding failed");
    }
}

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGraphQL("/graphql");
// /health — core liveness check (database only; excludes messaging bus)
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = reg => !reg.Tags.Contains("masstransit"),
    ResultStatusCodes =
    {
        [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy]   = StatusCodes.Status200OK,
        [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded]  = StatusCodes.Status200OK,
        [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
    },
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new { name = e.Key, status = e.Value.Status.ToString(), description = e.Value.Description })
        };
        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result));
    }
});
// /health/full — includes messaging bus status (for monitoring dashboards)
app.MapHealthChecks("/health/full", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResultStatusCodes =
    {
        [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy]   = StatusCodes.Status200OK,
        [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded]  = StatusCodes.Status200OK,
        [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy] = StatusCodes.Status200OK,
    },
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new { name = e.Key, status = e.Value.Status.ToString(), description = e.Value.Description })
        };
        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result));
    }
});

// Minimal API endpoints
app.MapGet("/api/audits/minimal", async (MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new AuditService.Application.Queries.GetAllAuditsQuery())))
    .WithTags("Audits-Minimal");

app.MapGet("/api/audits/minimal/{id}", async (int id, MediatR.IMediator mediator) =>
{
    var result = await mediator.Send(new AuditService.Application.Queries.GetAuditByIdQuery(id));
    return result is not null ? Results.Ok(result) : Results.NotFound();
}).WithTags("Audits-Minimal");

app.MapPost("/api/audits/minimal", async (AuditService.Application.DTOs.CreateAuditDto dto, MediatR.IMediator mediator) =>
    Results.Created($"/api/audits/minimal/{0}", await mediator.Send(new AuditService.Application.Commands.CreateAuditCommand(dto))))
    .WithTags("Audits-Minimal");

app.MapPut("/api/audits/minimal", async (AuditService.Application.DTOs.UpdateAuditDto dto, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new AuditService.Application.Commands.UpdateAuditCommand(dto))))
    .WithTags("Audits-Minimal");

app.MapDelete("/api/audits/minimal/{id}", async (int id, MediatR.IMediator mediator) =>
    await mediator.Send(new AuditService.Application.Commands.DeleteAuditCommand(id)) ? Results.NoContent() : Results.NotFound())
    .WithTags("Audits-Minimal");

app.MapGet("/api/audits/minimal/types", async (MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new AuditService.Application.Queries.GetAuditTypesQuery())))
    .WithTags("Audits-Minimal");

app.MapGet("/api/audits/minimal/{auditId}/site-audits", async (int auditId, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new AuditService.Application.Queries.GetSiteAuditsQuery(auditId))))
    .WithTags("Audits-Minimal");

app.Run();

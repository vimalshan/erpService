using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Polly;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using EmployeeService.Application;
using EmployeeService.Infrastructure;
using EmployeeService.Infrastructure.Persistence;
using EmployeeService.API.GraphQL;
using EmployeeService.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ─────────────────────────────────────────────
// 1. Application + Infrastructure layers
// ─────────────────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ─────────────────────────────────────────────
// 2. Controllers + Swagger
// ─────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Employee Service API",
        Version = "v1",
        Description = "ERP Employee microservice — time-info, approvers, calendars, patterns & shifts."
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {token}"
    });
    c.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", doc),
            new List<string>()
        }
    });
});

// ─────────────────────────────────────────────
// 3. JWT Authentication & Authorization
// ─────────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"] ?? throw new InvalidOperationException("JWT Key not configured.");

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// JWT token generator (dev helper)
builder.Services.AddSingleton<EmployeeService.API.Auth.JwtTokenService>();

// ─────────────────────────────────────────────
// 4. GraphQL (HotChocolate)
// ─────────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<EmployeeQuery>()
    .AddMutationType<EmployeeMutation>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddAuthorization();

// ─────────────────────────────────────────────
// 5. Polly Circuit-Breaker for HttpClient
// ─────────────────────────────────────────────
builder.Services.AddHttpClient("resilient")
    .AddStandardResilienceHandler();

// ─────────────────────────────────────────────
// 6. Health Checks
// ─────────────────────────────────────────────
builder.Services
    .AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("EmployeeDb")!,
        name: "employeedb-sql",
        tags: ["db", "sql"])
    .AddDbContextCheck<EmployeeDbContext>(name: "employeedb-efcore");

// ─────────────────────────────────────────────
// Build
// ─────────────────────────────────────────────
var app = builder.Build();

// ─────────────────────────────────────────────
// 7. Middleware pipeline
// ─────────────────────────────────────────────
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Service v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// GraphQL endpoint — accessible at /graphql with Banana Cake Pop
app.MapGraphQL("/graphql");

// Health check endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = hc => hc.Tags.Contains("db"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// ─────────────────────────────────────────────
// 8. Minimal API endpoints (lightweight CRUD)
// ─────────────────────────────────────────────
var timeapi = app.MapGroup("/api/minimal/timeinfo").RequireAuthorization();

timeapi.MapGet("/{empSysId:long}", async (long empSysId, MediatR.IMediator mediator, CancellationToken ct) =>
{
    var result = await mediator.Send(new EmployeeService.Application.Queries.GetTimeInfo.GetTimeInfoByEmployeeQuery(empSysId), ct);
    return Results.Ok(result);
})
.WithName("MinimalGetTimeInfo")
.WithOpenApi();

timeapi.MapPost("/", async (EmployeeService.Application.Commands.RecordTimeInfo.RecordTimeInfoCommand cmd, MediatR.IMediator mediator, CancellationToken ct) =>
{
    var result = await mediator.Send(cmd, ct);
    return Results.Created($"/api/minimal/timeinfo/{result.TimeInfoId}", result);
})
.WithName("MinimalRecordTimeInfo")
.WithOpenApi();

// ─────────────────────────────────────────────
// 9. Auto-migrate on startup (dev only)
// ─────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();
    await db.Database.MigrateAsync();
}

await app.RunAsync();

using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using TravelService.API.GraphQL;
using TravelService.API.Middleware;
using TravelService.API.Services;
using TravelService.Application;
using TravelService.Application.Common.Interfaces;
using TravelService.Infrastructure;
using TravelService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// ─── Application & Infrastructure layers ───────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ─── Current User ──────────────────────────────────────────────────────────
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

// ─── Controllers ───────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ─── Swagger / OpenAPI ─────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TravelService API",
        Version = "v1",
        Description = "Travel & Tour Plan Management Microservice"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter JWT token: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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

// ─── GraphQL (Hot Chocolate) ────────────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<TravelQueryType>()
    .AddMutationType<TravelMutationType>()
    .AddAuthorization()
    .ModifyOptions(o => o.EnableOneOf = false);

// ─── Health Checks ──────────────────────────────────────────────────────────
builder.Services
    .AddHealthChecks()
    .AddSqlServer(
        connectionString: builder.Configuration.GetConnectionString("TravelDb")!,
        name: "sql-server",
        tags: new[] { "db", "sql" });

// ─── Polly Circuit Breaker (outbound HTTP resilience) ───────────────────────
builder.Services.AddHttpClient("TravelServiceClient")
    .AddStandardResilienceHandler(options =>
    {
        options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
        options.CircuitBreaker.MinimumThroughput = 5;
    });

// ─── CORS ───────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

// ─── Seed database (dev) ─────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    var seedLogger = app.Services.GetRequiredService<ILogger<Program>>();
    await TravelDbContextSeed.SeedAsync(app.Services, seedLogger);
}

// ─── Middleware pipeline ─────────────────────────────────────────────────────
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TravelService API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ─── GraphQL endpoint ────────────────────────────────────────────────────────
app.MapGraphQL("/graphql");

// ─── Health check endpoints ──────────────────────────────────────────────────
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

// ─── Minimal API endpoints ─────────────────────────────────────────────────
var tourPlanGroup = app.MapGroup("/api/v1/minimal/tourplans")
    .WithTags("TourPlans - Minimal")
    .RequireAuthorization();

tourPlanGroup.MapGet("/ping", () => Results.Ok(new { message = "TravelService is running", timestamp = DateTime.UtcNow }))
    .WithName("Ping")
    .AllowAnonymous();

tourPlanGroup.MapGet("/{id}", async (string id, MediatR.ISender sender, CancellationToken ct) =>
{
    var result = await sender.Send(new TravelService.Application.TourPlans.Queries.GetTourPlan.GetTourPlanByIdQuery(id), ct);
    return result is null ? Results.NotFound() : Results.Ok(result);
}).WithName("GetTourPlanMinimal");

tourPlanGroup.MapPost("/", async (TravelService.Application.TourPlans.Commands.CreateTourPlan.CreateTourPlanCommand cmd,
    MediatR.ISender sender, CancellationToken ct) =>
{
    var result = await sender.Send(cmd, ct);
    return Results.Created($"/api/v1/minimal/tourplans/{result.Id}", result);
}).WithName("CreateTourPlanMinimal");

await app.RunAsync();

public partial class Program { }

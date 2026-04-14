using System.Text;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using ReceivingService.API.HealthChecks;
using ReceivingService.API.GraphQL;
using ReceivingService.API.Middleware;
using ReceivingService.API.MinimalApis;
using ReceivingService.Application;
using ReceivingService.Infrastructure;
using ReceivingService.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// ──────────────────────────────────────────────────────────────────────────────
// Application + Infrastructure layers
// ──────────────────────────────────────────────────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ──────────────────────────────────────────────────────────────────────────────
// Controllers + OpenAPI (built-in .NET 10) + Scalar UI
// ──────────────────────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer((document, context, ct) =>
    {
        document.Info.Title       = "Receiving Service API";
        document.Info.Version     = "v1";
        document.Info.Description = "WMS Receiving microservice – REST, GraphQL and Minimal API endpoints";
        return Task.CompletedTask;
    });
});

// ──────────────────────────────────────────────────────────────────────────────
// JWT Authentication & Authorization
// ──────────────────────────────────────────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = jwtSection["Issuer"],
            ValidAudience            = jwtSection["Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["Key"]!))
        };
    });
builder.Services.AddAuthorization();

// ──────────────────────────────────────────────────────────────────────────────
// GraphQL (Hot Chocolate)
// ──────────────────────────────────────────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .BindRuntimeType<DateTime, FlexibleDateTimeType>()
    .AddQueryType<ReceivingService.API.GraphQL.ReceivingQuery>()
    .AddMutationType<ReceivingService.API.GraphQL.ReceivingMutation>();

// ──────────────────────────────────────────────────────────────────────────────
// Health Checks
// ──────────────────────────────────────────────────────────────────────────────
builder.Services
    .AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database", tags: new[] { "db", "ready" })
    .AddDbContextCheck<ReceivingDbContext>("ef-core", tags: new[] { "db", "ready" });

var app = builder.Build();

// ──────────────────────────────────────────────────────────────────────────────
// Apply EF Core migrations on startup
// ──────────────────────────────────────────────────────────────────────────────
await DatabaseInitialiser.InitialiseAsync(app);

// ──────────────────────────────────────────────────────────────────────────────
// Middleware pipeline
// ──────────────────────────────────────────────────────────────────────────────
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    // Built-in OpenAPI document at /openapi/v1.json
    app.MapOpenApi();
    // Scalar UI accessible at /scalar (compatible with Swagger UI workflows)
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Receiving Service API");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL("/graphql");

// Minimal API endpoints
app.MapReceivingEndpoints();

// Health check endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate       = _ => true,
    ResponseWriter  = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate      = hc => hc.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });

app.Run();

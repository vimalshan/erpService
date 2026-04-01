using System.Text;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerUI;
using DevelopmentService.API;
using DevelopmentService.API.BackgroundServices;
using DevelopmentService.API.HealthChecks;
using DevelopmentService.API.Middleware;
using DevelopmentService.API.MinimalApis;
using DevelopmentService.Application;
using DevelopmentService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ──────────────────────────────────────────────────────────────────────────────
// Application & Infrastructure layers
// ──────────────────────────────────────────────────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ──────────────────────────────────────────────────────────────────────────────
// JWT Authentication & Authorization
// ──────────────────────────────────────────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection("Jwt");
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
            IssuerSigningKey         = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["Key"]!))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// ──────────────────────────────────────────────────────────────────────────────
// Controllers, OpenAPI (native .NET 10)
// ──────────────────────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info.Title       = "Development Service API";
        document.Info.Version     = "v1";
        document.Info.Description = "Learning & Development Plan Microservice";
        return Task.CompletedTask;
    });
});

// ──────────────────────────────────────────────────────────────────────────────
// GraphQL via HotChocolate
// ──────────────────────────────────────────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .BindRuntimeType<char, HotChocolate.Types.StringType>()
    .BindRuntimeType<char?, HotChocolate.Types.StringType>()
    .AddTypeConverter<char, string>(c => c.ToString())
    .AddTypeConverter<string, char>(s => string.IsNullOrWhiteSpace(s) ? '\0' : s[0])
    .AddTypeConverter<string, char?>(s => string.IsNullOrWhiteSpace(s) ? null : s[0])
    .AddQueryType<DevelopmentService.API.GraphQL.Queries.DevelopmentQuery>()
    .AddMutationType<DevelopmentService.API.GraphQL.Mutations.DevelopmentMutation>()
    .AddAuthorization();

// ──────────────────────────────────────────────────────────────────────────────
// Health Checks
// ──────────────────────────────────────────────────────────────────────────────
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "sql-server",
        tags: ["db", "sql"])
    .AddCheck<DevelopmentDbHealthCheck>("efcore-db-check", tags: ["db"])
    .AddCheck<RabbitMqHealthCheck>("rabbitmq", Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded, ["mq", "rabbitmq"]);

// ──────────────────────────────────────────────────────────────────────────────
// Background Services (Azure Function equivalent)
// ──────────────────────────────────────────────────────────────────────────────
builder.Services.AddHostedService<DevelopmentCleanupService>();

// ──────────────────────────────────────────────────────────────────────────────
// CORS
// ──────────────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

// ──────────────────────────────────────────────────────────────────────────────
// Middleware pipeline
// ──────────────────────────────────────────────────────────────────────────────
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Development Service API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL("/graphql");

// Minimal API endpoints (v2 surface)
app.MapDevelopmentEndpoints();

// Health check endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate      = hc => hc.Tags.Contains("db"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

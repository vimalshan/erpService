using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using ApiGateway.HealthChecks;
using ApiGateway.Middleware;
using ApiGateway.Messaging;

var builder = WebApplication.CreateBuilder(args);

// ─── Ocelot Configuration ─────────────────────────────────────────────────────
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

// ─── Controllers & Swagger ────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Canteen Services API Gateway",
        Version = "v1",
        Description = "Ocelot API Gateway for all Canteen microservices"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {token}",
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

// ─── JWT Authentication ───────────────────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"] ?? throw new InvalidOperationException("JWT Key not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// ─── Ocelot + Cache ───────────────────────────────────────────────────────────
builder.Services.AddOcelot(builder.Configuration)
    .AddCacheManager(x => x.WithDictionaryHandle());

// ─── RabbitMQ Gateway Event Listener ──────────────────────────────────────────
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton<GatewayEventPublisher>();
builder.Services.AddHostedService<GatewayEventConsumer>();

// ─── Health Checks ────────────────────────────────────────────────────────────
var healthBuilder = builder.Services.AddHealthChecks()
    .AddCheck<DownstreamServiceHealthCheck>("downstream-services", tags: new[] { "ready" });

// Add RabbitMQ health check if configured
var rabbitHost = builder.Configuration["RabbitMQ:Host"];
if (!string.IsNullOrEmpty(rabbitHost))
{
    var rabbitConnStr = $"amqp://{builder.Configuration["RabbitMQ:Username"] ?? "guest"}:{builder.Configuration["RabbitMQ:Password"] ?? "guest"}@{rabbitHost}:{builder.Configuration["RabbitMQ:Port"] ?? "5672"}{builder.Configuration["RabbitMQ:VirtualHost"] ?? "/"}";
    healthBuilder.AddRabbitMQ(new Uri(rabbitConnStr), name: "rabbitmq", tags: new[] { "ready", "messaging" });
}

// ─── HttpClient for downstream health checks ─────────────────────────────────
builder.Services.AddHttpClient("HealthCheckClient")
    .ConfigureHttpClient(c => c.Timeout = TimeSpan.FromSeconds(5));

// ─── CORS ─────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// ─── Build ────────────────────────────────────────────────────────────────────
var app = builder.Build();

// ─── Middleware Pipeline ──────────────────────────────────────────────────────
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway v1");
    // Downstream service swagger docs
    c.SwaggerEndpoint("/api/canteen-unit/swagger/v1/swagger.json", "CanteenUnit Service");
    c.SwaggerEndpoint("/api/card-management/swagger/v1/swagger.json", "CardManagement Service");
    c.SwaggerEndpoint("/api/deduction/swagger/v1/swagger.json", "Deduction Service");
    c.SwaggerEndpoint("/api/eligibility/swagger/v1/swagger.json", "Eligibility Service");
    c.SwaggerEndpoint("/api/itemmaster/swagger/v1/swagger.json", "ItemMaster Service");
    c.SwaggerEndpoint("/api/referencedata/swagger/v1/swagger.json", "ReferenceData Service");
    c.SwaggerEndpoint("/api/swipe-transaction/swagger/v1/swagger.json", "SwipeTransaction Service");
    c.SwaggerEndpoint("/api/canteen-transaction/swagger/v1/swagger.json", "CanteenTransaction Service");
    c.RoutePrefix = "swagger";
});

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ─── Health Check endpoints ───────────────────────────────────────────────────
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = async (ctx, report) =>
    {
        ctx.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.TotalMilliseconds
            })
        });
        await ctx.Response.WriteAsync(result);
    }
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

// ─── Gateway dashboard info endpoint ──────────────────────────────────────────
app.MapGet("/", () => Results.Json(new
{
    service = "Canteen API Gateway",
    version = "1.0",
    documentation = "/swagger/index.html",
    health = "/health",
    services = new[]
    {
        new { name = "CanteenUnit", route = "/api/canteen-unit", swagger = "/api/canteen-unit/swagger/v1/swagger.json" },
        new { name = "CardManagement", route = "/api/card-management", swagger = "/api/card-management/swagger/v1/swagger.json" },
        new { name = "Deduction", route = "/api/deduction", swagger = "/api/deduction/swagger/v1/swagger.json" },
        new { name = "Eligibility", route = "/api/eligibility", swagger = "/api/eligibility/swagger/v1/swagger.json" },
        new { name = "ItemMaster", route = "/api/itemmaster", swagger = "/api/itemmaster/swagger/v1/swagger.json" },
        new { name = "ReferenceData", route = "/api/referencedata", swagger = "/api/referencedata/swagger/v1/swagger.json" },
        new { name = "SwipeTransaction", route = "/api/swipe-transaction", swagger = "/api/swipe-transaction/swagger/v1/swagger.json" },
        new { name = "CanteenTransaction", route = "/api/canteen-transaction", swagger = "/api/canteen-transaction/swagger/v1/swagger.json" }
    }
}));

// ─── Ocelot pipeline (must be last) ───────────────────────────────────────────
await app.UseOcelot();

app.Run();

public partial class Program { }

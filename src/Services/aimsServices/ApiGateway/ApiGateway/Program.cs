using System.Text;
using System.Text.Json;
using ApiGateway.DomainEvents;
using ApiGateway.GraphQL;
using ApiGateway.HealthChecks;
using ApiGateway.Messaging;
using ApiGateway.Resilience;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ── Serilog ──
builder.Host.UseSerilog((ctx, cfg) => cfg
    .ReadFrom.Configuration(ctx.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console());

// ── Ocelot ──
var env = builder.Environment.EnvironmentName;
builder.Configuration
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"ocelot.{env}.json", optional: true, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration)
    .AddPolly();

// ── JWT Authentication ──
var jwtKey = builder.Configuration["Jwt:Key"] ?? "ApiGateway_SuperSecretSigningKey_2026_MinLength32Chars!!";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
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

// ── Rate Limiting (AspNetCoreRateLimit) ──
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// ── Circuit Breaker (Polly) ──
builder.Services.AddCircuitBreakerPolicies(builder.Configuration);

// ── RabbitMQ Consumers ──
builder.Services.AddRabbitMqMessaging(builder.Configuration);

// ── MediatR (Domain Events) ──
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

// ── Health Checks ──
builder.Services.AddGatewayHealthChecks(builder.Configuration);

// ── GraphQL Gateway ──
builder.Services.AddGatewayGraphQL(builder.Configuration);

// ── Controllers + Swagger ──
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ERP API Gateway", Version = "v1" });
});

// ── CORS ──
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// ── Middleware Pipeline ──
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ERP API Gateway v1"));
}

app.UseCors("AllowAll");
app.UseIpRateLimiting();
app.UseAuthentication();
app.UseAuthorization();

// Map gateway's own endpoints
app.MapControllers();
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = 200,
        [HealthStatus.Degraded] = 200,
        [HealthStatus.Unhealthy] = 503
    },
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration.ToString(),
            entries = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.ToString()
            })
        });
        await context.Response.WriteAsync(result);
    }
});
app.MapGatewayGraphQL();

// Run Ocelot only for downstream service routes (exclude gateway's own /api/gateway and /api/graphqlproxy)
app.MapWhen(
    context => context.Request.Path.StartsWithSegments("/api")
              && !context.Request.Path.StartsWithSegments("/api/gateway")
              && !context.Request.Path.StartsWithSegments("/api/graphqlproxy"),
    ocelotApp =>
    {
        ocelotApp.UseOcelot().Wait();
    });

app.Run();

public partial class Program { }

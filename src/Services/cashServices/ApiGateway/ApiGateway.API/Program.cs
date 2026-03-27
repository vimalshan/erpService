using System.Text;
using System.Threading.RateLimiting;
using ApiGateway.API.Auth;
using ApiGateway.API.BlobStorage;
using ApiGateway.API.Configuration;
using ApiGateway.API.Endpoints;
using ApiGateway.API.GraphQL;
using ApiGateway.API.HealthChecks;
using ApiGateway.API.Messaging;
using ApiGateway.API.Middleware;
using ApiGateway.API.Resilience;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// ===== Configuration Binding =====
builder.Services.Configure<ServiceEndpoints>(builder.Configuration.GetSection(ServiceEndpoints.SectionName));
builder.Services.Configure<RateLimitingOptions>(builder.Configuration.GetSection(RateLimitingOptions.SectionName));
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection(RabbitMqSettings.SectionName));

var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()!;
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddSingleton<TokenService>();

// ===== YARP Reverse Proxy =====
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// ===== Authentication =====
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

// ===== Rate Limiting =====
var rateLimitConfig = builder.Configuration.GetSection(RateLimitingOptions.SectionName).Get<RateLimitingOptions>() ?? new();
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // Fixed window limiter for general requests
    options.AddPolicy("fixed", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = rateLimitConfig.PermitLimit,
                Window = TimeSpan.FromSeconds(rateLimitConfig.WindowSeconds),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = rateLimitConfig.QueueLimit
            }));

    // Token bucket limiter for authenticated users
    options.AddPolicy("token", context =>
        RateLimitPartition.GetTokenBucketLimiter(
            partitionKey: context.User?.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            factory: _ => new TokenBucketRateLimiterOptions
            {
                TokenLimit = rateLimitConfig.TokenPermitLimit,
                ReplenishmentPeriod = TimeSpan.FromSeconds(rateLimitConfig.TokenReplenishSeconds),
                TokensPerPeriod = rateLimitConfig.TokenPermitLimit / 2,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = rateLimitConfig.QueueLimit,
                AutoReplenishment = true
            }));
});

// ===== Resilience (Polly via Microsoft.Extensions.Http.Resilience) =====
builder.Services.AddGatewayResilience();

// ===== Health Checks =====
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "gateway-sqlserver",
        failureStatus: HealthStatus.Degraded,
        tags: ["database"])
    .AddCheck<DownstreamServiceHealthCheck>(
        "downstream-services",
        failureStatus: HealthStatus.Degraded,
        tags: ["downstream", "ready"]);

// RabbitMQ health check — optional
try
{
    var rabbitConfig = builder.Configuration.GetSection(RabbitMqSettings.SectionName).Get<RabbitMqSettings>();
    if (rabbitConfig is not null && !string.IsNullOrWhiteSpace(rabbitConfig.HostName))
    {
        builder.Services.AddHealthChecks()
            .AddRabbitMQ(
                async _ =>
                {
                    var factory = new ConnectionFactory
                    {
                        HostName = rabbitConfig.HostName,
                        Port = rabbitConfig.Port,
                        UserName = rabbitConfig.UserName,
                        Password = rabbitConfig.Password,
                        VirtualHost = rabbitConfig.VirtualHost
                    };
                    return await factory.CreateConnectionAsync();
                },
                name: "gateway-rabbitmq",
                failureStatus: HealthStatus.Degraded,
                tags: ["messaging"]);
    }
}
catch
{
    // RabbitMQ health check registration failed — non-critical
}

// ===== GraphQL =====
builder.Services
    .AddGraphQLServer()
    .AddQueryType<GatewayQuery>()
    .AddMutationType<GatewayMutation>()
    .AddAuthorization();

// ===== Controllers & Swagger =====
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "ERP API Gateway", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
});

// ===== CORS =====
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ===== Messaging (RabbitMQ - graceful degradation) =====
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<IGatewayEventPublisher, NoOpGatewayEventPublisher>();
}
else
{
    builder.Services.AddSingleton<IGatewayEventPublisher, RabbitMqGatewayEventPublisher>();
}

// ===== Blob Storage =====
builder.Services.AddSingleton<IGatewayBlobService, AzureGatewayBlobService>();

var app = builder.Build();

// ===== Middleware Pipeline =====
app.UseMiddleware<GatewayExceptionMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ERP API Gateway v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

// ===== Endpoints =====
app.MapControllers();
app.MapGatewayEndpoints();
app.MapGraphQL("/graphql");

// ===== YARP Reverse Proxy =====
app.MapReverseProxy();

// ===== Health Checks =====
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration.TotalMilliseconds,
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                duration = e.Value.Duration.TotalMilliseconds,
                description = e.Value.Description,
                data = e.Value.Data
            })
        };
        await context.Response.WriteAsJsonAsync(result);
    }
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString()
            })
        });
    }
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false // No checks — just confirms the app is running
});

app.Logger.LogInformation("ERP API Gateway started on {Urls}", string.Join(", ", app.Urls));

app.Run();

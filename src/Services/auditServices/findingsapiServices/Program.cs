// Program.cs
using FindingsAPI.Gateway;
using FindingsAPI.Gateway.Data;
using FindingsAPI.Gateway.Extensions;
using FindingsAPI.Gateway.GraphQL;
using FindingsAPI.Gateway.GraphQL.Middleware;
using FindingsAPI.Gateway.GraphQL.Queries;
using FindingsAPI.Gateway.GraphQL.Mutations;
// using FindingsAPI.Gateway.GraphQL.Subscriptions;
using FindingsAPI.Gateway.GraphQL.Types;
using FindingsAPI.Gateway.GraphQL.DataLoaders;
using FindingsAPI.Gateway.Middleware;
using FindingsAPI.Gateway.Services;
using FindingsAPI.Gateway.Repositories;
using HealthChecks.UI.Client;
using HotChocolate;
using HotChocolate.AspNetCore;
using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using StackExchange.Redis;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {CorrelationId} {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// GraphQL Server
builder.Services
    .AddGraphQLServer()
    .AddMutationConventions(applyToAllMutations: true)
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddTypeExtension<FindingsDomainMutation>()
    // .AddSubscriptionType<Subscription>()
    .AddType<FindingType>()
    .AddType<CompanyType>()
    .AddType<SiteType>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddAuthorization()
    .AddHttpRequestInterceptor<GraphQLHttpRequestInterceptor>()
    // .AddSocketSessionInterceptor<GraphQLSocketSessionInterceptor>()
    .AddDiagnosticEventListener<GraphQLExecutionLogger>()
    .AddErrorFilter<GraphQLErrorFilter>()
    .AddDataLoader<CompanyDataLoader>()
    // .AddDataLoader<SiteDataLoader>()
    // .PublishSchema(c => c
    //     .SetName("findings")
    //     .PublishToRedis("FindingsSchema", sp => sp.GetRequiredService<IConnectionMultiplexer>()))
    // .AddApolloTracing()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);

// HTTP Clients with Polly policies - REMOVED: No longer needed as services now use database directly
// builder.Services.AddHttpClient("FindingsService", client =>
// {
//     client.BaseAddress = new Uri(builder.Configuration["Services:FindingsService"]);
//     client.DefaultRequestHeaders.Add("X-API-Version", "1.0");
// })
// .AddHttpMessageHandler<CorrelationIdHandler>();

// builder.Services.AddHttpClient("CompanyService", client =>
// {
//     client.BaseAddress = new Uri(builder.Configuration["Services:CompanyService"]);
//     client.DefaultRequestHeaders.Add("X-API-Version", "1.0");
// })
// .AddHttpMessageHandler<CorrelationIdHandler>();

// builder.Services.AddHttpClient("SiteService", client =>
// {
//     client.BaseAddress = new Uri(builder.Configuration["Services:SiteService"]);
//     client.DefaultRequestHeaders.Add("X-API-Version", "1.0");
// })
// .AddHttpMessageHandler<CorrelationIdHandler>();

// Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };

    // For GraphQL WebSocket authentication
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            
            if (!string.IsNullOrEmpty(accessToken) && 
                path.StartsWithSegments("/graphql"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

// Authorization
static bool HasAnyRole(ClaimsPrincipal user, params string[] allowedRoles)
{
    var normalizedRoles = allowedRoles.Select(role => role.ToUpperInvariant()).ToHashSet();
    var roleClaimTypes = new[]
    {
        "role",
        ClaimTypes.Role,
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    };

    return user.Claims.Any(claim =>
        roleClaimTypes.Contains(claim.Type) &&
        normalizedRoles.Contains(claim.Value.ToUpperInvariant()));
}

static bool CanReadFindings(ClaimsPrincipal user)
{
    return user.Claims.Any(claim =>
               claim.Type == "permissions" &&
               claim.Value.Contains("findings:read", StringComparison.OrdinalIgnoreCase))
           || HasAnyRole(user, "admin", "auditor");
}

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => 
        policy.RequireAssertion(context => HasAnyRole(context.User, "admin")));
    
    options.AddPolicy("Auditor", policy => 
        policy.RequireAssertion(context => HasAnyRole(context.User, "auditor", "admin")));
    
    options.AddPolicy("User", policy => 
        policy.RequireAssertion(context => HasAnyRole(context.User, "user", "auditor", "admin")));
    
    options.AddPolicy("CanViewFindings", policy =>
        policy.RequireAssertion(context => CanReadFindings(context.User)));
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("GatewayCors", policyBuilder =>
    {
        policyBuilder.WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>())
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials()
               .WithExposedHeaders("X-Correlation-Id", "X-Request-Id");
    });
});

// Health Checks
// builder.Services.AddHealthChecks()
//     // .AddUrlGroup(new Uri(builder.Configuration["Services:FindingsService"] + "/health"), "Findings Service")
//     // .AddUrlGroup(new Uri(builder.Configuration["Services:CompanyService"] + "/health"), "Company Service")
//     // .AddRedis(builder.Configuration.GetConnectionString("Redis"))
//     // .AddApplicationInsightsPublisher();

// Redis for caching and distributed rate limiting (disabled locally — uses in-memory fallback)
// builder.Services.AddStackExchangeRedisCache(options =>
// {
//     options.Configuration = builder.Configuration.GetConnectionString("Redis");
//     options.InstanceName = "FindingsGateway:";
// });

// Memory Cache for local caching
builder.Services.AddMemoryCache();

// Distributed Cache (in-memory for local dev; swap to Redis in production)
builder.Services.AddDistributedMemoryCache();

// Application Insights
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
    options.EnableAdaptiveSampling = false;
    options.EnablePerformanceCounterCollectionModule = true;
});

// Custom Services
builder.Services.AddScoped<IFindingService, FindingService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ISiteService, SiteService>();
builder.Services.AddSingleton<CorrelationIdHandler>();
builder.Services.AddSingleton<ICorrelationIdProvider, CorrelationIdProvider>();

// Domain Layer Services (MediatR, Domain Repos, MassTransit, Health Checks)
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddMessagingServices(builder.Configuration);
builder.Services.AddHealthCheckServices(builder.Configuration);

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Disabled for development
app.UseCors("GatewayCors");
// app.UseRateLimiter();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapControllers();
app.MapGraphQL("/graphql").RequireAuthorization();
app.MapGraphQLSchema("/graphql/schema");
app.MapHealthChecks("/health");

// WebSocket support for GraphQL subscriptions
app.UseWebSockets();

// Schema stitching endpoint (for federated GraphQL)
app.MapGet("/graphql/sdl", () =>
{
    var schema = app.Services.GetRequiredService<ISchema>();
    return schema.Print();
});

app.Run();

// Polly Retry Policy
static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
        .WaitAndRetryAsync(3, retryAttempt => 
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) + Random.Shared.NextDouble()));
}
